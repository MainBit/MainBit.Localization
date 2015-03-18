using MainBit.Localization.Models;
using MainBit.Localization.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;

namespace MainBit.Localization.Handlers
{
    public class DomainLocalizationSettingsPartHandler : ContentHandler {

        public DomainLocalizationSettingsPartHandler(
            IDomainCultureService domainCultureService)
        {
            Filters.Add(new ActivatingFilter<DomainLocalizationSettingsPart>("Site"));

            OnLoading<DomainLocalizationSettingsPart>((context, part) => 
                part.CulturesField.Loader(() => domainCultureService.GetCultures()));
        }

        public Localizer T { get; set; }
    }
}