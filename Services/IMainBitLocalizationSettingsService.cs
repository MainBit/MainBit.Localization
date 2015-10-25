using MainBit.Localization.Models;
using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Services
{
    public interface IMainBitLocalizationSettingsService : IDependency
    {
        MainBitLocalizationSettings GetSettings();
    }

    public class MainBitLocalizationSettingsService : IMainBitLocalizationSettingsService
    {
        private readonly IWorkContextAccessor _wca;

        public MainBitLocalizationSettingsService(IWorkContextAccessor wca)
        {
            _wca = wca;
        }

        public MainBitLocalizationSettings GetSettings()
        {
            var workContext = _wca.GetContext();
            var _providers = workContext.Resolve<IList<IMainBitLocalizationSettingsProvider>>();
            foreach (var provider in _providers)
            {
                var settings = provider.GetSettings();
                if (settings != null)
                {
                    return settings;
                }
            }

            return new MainBitLocalizationSettings(new List<MainBitCultureRecord>());
        }
    }
}