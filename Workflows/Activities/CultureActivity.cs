using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;
using Orchard.Localization.Services;
using Orchard;

namespace MainBit.Localization.Workflows.Activities {
    public class CultureActivity : Task {

        private readonly IWorkContextAccessor _wca;
        private readonly ICultureManager _cultureManager;

        public CultureActivity(IWorkContextAccessor wca,
            ICultureManager cultureManager) {
            _wca = wca;
            _cultureManager = cultureManager;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext) {
            return true;
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            return _cultureManager.ListCultures().Select(x => T(x));
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext) {
            yield return T(_wca.GetContext().CurrentCulture);
        }

        public override string Name {
            get { return "Culture"; }
        }

        public override LocalizedString Category {
            get { return T("Flow"); }
        }

        public override LocalizedString Description {
            get { return T("Splits the workflow on different branches by culture, activating only one."); }
        }
    }
}