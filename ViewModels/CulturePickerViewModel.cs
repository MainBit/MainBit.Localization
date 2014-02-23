using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MainBit.Localization.ViewModels
{
    public class CulturePickerViewModel
    {
        public string Url { get; set; }
        public string DisplayName;
        public CultureInfo CultureInfo;
    }
}