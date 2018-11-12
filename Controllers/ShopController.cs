using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.XPath;
using NPOI.SS.Formula.Functions;
using Trading.Extensions;
using Trading.Extensions.Helpers;
using Trading.Models;

namespace Trading.Controllers
{
    public class ShopController : BaseController
    {
        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult RegionSearch(string query)
        {
            var arts =
                DB.MapRegions.Where(x => SqlMethods.Like(x.Name.ToLower(), "{0}%".FormatWith(query.Trim().ToLower())) && x.MapCountry.Name == "Россия")
                    .ToList();
            return Json(arts.Select(x => new { label = /*x.MapCountry.Name + ", " +*/ x.Name, value = x.Name }), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult CitySearch(string query, string region)
        {
            var regions =
                DB.MapRegions.Where(
                    x =>
                        x.Name.ToLower() == region.ToLower() &&
                        x.MapCountry.Name == "Россия").Select(x => x.ID)
                    .ToList();

            var cityes =
                DB.MapCities.Where(x => SqlMethods.Like(x.Name.ToLower(), "{0}%".FormatWith(query.Trim().ToLower())) && x.MapCountry.Name == "Россия" && (!regions.Any() || regions.Contains(x.RegionID)))
                    .ToList();
            return
                Json(
                    cityes.Select(
                        x => new { label = x.MapRegion.Name + ", " + x.Name, value = x.Name }),
                    JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult ArtSearch(string query)
        {
            var arts = DB.Products.Where(x => x.OwnerID == CurrentUser.ShopOwnerID && SqlMethods.Like(x.Article, "{0}%".FormatWith(query.Trim()))).ToList();
            return Json(arts.Select(x => new { label = x.Article + " - " + x.Name, value = x.ID }), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult NameSearch(string query)
        {
            var arts = DB.Products.Where(x => x.OwnerID == CurrentUser.ShopOwnerID && SqlMethods.Like(x.Name, "{0}%".FormatWith(query.Trim()))).ToList();
            return Json(arts.Select(x => new { label = x.Article + " - " + x.Name, value = x.ID }), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult UserSearch(string query)
        {
            var arts =
                DB.Consumers.Where(
                    x =>
                        x.Orders.Any(z => z.Shop.Owner == CurrentUser.ShopOwnerID) &&
                        SqlMethods.Like(x.Surname, "{0}%".FormatWith(query.Trim()))).ToList();
            return Json(arts.Select(x => new { label = x.FullName, value = x.ID }), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult UserSearchByPhone(string query)
        {
            var rx = new Regex("\\D");
            query = rx.Replace(HttpUtility.UrlDecode(query), "");
            if (query.Length < 5)
                return Json(null, JsonRequestBehavior.AllowGet);
            var arts =
                DB.Consumers.Where(
                    x =>
                        x.Orders.Any(z => z.Shop.Owner == CurrentUser.ShopOwnerID) &&
                        SqlMethods.Like(x.Phone, "{0}%".FormatWith(query.Trim())) ||
                        SqlMethods.Like(x.AddPhone, "{0}%".FormatWith(query.Trim()))).ToList();

            return Json(arts.Select(x => new { label = x.Phone + " - " + x.FullName, value = x.ID }), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult UserSearchByEmail(string query)
        {
            var arts =
                DB.Consumers.Where(
                    x =>
                        x.Orders.Any(z => z.Shop.Owner == CurrentUser.ShopOwnerID) &&
                        SqlMethods.Like(x.Email, "{0}%".FormatWith(query.Trim()))).ToList();
            return Json(arts.Select(x => new { label = x.Email + " - " + x.FullName, value = x.ID }), JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult ProductInfo(int id, int shop)
        {
            var p = DB.Products.First(x => x.ID == id);
            /*var allowed = new List<string>();*/
            /*
                        if (p.OrderedProducts.Any())
                        {
                            if (p.OrderedProducts.First()
                                .Order.Shop != null)
                            {
                                allowed =
                                    p.OrderedProducts.First()
                                        .Order.Shop.ShopProductChars.Where(x => x.LoadInForm)
                                        .Select(x => x.Name)
                                        .ToList()
                                        .Concat(
                                            DB.ShopProductChars.Where(x => !x.ShopID.HasValue && x.UserID == CurrentUser.ShopOwnerID && x.LoadInForm)
                                                .Select(x => x.Name).ToList())
                                        .Distinct()
                                        .ToList();
                            }
                        }
            */

            var charList = new List<object>();
            if (p.OrderedProducts.Any())
            {
                var chars =
                    p.OrderedProducts.SelectMany(c => c
                        .ProductChars/*.Where(x => allowed.Contains(x.Name))*/
                        .Select(x => new
                        {
                            Name = x.Name,
                            Value = x.Value,
                            Exist = DB.ShopProductChars.Any(
                                z =>
                                    (!z.ShopID.HasValue || z.ShopID == shop) && z.UserID == CurrentUser.ShopOwnerID &&
                                    z.Name.ToLower() == x.Name.ToLower())
                        }))
                        .Where(x => x.Value.IsFilled()).OrderBy(x => x.Name)
                        .ToList();

                var names = new List<string>();
                foreach (var ch in chars)
                {
                    if (!names.Contains(ch.Name))
                    {
                        names.Add(ch.Name);
                        charList.Add(ch);
                    }
                }
            }
            return
                Json(
                    new
                    {
                        p.Article,
                        p.Name,
                        Price = (p.Price.HasValue && p.Price.Value > 0) ? p.Price.Value.ToString() : (p.OrderedProducts.Any() ? p.OrderedProducts.First().Price.ToString() : ""),
                        p.UnitCode,
                        SelfCost = p.SelfCost,
                        OptCost = p.OptCost,
                        Chars = charList
                    }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult UserInfo(int id)
        {
            var p = DB.Consumers.First(x => x.ID == id);
            var a = p.Orders.Any(x => x.OrderDelivery != null)
                ? p.Orders.First(x => x.OrderDelivery != null).OrderDelivery
                : new DeliveryAddress();
            return
                Json(
                    new
                    {
                        p.Surname,
                        p.Name,
                        p.Patrinomic,
                        p.Phone,
                        CustomerID = p.ID,
                        p.AddPhone,
                        p.Email,
                        Sex = p.Sex ? "True" : "False",
                        DeliveryAddress = new
                        {
                            a.Region,
                            a.Town,
                            a.Street,
                            a.House,
                            a.Building,
                            a.Doorway,
                            a.Code,
                            a.Floor,
                            a.Flat,
                            a.Section
                        },
                    },
                    JsonRequestBehavior.AllowGet);
        }

        public ActionResult MarkDialog(int ID)
        {
            var order = DB.Orders.FirstOrDefault(x => x.ID == ID);
            if (order == null || order.Marks.Any())
            {
                order = new Order();
            }
            return PartialView(order);
        }


        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult ChangeStatus(int Status, string List)
        {
            var helper = new PermissionHelper();
            var ids = List.Split<int>(";").ToList();
            var orders = DB.Orders.Where(x => ids.Contains(x.ID)).ToList();
            foreach (var order in orders)
            {
                if (Status == -10 && helper.HasPermission(Permissions.OrderDelete))
                {
                    order.Status = -10;
                }
                else if (helper.HasPermission(Permissions.OrderStatusChange) && Status != 50 && Status != -50)
                {
                    if (Status == 100 && order.DeliveryDate.HasValue ||
                        (Status != 100 && order.Status != -1 && order.Status != -10))
                    {
                        order.Status = Status;
                    }
                }
                else if (helper.HasPermission(Permissions.OrderStatusPayed) && (Status == 50 || Status == -50))
                {
                    order.IsPayed = Status == 50;
                }

            }
            DB.SubmitChanges();
            return new ContentResult();
        }

        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult SendContractor(int ID, string List)
        {
            var helper = new PermissionHelper();
            var ids = List.Split<int>(";").ToList();
            var orders = DB.Orders.Where(x => ids.Contains(x.ID)).ToList();
            foreach (var order in orders)
            {

                var contract =
                    DB.ContractedOrders.FirstOrDefault(
                        x => x.SenderID == CurrentUser.ShopOwnerID && x.OrderID == order.ID);

                if (ID > 0)
                {
                    if (contract != null)
                    {
                        contract.ContractorID = ID;
                    }
                    else
                    {
                        contract = new ContractedOrder()
                        {
                            OrderID = order.ID,
                            SenderID = CurrentUser.ShopOwnerID,
                            ContractorID = ID
                        };
                        DB.ContractedOrders.InsertOnSubmit(contract);
                    }
                }
                else
                {
                    if (contract != null)
                        DB.ContractedOrders.DeleteOnSubmit(contract);
                }
            }
            DB.SubmitChanges();
            return new ContentResult();
        }
        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult ChangeWarning(int Status, string List)
        {
            var helper = new PermissionHelper();
            var ids = List.Split<int>(";").ToList();
            var orders = DB.Orders.Where(x => ids.Contains(x.ID)).ToList();
            foreach (var order in orders)
            {
                if (helper.HasPermission(Permissions.OrderEdit))
                {
                    order.IsImportant = Status == 1;
                }
            }
            DB.SubmitChanges();
            return new ContentResult();
        }


        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult Index()
        {

            var shops = CurrentUser.UserRoles.Contains("ShopOwner") ? DB.Shops.Where(x => x.Owner == CurrentUser.ID) : CurrentUser.ManagerProfiles.First().ShopManagers.Select(x => x.Shop);
            return View(shops);
        }




        [HttpGet]
        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult Edit(int? ID)
        {
            ViewBag.Taxes = DB.TaxPlanes.ToList();
            Shop shop;
            if (ID.HasValue)
            {
                shop = DB.Shops.FirstOrDefault(x => x.ID == ID);
            }
            else
            {
                shop = new Shop();
            }
            return View(shop);
        }

        [HttpPost]
        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult Edit(Shop shop, FormCollection collection)
        {

            shop.IsPost = true;
            ViewBag.Taxes = DB.TaxPlanes.ToList();
            if (shop.Name.IsNullOrEmpty() || shop.Link.IsNullOrEmpty() || !shop.TaxPlanID.HasValue)
                return View(shop);
            if (shop.ID == 0)
            {
                shop.Owner = CurrentUser.ShopOwnerID;
                shop.CreateDate = DateTime.Now;
                DB.Shops.InsertOnSubmit(shop);
                Logger.WriteEvent(Logger.EventType.ShopAdd, "Добавление магазина " + shop.Name);
            }
            else
            {
                var s = DB.Shops.FirstOrDefault(x => x.ID == shop.ID);
                s.LoadPossibleProperties(shop);
                Logger.WriteEvent(Logger.EventType.ShopAdd, "Редактироание магазина " + shop.Name);
            }
            DB.SubmitChanges();

            var mids =
                collection.AllKeys.Where(x => x.StartsWith("Manager"))
                    .Select(x => x.Split<string>("_"))
                    .Where(x => x.Count() == 2)
                    .Select(x => x.ElementAt(1).ToInt()).ToList();

            var forDel = DB.ShopManagers.Where(x => x.ShopID == shop.ID && !mids.Contains(x.ManagerID));
            DB.ShopManagers.DeleteAllOnSubmit(forDel);
            DB.SubmitChanges();

            foreach (var mid in mids)
            {
                var m = DB.ShopManagers.FirstOrDefault(x => x.ShopID == shop.ID && x.ManagerID == mid);
                if (m == null)
                {
                    DB.ShopManagers.InsertOnSubmit(new ShopManager() { ManagerID = mid, ShopID = shop.ID });
                }
            }
            DB.SubmitChanges();


            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult Delete(int ID)
        {
            var shop = DB.Shops.First(x => x.ID == ID);

            Logger.WriteEvent(Logger.EventType.ShopDelete, "Удаление магазина '" + shop.Name + "' из системы ");
            DB.Shops.DeleteOnSubmit(shop);
            DB.SubmitChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ShopOwner, Manager")]
        [HttpGet]
        public ActionResult CreateOrder(int? ID, int? ShopID)
        {
            Order order = null;
            bool noShops = false;
            if (CurrentUser.UserRoles.Contains("ShopOwner"))
            {
                noShops = !DB.Shops.Any(x => x.Owner == CurrentUser.ID);
            }
            else if (CurrentUser.UserRoles.Contains("Manager"))
            {
                noShops = !DB.ShopManagers.Any(x => x.Manager.ManagerUserID == CurrentUser.ID);
            }
            else
            {
                noShops = true;
            }
            if (noShops)
            {
                ViewBag.NoShops = true;
            }
            else
            {
                if (ID.HasValue)
                {
                    order = DB.Orders.FirstOrDefault(x => x.ID == ID);
                    if (order != null)
                    {
                        if (ShopID.HasValue)
                        {
                            order.ShopID = ShopID;
                        }

                        ViewBag.ShopID = order.ShopID;
                    }
                    if (ViewBag.ShopID == null)
                    {
                        ViewBag.ShopID = Request.QueryString["ShopID"].IsNullOrEmpty()
                            ? (DefShopID)
                            : Request.QueryString["ShopID"].ToInt();

                    }

                }
                else
                {
                    order = new Order()
                    {
                        IsTemp = true,
                        AddedByID = CurrentUser.ID,
                        CreateDate = DateTime.Now,
                        Status = -1,
                        IsPayed = false
                    };

                    order.ShopID = ViewBag.ShopID = Request.QueryString["ShopID"].IsNullOrEmpty()
                        ? (DefShopID)
                        : Request.QueryString["ShopID"].ToInt();

                    if (ShopID.HasValue)
                    {
                        order.ShopID = ShopID;
                    }

                    DB.Orders.InsertOnSubmit(order);
                    DB.SubmitChanges();


                    return RedirectToAction("CreateOrder", new { ID = order.ID });
                }
            }
            return View(order);
        }

        [HttpGet]
        public ActionResult ViewOrder(int ID)
        {
            Order order = DB.Orders.FirstOrDefault(x => x.ID == ID);
            if (order == null)
                return RedirectToAction("Orders");
            return View(order);
        }

        [Authorize(Roles = "ShopOwner, Manager")]
        [HttpGet]
        public ActionResult WarningBlock(int ID, int? ShopID)
        {
            var order = DB.Orders.FirstOrDefault(x => x.ID == ID) ?? new Order();
            if (!order.OrderChars.Any())
            {
                int? shid = order.ShopID;
                if (ShopID.HasValue)
                {
                    shid = ShopID.Value;
                }
                if (!shid.HasValue)
                {
                    shid = DefShopID;
                }

                order.OrderChars.AddRange(
                    DB.ShopProductChars.Where(
                        x =>
                            ((!x.ShopID.HasValue && x.UserID == CurrentUser.ID) ||
                             (x.ShopID == shid && x.UserID == CurrentUser.ID)) && x.Type >= 2).ToList()
                        .Select(x => new OrderChar() { Name = x.Name, Value = x.DefValue }));
            }
            return PartialView(order);
        }

        [Authorize(Roles = "ShopOwner, Manager")]
        [HttpPost]
        public ActionResult WarningBlock(int ID, int? ShopID, string Warning, bool IsImportant, string OrderNumber, DateTime? DeliveryDate, string DeliveryAddress, int? ContractorID, decimal? ContractorCost, int? ContractorCostType, FormCollection collection)
        {
            var order = DB.Orders.FirstOrDefault(x => x.ID == ID);
            if (order != null)
            {
                var helper = new PermissionHelper();
                order.Warning = Warning;
                order.IsImportant = IsImportant;
                order.OrderNumber = OrderNumber;
                //order.DeliveryAddress = DeliveryAddress;
                if (!order.DeliveryDate.HasValue ||
                    (order.DeliveryDate.Value > DateTime.Now) ||
                    helper.HasPermission(Permissions.DeliveryEdit))
                {
                    order.DeliveryDate = DeliveryDate;
                }
                DB.SubmitChanges();
                ViewBag.Message = "Данные успешно сохранены";

                var charList = new List<OrderChar>();
                if (order.OrderChars.Any())
                {
                    charList.AddRange(order.OrderChars.ToList());
                    charList.AddRange(
                        DB.ShopProductChars.Where(
                            x =>
                                ((!x.ShopID.HasValue && x.UserID == CurrentUser.ID) ||
                                 (x.ShopID == order.ShopID && x.UserID == CurrentUser.ID)) && x.Type >= 2).ToList()
                            .Select(x => new OrderChar() { Name = x.Name, Value = x.DefValue, ID = x.ID }));

                }
                else
                {
                    int? shid = order.ShopID;
                    if (!shid.HasValue)
                    {
                        shid =
                            (DB.Shops.FirstOrDefault(x => x.ID == (ShopID ?? 0)) ?? new Shop() { ID = DefShopID }).ID;
                    }

                    charList.AddRange(
                        DB.ShopProductChars.Where(
                            x =>
                                ((!x.ShopID.HasValue && x.UserID == CurrentUser.ID) ||
                                 (x.ShopID == shid && x.UserID == CurrentUser.ID)) && x.Type >= 2).ToList()
                            .Select(x => new OrderChar() { Name = x.Name, Value = x.DefValue }));
                }

                var modified = new List<OrderChar>();
                foreach (var key in collection.AllKeys)
                {
                    if (key.StartsWith("Char_"))
                    {
                        var clear = key.Replace("Char_", "");
                        if (clear.Contains("_"))
                        {
                            var args = clear.Split<string>("_").ToList();
                            if (args[1] == "0")
                            {
                                var target = charList.ElementAt(int.Parse(args[0]) - 1);
                                if (target != null)
                                {
                                    target.Value = collection[key];
                                    modified.Add(target);
                                }

                            }
                            else
                            {
                                var target = charList.FirstOrDefault(x => x.ID == int.Parse(args[1]));
                                if (target != null)
                                {
                                    target.Value = collection[key];
                                    modified.Add(target);
                                }
                            }
                        }
                        else
                        {
                            var ch = new OrderChar()
                            {
                                Name = collection["CharName_" + clear],
                                Value = collection[key]
                            };
                            charList.Add(ch);
                            modified.Add(ch);
                        }
                    }
                }

                charList = modified;
                order.SaveChars(DB, charList);
                var chars = DB.OrderChars.Where(x => x.OrderID == order.ID);
                order.OrderChars.Clear();
                order.OrderChars.AddRange(chars);

                var contract =
                    DB.ContractedOrders.FirstOrDefault(x => x.SenderID == CurrentUser.ShopOwnerID);

                if (ContractorID.HasValue && ContractorID.Value > 0)
                {
                    if (contract != null)
                    {
                        contract.ContractorID = ContractorID.Value;
                        contract.ContractorCostType = ContractorCostType;
                        contract.ContractorCost = ContractorCost;
                    }
                    else
                    {
                        contract = new ContractedOrder()
                        {
                            OrderID = order.ID,
                            SenderID = CurrentUser.ShopOwnerID,
                            ContractorID = ContractorID.Value,
                            ContractorCostType = ContractorCostType,
                            ContractorCost = ContractorCost

                        };
                        DB.ContractedOrders.InsertOnSubmit(contract);
                    }
                }
                else
                {
                    if (contract != null)
                        DB.ContractedOrders.DeleteOnSubmit(contract);
                }
                DB.SubmitChanges();
            }
            else
            {
                ViewBag.Message = "Произошла ошибка";
            }
            return PartialView(order ?? new Order());
        }


        [HttpGet]
        [Authorize]
        public ActionResult ProductForm(int ID, int? OrderedProductID, int? ShopID)
        {
            ViewBag.ShopID = !ShopID.HasValue
                ? DefShopID
                : ShopID.Value;

            var model = new ProductModel() { OrderID = ID, OrderedProductID = OrderedProductID };
            if (OrderedProductID.HasValue)
            {

                var op = DB.OrderedProducts.FirstOrDefault(x => x.ID == OrderedProductID);
                if (op != null)
                {
                    if (op.Order.ShopID.HasValue)
                        ViewBag.ShopID = op.Order.ShopID.Value;

                    model.Amount = op.Amount;
                    model.Article = op.Product.Article;
                    model.Name = op.Product.Name;
                    model.Price = op.Price;
                    model.UnitCode = op.Product.UnitCode;
                    model.Weight = op.Product.Weight;
                    model.SelfCost = op.Product.SelfCost;
                    model.OptCost = op.Product.OptCost;
                    model.Chars = new List<ProductModelChar>();


                    var myChars =
                        op.ProductChars.Select(x => new ProductModelChar() { ID = x.ID, Name = x.Name, Value = x.Value }).ToList();
                    /*
                                        if (ShopID.HasValue)
                                        {
                                            myChars = myChars.Where(x => x.Value.IsFilled()).ToList();
                                            var shopChars = DB.ShopProductChars.Where(
                                                x =>
                                                    ((!x.ShopID.HasValue && x.UserID == CurrentUser.ID) ||
                                                     (x.ShopID == ShopID && x.UserID == CurrentUser.ID)) && x.Type <= 2)
                                                .Select(x => new ProductModelChar() {Name = x.Name, Value = x.DefValue});
                                            foreach (var ch in shopChars)
                                            {
                                                if (myChars.All(x => x.Name != ch.Name))
                                                {
                                                    myChars.Add(ch);
                                                }
                                            }
                                        }
                    */
                    model.Chars.AddRange(myChars);

                    var exist = op.ProductChars.ToList();

                    foreach (var ch in model.Chars)
                    {
                        var f = exist.FirstOrDefault(x => x.Name == ch.Name);
                        if (f != null)
                            ch.Value = f.Value;
                    }

                    foreach (var ch in exist)
                    {
                        if (model.Chars.All(x => x.Name != ch.Name))
                        {
                            model.Chars.Add(new ProductModelChar() { Name = ch.Name, Value = ch.Value });
                        }
                    }
                }
            }
            else
            {
                model.Chars = new List<ProductModelChar>();

                var order = DB.Orders.First(x => x.ID == ID);

                int? shid = order.ShopID;
                if (ShopID.HasValue)
                {
                    shid = ShopID;
                }
                if (!shid.HasValue)
                {
                    shid = DefShopID;
                }

                model.Chars.AddRange(
                    DB.ShopProductChars.Where(
                        x =>
                            ((!x.ShopID.HasValue && x.UserID == CurrentUser.ID) ||
                             (x.ShopID == shid && x.UserID == CurrentUser.ID)) && x.Type <= 2)
                        .Select(x => new ProductModelChar() { Name = x.Name, Value = x.DefValue }));
            }
            return PartialView(model);
        }


      

        [HttpPost]
        [Authorize]
        public ActionResult ProductForm(int ID, int? OrderedProductID, int? ShopID, ProductModel model, FormCollection collection)
        {
            var helper = new PermissionHelper();
            ViewBag.ShopID = !ShopID.HasValue ? DefShopID : ShopID.Value;

            if (!ShopID.HasValue)
                ShopID = ViewBag.ShopID;

            model.IsPost = true;
            model.OrderID = ID;
            model.OrderedProductID = OrderedProductID;
            model.Chars = new List<ProductModelChar>();


            var order = DB.Orders.FirstOrDefault(x => x.ID == ID);
            if (order == null)
            {
                return PartialView(model);
            }


            if (OrderedProductID.HasValue)
            {
                var op = DB.OrderedProducts.FirstOrDefault(x => x.ID == OrderedProductID);
                if (op != null)
                {
                    if (op.Order.ShopID.HasValue)
                        ViewBag.ShopID = op.Order.ShopID.Value;

                    model.Chars.AddRange(
                        op.ProductChars.Select(x => new ProductModelChar() { Name = x.Name, ID = x.ID, Value = x.Value }));
                    model.Chars.AddRange(
                        DB.ShopProductChars.Where(
                            x =>
                                ((!x.ShopID.HasValue && x.UserID == CurrentUser.ID) ||
                                 (x.ShopID == op.Order.ShopID && x.UserID == CurrentUser.ID)) && x.Type <= 2)
                            .Select(x => new ProductModelChar() { Name = x.Name, Value = x.DefValue, ID = x.ID }));



                }
            }
            else
            {
                int? shid = order.ShopID;
                if (!shid.HasValue)
                {
                    shid =
                        (DB.Shops.FirstOrDefault(x => x.ID == (ShopID ?? 0)) ?? new Shop() { ID = DefShopID }).ID;
                }

                model.Chars.AddRange(
                    DB.ShopProductChars.Where(
                        x =>
                            ((!x.ShopID.HasValue && x.UserID == CurrentUser.ID) ||
                            (x.ShopID == shid && x.UserID == CurrentUser.ID)) && x.Type <= 2)
                        .Select(x => new ProductModelChar() { Name = x.Name, Value = x.DefValue }));
            }

            var modified = new List<ProductModelChar>();
            foreach (var key in collection.AllKeys)
            {
                if (key.StartsWith("Char_"))
                {
                    var clear = key.Replace("Char_", "");
                    if (clear.Contains("_"))
                    {
                        var args = clear.Split<string>("_").ToList();
                        if (args[1] == "0")
                        {
                            var target = model.Chars.ElementAt(int.Parse(args[0]) - 1);
                            if (target != null)
                            {
                                target.Value = collection[key];
                                modified.Add(target);
                            }

                        }
                        else
                        {
                            var target = model.Chars.FirstOrDefault(x => x.ID == int.Parse(args[1]));
                            if (target != null)
                            {
                                target.Value = collection[key];
                                modified.Add(target);
                            }
                        }
                    }
                    else
                    {
                        var ch = new ProductModelChar()
                        {
                            Name = collection["CharName_" + clear],
                            Value = collection[key]
                        };
                        model.Chars.Add(ch);
                        modified.Add(ch);
                    }
                }
            }

            model.Chars = modified;

            if (model.Name.IsNullOrEmpty() || model.Article.IsNullOrEmpty() || !model.Amount.HasValue || !model.Price.HasValue)
                return PartialView(model);



            if (OrderedProductID.HasValue)
            {
                var op = DB.OrderedProducts.FirstOrDefault(x => x.ID == OrderedProductID);
                if (op != null)
                {
                    
                    op.Amount = model.Amount.Value;
                    op.Price = model.Price.Value;
                    op.Product.Name = model.Name;
                    op.Product.Article = model.Article;
                    op.Product.UnitCode = model.UnitCode;
                    op.Product.Weight = model.Weight;
                    if (helper.HasPermission(Permissions.SelfCostWrite))
                    {
                        op.Product.SelfCost = model.SelfCost;
                    }
                    if (helper.HasPermission(Permissions.OptCostWrite))
                    {
                        op.Product.OptCost = model.OptCost;
                    }
                    op.SaveChars(DB, model.Chars);
                    model.IsSuccess = true;
                    return PartialView(model);


                }
                else
                {
                    op = model.ToOrderedProduct();
                    op.OrderID = ID;
                    
                    var prod =
                        DB.Products.FirstOrDefault(
                            x =>
                                (x.Article.ToLower() == model.Article.ToLower() ||
                                 x.Name.ToLower() == model.Name.ToLower()) && x.OwnerID == CurrentUser.ID);
                    if (prod != null)
                    {
                        op.Product = prod;
                        op.Product.UnitCode = model.UnitCode;
                        op.Product.Weight = model.Weight;
                    }
                    else
                    {
                        op.Product = new Product()
                        {
                            OwnerID = CurrentUser.ID,
                            AddedDate = DateTime.Now,
                            Article = model.Article,
                            UnitCode = model.UnitCode,
                            Name = model.Name.ToNiceForm(),
                            Weight = model.Weight
                        };
                        if (helper.HasPermission(Permissions.SelfCostWrite))
                        {
                            op.Product.SelfCost = model.SelfCost;
                        }
                        if (helper.HasPermission(Permissions.OptCostWrite))
                        {
                            op.Product.OptCost = model.OptCost;
                        }
                    }


                    DB.OrderedProducts.InsertOnSubmit(op);
                    DB.SubmitChanges();

                    op.Product.SaveShopRel(ShopID.Value);


                    op.SaveChars(DB, model.Chars);
                    model.IsSuccess = true;
                    return PartialView(model);
                }
            }
            else
            {
                var op = model.ToOrderedProduct();
                op.OrderID = ID;
                
                var prod =
                      DB.Products.FirstOrDefault(
                          x =>
                              (x.Article.ToLower() == model.Article.ToLower() &&
                               x.Name.ToLower() == model.Name.ToLower()) && x.OwnerID == CurrentUser.ID);
                if (prod != null)
                {
                    op.Product = prod;
                    op.Product.UnitCode = model.UnitCode;
                    op.Product.Weight = model.Weight;
                }
                else
                {
                    op.Product = new Product()
                    {
                        OwnerID = CurrentUser.ID,
                        AddedDate = DateTime.Now,
                        Article = model.Article,
                        UnitCode = model.UnitCode,
                        Name = model.Name.ToNiceForm(),
                        Weight = model.Weight

                    };
                    if (helper.HasPermission(Permissions.SelfCostWrite))
                    {
                        op.Product.SelfCost = model.SelfCost;
                    }
                    if (helper.HasPermission(Permissions.OptCostWrite))
                    {
                        op.Product.OptCost = model.OptCost;
                    }

                }

                DB.OrderedProducts.InsertOnSubmit(op);
                DB.SubmitChanges();

                op.Product.SaveShopRel(ShopID.Value);

                op.SaveChars(DB, model.Chars);
                model.IsSuccess = true;
                return PartialView(model);

            }

        }


        [HttpGet]
        [Authorize]
        public ActionResult ProductsList(int ID, bool? SkipDel)
        {
            ViewBag.Order = DB.Orders.First(x => x.ID == ID);
            ViewBag.SkipDel = SkipDel ?? false;
            return PartialView(DB.OrderedProducts.Where(x => x.OrderID == ID));
        }

        [HttpPost]
        [Authorize]

        public ActionResult DeleteProduct(int ID)
        {
            var p = DB.OrderedProducts.FirstOrDefault(x => x.ID == ID && x.Order.AddedByID == CurrentUser.ID);
            if (p != null)
            {
                DB.OrderedProducts.DeleteOnSubmit(p);
                DB.SubmitChanges();
            }
            return new ContentResult();
        }


        [HttpGet]
        [Authorize]
        public ActionResult UserForm(int ID)
        {
            return PartialView(DB.Orders.First(x => x.ID == ID).Customer);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ProductListShort(Order order)
        {
            return PartialView(order);
        }

        [HttpGet]
        [Authorize]
        public ActionResult UserFormView(int ID)
        {
            var order = DB.Orders.First(x => x.ID == ID);
            return PartialView(order.Customer);

        }

        [HttpPost]
        [Authorize]
        public ActionResult UserForm(Customer user)
        {
            user.IsPost = true;
            if (user.Name.IsNullOrEmpty() || user.Surname.IsNullOrEmpty() || user.Phone.IsNullOrEmpty() || !user.Sex.HasValue)
            {
                user.Message = "Не заполнены обязательные поля";
                return PartialView(user);
            }
            if (user.CustomerID > 0)
            {
                var consumer = DB.Consumers.FirstOrDefault(x => x.ID == user.CustomerID);
                if (consumer != null)
                {
                    consumer.Name = user.Name;
                    consumer.Surname = user.Surname;
                    consumer.Phone = user.Phone;
                    consumer.Patrinomic = user.Patrinomic;
                    consumer.AddPhone = user.AddPhone ?? "";
                    consumer.Email = user.Email ?? "";
                    consumer.Sex = user.Sex.Value;
                    var order = DB.Orders.First(x => x.ID == user.OrderID);
                    order.ConsumerID = consumer.ID;

                    DB.SubmitChanges();
                    user.Message = "Информация успешно сохранена";
                }
            }
            else
            {
                var consumer = new Consumer()
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Patrinomic = user.Patrinomic,
                    Phone = user.Phone,
                    AddPhone = user.AddPhone ?? "",
                    Email = user.Email ?? "",
                    Sex = user.Sex.Value


                };
                DB.Consumers.InsertOnSubmit(consumer);
                var order = DB.Orders.First(x => x.ID == user.OrderID);
                order.Consumer = consumer;
                DB.SubmitChanges();
                user.Message = "Информация успешно сохранена";
            }
            return PartialView(user);
        }

        [Authorize]
        [HttpGet]
        public ActionResult SubmitForm(int? ShopID, int ID)
        {

            ViewBag.ShopID = !ShopID.HasValue
                ? (CurrentUser.UserRoles.Contains("ShopOwner") ? DB.Shops.First(x => x.Owner == CurrentUser.ID).ID : DB.ShopManagers.First(x => x.Manager.ManagerUserID == CurrentUser.ID).Shop.ID)
                : ShopID.Value;

            var order = DB.Orders.First(x => x.ID == ID);
            var errors = new List<string>();
            if (!order.ConsumerID.HasValue)
            {
                errors.Add("Необходимо указать данные о покупателе");
            }
            if (!order.OrderedProducts.Any())
            {
                errors.Add("Необходимо добавить товары в заказ");
            }
            if (order.OrderDelivery == null)
            {
                errors.Add("Необходимо указать информацию о доставке");
            }
            if (!order.DeliveryDate.HasValue)
            {
                errors.Add("Необходимо указать дату доставки");
            }
            ViewBag.Errors = errors;
            return PartialView(DB.Orders.First(x => x.ID == ID));
        }


        [Authorize]
        [HttpPost]
        public ActionResult SubmitForm(int? ShopID, int ID, FormCollection collection)
        {
            if (collection["SubmitForm"].IsNullOrEmpty())
            {
                return SubmitForm(ShopID, ID);
            }

            ViewBag.ShopID = !ShopID.HasValue
                ? (DefShopID)
                : ShopID.Value;

            var order = DB.Orders.First(x => x.ID == ID);

            if (!order.OrderedProducts.Any() || order.Consumer == null || order.OrderDelivery == null || !order.DeliveryDate.HasValue)
            {
                var errors = new List<string>();
                if (!order.ConsumerID.HasValue)
                {
                    errors.Add("Необходимо указать данные о покупателе");
                }
                if (!order.OrderedProducts.Any())
                {
                    errors.Add("Необходимо добавить товары в заказ");
                }
                if (order.OrderDelivery == null)
                {
                    errors.Add("Необходимо указать информацию о доставке");
                }
                if (!order.DeliveryDate.HasValue)
                {
                    errors.Add("Необходимо указать дату доставки");
                }

                ViewBag.Errors = errors;
                return PartialView(order);
            }

            order.Status = 0;
            order.ShopID = (int)ViewBag.ShopID;
            DB.SubmitChanges();
            Logger.WriteEvent(Logger.EventType.OrderCreate, "Добавление в обработку заказа #" + order.ID.ToString("d8"));

            ViewBag.RedirectURL = Url.Action("Orders", "Shop", new { ShopID = order.ShopID });

            return PartialView(order);

        }

        [HttpGet]
        public ActionResult Orders(int? Status, int? ShopID)
        {
            return RedirectToRoute("DefaultOrderSearch", new { Status, Page = "Common", controller = "Common", action = "OrderSearch", ShopID = ShopID ?? DefShopID });
        }


        public ActionResult OrderSearch(OrderFilter filter, int? Status)
        {
            if (Status.HasValue)
            {
                filter.Status = Status.Value;
            }
            filter.Search(DB);
            return PartialView(filter);
        }

        [HttpGet]
        public ActionResult OperatorOrders()
        {
            var filter = new OrderFilter();
            return View(filter);
        }


        public ActionResult OperatorOrderSearch(OrderFilter filter)
        {
            filter.Search(DB);
            return PartialView(filter);
        }


        public ActionResult OrderDetails(int id)
        {
            var order = DB.Orders.FirstOrDefault(x => x.ID == id);
            return View(order);
        }

        public ActionResult UserFormDetails(int id)
        {
            var order = DB.Orders.First(x => x.ID == id);
            var customer = order.Consumer;
            return
                PartialView(new Customer()
                {
                    CustomerID = customer.ID,
                    Name = customer.Name,
                    Patrinomic = customer.Patrinomic,
                    Surname = customer.Surname,
                    Phone = customer.Phone,
                    AddPhone = customer.AddPhone,
                    Email = customer.Email,
                    DeliveryAdress = order.OrderDeliveryOrNull.FullAddress,
                    DeliveryDate = order.DeliveryDate,
                    Sex = customer.Sex

                });
        }

        public ActionResult ProductsListDetails(int id)
        {
            var order = DB.Orders.First(x => x.ID == id);
            return PartialView(order.OrderedProducts);
        }

        [HttpGet]
        public ActionResult MarkForm(int id, bool? Popup)
        {
            ViewBag.Popup = Popup;
            var mark = DB.Marks.FirstOrDefault(x => x.OrderID == id) ?? new Mark() { OrderID = id };
            return PartialView(mark);
        }

        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult AddCharToDB(string name, string value, int id, int shop, int type)
        {

            var c =
                DB.ShopProductChars.FirstOrDefault(
                    x => x.Name.ToLower() == name.ToLower() && x.UserID == CurrentUser.ShopOwnerID && (x.ShopID == shop || !x.ShopID.HasValue));

            if (c == null)
            {
                c = new ShopProductChar()
                {
                    DefValue = value,
                    Name = name,
                    ShopID = shop,
                    UserID = CurrentUser.ShopOwnerID,
                    Type = type,
                    LoadInForm = true,
                    IsMain = true
                };
                DB.ShopProductChars.InsertOnSubmit(c);

            }
            else
            {
                if (type == 1)
                {
                    if (c.Type == 3)
                    {
                        c.Type = 2;
                    }
                }
                if (type == 3)
                {
                    if (c.Type == 1)
                    {
                        c.Type = 2;
                    }
                }
            }
            DB.SubmitChanges();
            return new ContentResult() { Content = "1" };
        }

        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult RemoveChar(string name, int order, int shop, int type)
        {

            var c =
                DB.ShopProductChars.FirstOrDefault(
                    x =>
                        x.Name.ToLower() == name.ToLower() && x.UserID == CurrentUser.ShopOwnerID &&
                        (x.ShopID == shop || !x.ShopID.HasValue));
            if (c != null)
            {
                if (type == 1)
                {
                    if (c.Type >= 2)
                    {
                        c.Type = 3;
                    }
                    else
                    {
                        DB.ShopProductChars.DeleteOnSubmit(c);
                    }
                }
                if (type == 3)
                {
                    if (c.Type <= 2)
                    {
                        c.Type = 1;
                    }
                    else
                    {
                        DB.ShopProductChars.DeleteOnSubmit(c);
                    }
                }
            }
            DB.SubmitChanges();
            return new ContentResult() { Content = "1" };
        }

        [HttpPost]
        public ActionResult MarkForm(Mark mark, bool? Popup)
        {
            ViewBag.Popup = Popup;
            var mk = DB.Marks.FirstOrDefault(x => x.OrderID == mark.OrderID);
            if (mk != null)
                return PartialView(mk);
            else
            {
                mark.MarkAuthorID = CurrentUser.ID;
                mark.MarkDate = DateTime.Now;
                DB.Marks.InsertOnSubmit(mark);
                DB.SubmitChanges();
                return PartialView(mark);
            }
        }
        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult DeleteOrder(int id)
        {
            var order = DB.Orders.FirstOrDefault(x => x.ID == id);
            if (order != null)
            {
                order.Status = -10;
                DB.SubmitChanges();
            }
            return RedirectToAction("Orders", "Shop", order != null ? (object)new { ShopID = order.ShopID } : new { });

        }

        [HttpGet]
        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult DeliveryBlock(int ID)
        {
            var order = DB.Orders.FirstOrDefault(x => x.ID == ID);
            if (order == null)
            {
                return PartialView(new DeliveryAddress() { Orders = new EntitySet<Order>() { new Order() } });
            }
            if (order.OrderDelivery == null)
            {
                DB.DeliveryAddresses.InsertOnSubmit(new DeliveryAddress() { Orders = new EntitySet<Order>() { order } });
                DB.SubmitChanges();

            }
            return PartialView(order.OrderDelivery);
        }
        [HttpPost]
        [Authorize(Roles = "ShopOwner, Manager")]
        public ActionResult DeliveryBlock(int ID, DeliveryAddress delivery)
        {
            var order = DB.Orders.FirstOrDefault(x => x.ID == ID);
            if (order == null)
            {
                return PartialView(new DeliveryAddress() { Orders = new EntitySet<Order>() { new Order() } });
            }
            if (order.OrderDelivery != null)
            {
                order.OrderDelivery.LoadPossibleProperties(delivery, new[] { "ID", "PointID" });
                DB.SubmitChanges();
                ViewBag.Message = "Информация успешно сохранена";
            }

            var adddr = delivery.FullAddress;
/*
            var wc = new WebClient();
            var responce = wc.DownloadString();
*/
            try
            {
                XPathDocument document =
                    new XPathDocument(HttpUtility.UrlPathEncode("http://geocode-maps.yandex.ru/1.x/?geocode=" + adddr));
                XPathNavigator navigator = document.CreateNavigator();
                XPathExpression query = navigator.Compile("x:ymaps/x:GeoObjectCollection/gml:featureMember/x:GeoObject/gml:Point/gml:pos");
                XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
                manager.AddNamespace("x", "http://maps.yandex.ru/ymaps/1.x");
                manager.AddNamespace("gml", "http://www.opengis.net/gml");
                query.SetContext(manager);
                var result = navigator.Select(query);
                while (result.MoveNext())
                {
                    var r = result.Current.Value.Trim().Replace(".", ",").Split<decimal>(" ").ToList();
                    if (r.Count == 2)
                    {
                        if (order.OrderDelivery != null)
                        {
                            if (!order.OrderDelivery.HasPoint)
                            {
                                var point = new MapPoint()
                                {
                                    Lat = r[1],
                                    Lng = r[0],
                                    Num = 0
                                };
                                DB.MapPoints.InsertOnSubmit(point);
                                order.OrderDelivery.MapPoint = point;
                            }
                            else
                            {
                                order.OrderDelivery.MapPoint.Lat = r[1];
                                order.OrderDelivery.MapPoint.Lng = r[0];
                            }
                            DB.SubmitChanges();
                        }
                    }
                    break;
                    
                }

            }
            catch
            {
                
            }


            return PartialView(order.OrderDelivery);

        }


        public string SenderName
        {
            get { return "KeepUP"; }
        }
        public string SenderEmail
        {
            get { return "KovetskiyAS@mail.ru"; }
        }

        public string KeepUpUnisenderKey
        {
            get { return "5dqmy3zytp6e9tnd4ruuotrt5myqboxkndqgso1e"; }
        }
        public int KeepUPUnisenderList
        {
            get { return 4375223; }
        }

        public ActionResult SendSpam(string type, string header, string message, string list)
        {
            dynamic client = new Client(KeepUpUnisenderKey);
            var ids = list.Split<int>(";").ToList();
            var orders = DB.Orders.Where(x => ids.Contains(x.ID) && x.Consumer != null).ToList();
            foreach (var order in orders)
            {
                var mail = order.Consumer.Email;
                var phone = order.Consumer.Phone;
                if (type == "sms")
                {
                    if (phone.IsFilled())
                    {
                        var result =
                            client.sendSms(new { phone = phone, sender = SenderName, text = message }.ToDictionary());
                    }
                }
                else
                {
                    if (mail.IsFilled())
                    {
                        var result = client.sendEmail(new
                        {
                            email = order.Consumer.Email,
                            sender_name = SenderName,
                            sender_email = SenderEmail,
                            subject = header,
                            body = message,
                            list_id = KeepUPUnisenderList
                        }.ToDictionary());
                    }

                }
            }
            return new ContentResult();
        }
    }
}
