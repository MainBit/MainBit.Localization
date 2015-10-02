using MainBit.Alias;
using MainBit.Alias.Descriptors;
using MainBit.Localization.Models;
using MainBit.Localization.Services;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
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

        public CultureUrlSegmentProvider(IWorkContextAccessor wca, ISignals signals)
        {
            _wca = wca;
            _signals = signals;
        }

        public static string Name
        {
            get { return "TwoLetterISOLanguageName"; }
        }

        public void Describe(DescribeUrlSegmentsContext context)
        {
            var workContext = _wca.GetContext();
            var settings = workContext.CurrentSite.As<MainBitLocalizationSettingsPart>();

            var defaultCulture = settings.Cultures.FirstOrDefault(c => c.Culture != workContext.CurrentSite.SiteCulture);
            var otherCultures = settings.Cultures.Where(c => c != defaultCulture);

            context.Element(Name,
                otherCultures.Select(c => c.UrlSegment),
                defaultCulture != null ? defaultCulture.UrlSegment : "");
        }

        public void MonitorChanged(AcquireContext<string> acquire)
        {
            acquire.Monitor(_signals.When("culturesChanged"));
            acquire.Monitor(_signals.When("MainBitCulture.Changed"));
        }
    }
}