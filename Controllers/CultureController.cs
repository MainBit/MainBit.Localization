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
using Orchard.Environment.Extensions;

namespace MainBit.Localization.Controllers {

    [OrchardFeature("MainBit.Localization.MultiTenancy")]
    public class CultureController : Controller {
        private readonly IMainBitLocalizationService _mainbitLocalizationService;
        
        public CultureController(IOrchardServices services,
            IMainBitLocalizationService mainbitLocalizationService)
        {
            Services = services;
            _mainbitLocalizationService = mainbitLocalizationService;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult MainItem(int id, string from, string to) {
            var destUrl = Services.WorkContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority); //_mainbitLocalizationService.GetDestUrl(id, from, to);
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