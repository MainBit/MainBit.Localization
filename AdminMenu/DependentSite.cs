using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace MainBit.Localization.AdminMenu
{
    [OrchardFeature("MainBit.Localization.DependentSite")]
    public class DependentSite : INavigationProvider
    {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {

            builder
                .Add(T("Settings"), menu => menu
                    .Add(T("Domain Localization"), "10.0",
                        menuItem => menuItem
                            .Action("GetSettings", "CultureAdmin", new { area = "MainBit.Localization" })
                            .Permission(StandardPermissions.SiteOwner)

                            .Add(T("Get Settings"), "10.0", submenuItem => submenuItem
                                .Action("GetSettings", "CultureAdmin", new { area = "MainBit.Localization" })
                                .Permission(StandardPermissions.SiteOwner).LocalNav())
                            .Add(T("Culture List"), "11.0", submenuItem => submenuItem
                                .Action("List", "CultureAdmin", new { area = "MainBit.Localization" })
                                .Permission(StandardPermissions.SiteOwner).LocalNav())
                            .Add(T("View"), "12.0", submenuItem => submenuItem
                                .Action("Create", "CultureAdmin", new { area = "MainBit.Localization" })
                                .Permission(StandardPermissions.SiteOwner).LocalNav())
                    )
                );
        }
    }
}
