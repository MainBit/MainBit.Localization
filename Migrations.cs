using MainBit.Localization.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace MainBit.Localization
{
    public class Migrations : DataMigrationImpl {

        public int Create() {
            ContentDefinitionManager.AlterPartDefinition(typeof (CulturePickerPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("CulturePickerWidget", cfg => cfg
               .WithPart("CulturePickerPart")
               .WithPart("WidgetPart")
               .WithPart("CommonPart", p => p.WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false"))
               .WithSetting("Stereotype", "Widget"));

            //SchemaBuilder.CreateTable("AppDomainSiteRecord",
            //   table => table
            //       .Column<int>("Id", column => column.PrimaryKey().Identity())
            //       .Column<string>("Title")
            //);

            SchemaBuilder.CreateTable("MainBitCultureRecord",
               table => table
                   .Column<int>("Id", column => column.PrimaryKey().Identity())
                   .Column<string>("Culture", col => col.WithLength(8))
                   .Column<string>("UrlSegment", col => col.WithLength(255))
                   .Column<string>("StoredPrefix", col => col.WithLength(255))
                   .Column<int>("Position")
                   .Column<string>("DisplayName")
                   .Column<bool>("IsMain")
                   .Column<int>("AppDomainSiteRecord_Id")
            );

            return 3;
        }
    }
}