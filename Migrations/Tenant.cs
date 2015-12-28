using MainBit.Localization.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace MainBit.Localization.Migrations
{
    [OrchardFeature("MainBit.Localization.Tenant")]
    public class Tenant : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("TenantLocalizationPartRecord",
               table => table
                   .ContentPartRecord()
                   .Column<int>("MasterContentItemId"));

            ContentDefinitionManager.AlterPartDefinition("TenantLocalizationPart", builder => builder
                .Attachable()
                .WithDescription("Provides the user interface to localize content items from default tenant"));

            return 1;
        }
    }
}