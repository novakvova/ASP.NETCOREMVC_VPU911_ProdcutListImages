using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductShop.ActionFilters
{
    public class InternationalizationAttribute : ActionFilterAttribute
    {
        private readonly IList<string> _supportedLocales;
        private readonly string _defaultLang;
        public InternationalizationAttribute()
        {

            // Get supported locales list
            _supportedLocales = Utils.LocalizationHelper.GetSupportedLocales();

            // Set default locale
            _defaultLang = _supportedLocales[0];
        }
        /// <summary>
        /// Apply locale to current thread
        /// </summary>
        /// <param name="lang">locale name</param>
        private void SetLang(string lang)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Get locale from route values
            string lang = (string)filterContext.RouteData.Values["lang"] ?? _defaultLang;

            // If we haven't found appropriate culture - seet default locale then
            if (!_supportedLocales.Contains(lang))
                lang = _defaultLang;

            filterContext.HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            string currentLang = Thread.CurrentThread.CurrentCulture.Name;
            SetLang(lang);
            if (currentLang != lang)
            {
                var request = filterContext.HttpContext.Request;
                var returnUrl = string.IsNullOrEmpty(request.Path) ? "~/" : $"~{request.Path.Value}";
                filterContext.Result = new RedirectResult(returnUrl); 
                return;
            }
        }
        
    }
}
