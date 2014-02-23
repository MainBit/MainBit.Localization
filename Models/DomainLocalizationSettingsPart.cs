using Orchard.ContentManagement;
using System.Collections.Generic;

namespace MainBit.Localization.Models
{
    public class DomainLocalizationSettingsPart : ContentPart<DomainLocalizationSettingsPartRecord>
    {
        public string Domain
        {
            get { return Record.Domain; }
            set { Record.Domain = value; }
        }
        public IList<DomainLocalizationSettingsItemRecord> Items 
        {
            get { return Record.Items; }
            set { Record.Items = value; }
        }
    }
}