using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MainBit.Localization.Models
{
    public class MainBitLocalizationSettings
    {
        private readonly IEnumerable<MainBitCulture> _cultures;

        public MainBitLocalizationSettings(IEnumerable<MainBitCulture> cultures)
        {
            _cultures = cultures;
        }

        public IEnumerable<MainBitCulture> Cultures
        { 
            get { return _cultures; }
        }
    }

    public class MainBitCulture
    {
        public virtual int Id { get; set; }
        public virtual string Culture { get; set; }
        public virtual string UrlSegment { get; set; }
        public virtual string StoredPrefix { get; set; }
        public virtual int Position { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual bool IsMain { get; set; }
    }
}