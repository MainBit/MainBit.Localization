using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System.Collections.Generic;

namespace MainBit.Localization.Models
{
    [OrchardFeature("MainBit.Localization.MainSite")]
    public class MainBitLocalizationPart : ContentPart<MainBitLocalizationPartRecord>
    {
        public IList<MainBitLocalizationItemRecord> Items
        {
            get { return Record.Items; }
            set { Record.Items = value; } 
        }
    }
}