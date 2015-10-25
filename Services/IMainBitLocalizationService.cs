using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MainBit.Localization.Models;
using Orchard;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Localization.Services;
using Orchard.Mvc.Html;
using System.Web.Mvc;
using Orchard.Localization.Models;
using MainBit.Localization.Helpers;
using MainBit.Alias.Services;
using Orchard.Mvc;
using MainBit.Localization.Providers;
using Orchard.Mvc.Extensions;

namespace MainBit.Localization.Services
{
    public interface IMainBitLocalizationService : IDependency {
        string GetUrl(IContent item, string cultureName);
    }

    public class MainBitLocalizationService : IMainBitLocalizationService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IRepository<MainBitLocalizationItemRecord> _itemRepository;
        private readonly ILocalizationService _localizationService;
        private readonly UrlHelper _urlHelper;
        private readonly IUrlService _urlService;
        private readonly IWorkContextAccessor _wca;
        private readonly IMainBitLocalizationSettingsService _mainBitLocalizationSettingsService;
        private readonly IList<IMainBitLocalizedItemProvider> _mainBitLocalizedItemProviders;

        public MainBitLocalizationService(
            IList<IMainBitLocalizedItemProvider> mainBitLocalizedItemProviders)
        {
            _mainBitLocalizedItemProviders = mainBitLocalizedItemProviders;
        }

        public string GetUrl(IContent item, string cultureName)
        {
            foreach (var mainBitLocalizedItemProvider in _mainBitLocalizedItemProviders)
            {
                var url = mainBitLocalizedItemProvider.GetUrl(item, cultureName);
                if (url != null) return url;
            }
            return null;
        }

    }
}