using System.Linq;
using System.Web;
using Orchard.Localization.Services;
using Orchard;
using MainBit.Alias.Services;
using Orchard.UI.Admin;
using MainBit.Alias.Descriptors;
using Orchard.Environment.Extensions;

namespace MainBit.Localization.Selectors
{
    [OrchardFeature("MainBit.Localization.Alias")]
    public class AliasCultureSelector : ICultureSelector {
        private readonly IOrchardServices _orchardServices;
        private readonly IUrlService _urlService;
        private readonly IUrlTemplateManager _urlTemplateManager;

        public AliasCultureSelector(
            IOrchardServices orchardServices,
            IUrlService urlService,
            IUrlTemplateManager urlTemplateManager)
        {
            _orchardServices = orchardServices;
            _urlService = urlService;
            _urlTemplateManager = urlTemplateManager;
        }

        public CultureSelectorResult GetCulture(HttpContextBase context) {

            if (context == null || AdminFilter.IsApplied(context.Request.RequestContext)) return null;

            var cultureSegment = _urlTemplateManager.DescribeUrlSegments().FirstOrDefault(s => s.Name == "culture");
            if (cultureSegment == null) { return null; }
            
            var urlContext = _urlService.GetCurrentContext();
            if (urlContext == null) { return null; }

            UrlSegmentValueDescriptor culture;
            if (!urlContext.Descriptor.Segments.TryGetValue("culture", out culture))
                return null;

            return new CultureSelectorResult { Priority = 10, CultureName = culture.Name };
        }
    }
}