using System;
using System.Collections.Generic;
using System.Globalization;
using MainBit.Localization.Models;
using Orchard;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;

namespace MainBit.Localization.Services
{
    public interface IDomainLocalizationService : IDependency {
        string GetUrl(int sourseContentItemId, string destCulture);
        string GetDestUrl(int sourceContentItemId, string sourceCulture, string destCulture);
    }
}