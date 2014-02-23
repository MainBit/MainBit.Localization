using System;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using Orchard.ContentManagement;
using MainBit.Localization.Models;
using System.Linq;
using MainBit.Localization.Services;
using Orchard.Mvc.Html;

namespace MainBit.Localization.Controllers {
    public class CultureController : Controller {
        private readonly IDomainLocalizationService _domainLocalizationService;
        
        public CultureController(IOrchardServices services,
            IDomainLocalizationService domainLocalizationService)
        {
            Services = services;
            _domainLocalizationService = domainLocalizationService;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult MainItem(int id, string culture, string toCulture) {
            var destUrl = _domainLocalizationService.GetDestUrl(id, culture, toCulture);
            return RedirectPermanent(destUrl);

        }

        public ActionResult Item(int id)
        {
            var contentItem = Services.ContentManager.Get(id);
            var helper = new UrlHelper(Services.WorkContext.HttpContext.Request.RequestContext);
            var url = helper.ItemDisplayUrl(contentItem);
            return RedirectPermanent(url);
        }
    }
}