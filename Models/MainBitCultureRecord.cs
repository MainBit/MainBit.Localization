﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Models
{
    public class MainBitCultureRecord
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