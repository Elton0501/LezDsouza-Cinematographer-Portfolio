using DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Photography.Web.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        [HttpPost]
        public JsonResult UploadImage()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                var file = Request.Files[0];
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/CategoryImages/"), fileName);
                file.SaveAs(path);
                result.Data = new { Success = true, ImageURL = string.Format("/Content/CategoryImages/{0}", fileName) };

            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = ex.Message };
            }
            return result;
        }

        public JsonResult MultipleUploadImageDefault()
        {
            List<ImageModel> model = new List<ImageModel>();
            try
            {
                var file = Request.Files;
                for (int i = 0; i < file.Count; i++)
                {
                    var files = file[i];
                    var fileName = Guid.NewGuid() + Path.GetExtension(files.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/ProductImages/"), fileName);
                    files.SaveAs(path);
                    var imgurl = string.Format("/Content/ProductImages/" + fileName);
                    model.Add(new ImageModel { Image = imgurl });
                }
                var data = JsonConvert.SerializeObject(model);
                return Json(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteImg(string img)
        {
            try
            {
                if (img.Length > 0)
                {
                    string removeimagepath = System.Web.Hosting.HostingEnvironment.MapPath(img);
                    if (System.IO.File.Exists(removeimagepath))
                    {
                        System.IO.File.Delete(removeimagepath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}