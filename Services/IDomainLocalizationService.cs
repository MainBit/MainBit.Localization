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

namespace MainBit.Localization.Services
{
    public interface IDomainLocalizationService : IDependency {
        string ItemDisplayUrl(IContent content);
        string GetUrl(int sourseContentItemId, string destCulture);
        string GetDestUrl(int sourceContentItemId, string sourceCulture, string destCulture);

        


    }

    public class DomainLocalizationService : IDomainLocalizationService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IRepository<DomainLocalizationItemRecord> _itemRepository;
        private readonly ILocalizationService _localizationService;
        private readonly UrlHelper _urlHelper;
        private readonly IDomainCultureHelper _domainCultureHelper;

        public DomainLocalizationService(IOrchardServices orchardServices,
            IRepository<DomainLocalizationItemRecord> itemRepository,
            UrlHelper urlHelper,
            ILocalizationService localizationService,
            IDomainCultureHelper domainCultureHelper)
        {
            _orchardServices = orchardServices;
            _itemRepository = itemRepository;
            _urlHelper = urlHelper;
            _localizationService = localizationService;
            _domainCultureHelper = domainCultureHelper;
        }

        public string ItemDisplayUrl(IContent content)
        {
            var virtualUrl = _urlHelper.ItemDisplayUrl(content);

            if(virtualUrl == null) { return null; }

            var settings = _orchardServices.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            var contentCultureName = _localizationService.GetContentCulture(content);
            var contentCulture = _domainCultureHelper.GetCultureByName(settings, contentCultureName);

            if (contentCulture == null) { return virtualUrl; }

            var absoluteUrl = UrlBuilder.Combine(
                contentCulture.BaseUrl,
                virtualUrl.Substring(_orchardServices.WorkContext.HttpContext.Request.ApplicationPath.Length));

            return absoluteUrl;
        }

        public string GetUrl(int sourseContentItemId, string destCultureName)
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
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
                return string.Format("{0}/{1}?id={2}&culture={3}&toculture={4}",
                    mainCulture.BaseUrl,
                    MainBit.Localization.Routes.MainLocalizationUrl,
                    sourseContentItemId,
                    _orchardServices.WorkContext.CurrentSite.SiteCulture.ToLower(),
                    destCulture.Culture.ToLower());
            }
        }

        public string GetDestUrl(int sourceContentItemId, string sourceCultureName, string destCultureName)
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            var mainCulture = _domainCultureHelper.GetMainCulture(settings);
            var sourceCulture = _domainCultureHelper.GetCultureByName(sourceCultureName);
            var destCulture = _domainCultureHelper.GetCultureByName(destCultureName);

            var destContentItenmId = 0;

            if (mainCulture == sourceCulture)
            {
                var destLocalizedItem = _itemRepository.Fetch(p => p.DomainLocalizationPartRecord_Id == sourceContentItemId).FirstOrDefault();
                destContentItenmId = destLocalizedItem == null ? 0 : destLocalizedItem.LocalizedContentItemId;
            }
            else if (mainCulture == destCulture)
            {
                var sourseLocalizedItem = _itemRepository.Fetch(p => p.LocalizedContentItemId == sourceContentItemId).FirstOrDefault();
                destContentItenmId = sourseLocalizedItem == null ? 0 : sourseLocalizedItem.DomainLocalizationPartRecord_Id;
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
                        .Fetch(p => p.DomainLocalizationPartRecord_Id == sourseLocalizedItem.DomainLocalizationPartRecord_Id)
                        .FirstOrDefault();
                    destContentItenmId = destLocalizedItem == null ? 0 : destLocalizedItem.LocalizedContentItemId;
                }

            }

            if (destContentItenmId == 0)
            {
                return destCulture.BaseUrl;
            }
            else
            {
                return string.Format("{0}/{1}?id={2}",
                        destCulture.BaseUrl,
                        MainBit.Localization.Routes.LocalizationUrl,
                        destContentItenmId);
            }
        }
    }
}