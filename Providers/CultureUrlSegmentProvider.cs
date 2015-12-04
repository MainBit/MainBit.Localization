using MainBit.Alias;
using MainBit.Alias.Descriptors;
using MainBit.Localization.Models;
using MainBit.Localization.Services;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Core.Settings.Models;
using Orchard.Localization.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Providers
{
    public class CultureUrlSegmentProvider : IUrlSegmentProvider
    {
        private readonly IWorkContextAccessor _wca;
        private readonly ISignals _signals;
        private readonly IMainBitLocalizationSettingsService _mainBitLocalizationSettingsService;

        public CultureUrlSegmentProvider(IWorkContextAccessor wca, ISignals signals, IMainBitLocalizationSettingsService mainBitLocalizationSettingsService)
        {
            _wca = wca;
            _signals = signals;
            _mainBitLocalizationSettingsService = mainBitLocalizationSettingsService;
        }

        public static string Name
        {
            get { return "culture"; }
        }

        public void Describe(DescribeUrlSegmentContext context)
        {
            var settings = _mainBitLocalizationSettingsService.GetSettings();
            if (!settings.Cultures.Any()) return;

            var describeFor = context.For(Name, Name, 0);
            foreach (var culture in settings.Cultures)
            {
                describeFor.Value(
                    culture.Culture,
                    culture.DisplayName,
                    culture.Position,
                    culture.UrlSegment,
                    culture.StoredPrefix,
                    culture.IsMain
                );
            }
        }

        public void MonitorChanged(AcquireContext<string> acquire)
        {
            acquire.Monitor(_signals.When("culturesChanged"));
            acquire.Monitor(_signals.When("MainBitCulture.Changed"));
        }
    }
}