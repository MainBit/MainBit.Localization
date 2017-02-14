using System.Linq;
using Orchard.DisplayManagement.Descriptors;
using Orchard;
using Orchard.Localization.ViewModels;

namespace MainBit.Layouts.Handlers
{
    public class LocalizationShapes : IShapeTableProvider
    {
        private readonly IWorkContextAccessor _wca;
        public LocalizationShapes(IWorkContextAccessor wca)
        {
            _wca = wca;
        }
        public void Discover(ShapeTableBuilder builder) {

            builder.Describe("EditorTemplate").OnDisplaying(context => {
                
                // shape type is EditorTemplate
                //if (context.ShapeMetadata.Type != "Parts_Localization_ContentTranslations_Edit")
                //{
                //    return;
                //}

                if (context.Shape.TemplateName != "Parts/Localization.ContentTranslations.Edit")
                {
                    return;
                }

                var model = context.Shape.Model as EditLocalizationViewModel;

                var siteCulture = _wca.GetContext().CurrentSite.SiteCulture;
                model.MissingCultures = model.MissingCultures.OrderByDescending(c => c == siteCulture).ToList();
            });
        }
    }
}
