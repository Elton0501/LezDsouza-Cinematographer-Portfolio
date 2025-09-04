using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Photography.Web.Controllers
{
	//public class AdminAuthorizationFilterAttribute : FilterAttribute, IAuthorizationFilter
	//{
	//    public void OnAuthorization(AuthorizationContext filterContext)
	//    {
	//        if (filterContext.HttpContext.Session["Admin"] == null)
	//        {
	//            filterContext.Result = new RedirectResult("/login");
	//        }
	//    }
	//}
	public class AdminAuthorizationFilterAttribute : FilterAttribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			var adminCookie = filterContext.HttpContext.Request.Cookies["AdminEmail"];

			if (adminCookie == null || string.IsNullOrEmpty(adminCookie.Value))
			{
				// Redirect to login if no cookie found
				filterContext.Result = new RedirectResult("/login");
			}
		}
	}

}