using MainBit.Localization.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace MainBit.Localization.Migrations
{
    //[OrchardFeature("MainBit.Localization.MainSite")]
    //public class MainSiteMigrations : DataMigrationImpl
    //{

    //    public int Create() {

            

    //        SchemaBuilder.CreateTable("MainBitLocalizationPartRecord", table => table
    //            .ContentPartRecord());

    //        ContentDefinitionManager.AlterPartDefinition("MainBitLocalizationPart", builder => 
    //            builder.Attachable());

    //        SchemaBuilder.CreateTable("MainBitLocalizationItemRecord",
    //            table => table
    //                .Column<int>("Id", column => column.PrimaryKey().Identity())
    //                .Column<int>("MainBitLocalizationPartRecord_Id")
    //                .Column<int>("MainBitLocalizationSettingsItemRecord_Id")
    //                .Column<int>("LocalizedContentItemId")
    //            );

    //        return 1;
    //    }
    //}
}