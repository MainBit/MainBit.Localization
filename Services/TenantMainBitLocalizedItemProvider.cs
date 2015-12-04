using MainBit.Alias.Services;
using MainBit.Localization.Models;
using MainBit.Localization.Providers;
using MainBit.MultiTenancy.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Extensions;
using Orchard.Mvc.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainBit.Localization.Services
{
    [OrchardFeature("MainBit.Localization.MultiTenancy")]
    public class TenantMainBitLocalizedItemProvider : IMainBitLocalizedItemProvider
    {
        private readonly IMainBitLocalizationSettingsService _mainBitLocalizationSettingsService;
        private readonly IWorkContextAccessor _wca;
        private readonly UrlHelper _urlHelper;
        private readonly IUrlService _urlService;
        private readonly IMainBitTenantService _mainBitTenantService;
        private readonly ITenantContentService _tenantContentService;

        public TenantMainBitLocalizedItemProvider(IMainBitLocalizationSettingsService mainBitLocalizationSettingsService,
            IWorkContextAccessor wca,
            UrlHelper urlHelper,
            IUrlService urlService,
            IMainBitTenantService mainBitTenantService,
            ITenantContentService tenantContentService)
        {
            _mainBitLocalizationSettingsService = mainBitLocalizationSettingsService;
            _wca = wca;
            _urlHelper = urlHelper;
            _urlService = urlService;
            _mainBitTenantService = mainBitTenantService;
            _tenantContentService = tenantContentService;
        }

        public string GetUrl(IContent item, string cultureName)
        {
            var tenantLocalizationPart = item.As<TenantLocalizationPart>();
            if (tenantLocalizationPart == null) return null;

            var settings = _mainBitLocalizationSettingsService.GetSettings();


            var contentCulture = settings.Cultures.FirstOrDefault(c => c.Culture == _wca.GetContext().CurrentCulture);
            var mainCulture = settings.Cultures.FirstOrDefault(c => c.IsMain);


            if (contentCulture.Culture == cultureName)
            {
                return _urlHelper.MakeAbsolute(_urlHelper.ItemDisplayUrl(item)); // already right culture
            }

            var urlContext = _urlService.GetCurrentContext();
            var newUrlContext = _urlService.ChangeSegmentValues(urlContext, new Dictionary<string, string>() {
                    { CultureUrlSegmentProvider.Name, mainCulture.UrlSegment }});

            var tenant = _mainBitTenantService.GetTenantByUrl(newUrlContext.Descriptor.BaseUrl);
            return _tenantContentService.ItemDisplayUrl(item, tenant.Name);
        }
    }
}