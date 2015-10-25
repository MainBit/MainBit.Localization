using MainBit.Localization.Models;
using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using MainBit.MultiTenancy.Services;
using Orchard.Environment.Extensions;

namespace MainBit.Localization.Services
{
    public interface IMainBitLocalizationSettingsProvider : IDependency
    {
        MainBitLocalizationSettings GetSettings();
    }


    [OrchardFeature("MainBit.Localization.CultureSettings")]
    public class MainSiteMainBitLocalizationSettingsProvider : IMainBitLocalizationSettingsProvider
    {
        private readonly IWorkContextAccessor _wca;
        public MainSiteMainBitLocalizationSettingsProvider(IWorkContextAccessor wca)
        {
            _wca = wca;
        }

        public MainBitLocalizationSettings GetSettings()
        {
            var workContext = _wca.GetContext();
            var settings = workContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            return settings != null ? settings.ToSettings() : null;
        }
    }


    [OrchardFeature("MainBit.Localization.DependentSite")]
    public class TenantMainBitLocalizationSettingsProvider : IMainBitLocalizationSettingsProvider
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ITenantWorkContextAccessor _twca;

        public TenantMainBitLocalizationSettingsProvider(IWorkContextAccessor wca)
        {
            _wca = wca;
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