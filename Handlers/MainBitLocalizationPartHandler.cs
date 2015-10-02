using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using MainBit.Localization.Models;
using Orchard.Environment.Extensions;

namespace MainBit.Localization.Handlers {
    [OrchardFeature("MainBit.Localization.MainSite")]
    public class MainBitLocalizationPartHandler : ContentHandler {

        public MainBitLocalizationPartHandler(IRepository<MainBitLocalizationPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}