using MainBit.Localization.Models;
using MainBit.MultiTenancy.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Services
{
    [OrchardFeature("MainBit.Localization.DependentSite")]
    public class TenantMainBitLocalizationSettingsProvider : IMainBitLocalizationSettingsProvider
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ITenantWorkContextAccessor _twca;

        public int Priority
        {
            get { return 10; }
        }

        public TenantMainBitLocalizationSettingsProvider(IWorkContextAccessor wca,
            ITenantWorkContextAccessor twca)
        {
            _wca = wca;
            _twca = twca;
        }

        public MainBitLocalizationSettings GetSettings()
        {
            var workContext = _wca.GetContext();
            var _twca = workContext.Resolve<ITenantWorkContextAccessor>();
            var defaultTenantWorkContext = _twca.GetDefaultTenantContext();
            var settings = defaultTenantWorkContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            return settings != null ? settings.ToSettings() : null;
        }
    }
}