using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MainBit.Localization.Models
{
    public class MainBitLocalizationSettingsPart : ContentPart
    {
        private readonly LazyField<IList<MainBitCultureRecord>> _cultures = new LazyField<IList<MainBitCultureRecord>>();
        public LazyField<IList<MainBitCultureRecord>> CulturesField { get { return _cultures; } }
        public IList<MainBitCultureRecord> Cultures
        {
            get { return _cultures.Value; }
            set { _cultures.Value = value; }
        }
    }

    public class MainBitLocalizationSettings
    {
        private readonly IEnumerable<MainBitCultureRecord> _cultures;

        public MainBitLocalizationSettings(IEnumerable<MainBitCultureRecord> cultures)
        {
            _cultures = cultures;
        }

        public IEnumerable<MainBitCultureRecord> Cultures
        { 
            get { return _cultures; }
        }
    }

    public static class MainBitLocalizationSettingsPartExtensions
    {
        public static MainBitLocalizationSettings ToSettings(this MainBitLocalizationSettingsPart settingsPart)
        {
            var settings = new MainBitLocalizationSettings(settingsPart.Cultures);
            return settings;
        }
    }
}