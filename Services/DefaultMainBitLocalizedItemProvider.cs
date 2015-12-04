using MainBit.Alias.Services;
using MainBit.Localization.Providers;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization.Models;
using Orchard.Localization.Services;
using Orchard.Mvc.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainBit.Localization.Services
{
    [OrchardFeature("MainBit.Localization.Default")]
    public class DefaultMainBitLocalizedItemProvider : IMainBitLocalizedItemProvider
    {
        private readonly IMainBitLocalizationSettingsService _mainBitLocalizationSettingsService;
        private readonly ILocalizationService _localizationService;
        private readonly UrlHelper _urlHelper;
        private readonly IUrlService _urlService;

        public DefaultMainBitLocalizedItemProvider(
            IMainBitLocalizationSettingsService mainBitLocalizationSettingsService,
            ILocalizationService localizationService,
            UrlHelper urlHelper,
            IUrlService urlService)
        {
            _mainBitLocalizationSettingsService = mainBitLocalizationSettingsService;
            _localizationService = localizationService;
            _urlHelper = urlHelper;
            _urlService = urlService;
        }

        public string GetUrl(IContent item, string cultureName)
        {
            var localizationPart = item.As<LocalizationPart>();
            if (localizationPart == null) return null; 

            var settings = _mainBitLocalizationSettingsService.GetSettings();
            var contentCulture = settings.Cultures.FirstOrDefault(c => c.Culture == localizationPart.Culture.Culture);
            var toCulture = settings.Cultures.FirstOrDefault(c => c.Culture == cultureName);

            var urlContext = _urlService.GetCurrentContext();
            var newUrlContext = _urlService.ChangeSegmentValues(urlContext, new Dictionary<string, string>() {
                    { CultureUrlSegmentProvider.Name, toCulture.UrlSegment }});

            var toLocalizationPart = _localizationService.GetLocalizedContentItem(item, cultureName);
            if (toLocalizationPart != null)
            {
                newUrlContext = _urlService.GetContext(newUrlContext.Descriptor.BaseUrl, _urlHelper.ItemDisplayUrl(toLocalizationPart));
            }
            else
            {
                newUrlContext = _urlService.GetContext(newUrlContext.Descriptor.BaseUrl, null);
            }

            return urlContext.GetFullDisplayUrl();
        }
    }
}