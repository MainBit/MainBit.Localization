using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MainBit.Localization.Models;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Localization.Services;
using Orchard.Environment;
using Orchard;

namespace MainBit.Localization.Services
{
    public class DomainLocalizationService : IDomainLocalizationService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IRepository<DomainLocalizationItemRecord> _itemRepository;

        public DomainLocalizationService(IOrchardServices orchardServices,
            IRepository<DomainLocalizationItemRecord> itemRepository)
        {
            _orchardServices = orchardServices;
            _itemRepository = itemRepository;
        }

        public string GetUrl(int sourseContentItemId, string destCulture)
        {
            var currentUrl = _orchardServices.WorkContext.HttpContext.Request.Url;
            var currentDomainUrl = currentUrl.Scheme + System.Uri.SchemeDelimiter + currentUrl.Host 
                + (currentUrl.IsDefaultPort ? "" : ":" + currentUrl.Port);

            var settings = _orchardServices.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();

            if (destCulture.ToLower() == _orchardServices.WorkContext.CurrentSite.SiteCulture.ToLower())
            {
                return string.Empty;
            }

            if (currentDomainUrl == settings.Domain)
            {
                return GetDestUrl(sourseContentItemId, _orchardServices.WorkContext.CurrentSite.SiteCulture.ToLower(), destCulture);
            }
            else
            {
                return string.Format("{0}/{1}?id={2}&culture={3}&toculture={4}",
                    settings.Domain,
                    MainBit.Localization.Routes.MainLocalizationUrl,
                    sourseContentItemId,
                    _orchardServices.WorkContext.CurrentSite.SiteCulture.ToLower(),
                    destCulture.ToLower());
            }
        }

        public string GetDestUrl(int sourceContentItemId, string sourceCulture, string destCulture)
        {
            var settings = _orchardServices.WorkContext.CurrentSite.As<DomainLocalizationSettingsPart>();
            var sourceCultureSettings = settings.Items.FirstOrDefault(p => p.Culture.ToLower() == sourceCulture.ToLower());
            var destCultureSettings = settings.Items.FirstOrDefault(p => p.Culture.ToLower() == destCulture.ToLower());

            var destContentItenmId = 0;

            if (settings.Domain == sourceCultureSettings.Domain)
            {
                var destLocalizedItem = _itemRepository.Fetch(p => p.DomainLocalizationPartRecord_Id == sourceContentItemId
                    && p.DomainLocalizationSettingsItemRecord_Id == destCultureSettings.Id).FirstOrDefault();
                destContentItenmId = destLocalizedItem == null ? 0 : destLocalizedItem.LocalizedContentItemId;
            }
            else if (settings.Domain == destCultureSettings.Domain)
            {
                var sourseLocalizedItem = _itemRepository.Fetch(p => p.LocalizedContentItemId == sourceContentItemId
                   && p.DomainLocalizationSettingsItemRecord_Id == sourceCultureSettings.Id).FirstOrDefault();
                destContentItenmId = sourseLocalizedItem == null ? 0 : sourseLocalizedItem.DomainLocalizationPartRecord_Id;
            }
            else 
            {
                var sourseLocalizedItem = _itemRepository.Fetch(p => p.LocalizedContentItemId == sourceContentItemId
                    && p.DomainLocalizationSettingsItemRecord_Id == sourceCultureSettings.Id).FirstOrDefault();
                if (sourseLocalizedItem == null)
                {
                    destContentItenmId = 0;
                }
                else
                {
                    var destLocalizedItem = _itemRepository.Fetch(p => p.DomainLocalizationPartRecord_Id == sourseLocalizedItem.DomainLocalizationPartRecord_Id
                        && p.DomainLocalizationSettingsItemRecord_Id == destCultureSettings.Id).FirstOrDefault();
                    destContentItenmId = destLocalizedItem == null ? 0 : destLocalizedItem.LocalizedContentItemId;
                }
                
            }

            if (destContentItenmId == 0)
            {
                return destCultureSettings.Domain;
            }
            else
            {
                return string.Format("{0}/{1}?id={2}",
                        destCultureSettings.Domain,
                        MainBit.Localization.Routes.LocalizationUrl,
                        destContentItenmId);
            }
        }
    }
}