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
using MainBit.Localization.Extensions;
using MainBit.Localization.Services;

namespace MainBit.Localization.Selectors
{
    public class DomainCultureSelector : ICultureSelector {
        private readonly IOrchardServices _orchardServices;
        private readonly IDomainCultureHelper _domainCultureHelper;

        public DomainCultureSelector(
            IOrchardServices orchardServices,
            IDomainCultureHelper domainCultureHelper)
        {
            _orchardServices = orchardServices;
            _domainCultureHelper = domainCultureHelper;
        }

        public CultureSelectorResult GetCulture(HttpContextBase context) {
            if (context == null || ContextHelpers.IsRequestAdmin(context)) return null;

            var settings = _orchardServices.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            var currentBaseUrl = context.Request.GetBaseUrl();
            var currentCulture = _domainCultureHelper.GetCultureByBaseUrl(settings, currentBaseUrl);

            if (currentCulture != null)
            {
                return new CultureSelectorResult { Priority = 10, CultureName = currentCulture.Culture };
            }

            return null;
        }
    }
}