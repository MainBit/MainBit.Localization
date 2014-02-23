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
using MainBit.Common.Services;

namespace MainBit.Localization.Drivers
{
    [UsedImplicitly]
    public class CulturePickerPartDriver : ContentPartDriver<CulturePickerPart> {
        private readonly ICultureManager _cultureManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IDomainLocalizationService _localizationService;
        public readonly ICurrentContentAccessor _currentContentAccessor;

        public CulturePickerPartDriver(
            ICultureManager cultureManager,
            IWorkContextAccessor workContextAccessor,
            IDomainLocalizationService localizationService,
            ICurrentContentAccessor currentContentAccessor)
        {
            _cultureManager = cultureManager;
            _workContextAccessor = workContextAccessor;
            _localizationService = localizationService;
            _currentContentAccessor = currentContentAccessor;
        }

        protected override DriverResult Display(CulturePickerPart part, string displayType, dynamic shapeHelper) {

            
            var settings = _workContextAccessor.GetContext().CurrentSite.As<DomainLocalizationSettingsPart>();
            var items = new List<CulturePickerViewModel>();

            foreach(var item in settings.Items.OrderByDescending(p => p.Position)) {
                items.Add(new CulturePickerViewModel()
                {
                    Url = _localizationService.GetUrl(_currentContentAccessor.CurrentContentItem.Id, item.Culture),
                    DisplayName = item.DisplayName,
                    CultureInfo = new System.Globalization.CultureInfo(item.Culture)
                });
            }
            
            var currentCulture = _workContextAccessor.GetContext().CurrentCulture;

            return ContentShape("Parts_CulturePicker", () => shapeHelper.Parts_CulturePicker(
                ViewModel: items));
        }
    }
}