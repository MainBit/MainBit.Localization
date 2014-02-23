using Orchard.ContentManagement.Records;
using System.Collections.Generic;

namespace MainBit.Localization.Models
{
    public class DomainLocalizationSettingsPartRecord : ContentPartRecord {
        public virtual string Domain { get; set; }
        public virtual IList<DomainLocalizationSettingsItemRecord> Items { get; set; }
    }
}