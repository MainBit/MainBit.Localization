using MainBit.Alias;
using MainBit.Alias.Descriptors;
using Orchard;
using Orchard.Caching;
using Orchard.Localization.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Services
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
            var cultureManager = _wca.GetContext().Resolve<ICultureManager>();
            var siteCulture = cultureManager.GetSiteCulture();
            var cultures = cultureManager.ListCultures()
                .Where(c => c != siteCulture)
                .Select(c => CultureInfo.GetCultureInfo(c).TwoLetterISOLanguageName);
            var defaultCultures = CultureInfo.GetCultureInfo(siteCulture).TwoLetterISOLanguageName;

            context.Element(Name, cultures, defaultCultures);
        }

        public void MonitorChanged(AcquireContext<string> acquire)
        {
            acquire.Monitor(_signals.When("culturesChanged"));
        }
    }
}