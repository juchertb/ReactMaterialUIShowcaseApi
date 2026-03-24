using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.Threading;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace ReactMaterialUIShowcaseApi.Helpers
{
    public class SetUserCultureAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is ControllerBase controller &&
                controller.HttpContext.RequestServices.GetService(typeof(IUserContextService)) is IUserContextService userContext)
            {
                var culture = userContext.Language switch
                {
                    LanguageEnum.iFrench => "fr",
                    LanguageEnum.iEnglish => "en",
                    _ => "en"
                };
                var cultureInfo = new CultureInfo(culture);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
            }
            base.OnActionExecuting(context);
        }
    }
}