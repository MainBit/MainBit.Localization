using System.Collections.Generic;
using JetBrains.Annotations;
using MainBit.Localization.Models;
using MainBit.Localization.Services;
using Orchard;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization.Services;
using Orchard.ContentManagement;
using MainBit.Localization.ViewModels;
using System.Linq;
using MainBit.Utility.Services;
using Orchard.Localization.Models;
using MainBit.Alias.Services;
using Orchard.Mvc;
using System.Globalization;

namespace MainBit.Localization.Drivers
{
    [UsedImplicitly]
    public class CulturePickerPartDriver : ContentPartDriver<CulturePickerPart> {
        private readonly IOrchardServices _orchardServices;
        private readonly IMainBitLocalizationService _mainbitLocalizationService;
        private readonly ICurrentContentAccessor _currentContentAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly IUrlService _urlService;
        private readonly IMainBitLocalizationSettingsService _mainBitLocalizationSettingsService;

        public CulturePickerPartDriver(
            IOrchardServices orchardServices,
            IMainBitLocalizationService mainbitLocalizationService,
            ICurrentContentAccessor currentContentAccessor,
            ILocalizationService localizationService,
            IUrlService urlService,
            IMainBitLocalizationSettingsService mainBitLocalizationSettingsService)
        {
            _orchardServices = orchardServices;
            _mainbitLocalizationService = mainbitLocalizationService;
            _currentContentAccessor = currentContentAccessor;
            _localizationService = localizationService;
            _urlService = urlService;
            _mainBitLocalizationSettingsService = mainBitLocalizationSettingsService;
        }

        protected override DriverResult Display(CulturePickerPart part, string displayType, dynamic shapeHelper) {

            var settings = _mainBitLocalizationSettingsService.GetSettings();
            var currentContent = _currentContentAccessor.CurrentContentItem;

            var viewModel = new CulturePickerViewModel();

            foreach (var culture in settings.Cultures)
            {
                var cultureEntry = new CultureEntry
                {
                    DisplayName = culture.DisplayName,
                    CultureInfo = new CultureInfo(culture.Culture),
                    MainBitCulture = culture,
                    Url = _mainbitLocalizationService.GetUrl(currentContent, culture.Culture)
                };
 
                viewModel.Cultures.Add(cultureEntry);
            }

            viewModel.CurrentCulture = viewModel.Cultures.FirstOrDefault(c => c.MainBitCulture.Culture == _orchardServices.WorkContext.CurrentCulture);

            return ContentShape("Parts_CulturePicker", () => shapeHelper.Parts_CulturePicker(ViewModel: viewModel));
        }
    }
}