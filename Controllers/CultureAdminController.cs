using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using Orchard.ContentManagement;
using MainBit.Localization.Models;
using Orchard.Data;
using Orchard.UI.Admin;
using MainBit.Utility.Extensions;
using MainBit.Localization.Services;
using MainBit.Localization.ViewModels;
using System.Net;
using Orchard.UI.Notify;
using Orchard.Logging;
using System.Globalization;
using Orchard.Environment.Extensions;

namespace MainBit.Localization.Controllers {

    [Admin]
    [OrchardFeature("MainBit.Localization.MainSite")]
    public class CultureAdminController : Controller, IUpdateModel
    {
        private readonly IMainBitCultureRepository _mainBitCultureRepository;
        private readonly INotifier _notifier;

        public CultureAdminController(IOrchardServices services,
            IMainBitCultureRepository mainBitCultureRepository,
            INotifier notifier)
        {
            Services = services;
            _mainBitCultureRepository = mainBitCultureRepository;
            _notifier = notifier;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult Index() {
            var settings = Services.WorkContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            return View(settings.Cultures);
        }

        public ActionResult Add()
        {
            var viewModel = CreateViewModel(null);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Add(MainBitCultureViewModel viewModel)
        {
            if (Validate(viewModel, null))
            {
                var record = new MainBitCultureRecord();
                UpdateRecord(viewModel, record);
                _mainBitCultureRepository.Create(record);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var record = _mainBitCultureRepository.Get(id);
            var viewModel = CreateViewModel(record);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, MainBitCultureViewModel viewModel)
        {
            var record = _mainBitCultureRepository.Get(id);
            if (Validate(viewModel, record))
            {
                UpdateRecord(viewModel, record);
                _mainBitCultureRepository.Update(record);
            }

            return RedirectToAction("Index");
        }

        private bool Validate(MainBitCultureViewModel viewModel, MainBitCultureRecord record)
        {
            return true;
        }
        private void UpdateRecord(MainBitCultureViewModel viewModel, MainBitCultureRecord record)
        {
            record.Culture = string.IsNullOrWhiteSpace(viewModel.Culture) ? viewModel.SystemCulture : viewModel.Culture;
            record.UrlSegment = viewModel.UrlSegment.TrimSafe();
            record.StoredPrefix = viewModel.StoredPrefix.TrimSafe();
            record.Position = viewModel.Position;
            record.DisplayName = viewModel.DisplayName;
            record.IsMain = viewModel.IsMain;
            
        }
        private MainBitCultureViewModel CreateViewModel(MainBitCultureRecord record)
        {
            var settings = Services.WorkContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            var viewModel = new MainBitCultureViewModel();

            viewModel.AvailableSystemCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(ci => ci.Name)
                .Where(s => !settings.Cultures.Any(c => c.Culture == s));

            if(record != null) {
                viewModel.Id = record.Id;
                viewModel.Culture = record.Culture;
                viewModel.UrlSegment = record.UrlSegment;
                viewModel.StoredPrefix = record.StoredPrefix;
                viewModel.Position = record.Position;
                viewModel.DisplayName = record.DisplayName;
                viewModel.IsMain = record.IsMain;
            }

            return viewModel;
        }

        public new bool TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) where TModel : class
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        public void AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}