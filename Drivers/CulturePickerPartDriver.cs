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

namespace MainBit.Localization.Drivers
{
    [UsedImplicitly]
    public class CulturePickerPartDriver : ContentPartDriver<CulturePickerPart> {
        private readonly IOrchardServices _orchardServices;
        private readonly IDomainLocalizationService _domainLocalizationService;
        private readonly ICurrentContentAccessor _currentContentAccessor;
        private readonly ILocalizationService _localizationService;

        public CulturePickerPartDriver(
            IOrchardServices orchardServices,
            IDomainLocalizationService domainLocalizationService,
            ICurrentContentAccessor currentContentAccessor,
            ILocalizationService localizationService)
        {
            _orchardServices = orchardServices;
            _domainLocalizationService = domainLocalizationService;
            _currentContentAccessor = currentContentAccessor;
            _localizationService = localizationService;
        }

        protected override DriverResult Display(CulturePickerPart part, string displayType, dynamic shapeHelper) {

            var settings = _orchardServices.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            var currentContent = _currentContentAccessor.CurrentContentItem;
            var localized = currentContent.As<LocalizationPart>();
            var localizations = new List<LocalizationPart>();
            
            if (localized != null)
            {
                localizations.AddRange(_localizationService.GetLocalizations(currentContent, VersionOptions.Published));
                if (!localizations.Any(l => l.Id == localized.Id))
                {
                    if (localized.Culture != null)
                    {
                        localizations.Add(localized);
                    }
                }
            }

            var viewModel = new CulturePickerViewModel();
            var currentCulture = _orchardServices.WorkContext.CurrentCulture;

            foreach (var culture in settings.Cultures)
            {
                var cultureEntry = new CultureEntry
                {
                    DisplayName = culture.DisplayName,
                    CultureInfo = new System.Globalization.CultureInfo(culture.Culture)
                };
                var localization = localizations.FirstOrDefault(l => l.Culture.Culture == culture.Culture);

                if (localization != null)
                {
                    cultureEntry.Url = _domainLocalizationService.ItemDisplayUrl(localization);
                }
                else
                {
                    //cultureEntry.Url = _domainLocalizationService.GetUrl(currentContent.Id, culture.Culture);
                    cultureEntry.Url = culture.BaseUrl;
                }

                viewModel.Cultures.Add(cultureEntry);

                if (viewModel.CurrentCulture == null
                    && string.Equals(culture.Culture, currentCulture, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    viewModel.CurrentCulture = cultureEntry;
                }
            }



            //foreach(var item in settings.Cultures.OrderByDescending(p => p.Position)) {
            //    items.Add(new CulturePickerViewModel()
            //    {
            //        Url = _domainLocalizationService.GetUrl(_currentContentAccessor.CurrentContentItem.Id, item.Culture),
            //        DisplayName = item.DisplayName,
            //        CultureInfo = new System.Globalization.CultureInfo(item.Culture)
            //    });
            //}
            
            //var currentCulture = _workContextAccessor.GetContext().CurrentCulture;

            return ContentShape("Parts_CulturePicker", () => shapeHelper.Parts_CulturePicker(
                ViewModel: viewModel));
        }
    }
}