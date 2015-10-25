using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Models
{
    public class TenantLocalizationPartRecord : ContentPartRecord
    {
        public virtual int MasterContentItemId { get; set; }
    }
}