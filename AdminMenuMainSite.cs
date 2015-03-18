using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Modules.Services;
using Orchard.Security;
using Orchard.UI.Navigation;
using System;
using System.Linq;

namespace MainBit.Localization
{
    [OrchardFeature("MainBit.Localization.MainSite")]
    public class AdminMenuMainSite : INavigationProvider
    {
        private readonly IModuleService _moduleService;

        public AdminMenuMainSite(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            
            builder
                .Add(T("Settings"), menu => menu
                    .Add(T("Domain Localization"), "10.0",
                        menuItem => menuItem
                            .Action("List", "CultureAdmin", new { area = "MainBit.Localization" })
                            .Permission(StandardPermissions.SiteOwner)

                            .Add(T("Culture List"), "11.0", submenuItem => submenuItem
                                .Action("List", "CultureAdmin", new { area = "MainBit.Localization" })
                                .Permission(StandardPermissions.SiteOwner).LocalNav())
                            .Add(T("Create / Edit"), "12.0", submenuItem => submenuItem
                                .Action("Create", "CultureAdmin", new { area = "MainBit.Localization" })
                                .Permission(StandardPermissions.SiteOwner).LocalNav())
                    )
                );
        }
    }
}
