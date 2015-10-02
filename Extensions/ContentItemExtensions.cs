using MainBit.Localization.Services;
using Orchard;
using Orchard.ContentManagement;
using System;
using System.Web.Mvc;

//namespace MainBit.Localization.Extensions
namespace MainBit.Localization.Extensions
{
    public static class ContentItemExtensions
    {
        public static string ItemDisplayUrl(this UrlHelper urlHelper, IContent content)
        {
            var workContext = urlHelper.RequestContext.GetWorkContext();
            var mainbitLocalizationService = workContext.Resolve<IMainBitLocalizationService>();
            return mainbitLocalizationService.ItemDisplayUrl(content);
        }
    }
}