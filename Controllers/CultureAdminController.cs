using System;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using Orchard.ContentManagement;
using MainBit.Localization.Models;
using Orchard.Data;
using Orchard.UI.Admin;

namespace MainBit.Localization.Controllers {
    [Admin]
    public class CultureAdminController : Controller {

        private readonly IRepository<DomainLocalizationSettingsItemRecord> _settingsRepository;
        public CultureAdminController(IOrchardServices services,
            IRepository<DomainLocalizationSettingsItemRecord> settingsRepository)
        {
            Services = services;
            _settingsRepository = settingsRepository;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult List() {
            var settings = Services.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            return View(settings.Items);
        }

        public ActionResult Create()
        {
            var model = new DomainLocalizationSettingsItemRecord();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DomainLocalizationSettingsItemRecord model)
        {
            model.DomainLocalizationSettingsPartRecord_Id = Services.WorkContext.CurrentSite.Id;
            _settingsRepository.Create(model);
            return RedirectToAction("List");
        }

        public ActionResult Edit(int id)
        {
            var model = _settingsRepository.Get(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DomainLocalizationSettingsItemRecord model)
        {
            _settingsRepository.Update(model);
            return RedirectToAction("List");
        }
    }
}