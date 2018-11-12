using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trading.Extensions;
using Trading.Models;

namespace Trading.Controllers
{
    public class ImportController : BaseController
    {
        [HttpGet]
        public ActionResult YML()
        {
            return View();
        }

        [HttpPost]
        public ActionResult YML(HttpPostedFileBase FileName, int? ShopID)
        {
            if (FileName == null || FileName.ContentLength == 0)
            {
                ViewBag.Message = "Необходимо загрузить YML файл";
                return View();
            }
            if (!Directory.Exists(Server.MapPath("/Content/files/")))
                Directory.CreateDirectory(Server.MapPath("/Content/files/"));
            var path = Server.MapPath("/Content/files/") + Guid.NewGuid() + ".yml";
            FileName.SaveAs(path);

            var model = new YmlImportModel() { ShopID = ShopID ?? DefShopID, FileName = path };
            var result = model.Import();

            if (result.Message.IsFilled())
            {
                ViewBag.Message = result.Message;
            }
            else
                ViewBag.Message = "Добавлено {0} записей, обновлено {1} записей".FormatWith(result.Added, result.Updated);

            return View();
        }

    }
}
