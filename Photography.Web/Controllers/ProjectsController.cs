using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Models;
using Services;

namespace Photography.Web.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: Projects
        [Route("Images")]
        public ActionResult Images()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var data = context.Images.Where(x => x.IsActive == true).ToList();
                    var userSession = HttpContext.Session["Images"];
                    if (userSession == null)
                    {
                        var UserSessionId = Guid.NewGuid();
                        HttpContext.Session["Images"] = UserSessionId;
                        var model = new PageVisitCount();
                        model.SessionID = UserSessionId.ToString();
                        model.VisitPage = "Images";
                        model.VisitDateTime = HelperService.Instance.getCurrentIST();
                        context.PageVisitCounts.Add(model);
                        context.SaveChanges();
                    }
                    return View(data);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Work()
        {
            using(var context = new ApplicationDbContext())
            {
                var data = context.Categories.Where(x => x.IsActive == true).ToList();
                return View(data);
            }
        }
        public ActionResult WorkPartial(int Id)
        {
            using (var context = new ApplicationDbContext())
            {
                var WorkAll = new List<Work>();
                var catName = "All";
                if (Id == 0)
                {
                    WorkAll = context.Work.Where(x => x.isAll == true).OrderByDescending(x=>x.SeqNo).ToList();
                }
                else
                {
                    WorkAll = context.Work.Where(x => x.WorkId == Id).OrderByDescending(x=>x.SeqNo).ToList();
                    var category = context.Categories.FirstOrDefault(x => x.Id == Id);
                    catName = category.Name;
                }
                var userSession = HttpContext.Session[catName];
                if (userSession == null)
                {
                    var UserSessionId = Guid.NewGuid();
                    HttpContext.Session[catName] = UserSessionId;
                    var model = new PageVisitCount();
                    model.SessionID = UserSessionId.ToString();
                    model.VisitPage = catName;
                    model.VisitDateTime = HelperService.Instance.getCurrentIST();
                    context.PageVisitCounts.Add(model);
                    context.SaveChanges();
                }
                return PartialView(WorkAll);
            }
        }
        public ActionResult Narrative(int Id)
        {
            using(var context = new ApplicationDbContext())
            {
                var works = context.Work.Where(x => x.WorkId == Id).OrderBy(x=>x.SeqNo).ToList();
                var category = context.Categories.FirstOrDefault(x => x.Id == Id);
                var userSession = HttpContext.Session[category.Name];
                if (userSession == null)
                {
                    var UserSessionId = Guid.NewGuid();
                    HttpContext.Session[category.Name] = UserSessionId;
                    var model = new PageVisitCount();
                    model.SessionID = UserSessionId.ToString();
                    model.VisitPage = category.Name;
                    model.VisitDateTime = HelperService.Instance.getCurrentIST();
                    context.PageVisitCounts.Add(model);
                    context.SaveChanges();
                }
                return PartialView(works);
            }
        }
    }
}