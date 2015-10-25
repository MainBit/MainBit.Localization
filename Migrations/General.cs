using MainBit.Localization.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace MainBit.Localization.Migrations
{
    public class General : DataMigrationImpl {

        public int Create() {
            ContentDefinitionManager.AlterPartDefinition(typeof(CulturePickerPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("CulturePickerWidget", cfg => cfg
               .WithPart("CulturePickerPart")
               .WithPart("WidgetPart")
               .WithPart("CommonPart", p => p.WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false"))
               .WithSetting("Stereotype", "Widget"));

            return 3;
        }
    }
}