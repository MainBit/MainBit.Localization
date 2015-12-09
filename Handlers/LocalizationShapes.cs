using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Descriptors.ResourceBindingStrategy;
using Orchard.Environment;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Settings;
using Orchard.UI;
using Orchard.UI.Resources;
using Orchard.UI.Zones;
using Orchard.Utility.Extensions;
using Orchard;
using Orchard.Environment.Extensions;
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
                if (context.ShapeMetadata.Type == "Parts_Localization_ContentTranslations_Edit")
                    return;

                var model = context.Shape.Model as EditLocalizationViewModel;
                var siteCulture = _wca.GetContext().CurrentSite.SiteCulture;
                model.MissingCultures = model.MissingCultures.OrderByDescending(c => c == siteCulture).ToList();
            });
        }


        /// <summary>
        /// Encodes dashed, dots and spaces so that they don't conflict in filenames 
        /// </summary>
        /// <param name="alternateElement"></param>
        /// <returns></returns>
        private string EncodeAlternateElement(string alternateElement)
        {
            return alternateElement.Replace(" ", "__").Replace("-", "__").Replace(".", "_");
        }
    }
}
