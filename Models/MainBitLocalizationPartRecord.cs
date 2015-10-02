using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;
using Orchard.Environment.Extensions;
using System.Collections.Generic;

namespace MainBit.Localization.Models
{
    [OrchardFeature("MainBit.Localization.MainSite")]
    public class MainBitLocalizationPartRecord : ContentPartRecord
    {
        public MainBitLocalizationPartRecord()
        {
            Items = new List<MainBitLocalizationItemRecord>();
        }

        [CascadeAllDeleteOrphan]
        public virtual IList<MainBitLocalizationItemRecord> Items { get; set; }
    }
}