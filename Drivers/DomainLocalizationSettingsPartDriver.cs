using MainBit.Localization.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace MainBit.Localization.Drivers
{
    public class DomainLocalizationSettingsPartDriver : ContentPartDriver<DomainLocalizationSettingsPart> {
        /// <summary>
        /// Always implement Prefix to avoid potential model binding naming collisions when another part uses the same property names.
        /// </summary>
        protected override string Prefix
        {
            get { return "DomainLocalizationSettings"; }
        }

        protected override DriverResult Editor(DomainLocalizationSettingsPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(DomainLocalizationSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater != null)
            {
                // We are in "postback" mode, so update our part
                if (updater.TryUpdateModel(part, Prefix, null, null))
                {
                }
            }
            else
            {
                // We are in render mode (not postback), so initialize our view model.
            }

            // Return the EditorTemplate shape, configured with proper values.
            return ContentShape("Parts_DomainLocalizationSettingsPart_Edit", () =>
                shapeHelper.EditorTemplate(TemplateName: "Parts/DomainLocalizationSettingsPart", Model: part, Prefix: Prefix))

                // Assign our editor to the "DomainLocalization" group
                .OnGroup("DomainLocalization");
        }
    }
}