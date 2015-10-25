using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Migrations
{
    [OrchardFeature("MainBit.Localization.CultureSettings")]
    public class CultureSettings : DataMigrationImpl
    {
        public int Create()
        {
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

            return 1;
        }
    }
}