using MainBit.Localization.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Services
{
    [OrchardFeature("MainBit.Localization.CultureSettings")]
    public class MainSiteMainBitLocalizationSettingsProvider : IMainBitLocalizationSettingsProvider
    {
        private readonly IWorkContextAccessor _wca;
        public MainSiteMainBitLocalizationSettingsProvider(IWorkContextAccessor wca)
        {
            _wca = wca;
        }

        public int Priority
        {
            get { return 20; }
        }

        public MainBitLocalizationSettings GetSettings()
        {
            var workContext = _wca.GetContext();
            var settings = workContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            return settings != null ? settings.ToSettings() : null;
        }
    }
}