using MainBit.Localization.Models;
using MainBit.Localization.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;

namespace MainBit.Localization.Handlers
{
    public class MainBitLocalizationSettingsPartHandler : ContentHandler {

        public MainBitLocalizationSettingsPartHandler(
            IMainBitCultureRepository mainBitCultureRepository)
        {
            Filters.Add(new ActivatingFilter<MainBitLocalizationSettingsPart>("Site"));

            OnLoading<MainBitLocalizationSettingsPart>((context, part) =>
                part.CulturesField.Loader(() => mainBitCultureRepository.GetList()));
        }

        public Localizer T { get; set; }
    }
}