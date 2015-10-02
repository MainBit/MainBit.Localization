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

namespace MainBit.Localization.Controllers {
    [Admin]
    public class CultureAdminController : Controller, IUpdateModel
    {

        private readonly IMainBitCultureService _domainCultureService;
        private readonly INotifier _notifier;

        public CultureAdminController(IOrchardServices services,
            IMainBitCultureService domainCultureService,
            INotifier notifier)
        {
            Services = services;
            _domainCultureService = domainCultureService;
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
                _domainCultureService.Create(record);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var record = _domainCultureService.Get(id);
            var viewModel = CreateViewModel(record);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, MainBitCultureViewModel viewModel)
        {
            var record = _domainCultureService.Get(id);
            if (Validate(viewModel, record))
            {
                UpdateRecord(viewModel, record);
                _domainCultureService.Update(record);
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
            record.Position = viewModel.Position;
            record.DisplayName = viewModel.DisplayName;
            record.IsMain = viewModel.IsMain;
            record.AppDomainSiteRecord_Id = viewModel.AppDomainSiteRecord_Id;
            
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
                viewModel.Position = record.Position;
                viewModel.DisplayName = record.DisplayName;
                viewModel.IsMain = record.IsMain;
                viewModel.AppDomainSiteRecord_Id = record.AppDomainSiteRecord_Id;
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