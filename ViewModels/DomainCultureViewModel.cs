using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.ViewModels
{
    public class DomainCultureViewModel
    {
        public int Id { get; set; }
        public IEnumerable<string> AvailableSystemCultures { get; set; }
        public string SystemCulture { get; set; }

        public string Culture { get; set; }
        public string BaseUrl { get; set; }
        public string UrlPrefix { get; set; }
        public int Position { get; set; }
        public string DisplayName { get; set; }
        public bool IsMain { get; set; }
        public int AppDomainSiteRecord_Id { get; set; }
        
    }
}