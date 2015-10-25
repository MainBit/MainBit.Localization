using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Modules.Services;
using Orchard.Security;
using Orchard.UI.Navigation;
using System;
using System.Linq;

namespace MainBit.Localization.AdminMenu
{
    [OrchardFeature("MainBit.Localization.CultureSettings")]
    public class CultureSettings : INavigationProvider
    {
        public CultureSettings(IModuleService moduleService)
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            
            builder
                .Add(T("Settings"), menu => menu
                    .Add(T("Localization"), "10.0", menuItem => menuItem
                        .Action("Index", "CultureAdmin", new { area = "MainBit.Localization" }).Permission(StandardPermissions.SiteOwner)
                            .Add(T("List of cultures"), "11.0", submenuItem => submenuItem
                                .Action("Index", "CultureAdmin", new { area = "MainBit.Localization" })
                                .Permission(StandardPermissions.SiteOwner).LocalNav())
                    )
                );
        }
    }
}
