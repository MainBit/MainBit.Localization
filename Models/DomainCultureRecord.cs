using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Models
{
    public class DomainCultureRecord
    {
        public virtual int Id { get; set; }
        public virtual string Culture { get; set; }
        public virtual string BaseUrl { get; set; }
        public virtual string UrlPrefix { get; set; }
        public virtual int Position { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual bool IsMain { get; set; }
        public virtual int AppDomainSiteRecord_Id { get; set; }
    }
}