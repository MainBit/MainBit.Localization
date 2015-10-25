using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Alias;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization.Services;
using MainBit.Localization.Models;
using Orchard;
using MainBit.Localization.Services;
using MainBit.Alias.Services;
using MainBit.Localization.Providers;
using Orchard.UI.Admin;

namespace MainBit.Localization.Selectors
{
    public class DomainCultureSelector : ICultureSelector {
        private readonly IOrchardServices _orchardServices;
        private readonly IUrlService _urlService;
        private readonly IMainBitLocalizationSettingsService _mainBitLocalizationSettingsService;

        public DomainCultureSelector(
            IOrchardServices orchardServices,
            IUrlService urlService,
            IMainBitLocalizationSettingsService mainBitLocalizationSettingsService)
        {
            _orchardServices = orchardServices;
            _urlService = urlService;
            _mainBitLocalizationSettingsService = mainBitLocalizationSettingsService;
        }

        public CultureSelectorResult GetCulture(HttpContextBase context) {
            if (context == null || AdminFilter.IsApplied(context.Request.RequestContext)) return null;

            var settings = _mainBitLocalizationSettingsService.GetSettings();
            var urlContext = _urlService.GetCurrentContext();
            if (urlContext == null) { return null; }
            var culture = settings.Cultures.FirstOrDefault(c => c.UrlSegment == urlContext.Descriptor.Segments[CultureUrlSegmentProvider.Name].Value);

            return new CultureSelectorResult { Priority = 10, CultureName = culture.Culture };
        }
    }
}