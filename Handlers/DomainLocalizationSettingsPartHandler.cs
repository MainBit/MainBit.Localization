using MainBit.Localization.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;

namespace MainBit.Localization.Handlers
{
    public class DomainLocalizationSettingsPartHandler : ContentHandler {

        public DomainLocalizationSettingsPartHandler(IRepository<DomainLocalizationSettingsPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<DomainLocalizationSettingsPart>("Site"));

            T = NullLocalizer.Instance;
            OnGetContentItemMetadata<DomainLocalizationSettingsPart>((context, part) => 
                context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("DomainLocalization"))));
        }

        public Localizer T { get; set; }
    }
}