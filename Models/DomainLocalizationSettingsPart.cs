using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MainBit.Localization.Models
{
    public class DomainLocalizationSettingsPart : ContentPart
    {
        private readonly LazyField<IList<DomainCultureRecord>> _cultures = new LazyField<IList<DomainCultureRecord>>();
        public LazyField<IList<DomainCultureRecord>> CulturesField { get { return _cultures; } }
        public IList<DomainCultureRecord> Cultures
        {
            get { return _cultures.Value; }
            set { _cultures.Value = value; }
        }


        


        //public int MainCultureId
        //{
        //    get { return this.Retrieve(p => p.MainCultureId); }
        //    set { this.Store(p => p.MainCultureId, value); }
        //}

        //public int[] SiteCultureIds {
        //    get { return DecodeSiteCultureIds(this.Retrieve<string>("SiteCultureIds")); }
        //    set { this.Store("SiteCultureIds", EncodeSiteCultureIds(value)); }
        //}


        //private static readonly char[] separator = new [] {'{', '}', ','};
        //private string EncodeSiteCultureIds(ICollection<int> ids) {
        //    if (ids == null || !ids.Any()) {
        //        return string.Empty;
        //    }

        //    // use {1},{2} format so it can be filtered with delimiters
        //    return "{" + string.Join("},{", ids.ToArray()) + "}";
        //}
        //private int[] DecodeSiteCultureIds(string ids) {
        //    if(String.IsNullOrWhiteSpace(ids)) {
        //        return new int[0];
        //    }

        //    return ids.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        //}
    }
}