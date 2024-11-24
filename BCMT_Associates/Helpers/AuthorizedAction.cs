using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using BCMT_Associates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCMT_Associates
{
    public class AuthorizedAction: ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.Session.GetString("email") == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "User" }, { "action", "Login" } });
                return;
            }

            var menus = JsonConvert.DeserializeObject<List<RoleMenu>>(filterContext.HttpContext.Session.GetString("menus"));
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];
            string url = "/" + controllerName + "/" + actionName;
            var sleect = menus.Where(s => s.Url == url).Select(x=>x).FirstOrDefault();
            if (!menus.Where(s => s.Url == url).Any())

            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "User" }, { "action", "Login" } });
                return;
            }
        }
    }
}
