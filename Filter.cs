using Orchard;
using Orchard.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MainBit.Utility.Services;
using Orchard.UI.Admin;
using Orchard.Localization.Services;
using MainBit.Alias.Services;
using Orchard.Environment.Extensions;

namespace MainBit.Localization
{
    [OrchardFeature("MainBit.Localization.Alias")]
    public class Filter : FilterProvider, IActionFilter
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ICurrentContentAccessor _currentContentAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly IUrlService _urlService;
        private readonly IUrlTemplateManager _urlTemplateManager;

        public Filter(IWorkContextAccessor wca,
            ICurrentContentAccessor currentContentAccessor,
            ILocalizationService localizationService,
            IUrlService urlService,
            IUrlTemplateManager urlTemplateManager)
        {
            _wca = wca;
            _currentContentAccessor = currentContentAccessor;
            _localizationService = localizationService;
            _urlService = urlService;
            _urlTemplateManager = urlTemplateManager;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (AdminFilter.IsApplied(filterContext.RequestContext)) { return; }

            var content = _currentContentAccessor.CurrentContentItem;
            if (content == null) { return; }

            var urlContext = _urlService.GetCurrentContext();
            if (urlContext == null) { return; }

            var cultureSegment = _urlTemplateManager.DescribeUrlSegments().FirstOrDefault(s => s.Name == "culture");
            if (cultureSegment == null) { return; }

            var contentCulture = _localizationService.GetContentCulture(content);
            var segmentValueShouldBe = cultureSegment.Values.FirstOrDefault(v => v.Name == contentCulture);
            if (segmentValueShouldBe == null) { return; }

            if (urlContext.Descriptor.Segments["culture"].Name != segmentValueShouldBe.Name)
            {
                var newUrlContext = _urlService.ChangeSegmentValues(urlContext, new Dictionary<string, string> {
                    { "culture", segmentValueShouldBe.Name }});

                if (newUrlContext != null)
                    filterContext.Result = new RedirectResult(newUrlContext.GetFullDisplayUrl());
            }
        }
    }
}