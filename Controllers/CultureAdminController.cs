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

        private readonly IDomainCultureService _domainCultureService;
        private readonly INotifier _notifier;

        public CultureAdminController(IOrchardServices services,
            IDomainCultureService domainCultureService,
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

        public ActionResult GetSettings()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSettings(string baseUrl)
        {
            return GetSettings();
        }


        public ActionResult List() {
            var settings = Services.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            return View(settings.Cultures);
        }

        public ActionResult Create()
        {
            var viewModel = CreateViewModel(null);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(DomainCultureViewModel viewModel)
        {
            if (Validate(viewModel, null))
            {
                var record = new DomainCultureRecord();
                UpdateRecord(viewModel, record);
                _domainCultureService.Create(record);
            }
           
            return RedirectToAction("List");
        }

        public ActionResult Edit(int id)
        {
            var record = _domainCultureService.Get(id);
            var viewModel = CreateViewModel(record);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, DomainCultureViewModel viewModel)
        {
            var record = _domainCultureService.Get(id);
            if (Validate(viewModel, record))
            {
                UpdateRecord(viewModel, record);
                _domainCultureService.Update(record);
            }

            return RedirectToAction("List");
        }

        private bool Validate(DomainCultureViewModel viewModel, DomainCultureRecord record)
        {
            viewModel.BaseUrl = viewModel.BaseUrl.TrimEnd('/');

            // ensure the base url is absolute if provided
            if (!String.IsNullOrWhiteSpace(viewModel.BaseUrl))
            {
                var previousBaseUrl = record != null ? record.BaseUrl : "";

                if (!Uri.IsWellFormedUriString(viewModel.BaseUrl, UriKind.Absolute))
                {
                    AddModelError("Base Url", T("The Base Url must be absolute."));
                    return false;
                }
                // if the base url has been modified, try to ping it
                else if (!String.Equals(previousBaseUrl, viewModel.BaseUrl, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var request = WebRequest.Create(viewModel.BaseUrl) as HttpWebRequest;
                        if (request != null)
                        {
                            using (request.GetResponse() as HttpWebResponse) { }
                        }
                    }
                    catch (Exception e)
                    {
                        _notifier.Warning(T("The base url you entered could not be requested from current location."));
                        Logger.Warning(e, "Could not query base url: {0}", viewModel.BaseUrl);
                    }
                }
            }

            return true;
        }
        private void UpdateRecord(DomainCultureViewModel viewModel, DomainCultureRecord record)
        {
            record.Culture = string.IsNullOrWhiteSpace(viewModel.Culture) ? viewModel.SystemCulture : viewModel.Culture;
            record.BaseUrl = viewModel.BaseUrl;
            record.UrlPrefix = viewModel.UrlPrefix;
            record.Position = viewModel.Position;
            record.DisplayName = viewModel.DisplayName;
            record.IsMain = viewModel.IsMain;
            record.AppDomainSiteRecord_Id = viewModel.AppDomainSiteRecord_Id;
            
        }
        private DomainCultureViewModel CreateViewModel(DomainCultureRecord record)
        {
            var settings = Services.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            var viewModel = new DomainCultureViewModel();

            viewModel.AvailableSystemCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(ci => ci.Name)
                .Where(s => !settings.Cultures.Any(c => c.Culture == s));

            if(record != null) {
                viewModel.Id = record.Id;
                viewModel.Culture = record.Culture;
                viewModel.BaseUrl = record.BaseUrl;
                viewModel.UrlPrefix = record.UrlPrefix;
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