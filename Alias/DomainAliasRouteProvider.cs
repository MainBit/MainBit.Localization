using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orchard.Alias.Implementation;
using Orchard.Alias.Implementation.Holder;
using Orchard.Environment.ShellBuilders.Models;
using Orchard.Mvc.Routes;
using Orchard;
using MainBit.Localization.Services;

namespace MainBit.Localization.Alias
{
    public class DomainAliasRouteProvider : IRouteProvider {
        private readonly ShellBlueprint _blueprint;
        private readonly IAliasHolder _aliasHolder;
        private readonly IWorkContextAccessor _wca;
        private readonly IDomainCultureHelper _domainCultureHelper;

        public DomainAliasRouteProvider(IWorkContextAccessor wca,
            IDomainCultureHelper domainCultureHelper,
            ShellBlueprint blueprint,
            IAliasHolder aliasHolder)
        {
            _wca = wca;
            _domainCultureHelper = domainCultureHelper;
            _blueprint = blueprint;
            _aliasHolder = aliasHolder;
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (RouteDescriptor routeDescriptor in GetRoutes()) {
                routes.Add(routeDescriptor);
            }
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            var distinctAreaNames = _blueprint.Controllers
                .Select(controllerBlueprint => controllerBlueprint.AreaName)
                .Distinct();

            return distinctAreaNames.Select(areaName =>
                new RouteDescriptor {
                    Priority = 100,
                    Route = new DomainAliasRoute(_wca, _domainCultureHelper, _aliasHolder, areaName, new MvcRouteHandler())
                }).ToList();
        }
    }
}