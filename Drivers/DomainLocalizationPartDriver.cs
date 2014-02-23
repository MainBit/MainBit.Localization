using MainBit.Localization.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace MainBit.Localization.Drivers
{
    [OrchardFeature("MainBit.Localization.MainSite")]
    public class DomainLocalizationPartDriver : ContentPartDriver<DomainLocalizationPart>
    {
        /// <summary>
        /// Always implement Prefix to avoid potential model binding naming collisions when another part uses the same property names.
        /// </summary>
        protected override string Prefix
        {
            get { return "DomainLocalizationPart"; }
        }

        protected override DriverResult Editor(DomainLocalizationPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(DomainLocalizationPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var domainLocalizationItemRecord = part.Items.Count > 0 ? part.Items[0] : new DomainLocalizationItemRecord();
            if (updater != null)
            {
                // We are in "postback" mode, so update our part
                // now only one localizde item
                if (updater.TryUpdateModel(domainLocalizationItemRecord, Prefix, null, null))
                {
                    if (domainLocalizationItemRecord.LocalizedContentItemId > 0)
                    {
                        if (part.Items.Count == 0)
                        {
                            part.Items.Add(domainLocalizationItemRecord);
                        }
                    }
                    else
                    {
                        part.Items.Clear();
                    }
                }
            }
            else
            {
                // We are in render mode (not postback), so initialize our view model.
            }

            // Return the EditorTemplate shape, configured with proper values.
            return ContentShape("Parts_DomainLocalizationPart_Edit", () =>
                shapeHelper.EditorTemplate(TemplateName: "Parts/DomainLocalizationPart", Model: domainLocalizationItemRecord, Prefix: Prefix));
        }
    }
}