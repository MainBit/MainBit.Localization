using MainBit.Localization.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainBit.Utility.Services;
using Orchard.Localization.Models;
using MainBit.Localization.Services;
using Orchard.UI.Admin;
using Orchard.Localization.Services;
using MainBit.Localization.Helpers;
using MainBit.Alias.Services;
using MainBit.Alias;
using System.Globalization;
using MainBit.Localization.Providers;

namespace MainBit.Localization
{
    public class Filter : FilterProvider, IActionFilter
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ICurrentContentAccessor _currentContentAccessor;
        private readonly IMainBitLocalizationService _mainBitLocalizationService;
        private readonly ILocalizationService _localizationService;
        private readonly IUrlService _urlService;
        private readonly IMainBitLocalizationSettingsService _mainBitLocalizationSettingsService;
        private readonly IUrlTemplateManager _urlTemplateManager;

        public Filter(IWorkContextAccessor wca,
            ICurrentContentAccessor currentContentAccessor,
            IMainBitLocalizationService mainBitLocalizationService,
            ILocalizationService localizationService,
            IUrlService urlService,
            IMainBitLocalizationSettingsService mainBitLocalizationSettingsService,
            IUrlTemplateManager urlTemplateManager)
        {
            _wca = wca;
            _currentContentAccessor = currentContentAccessor;
            _mainBitLocalizationService = mainBitLocalizationService;
            _localizationService = localizationService;
            _urlService = urlService;
            _mainBitLocalizationSettingsService = mainBitLocalizationSettingsService;
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

            var cultureSegment = _urlTemplateManager.DescribeUrlSegments().FirstOrDefault(s => s.Name == CultureUrlSegmentProvider.Name);
            if (cultureSegment == null) { return; }

            var contentCulture = _localizationService.GetContentCulture(content);
            var segmentValueShouldBe = cultureSegment.Values.FirstOrDefault(v => v.Name == contentCulture);
            if (segmentValueShouldBe == null) { return; }

            if (urlContext.Descriptor.Segments[CultureUrlSegmentProvider.Name].Name != segmentValueShouldBe.Name)
            {
                var newUrlContext = _urlService.ChangeSegmentValues(urlContext, new Dictionary<string, string> {
                    { CultureUrlSegmentProvider.Name, segmentValueShouldBe.Value }});

                if (newUrlContext != null)
                    filterContext.Result = new RedirectResult(newUrlContext.GetFullDisplayUrl());
            }
        }
    }
}