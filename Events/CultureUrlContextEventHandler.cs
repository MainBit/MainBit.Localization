using MainBit.Alias.Events;
using Orchard.ContentManagement;
using Orchard.Localization.Models;
using Orchard.Localization.Services;
using System.Web.Mvc;
using Orchard.Mvc.Html;

namespace MainBit.Localization.Events
{
    public class CultureUrlContextEventHandler : IUrlContextEventHandler
    {
        private readonly ILocalizationService _localizationService;
        private readonly UrlHelper _urlHelper;

        public CultureUrlContextEventHandler(ILocalizationService localizationService,
            UrlHelper urlHelper)
        {
            _localizationService = localizationService;
            _urlHelper = urlHelper;
        }

        public void Changing(ChangingUrlContext context)
        {
            if (context.Content == null) return;

            foreach (var segment in context.ChangingSegments)
            {
                if (segment.UrlSegmentDescriptor.Name == "culture")
                {
                    var localizationPart = context.Content.As<LocalizationPart>();
                    if (localizationPart == null)
                    {
                        context.NewDisplayVirtualPath = "/";
                        return;
                    }

                    var toLocalizationPart = _localizationService.GetLocalizedContentItem(context.Content, segment.NewValue.Name);
                    if (toLocalizationPart == null)
                    {
                        context.NewDisplayVirtualPath = "/";
                        return;
                    }

                    context.NewDisplayVirtualPath = _urlHelper.ItemDisplayUrl(toLocalizationPart).Substring(1);
                }
            }
        }
    }
}