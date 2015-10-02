using MainBit.Localization.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace MainBit.Localization.Drivers
{
    [OrchardFeature("MainBit.Localization.MainSite")]
    public class MainBitLocalizationPartDriver : ContentPartDriver<MainBitLocalizationPart>
    {
        /// <summary>
        /// Always implement Prefix to avoid potential model binding naming collisions when another part uses the same property names.
        /// </summary>
        protected override string Prefix
        {
            get { return "MainBitLocalizationPart"; }
        }

        protected override DriverResult Editor(MainBitLocalizationPart part, dynamic shapeHelper)
        {
            return Editor(part, null, shapeHelper);
        }

        protected override DriverResult Editor(MainBitLocalizationPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var mainbitLocalizationItemRecord = part.Items.Count > 0 ? part.Items[0] : new MainBitLocalizationItemRecord();
            if (updater != null)
            {
                // We are in "postback" mode, so update our part
                // now only one localizde item
                if (updater.TryUpdateModel(mainbitLocalizationItemRecord, Prefix, null, null))
                {
                    if (mainbitLocalizationItemRecord.LocalizedContentItemId > 0)
                    {
                        if (part.Items.Count == 0)
                        {
                            part.Items.Add(mainbitLocalizationItemRecord);
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
            return ContentShape("Parts_MainBitLocalizationPart_Edit", () =>
                shapeHelper.EditorTemplate(TemplateName: "Parts/MainBitLocalizationPart", Model: mainbitLocalizationItemRecord, Prefix: Prefix));
        }
    }
}