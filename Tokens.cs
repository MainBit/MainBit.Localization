using Orchard;
using Orchard.Localization;
using Orchard.Tokens;
using System.Globalization;

namespace MainBit.Localization
{

    public class Tokens : ITokenProvider
    {
        private readonly IWorkContextAccessor _wca;
        public Tokens(IWorkContextAccessor wca)
        {
            _wca = wca;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

            

        public void Describe(DescribeContext context)
        {
            context.For("WorkContext", T("Work Context"), T("Current Work Context"))
                .Token("CurrentCulture", T("CurrentCulture"), T("CurrentCulture"), "Culture");
        }

        public void Evaluate(EvaluateContext context)
        {
            context.For<WorkContext>("WorkContext", () => _wca.GetContext())
                .Token("CurrentCulture", x => _wca.GetContext().CurrentCulture)
                .Chain("CurrentCulture", "Culture", x => CultureInfo.GetCultureInfo(_wca.GetContext().CurrentCulture))
                ;
        }
    }
}