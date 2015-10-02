using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;
using Orchard.Localization.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Models
{
    [OrchardFeature("MainBit.Localization.MainSite")]
    public class MainBitLocalizationItemRecord
    {
        public virtual int Id { get; set; }
        public virtual int MainBitLocalizationPartRecord_Id { get; set; }
        public virtual int CultureRecord_Id { get; set; }
        public virtual int LocalizedContentItemId { get; set; }
    }
}