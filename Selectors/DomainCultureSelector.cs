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
using MainBit.Alias.Services;
using MainBit.Localization.Providers;

namespace MainBit.Localization.Selectors
{
    public class DomainCultureSelector : ICultureSelector {
        private readonly IOrchardServices _orchardServices;
        private readonly IDomainCultureHelper _domainCultureHelper;
        private readonly IUrlService _urlService;

        public DomainCultureSelector(
            IOrchardServices orchardServices,
            IDomainCultureHelper domainCultureHelper,
            IUrlService urlService)
        {
            _orchardServices = orchardServices;
            _domainCultureHelper = domainCultureHelper;
            _urlService = urlService;
        }

        public CultureSelectorResult GetCulture(HttpContextBase context) {
            if (context == null || ContextHelpers.IsRequestAdmin(context)) return null;

            var settings = _orchardServices.WorkContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            var urlContext = _urlService.CurrentUrlContext();
            if (urlContext == null) { return null; }
            var culture = settings.Cultures.FirstOrDefault(c => c.UrlSegment == urlContext.Descriptor.Segments[CultureUrlSegmentProvider.Name]);

            return new CultureSelectorResult { Priority = 10, CultureName = culture.Culture };
        }
    }
}