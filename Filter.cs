using MainBit.Localization.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainBit.Common.Services;
using Orchard.Localization.Models;
using MainBit.Localization.Services;
using Orchard.UI.Admin;
using Orchard.Localization.Services;
using MainBit.Localization.Helpers;

namespace MainBit.Localization
{
    public class Filter : FilterProvider, IActionFilter
    {
        private readonly IOrchardServices _orchardServices;
        private readonly ICurrentContentAccessor _currentContentAccessor;
        private readonly IDomainLocalizationService _domainLocalizationService;
        private readonly ILocalizationService _localizationService;
        private readonly IDomainCultureHelper _domainCultureHelper;

        public Filter(IOrchardServices orchardServices,
            ICurrentContentAccessor currentContentAccessor,
            IDomainLocalizationService domainLocalizationService,
            ILocalizationService localizationService,
            IDomainCultureHelper domainCultureHelper)
        {
            _orchardServices = orchardServices;
            _currentContentAccessor = currentContentAccessor;
            _domainLocalizationService = domainLocalizationService;
            _localizationService = localizationService;
            _domainCultureHelper = domainCultureHelper;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (AdminFilter.IsApplied(filterContext.RequestContext)) { return; }

            var content = _currentContentAccessor.CurrentContentItem;
            if (content == null) { return; }

            var workContext = _orchardServices.WorkContext;
            var settings = workContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            var contentCultureName = _localizationService.GetContentCulture(content);
            var contentCulture = _domainCultureHelper.GetCultureByName(settings, contentCultureName);
            var currentCulture = _domainCultureHelper.GetCurrentCulture(settings);

            var virtualPath = filterContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2)
                    + filterContext.HttpContext.Request.PathInfo;
            var needRedirect = false;
            if (virtualPath == contentCulture.UrlPrefix)
            {
                needRedirect = true;
                virtualPath = "";
            }
            else if (virtualPath.StartsWith(contentCulture.UrlPrefix + "/"))
            {
                needRedirect = true;
                virtualPath = virtualPath.Substring(contentCulture.UrlPrefix.Length + 1);
            }

            if (contentCulture != currentCulture || needRedirect)
            {
                filterContext.Result = new RedirectResult(UrlBuilder.Combine(contentCulture.BaseUrl, virtualPath));
            }
        }
    }
}