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
               .WithPart("CommonPart")
               .WithSetting("Stereotype", "Widget"));

            return 1;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.CreateTable("DomainLocalizationSettingsPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("Domain")
               );

            SchemaBuilder.CreateTable("DomainLocalizationSettingsItemRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("DomainLocalizationSettingsPartRecord_Id")
                    .Column<string>("Culture")
                    .Column<string>("DisplayName")
                    .Column<int>("Position")
                    .Column<string>("Domain")
                );

            return 2;
        }
    }
}