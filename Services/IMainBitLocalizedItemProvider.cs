using Orchard;
using Orchard.ContentManagement;

namespace MainBit.Localization.Services
{
    public interface IMainBitLocalizedItemProvider : IDependency
    {
        string GetUrl(IContent item, string cultureName);
    }
}