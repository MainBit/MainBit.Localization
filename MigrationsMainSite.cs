using MainBit.Localization.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace MainBit.Localization
{
    [OrchardFeature("MainBit.Localization.MainSite")]
    public class MigrationsMainSite : DataMigrationImpl {

        public int Create() {

            SchemaBuilder.CreateTable("DomainLocalizationPartRecord", table => table
                .ContentPartRecord());

            ContentDefinitionManager.AlterPartDefinition("DomainLocalizationPart", builder => 
                builder.Attachable());

            SchemaBuilder.CreateTable("DomainLocalizationItemRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("DomainLocalizationPartRecord_Id")
                    .Column<int>("DomainLocalizationSettingsItemRecord_Id")
                    .Column<int>("LocalizedContentItemId")
                );

            return 1;
        }
    }
}