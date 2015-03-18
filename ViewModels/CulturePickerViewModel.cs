using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MainBit.Localization.ViewModels
{
    public class CulturePickerViewModel
    {
        public CulturePickerViewModel()
        {
            Cultures = new List<CultureEntry>();
        }

        public CultureEntry CurrentCulture { get; set; } 
        public List<CultureEntry> Cultures { get; set; }
    }

    public class CultureEntry
    {
        public string Url { get; set; }
        public string DisplayName { get; set; }
        public CultureInfo CultureInfo { get; set; }
    }
}