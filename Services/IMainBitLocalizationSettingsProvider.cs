using MainBit.Localization.Models;
using Orchard;

namespace MainBit.Localization.Services
{
    public interface IMainBitLocalizationSettingsProvider : IDependency
    {
        int Priority { get; }
        MainBitLocalizationSettings GetSettings();
    }
}