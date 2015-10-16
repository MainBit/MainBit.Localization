using Orchard;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MainBit.Localization.Models;
using System.Text.RegularExpressions;
using MainBit.Alias.Services;
using Orchard.Mvc;
using System.Globalization;
using MainBit.Localization.Providers;

namespace MainBit.Localization.Services
{
    public interface IDomainCultureHelper : IDependency
    {
        MainBitCultureRecord GetMainCulture();
        MainBitCultureRecord GetMainCulture(MainBitLocalizationSettingsPart settings);
        MainBitCultureRecord GetCurrentCulture();
        MainBitCultureRecord GetCurrentCulture(MainBitLocalizationSettingsPart settings);
        MainBitCultureRecord GetCultureByName(string cultureName);
        MainBitCultureRecord GetCultureByName(MainBitLocalizationSettingsPart settings, string cultureName);
        MainBitCultureRecord GetCultureByBaseUrl(string cultureName);
        MainBitCultureRecord GetCultureByBaseUrl(MainBitLocalizationSettingsPart settings, string cultureName);
    }

    public class DomainCultureHelper : IDomainCultureHelper
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IUrlService _urlService;

        public DomainCultureHelper(IOrchardServices orchardServices,
            IUrlService urlService)
        {
            _orchardServices = orchardServices;
            _urlService = urlService;
        }

        public MainBitCultureRecord GetMainCulture()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            return GetMainCulture(settings);
        }
        public MainBitCultureRecord GetMainCulture(MainBitLocalizationSettingsPart settings)
        {
            return settings.Cultures.FirstOrDefault(c => c.IsMain);
        }

        public MainBitCultureRecord GetCurrentCulture()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            return GetCurrentCulture(settings);
        }
        public MainBitCultureRecord GetCurrentCulture(MainBitLocalizationSettingsPart settings)
        {
            var currentCulture = _orchardServices.WorkContext.CurrentCulture;
            return settings.Cultures.FirstOrDefault(c => string.Equals(c.Culture, currentCulture, StringComparison.InvariantCultureIgnoreCase));
        }

        public MainBitCultureRecord GetCultureByName(string cultureName)
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            return GetCultureByName(settings, cultureName);
        }
        public MainBitCultureRecord GetCultureByName(MainBitLocalizationSettingsPart settings, string cultureName)
        {
            return settings.Cultures.FirstOrDefault(c => string.Equals(cultureName, c.Culture, StringComparison.InvariantCultureIgnoreCase));
        }

        public MainBitCultureRecord GetCultureByBaseUrl(string baseUrl)
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<MainBitLocalizationSettingsPart>();
            return GetCultureByBaseUrl(settings, baseUrl);
        }
        public MainBitCultureRecord GetCultureByBaseUrl(MainBitLocalizationSettingsPart settings, string baseUrl)
        {
            var urlContext = _urlService.GetContext(baseUrl);
            if (urlContext == null) { return null; }

            var culture = settings.Cultures.FirstOrDefault(c =>
                c.UrlSegment == urlContext.Descriptor.Segments[CultureUrlSegmentProvider.Name].Value);
            return culture;
        }
    }
}