using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;
using Orchard.Environment.Extensions;
using System.Collections.Generic;

namespace MainBit.Localization.Models
{
    [OrchardFeature("MainBit.Localization.MainSite")]
    public class DomainLocalizationPartRecord : ContentPartRecord
    {
        public DomainLocalizationPartRecord()
        {
            Items = new List<DomainLocalizationItemRecord>();

        }
        [CascadeAllDeleteOrphan]
        public virtual IList<DomainLocalizationItemRecord> Items { get; set; }
    }
}