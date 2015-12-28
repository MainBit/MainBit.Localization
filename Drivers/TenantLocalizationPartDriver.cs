using MainBit.Localization.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace MainBit.Localization.Drivers
{
    [OrchardFeature("MainBit.Localization.Tenant")]
    public class TenantLocalizationPartDriver : ContentPartDriver<TenantLocalizationPart>
    {
        /// <summary>
        /// Always implement Prefix to avoid potential model binding naming collisions when another part uses the same property names.
        /// </summary>
        protected override string Prefix
        {
            get { return "TenantLocalizationPart"; }
        }

        protected override DriverResult Editor(TenantLocalizationPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_TenantLocalizationPart_Edit", () =>
                shapeHelper.EditorTemplate(TemplateName: "Parts/TenantLocalizationPart", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(TenantLocalizationPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}