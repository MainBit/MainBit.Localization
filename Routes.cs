using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using MainBit.Localization.Models;

namespace MainBit.Localization { 
    public class Routes : IRouteProvider {
        public static string MainLocalizationUrl = "mainbit/main-localization";
        public static string LocalizationUrl = "mainbit/localization";

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {
                                                     Priority = 9999,
                                                     Route = new Route(
                                                         MainLocalizationUrl,
                                                         new RouteValueDictionary {
                                                                                      {"area", "MainBit.Localization"},
                                                                                      {"controller", "Culture"},
                                                                                      {"action", "MainItem"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "MainBit.Localization"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                             new RouteDescriptor {
                                                     Priority = 9999,
                                                     Route = new Route(
                                                         LocalizationUrl,
                                                         new RouteValueDictionary {
                                                                                      {"area", "MainBit.Localization"},
                                                                                      {"controller", "Culture"},
                                                                                      {"action", "Item"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "MainBit.Localization"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                         };
        }
    }
}