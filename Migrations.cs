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

            SchemaBuilder.CreateTable("DomainCultureRecord",
               table => table
                   .Column<int>("Id", column => column.PrimaryKey().Identity())
                   .Column<string>("Culture")
                   .Column<string>("BaseUrl")
                   .Column<string>("AllowedBaseUrl")
                   .Column<string>("UrlPrefix")
                   .Column<int>("Position")
                   .Column<string>("DisplayName")
                   .Column<bool>("IsMain")
                   .Column<int>("AppDomainSiteRecord_Id")
            );

            return 1;
        }

        public int UpdateFrom1()
        {
            

            return 2;
        }

        public int UpdateFrom2()
        {

            return 3;
        }
    }
}