using MainBit.Localization.Models;
using MainBit.Localization.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using System;
using System.Linq;

namespace MainBit.Localization.Drivers
{
    public class DomainLocalizationSettingsPartDriver : ContentPartDriver<DomainLocalizationSettingsPart> {

        private readonly IOrchardServices _orchardServices;

        public DomainLocalizationSettingsPartDriver(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
        }
        /// <summary>
        /// Always implement Prefix to avoid potential model binding naming collisions when another part uses the same property names.
        /// </summary>
        protected override string Prefix
        {
            get { return "DomainLocalizationSettings"; }
        }

        protected override DriverResult Editor(DomainLocalizationSettingsPart part, dynamic shapeHelper)
        {
            //return ContentShape("Parts_DomainLocalizationSettingsPart_Edit", () =>
            //    shapeHelper.EditorTemplate(TemplateName: "Parts/DomainLocalizationSettingsPart", Model: part, Prefix: Prefix))
            //    // Assign our editor to the "DomainLocalization" group
            //    .OnGroup("Domain Localization");
            return null;
        }

        protected override DriverResult Editor(DomainLocalizationSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            //if (updater.TryUpdateModel(part, Prefix, null, null))
            //{
                
            //}

            return Editor(part, shapeHelper);
        }
    }
}