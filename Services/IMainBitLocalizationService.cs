using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MainBit.Localization.Models;
using Orchard;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Localization.Services;
using Orchard.Mvc.Html;
using System.Web.Mvc;
using Orchard.Localization.Models;
using MainBit.Localization.Helpers;
using MainBit.Alias.Services;
using Orchard.Mvc;
using MainBit.Localization.Providers;

namespace MainBit.Localization.Services
{
    public interface IMainBitLocalizationService : IDependency {

        string ItemDisplayUrl(IContent content);
        string GetUrl(int sourseContentItemId, string destCulture);
        string GetDestUrl(int sourceContentItemId, string sourceCulture, string destCulture);

        MainBitLocalizationSettingsPart GetSettings();
        MainBitCultureRecord GetCulture(string culture);
    }

    public class MainBitLocalizationService : IMainBitLocalizationService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IRepository<MainBitLocalizationItemRecord> _itemRepository;
        private readonly ILocalizationService _localizationService;
        private readonly UrlHelper _urlHelper;
        private readonly IDomainCultureHelper _domainCultureHelper;
        private readonly IUrlService _urlService;
        private readonly IWorkContextAccessor _wca;

        public MainBitLocalizationService(IOrchardServices orchardServices,
            IRepository<MainBitLocalizationItemRecord> itemRepository,
            UrlHelper urlHelper,
            ILocalizationService localizationService,
            IDomainCultureHelper domainCultureHelper,
            IUrlService urlService,
            IWorkContextAccessor wca)
        {
            _orchardServices = orchardServices;
            _itemRepository = itemRepository;
            _urlHelper = urlHelper;
            _localizationService = localizationService;
            _domainCultureHelper = domainCultureHelper;
            _urlService = urlService;
            _wca = wca;
        }

        public string ItemDisplayUrl(IContent content)
        {
            var virtualUrl = _urlHelper.ItemDisplayUrl(content);

            if(virtualUrl == null) { return null; }

            var settings = _orchardServices.WorkContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            var contentCultureName = _localizationService.GetContentCulture(content);
            var contentCulture = _domainCultureHelper.GetCultureByName(settings, contentCultureName);

            if (contentCulture == null) { return virtualUrl; }

            var urlContext = _urlService.CurrentUrlContext();
            if (urlContext == null) { return virtualUrl; }

            var newUrlContext = _urlService.ChangeSegmentValues(urlContext, new Dictionary<string, string>() {
                { CultureUrlSegmentProvider.Name, contentCulture.UrlSegment }});

            var virtualPath = virtualUrl.Substring(_orchardServices.WorkContext.HttpContext.Request.ApplicationPath.Length).TrimStart('/');
            if (virtualPath.Equals(newUrlContext.Descriptor.StoredPrefix, StringComparison.InvariantCultureIgnoreCase))
            {
                virtualPath = virtualPath.Substring(newUrlContext.Descriptor.StoredPrefix.Length);
            }
            else if (virtualPath.StartsWith(newUrlContext.Descriptor.StoredPrefix + "/", StringComparison.InvariantCultureIgnoreCase))
            {
                virtualPath = virtualPath.Substring(newUrlContext.Descriptor.StoredPrefix.Length + 1);
            }
            else
            {
                
            }

            var absoluteUrl = UrlBuilder.Combine(newUrlContext.Descriptor.BaseUrl, virtualPath);

            return absoluteUrl;
        }

        public string GetUrl(int sourseContentItemId, string destCultureName)
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            var mainCulture = _domainCultureHelper.GetMainCulture(settings);
            var currentCulture = _domainCultureHelper.GetCurrentCulture(settings);
            var destCulture = _domainCultureHelper.GetCultureByName(destCultureName);


            if (currentCulture == destCulture)
            {
                return string.Empty;
            }

            if (currentCulture == mainCulture)
            {
                return GetDestUrl(sourseContentItemId, _orchardServices.WorkContext.CurrentSite.SiteCulture.ToLower(), destCulture.Culture);
            }
            else
            {
                var urlContext = _urlService.CurrentUrlContext();
                var newUrlContext = _urlService.ChangeSegmentValues(urlContext, new Dictionary<string, string>() {
                    { CultureUrlSegmentProvider.Name, new CultureInfo(mainCulture.Culture).TwoLetterISOLanguageName }});

                return string.Format("{0}/{1}?id={2}&culture={3}&toculture={4}",
                    newUrlContext.Descriptor.BaseUrl,
                    MainBit.Localization.Routes.MainLocalizationUrl,
                    sourseContentItemId,
                    _orchardServices.WorkContext.CurrentSite.SiteCulture.ToLower(),
                    destCulture.Culture.ToLower());
            }
        }

        public string GetDestUrl(int sourceContentItemId, string sourceCultureName, string destCultureName)
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            var mainCulture = _domainCultureHelper.GetMainCulture(settings);
            var sourceCulture = _domainCultureHelper.GetCultureByName(sourceCultureName);
            var destCulture = _domainCultureHelper.GetCultureByName(destCultureName);

            var destContentItenmId = 0;

            if (mainCulture == sourceCulture)
            {
                var destLocalizedItem = _itemRepository.Fetch(p => p.MainBitLocalizationPartRecord_Id == sourceContentItemId).FirstOrDefault();
                destContentItenmId = destLocalizedItem == null ? 0 : destLocalizedItem.LocalizedContentItemId;
            }
            else if (mainCulture == destCulture)
            {
                var sourseLocalizedItem = _itemRepository.Fetch(p => p.LocalizedContentItemId == sourceContentItemId).FirstOrDefault();
                destContentItenmId = sourseLocalizedItem == null ? 0 : sourseLocalizedItem.MainBitLocalizationPartRecord_Id;
            }
            else
            {
                var sourseLocalizedItem = _itemRepository.Fetch(p => p.LocalizedContentItemId == sourceContentItemId).FirstOrDefault();
                if (sourseLocalizedItem == null)
                {
                    destContentItenmId = 0;
                }
                else
                {
                    var destLocalizedItem = _itemRepository
                        .Fetch(p => p.MainBitLocalizationPartRecord_Id == sourseLocalizedItem.MainBitLocalizationPartRecord_Id)
                        .FirstOrDefault();
                    destContentItenmId = destLocalizedItem == null ? 0 : destLocalizedItem.LocalizedContentItemId;
                }

            }

            var urlContext = _urlService.CurrentUrlContext();
            var newUrlContext = _urlService.ChangeSegmentValues(urlContext, new Dictionary<string, string>() {
                    { CultureUrlSegmentProvider.Name, new CultureInfo(mainCulture.Culture).TwoLetterISOLanguageName }});

            if (destContentItenmId == 0)
            {
                return newUrlContext.Descriptor.BaseUrl;
            }
            else
            {
                return string.Format("{0}/{1}?id={2}",
                        newUrlContext.Descriptor.BaseUrl,
                        MainBit.Localization.Routes.LocalizationUrl,
                        destContentItenmId);
            }
        }

        public MainBitLocalizationSettingsPart GetSettings()
        {
            var workContext = _wca.GetContext();
            var settings = workContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            return settings;
        }

        public MainBitCultureRecord GetCulture(string culture)
        {
            var settings = GetSettings();
            return settings.Cultures.FirstOrDefault(c => c.Culture == culture);
        }
    }
}