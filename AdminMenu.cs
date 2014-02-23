using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;

namespace Orchard.Warmup {
    public class AdminMenu : INavigationProvider {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder
                .Add(T("Settings"), menu => menu
                    .Add(T("DomainLocalization"), "10.0", subMenu => subMenu.Action("Index", "Admin", new { area = "Settings", groupInfoId = "DomainLocalization" }).Permission(StandardPermissions.SiteOwner)
                        .Add(T("Main Settings"), "10.0", item => item.Action("Index", "Admin", new { area = "Settings", groupInfoId = "DomainLocalization" }).Permission(StandardPermissions.SiteOwner).LocalNav())
                        .Add(T("Culture List"), "11.0", item => item.Action("List", "CultureAdmin", new { area = "MainBit.Localization" }).Permission(StandardPermissions.SiteOwner).LocalNav())
                        .Add(T("Create / Edit"), "12.0", item => item.Action("Create", "CultureAdmin", new { area = "MainBit.Localization" }).Permission(StandardPermissions.SiteOwner).LocalNav())
                    ));
        }
    }
}
