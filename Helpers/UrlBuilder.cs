using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MainBit.Localization.Helpers
{
    public static class UrlBuilder
    {
        public static string Combine(params string[] urlParts)
        {
            var url = new StringBuilder();

            foreach (var urlPart in urlParts)
            {
                if (string.IsNullOrEmpty(urlPart)) { continue; }

                if (url.Length == 0)
                {
                    url.Append(urlPart);
                }
                else
                {
                    if (url[url.Length - 1] != '/')
                    {
                        url.Append('/');
                    }
                    url.Append(urlPart.TrimStart('/'));
                }
            }

            return url.ToString();
        }
    }
}