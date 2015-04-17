using Orchard;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MainBit.Localization.Models;
using System.Text.RegularExpressions;

namespace MainBit.Localization.Services
{
    public interface IDomainCultureHelper : IDependency
    {
        DomainCultureRecord GetMainCulture();
        DomainCultureRecord GetMainCulture(DomainLocalizationSettingsPart settings);
        DomainCultureRecord GetCurrentCulture();
        DomainCultureRecord GetCurrentCulture(DomainLocalizationSettingsPart settings);
        DomainCultureRecord GetCultureByName(string cultureName);
        DomainCultureRecord GetCultureByName(DomainLocalizationSettingsPart settings, string cultureName);
        DomainCultureRecord GetCultureByBaseUrl(string cultureName);
        DomainCultureRecord GetCultureByBaseUrl(DomainLocalizationSettingsPart settings, string cultureName);
        bool IsAllowedBaseUrl(DomainCultureRecord culture, string baseUrl);
    }

    public class DomainCultureHelper : IDomainCultureHelper
    {
        private readonly IOrchardServices _orchardServices;

        public DomainCultureHelper(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }

        public DomainCultureRecord GetMainCulture()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            return GetMainCulture(settings);
        }
        public DomainCultureRecord GetMainCulture(DomainLocalizationSettingsPart settings)
        {
            return settings.Cultures.FirstOrDefault(c => c.IsMain);
        }

        public DomainCultureRecord GetCurrentCulture()
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            return GetCurrentCulture(settings);
        }
        public DomainCultureRecord GetCurrentCulture(DomainLocalizationSettingsPart settings)
        {
            var currentCulture = _orchardServices.WorkContext.CurrentCulture;
            return settings.Cultures.FirstOrDefault(c => string.Equals(c.Culture, currentCulture, StringComparison.InvariantCultureIgnoreCase));
        }

        public DomainCultureRecord GetCultureByName(string cultureName)
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            return GetCultureByName(settings, cultureName);
        }
        public DomainCultureRecord GetCultureByName(DomainLocalizationSettingsPart settings, string cultureName)
        {
            return settings.Cultures.FirstOrDefault(c => string.Equals(cultureName, c.Culture, StringComparison.InvariantCultureIgnoreCase));
        }

        public DomainCultureRecord GetCultureByBaseUrl(string baseUrl)
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            return GetCultureByBaseUrl(settings, baseUrl);
        }
        public DomainCultureRecord GetCultureByBaseUrl(DomainLocalizationSettingsPart settings, string baseUrl)
        {
            DomainCultureRecord currentCulture = null;

            foreach (var culture in settings.Cultures)
            {
                if (string.Equals(culture.BaseUrl, baseUrl, StringComparison.InvariantCultureIgnoreCase))
                {
                    currentCulture = culture;
                    break;
                }

                if (IsAllowedBaseUrl(culture, baseUrl))
                {
                    currentCulture = culture;
                    break;
                }
            }

            return currentCulture;
        }

        public bool IsAllowedBaseUrl(DomainCultureRecord culture, string baseUrl)
        {
            if (!string.IsNullOrEmpty(culture.AllowedBaseUrl))
            {
                var regex = new Regex(culture.AllowedBaseUrl);
                if (regex.IsMatch(baseUrl))
                {
                    return true;
                }
            }

            return false;
        }
    }
}