using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trading.Extensions;
using Trading.Extensions.Helpers;
using Trading.Models;

namespace Trading.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {

        [Authorize(Roles = "ShopOwner,Manager")]
        public ActionResult Chars(int? ShopID)
        {
            var chars = new List<ShopProductChar>();
            if (CurrentUser.UserRoles.Contains("ShopOwner"))
            {
                chars = ShopID.HasValue
                    ? DB.ShopProductChars.Where(x => x.ShopID == ShopID.Value && x.UserID == CurrentUser.ID).ToList()
                    : DB.ShopProductChars.Where(x => !x.ShopID.HasValue && x.UserID == CurrentUser.ID).ToList();
            }
            else if(CurrentUser.UserRoles.Contains("Manager"))
            {
                if (!CurrentUser.ManagerProfiles.First().ShopManagers.Any())
                    return View(chars);
                if (!ShopID.HasValue)
                    ShopID = CurrentUser.ManagerProfiles.First().ShopManagers.First().ShopID;

                chars =
                    DB.ShopProductChars.Where(
                        x => x.ShopID == ShopID.Value && x.UserID == CurrentUser.ManagerProfiles.First().ShopOwnerID)
                        .ToList();

            }
            return View(chars);
        }

        [Authorize(Roles = "ShopOwner,Manager")]
        [HttpGet]
        public ActionResult EditChar(int? ShopID, int? ID)
        {
            if (ID.HasValue)
            {
                var ch = DB.ShopProductChars.FirstOrDefault(x => x.ID == ID && x.UserID == CurrentUser.ShopOwnerID);
                if (ch != null)
                    return View(ch);
                else return RedirectToAction("Chars");
            }
            else
            {
                var ch = new ShopProductChar() { ShopID = ShopID };
                return View(ch);
            }
        }

        [Authorize(Roles = "ShopOwner,Manager")]
        [HttpPost]
        public ActionResult EditChar(ShopProductChar model)
        {
            model.IsPost = true;
            if (model.Name.IsNullOrEmpty())
                return View(model);
            if (model.ID == 0)
            {
                model.UserID = CurrentUser.ShopOwnerID;
                DB.ShopProductChars.InsertOnSubmit(model);
                Logger.WriteEvent(
                    model.ShopID.HasValue ? Logger.EventType.ShopCharAdd : Logger.EventType.ShopCharCommonAdd,
                    "Добавление характеристики " + model.Name +
                    (model.ShopID.HasValue ? (" для магазина " + DB.Shops.First(x => x.ID == model.ShopID).Name) : ""));

            }
            else
            {
                var ch = DB.ShopProductChars.FirstOrDefault(x => x.ID == model.ID && x.UserID == CurrentUser.ShopOwnerID);
                if (ch != null)
                    ch.LoadPossibleProperties(model, new[]{"ID", "UserID"});
                Logger.WriteEvent(
                    model.ShopID.HasValue ? Logger.EventType.ShopCharEdit : Logger.EventType.ShopCharCommonEdit,
                    "Редактирование характеристики " + model.Name +
                    (model.ShopID.HasValue ? (" для магазина " + DB.Shops.First(x => x.ID == model.ShopID).Name) : ""));

            }
            DB.SubmitChanges();
            return RedirectToAction("Chars", new { ShopID = model.ShopID });
        }

        [Authorize(Roles = "ShopOwner,Manager")]
        public ActionResult DeleteChar(int id)
        {
            
            var ch = DB.ShopProductChars.FirstOrDefault(x => x.ID == id && x.UserID == CurrentUser.ShopOwnerID);
            if (ch != null)
            {
                int? shid = ch.ShopID;

                Logger.WriteEvent(
                    ch.ShopID.HasValue ? Logger.EventType.ShopCharDelete : Logger.EventType.ShopCharCommonDelete,
                    "Удаление характеристики " + ch.Name +
                    (ch.ShopID.HasValue ? (" для магазина " + DB.Shops.First(x => x.ID == ch.ShopID).Name) : ""));


                DB.ShopProductChars.DeleteOnSubmit(ch);
                DB.SubmitChanges();

                return RedirectToAction("Chars", new { ShopID = shid });
            }
            else
            {
                return RedirectToAction("Chars");
            }
        }
      

        private List<ShopSetting> CharListDef = new List<ShopSetting>()
            {
                new ShopSetting(){Name = "Наименование юр. лица", ItemKey = "OrgName"},
                new ShopSetting(){Name = "ИНН", ItemKey = "INN"},
                new ShopSetting(){Name = "Адрес", ItemKey = "Address"},
                new ShopSetting(){Name = "Тел./факс", ItemKey = "Phones"},
                new ShopSetting(){Name = "Р/с", ItemKey = "OrgAccount"},
                new ShopSetting(){Name = "Банк", ItemKey = "Bank"},
                new ShopSetting(){Name = "БИК", ItemKey = "Bik"},
                new ShopSetting(){Name = "К/с", ItemKey = "Korr"},
                new ShopSetting(){Name = "Код ОКПО", ItemKey = "OKPO"},
            };
        [HttpGet]
        [Authorize(Roles = "ShopOwner")]
        public ActionResult EditSettings(int? ShopID)
        {
            ViewBag.NoShops = !CurrentUser.ShopList.Any();

            var charListDef = new ShopSetting[CharListDef.Count];
            CharListDef.CopyTo(charListDef);
            var settings = DB.ShopSettings.Where(x => x.ShopID == (ShopID ?? DefShopID)).ToList();
            foreach (var shopSetting in charListDef)
            {
                var s = settings.FirstOrDefault(x => x.ItemKey == shopSetting.ItemKey);
                if (s != null)
                {
                    shopSetting.Value = s.Value;
                }
            }
            return View(charListDef);
        }     
        
        [HttpPost]
        [Authorize(Roles = "ShopOwner")]
        public ActionResult EditSettings(int? ShopID, FormCollection collection)
        {
            var list = new List<ShopSetting>();
            foreach (var shopSetting in CharListDef)
            {
                var settings =
                    DB.ShopSettings.FirstOrDefault(
                        x => x.ShopID == (ShopID ?? DefShopID) && x.ItemKey == shopSetting.ItemKey);
                if (settings == null)
                {
                    settings = new ShopSetting()
                    {
                        ItemKey = shopSetting.ItemKey,
                        Name = shopSetting.Name,
                        ShopID = (ShopID ?? DefShopID)
                    };
                    DB.ShopSettings.InsertOnSubmit(settings);
                }
                settings.Value = (string)collection.GetValue(shopSetting.ItemKey).ConvertTo(typeof(string));
                list.Add(settings);
            }
            DB.SubmitChanges();
            ViewBag.Message = "Данные успешно сохранены";
            return View(list);
        }
    }
}
