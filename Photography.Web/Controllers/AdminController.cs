using DataBase;
using DataModels;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace Photography.Web.Controllers
{
    [AdminAuthorizationFilterAttribute]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var model = new AdminIndexModel();
            using(var context = new ApplicationDbContext())
            {
                var WebVisitCount = context.WebVisitCounts.ToList();
                model.WebVisitCount = WebVisitCount.Count();
                var workCat = context.Categories.ToList();
                var visitpagecount = context.PageVisitCounts.ToList();
                var ImagesVisitCount = visitpagecount.Where(x => x.VisitPage == "Images").ToList();
                model.ImagesVisitCount = ImagesVisitCount != null ? ImagesVisitCount.Count().ToString() : "0";
                model.WorkCategory = workCat.Select(x =>
                {
                    x.VisitCount = HelperService.Instance.VisitPageCount(x.Name);
                    return x;
                }).ToList();
                HelperService.Instance.RemoveOldUserVist();
            }
            return View(model);
        }
        #region WorkCategory
        public ActionResult CategoryList()
        {
            using (var context = new ApplicationDbContext())
            {
                var categories = context.Categories.ToList();
                return View(categories);
            }
        }
        [HttpPost]
        public ActionResult CategoryStatus(int catId, bool status)
        {
            var result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categories = context.Categories.FirstOrDefault(x => x.Id == catId);
                    categories.IsActive = status;
                    context.Entry(categories).State = EntityState.Modified;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(result);
        }
        public ActionResult CategoryAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CategoryAdd(Category category)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    category.CreatedOn = HelperService.Instance.getCurrentIST();
                    category.IsActive = true;
                    context.Categories.Add(category);
                    context.SaveChanges();
                    result = "true";
                }
            }
            catch (Exception ex)
            {
                return View(category);
                throw ex;
            }
            return Json(result);
        }
        public ActionResult CategoryEdit(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var category = context.Categories.FirstOrDefault(x => x.Id == id);
                    return View(category);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult CategoryEdit(Category category)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(category).State = EntityState.Modified;
                    context.SaveChanges();
                }
                result = "true";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result);

        }
        #endregion
        #region Work
        public ActionResult WorkList()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var workList = context.Work.ToList();
                    var catList = context.Categories.ToList();
                    workList = workList.Select(x =>
                    {
                        x.Category = catList.FirstOrDefault(z => z.Id == x.WorkId);
                        return x;
                    }).OrderByDescending(x=>x.SeqNo).ToList();
                    return View(workList);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult WorkStatus(int proId, bool status)
        {
            var result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var product = context.Work.FirstOrDefault(x => x.Id == proId);
                    product.IsActive = status;
                    context.Entry(product).State = EntityState.Modified;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(result);
        }
        public ActionResult WorkAdd()
        {
            using (var context = new ApplicationDbContext())
            {
                var catList = new List<SelectListItem>();
                var category = context.Categories.Where(x=>x.IsActive == true).ToList();
                for (int i = 0; i < category.Count; i++)
                {
                    var catData = new SelectListItem() { Text = category[i].Name, Value = category[i].Id.ToString() };
                    catList.Add(catData);
                }
                ViewBag.CatItem = catList;
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WorkAdd(Work work)
        {
            string result = "false";
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    work.CreatedOn = HelperService.Instance.getCurrentIST();
                    work.IsActive = true;
                    context.Work.Add(work);
                    context.SaveChanges();
                    result = "true";
                }
            }
            catch (Exception ex)
            {
                result = "false";
            }
            return Json(result);
        }
        public ActionResult WorkEdit(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var catList = new List<SelectListItem>();
                    var category = context.Categories.Where(x => x.IsActive == true).ToList();
                    for (int i = 0; i < category.Count; i++)
                    {
                        var catData = new SelectListItem() { Text = category[i].Name, Value = category[i].Id.ToString() };
                        catList.Add(catData);
                    }
                    ViewBag.CatItem = catList;
                    var workData = context.Work.FirstOrDefault(x => x.Id == id);
                    return View(workData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WorkEdit(Work work)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(work).State = EntityState.Modified;
                    context.SaveChanges();
                }
                ImageController imgc = new ImageController();
                if (work.oldimg != null && work.oldimg.Length > 1 && work.Image != work.oldimg)
                {
                    imgc.DeleteImg(work.oldimg);
                }
                result = "true";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result);

        }

        #endregion
        #region Image
        public ActionResult ImageList()
        {
            var catList = new List<SelectListItem>();
            var imageType = Enum.GetValues(typeof(Constant.ImageType))
                    .Cast<Constant.ImageType>()
                    .Select(d => (d, (int)d))
                    .ToList();
            for (int i = 0; i < imageType.Count; i++)
            {
                var catData = new SelectListItem() { Text = imageType[i].d.ToString(), Value = imageType[i].Item2.ToString() };
                catList.Add(catData);
            }
            ViewBag.CatItem = catList;
            return View();
        }
        public ActionResult ImageListPartial(DateTime? StartDate, DateTime? EndDate,int? pageNo,int ImageType = -1, int Id=0)
        {
            int page = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            //Change PageSize
            int pagesize = 2;
            var model = new ImagesDataModel();
            using (var context = new ApplicationDbContext())
            {
                var imageList = context.Images.ToList();
                imageList = imageList.Select(x =>
                {
                    x.TypeName = Constant.ImageType.GetName(typeof(Constant.ImageType), Convert.ToInt32(x.Type));
                    return x;
                }).ToList();
                if (Id > 0)
                {
                    imageList = imageList.Where(x => x.Id == Id).ToList();
                }
                if (ImageType >= 0)
                {
                    imageList = imageList.Where(x => x.Type == ImageType.ToString()).ToList();
                }
                if (StartDate.HasValue && StartDate != null)
                {
                    StartDate = Convert.ToDateTime(StartDate.Value.ToString("dd-MM-yyyy"));
                    imageList = imageList.Where(x => x.CreatedOn.Date >= StartDate).ToList();
                }
                if (EndDate.HasValue && EndDate != null)
                {
                    EndDate = Convert.ToDateTime(EndDate.Value.ToString("dd-MM-yyyy"));
                    imageList = imageList.Where(x => x.CreatedOn.Date <= EndDate).ToList();
                }
                //Pagination
                int totalItems = imageList.Count;

                var dataSubset = imageList.Skip((page - 1) * pagesize).Take(pagesize).ToList();
                model.Images = dataSubset;
                model.pageNo = page;
                model.Pager = new Pager(imageList.Count(), pageNo, pagesize);
                return PartialView(model);
            }
        }
        public ActionResult ImageStatus(int proId, bool status)
        {
            var result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var product = context.Images.FirstOrDefault(x => x.Id == proId);
                    product.IsActive = status;
                    context.Entry(product).State = EntityState.Modified;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(result);
        }
        public ActionResult AddImage()
        {
            using (var context = new ApplicationDbContext())
            {
                var catList = new List<SelectListItem>();
                var imageType = Enum.GetValues(typeof(Constant.ImageType))
                        .Cast<Constant.ImageType>()
                        .Select(d => (d, (int)d))
                        .ToList();
                for (int i = 0; i < imageType.Count; i++)
                {
                    var catData = new SelectListItem() { Text = imageType[i].d.ToString(), Value = imageType[i].Item2.ToString() };
                    catList.Add(catData);
                }
                ViewBag.CatItem = catList;
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddImage(Images images)
        {
            try
            {
                string result;
                try
                {
                    using (var context = new ApplicationDbContext())
                    {
                        images.CreatedOn = HelperService.Instance.getCurrentIST();
                        images.IsActive = true;
                        context.Images.Add(images);
                        context.SaveChanges();
                        result = "true";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return Json(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult EditImage(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var catList = new List<SelectListItem>();
                    var imageType = Enum.GetValues(typeof(Constant.ImageType))
                            .Cast<Constant.ImageType>()
                            .Select(d => (d, (int)d))
                            .ToList();
                    for (int i = 0; i < imageType.Count; i++)
                    {
                        var catData = new SelectListItem() { Text = imageType[i].d.ToString(), Value = imageType[i].Item2.ToString() };
                        catList.Add(catData);
                    }
                    ViewBag.CatItem = catList;
                    var imageData = context.Images.FirstOrDefault(x => x.Id == id);
                    return View(imageData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult EditImage(Images images)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(images).State = EntityState.Modified;
                    context.SaveChanges();
                }
                ImageController imgc = new ImageController();
                if (images.oldimg.Length > 0 && images.Image != images.oldimg)
                {
                    imgc.DeleteImg(images.oldimg);
                }
                result = "true";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result);

        }
        #endregion
        #region Key
        public ActionResult WebsiteInformation()
        {
            using (var context = new ApplicationDbContext())
            {
                var keys = context.Keys.ToList();
                return View(keys);
            }
        }
        public ActionResult InfoAdd()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult InfoAdd(Key Key)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    Key.CreatedOn = HelperService.Instance.getCurrentIST();
                    context.Keys.Add(Key);
                    context.SaveChanges();
                    result = "true";
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult InfoEdit(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var key = context.Keys.FirstOrDefault(x => x.Id == id);
                    return PartialView(key);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult InfoEdit(Key key)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(key).State = EntityState.Modified;
                    context.SaveChanges();
                    result = "true";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result);
        }

        public bool InfoDelete(int id)
        {
            bool result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var delete = context.Keys.Find(id);
                    context.Keys.Remove(delete);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }
        #endregion
        #region Banner
        public ActionResult BannerList()
        {
            try
            {
                using(var context = new ApplicationDbContext())
                {
                    var data = context.HomeBanner.ToList();
                    return View(data);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult BannerStatus(int banId, bool status)
        {
            var result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categories = context.HomeBanner.FirstOrDefault(x => x.Id == banId);
                    categories.IsActive = status;
                    context.Entry(categories).State = EntityState.Modified;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(result);
        }
        public ActionResult AddBanner()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult AddBanner(HomeBanner homeBanner)
        {
            var result = "false";
            try
            {
                using(var context = new ApplicationDbContext())
                {
                    homeBanner.CreatedOn = HelperService.Instance.getCurrentIST();
                    homeBanner.IsActive = true;
                    context.HomeBanner.Add(homeBanner);
                    context.SaveChanges();
                    result = "true";
                }
                return Json(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult EditBanner(int Id)
        {
            using(var context = new ApplicationDbContext())
            {
                var data = context.HomeBanner.FirstOrDefault(x=>x.Id == Id);
                return View(data);
            }
        }
        [HttpPost]
        public ActionResult EditBanner(HomeBanner homeBanner)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                context.Entry(homeBanner).State = EntityState.Modified;
                context.SaveChanges();
                result = "true";
            }
            return Json(result);
        }
        [HttpPost]
        public ActionResult DeleteBanner(int Id)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                var data = context.HomeBanner.FirstOrDefault(x=>x.Id == Id);
                context.HomeBanner.Remove(data);
                context.SaveChanges();
                result = "true";
            }
            return Json(result);
        }
        #endregion
        #region Charts
        public ActionResult pieChart()
        {
            var result = new List<WebCharts>();
            var WebCount = new List<WVCountPerDay>();
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var visit = context.WebVisitCounts.ToList();
                    WebCount = visit.Select(x => new WVCountPerDay()
                    {
                        day = x.VisitDateTime.ToString("d ddd"),
                        Count = visit.Where(z => z.VisitDateTime.Date == x.VisitDateTime.Date).ToList().Count()
                    }).ToList();
                    result = WebCount.Select(x => new WebCharts()
                    {
                        Key = x.day.ToString(),
                        Value = x.Count
                    }).DistinctBy(x => x.Key).OrderByDescending(x => x.Value).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult doughnut()
        {
            var result = new List<WebCharts>();
            var imagesCount = new List<ImagesCount>();
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var pageVisitCounts = context.PageVisitCounts.ToList();
                    imagesCount = pageVisitCounts.Select(x => new ImagesCount()
                    {
                        day = x.VisitDateTime.ToString("d ddd"),
                        Count = pageVisitCounts.Where(z => z.VisitDateTime.Date == x.VisitDateTime.Date).ToList().Count()
                    }).ToList();
                    result = imagesCount.Select(x => new WebCharts()
                    {
                        Key = x.day.ToString(),
                        Value = x.Count
                    }).DistinctBy(x => x.Key).OrderByDescending(x => x.Value).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult barChart(string First, string Second,string name)
        {
            DateTime first = First != null && First != string.Empty ? Convert.ToDateTime(First) : DateTime.Now.AddDays(-7);
            DateTime second = Second != null && Second != string.Empty ? Convert.ToDateTime(Second).AddDays(1) : DateTime.Now;
            var result = new List<WebCharts>();
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var pageVisit = context.PageVisitCounts.Where(x => x.VisitDateTime < second && x.VisitDateTime >= first && x.VisitPage == name).ToList();
                    result = pageVisit.Select(x => new WebCharts()
                    {
                        Key = x.VisitDateTime.Date.ToString("MMM-dd"),
                        Value = pageVisit.Where(y => y.VisitDateTime.Date == x.VisitDateTime.Date).Count()
                    }).DistinctBy(x => x.Key).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}