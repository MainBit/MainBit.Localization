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
using MainBit.Alias.Descriptors;

namespace MainBit.Localization.Selectors
{
    public class AliasCultureSelector : ICultureSelector {
        private readonly IOrchardServices _orchardServices;
        private readonly IUrlService _urlService;
        private readonly IMainBitLocalizationSettingsService _mainBitLocalizationSettingsService;
        private readonly IUrlTemplateManager _urlTemplateManager;

        public AliasCultureSelector(
            IOrchardServices orchardServices,
            IUrlService urlService,
            IMainBitLocalizationSettingsService mainBitLocalizationSettingsService,
            IUrlTemplateManager urlTemplateManager)
        {
            _orchardServices = orchardServices;
            _urlService = urlService;
            _mainBitLocalizationSettingsService = mainBitLocalizationSettingsService;
            _urlTemplateManager = urlTemplateManager;
        }

        public CultureSelectorResult GetCulture(HttpContextBase context) {

            if (context == null || AdminFilter.IsApplied(context.Request.RequestContext)) return null;

            var cultureSegment = _urlTemplateManager.DescribeUrlSegments().FirstOrDefault(s => s.Name == CultureUrlSegmentProvider.Name);
            if (cultureSegment == null) { return null; }
            
            var urlContext = _urlService.GetCurrentContext();
            if (urlContext == null) { return null; }

            UrlSegmentValueDescriptor culture;
            if (!urlContext.Descriptor.Segments.TryGetValue(CultureUrlSegmentProvider.Name, out culture))
                return null;

            return new CultureSelectorResult { Priority = 10, CultureName = culture.Name };
        }
    }
}