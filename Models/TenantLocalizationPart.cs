using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Models
{
    public class TenantLocalizationPart : ContentPart<TenantLocalizationPartRecord>
    {
        public int MasterContentItemId
        {
            get { return Retrieve(m => m.MasterContentItemId); }
            set { Store(m => m.MasterContentItemId, value); }
        }
    }
}