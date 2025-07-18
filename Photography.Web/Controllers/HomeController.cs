using DataBase;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Photography.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                var request = Request.UserAgent?.ToLower();
				if (!string.IsNullOrEmpty(request))
				{
					if (request.Contains("iphone") || request.Contains("android") || request.Contains("mobile"))
					{
						return RedirectToAction("Work", "Projects");
					}
				}
				var userSession = HttpContext.Session["UserSession"];
                using (var context = new ApplicationDbContext())
                {
					if (userSession == null)
                    {
                        var UserSessionId = Guid.NewGuid();
                        HttpContext.Session["UserSession"] = UserSessionId;
                        var model = new WebVisitCount();
                        model.SessionID = UserSessionId.ToString();
                        model.VisitDateTime = HelperService.Instance.getCurrentIST();
                        context.WebVisitCounts.Add(model);
                        context.SaveChanges();
                    }
					var data = context.HomeBanner.FirstOrDefault(x => x.IsActive == true);
					return View(data);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}