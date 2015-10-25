using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using MainBit.Localization.Models;
using Orchard.Environment.Extensions;

namespace MainBit.Localization.Handlers {
    [OrchardFeature("MainBit.Localization.DependentSite")]
    public class TenantLocalizationPartHandler : ContentHandler {

        public TenantLocalizationPartHandler(IRepository<TenantLocalizationPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}