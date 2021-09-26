using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductShop.Utils
{
    public static class LocalizationHelper
    {
        private readonly static IList<string> _supportedLocalesList = new List<string> {  "uk",  "en"};

        public static IList<string> GetSupportedLocales()
        {
            return _supportedLocalesList;
        }
    }
}
