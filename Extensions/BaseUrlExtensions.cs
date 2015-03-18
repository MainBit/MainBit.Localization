using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Extensions
{
    public static class BaseUrlExtensions
    {
        public static string GetBaseUrl(this HttpRequest request)
        {
            return (request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath).TrimEnd('/');
        }
    }
}