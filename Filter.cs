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

        public Filter(IWorkContextAccessor wca,
            ICurrentContentAccessor currentContentAccessor,
            IMainBitLocalizationService mainBitLocalizationService,
            ILocalizationService localizationService,
            IUrlService urlService,
            IMainBitLocalizationSettingsService mainBitLocalizationSettingsService)
        {
            _wca = wca;
            _currentContentAccessor = currentContentAccessor;
            _mainBitLocalizationService = mainBitLocalizationService;
            _localizationService = localizationService;
            _urlService = urlService;
            _mainBitLocalizationSettingsService = mainBitLocalizationSettingsService;
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

            var contentCulture = _localizationService.GetContentCulture(content);
            var settings = _mainBitLocalizationSettingsService.GetSettings();
            var contentMainBitCulture = settings.Cultures.FirstOrDefault(c => c.Culture == contentCulture);
            if (contentMainBitCulture == null) { return; }

            if (urlContext.Descriptor.Segments[CultureUrlSegmentProvider.Name].Value != contentMainBitCulture.UrlSegment)
            {
                var newUrlContext = _urlService.ChangeSegmentValues(urlContext, new Dictionary<string, string> {
                    { CultureUrlSegmentProvider.Name, contentMainBitCulture.UrlSegment }});

                if (newUrlContext != null)
                    filterContext.Result = new RedirectResult(newUrlContext.GetFullDisplayUrl());
            }
        }
    }
}