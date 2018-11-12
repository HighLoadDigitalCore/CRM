using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trading.Extensions;
using Trading.Models;

namespace Trading.Controllers
{
    public class CommonController : BaseController
    {
        [Authorize]
        [HttpGet]

        public ActionResult LoadSpamContent(string template)
        {
            var mailing = DB.MailingLists.FirstOrDefault(x => x.LetterKey == template);
            if (mailing != null)
                return new JsonResult()
                {
                    Data = new {Header = mailing.Header, Template = mailing.Letter},
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            return new JsonResult()
            {
                Data = new {Header = "", Template = ""},
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [Authorize]
        [HttpPost]
        public ActionResult SaveStoreShops(int StoreID, string Shops)
        {
            var shops = Shops.Split<int>(";").ToList();
            var rels = DB.ShopStores.Where(x => x.StoreID == StoreID).ToList();
            var forAdd = shops.Where(x => rels.All(z => z.ShopID != x)).ToList();
            var forDel = rels.Where(x => !shops.Contains(x.ShopID)).ToList();
            if (forDel.Any())
            {
                DB.ShopStores.DeleteAllOnSubmit(forDel);
                DB.SubmitChanges();
            }
            if (forAdd.Any())
            {
                DB.ShopStores.InsertAllOnSubmit(forAdd.Select(x => new ShopStore() { StoreID = StoreID, ShopID = x }).ToList());
                DB.SubmitChanges();
            }
            return new ContentResult();
        }

        [Authorize]
        [HttpGet]
        public ActionResult ShopSelectMulti(int StoreID)
        {
            ViewBag.StoreID = StoreID;
            var shops = DB.ShopStores.Where(x => x.StoreID == StoreID);
            return PartialView(shops.ToList());
        }

        [Authorize]
        public ActionResult ProductSearch(string page)
        {
            var psb = new ProductSearchBlock(page);
            var filter = new ProductFilter();
            psb.Model = filter;
            return View(psb);

        }

        [Authorize]
        public ActionResult ProductSearchPartial(ProductFilter filter, string page, string FormSettings)
        {
            var options = new ProductSearchTableOptions();
            if (FormSettings.IsFilled())
            {
                options =
                    (ProductSearchTableOptions)
                        new JsonSerializable(new ProductSearchTableOptions()).FromString(FormSettings);
            }

            var osb = new ProductSearchBlock(options.Page.IsNullOrEmpty() ? page : options.Page);
            osb.Options = options;
            filter.Search(DB);
            osb.Model = filter;
            return PartialView(osb);
        }


        [Authorize]
        public ActionResult OrderSearch(string page, int? Status)
        {
            var osb = new OrderSearchBlock(page);
            var filter = Status.HasValue ? new OrderFilter(Status.Value, DateTime.Now) : new OrderFilter();
            osb.Model = filter;
            return View(osb);
        }
        [Authorize]
        public ActionResult OrderSearchPartial(OrderFilter filter, string page, int? Status, string FormSettings, string ListOverride)
        {
            var options = new OrderSearchTableOptions();
            if (FormSettings.IsFilled())
            {
                options =
                    (OrderSearchTableOptions)
                        new JsonSerializable(new OrderSearchTableOptions()).FromString(FormSettings);
            }

            var osb = new OrderSearchBlock(options.Page.IsNullOrEmpty() ? page : options.Page);
            osb.Options = options;
            if (Status.HasValue)
            {
                filter.Status = Status.Value;
            }
            filter.ListOverride = ListOverride.Split<int>("_").ToList();
            filter.Search(DB);
            osb.Model = filter;


            return PartialView(osb);
        }

        [Authorize]
        public ActionResult OrderSearchSpamPopup()
        {
            return PartialView();
        }

        [Authorize]
        public ActionResult OrderSearchSavePopup()
        {
            return PartialView();
        }


        [Authorize]
        public ActionResult ShopSelect(int? ShopID)
        {
            if (CurrentUser.UserRoles.Contains("ShopOwner"))
            {
                var shops = CurrentUser.Shops.Select(x => new SelectListItem()
                {
                    Selected = x.ID == (ShopID ?? 0),
                    Text = x.Name,
                    Value = x.ID.ToString()
                }).ToList();

                shops.Insert(0, new SelectListItem() { Selected = !ShopID.HasValue, Text = "Все магазины", Value = "" });
                return PartialView(shops);
            }
            else if (CurrentUser.UserRoles.Contains("Manager"))
            {
                var shops = CurrentUser.ManagerProfiles.First().ShopManagers.Select(x => x.Shop).Select(x => new SelectListItem()
                {
                    Selected = x.ID == (ShopID ?? 0),
                    Text = x.Name,
                    Value = x.ID.ToString()
                }).ToList();
                return PartialView(shops);

            }
            return PartialView(new List<SelectListItem>());
        }

        [Authorize]
        public ActionResult ShopSelectRequired(int? ShopID)
        {
            if (!ShopID.HasValue)
            {
                ShopID = Request.QueryString["ShopID"].ToNullInt();
            }
            if (CurrentUser.UserRoles.Contains("ShopOwner"))
            {
                if (!ShopID.HasValue)
                {
                    ShopID = (CurrentUser.Shops.FirstOrDefault() ?? new Shop()).ID;
                }
                var shops = CurrentUser.Shops.Select(x => new SelectListItem()
                {
                    Selected = x.ID == (ShopID ?? 0),
                    Text = x.Name,
                    Value = x.ID.ToString()
                }).ToList();
                return PartialView(shops);
            }
            else if (CurrentUser.UserRoles.Contains("Manager"))
            {
                if (!ShopID.HasValue)
                {
                    ShopID = (CurrentUser.ManagerProfiles.First().ShopManagers.First().Shop ?? new Shop()).ID;
                }
                var shops = CurrentUser.ManagerProfiles.First().ShopManagers.Select(x => x.Shop).Select(x => new SelectListItem()
                {
                    Selected = x.ID == (ShopID ?? 0),
                    Text = x.Name,
                    Value = x.ID.ToString()
                }).ToList();
                return PartialView(shops);
            }
            return PartialView(new List<SelectListItem>());

        }

        [Authorize]
        public ActionResult OrderSearchSavePopupContent(string uids)
        {
            var model = new SaveOrdersModel(uids);
            return PartialView(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult OrderSearchSavePopupContent(int? ID, string OrderList, string Name, string GraphListPlane)
        {
            var model = new SaveOrdersModel(ID ?? 0, OrderList, Name, GraphListPlane);
            model.Save();
            model.IsPost = true;
            model.GraphListPlane = GraphListPlane;
            if (model.Result.IsNullOrEmpty())
                model.SubmitSuccess = true;
            return PartialView(model);
        }

        [Authorize]
        public ActionResult OrderSearchSavePopupContentGraphSelect(int? SerialID, string GraphListPlane)
        {
            ViewBag.GraphList = new SaveOrdersModel().MyGraphList;
            ViewBag.GraphListForSerial =
                DB.GraphSerialsRels.Where(x => x.SerialID == SerialID).Select(x => x.GraphID).ToList();

            if (GraphListPlane.IsFilled())
            {
                ViewBag.GraphListForSerial = GraphListPlane.Split<int>(",").ToList();
            }
            ViewBag.GraphListPlane = GraphListPlane;


            return PartialView();
        }

        [Authorize]
        public ActionResult OrderSearchSavePopupContentSeriesSelect(int id, string SerialListPlane)
        {
            ViewBag.SerialList = new SaveOrdersModel().MySerialList;
            ViewBag.SerialListForGraph =
                DB.GraphSerialsRels.Where(x => x.GraphID == id).Select(x => x.SerialID).ToList();
            if (SerialListPlane.IsFilled())
            {
                ViewBag.SerialListForGraph = SerialListPlane.Split<int>(",").ToList();
            }
            ViewBag.SerialListPlane = SerialListPlane;
            return PartialView();
        }
        [Authorize]
        public ActionResult OrderSearchGraphPopup()
        {
            return PartialView();
        }
        [Authorize]
        [HttpGet]
        public ActionResult OrderSearchGraphPopupContent(int ID)
        {
            var graph = DB.Graphs.FirstOrDefault(x => x.ID == ID) ?? new Graph() { };
            if (graph.ID == 0)
            {
                graph.TypeID = graph.GetTypeFuncList(graph.TypeList.First().ID).First().ID;
            }
            graph.SerialListPlane = graph.GraphSerialsRels.Select(x => x.SerialID).JoinToString(",");
            return PartialView(graph);
        }
        [Authorize]
        [HttpPost]
        public ActionResult OrderSearchGraphPopupContent(Graph graph, FormCollection collection)
        {
            graph.IsPost = true;
            ViewBag.SerialListPlane = graph.SerialListPlane;
            if (graph.Name.IsNullOrEmpty() || graph.SerialListPlane.IsNullOrEmpty() || graph.TypeID == 0)
                return PartialView(graph);


            var typeFunc = DB.GraphTypeFuncs.First(x => x.ID == graph.TypeID);
            var splitted = graph.SerialListPlane.Split<int>(",").ToList();
            if (typeFunc.MinSequenceCount > splitted.Count)
            {
                graph.ErrorText = "Минимальное количество последовательностей для построения графика равно " + typeFunc.MinSequenceCount;
                return PartialView(graph);
            }



            var g = DB.Graphs.FirstOrDefault(x => x.ID == graph.ID) ?? new Graph() { UserID = CurrentUser.ID, OrderNum = DB.Graphs.Count() + 1 };
            g.LoadPossibleProperties(graph, new[] { "ID", "UserID", "OrderNum" });
            if (graph.ID == 0)
            {
                DB.Graphs.InsertOnSubmit(g);
                graph.HasCreated = true;
            }
            DB.SubmitChanges();

            foreach (var seqID in splitted.Where(seqID => g.GraphSerialsRels.All(x => x.SerialID != seqID)))
            {
                DB.GraphSerialsRels.InsertOnSubmit(new GraphSerialsRel() { GraphID = g.ID, SerialID = seqID });
            }
            DB.SubmitChanges();
            foreach (var rel in g.GraphSerialsRels.Where(rel => !splitted.Contains(rel.SerialID)))
            {
                DB.GraphSerialsRels.DeleteOnSubmit(rel);
            }
            DB.SubmitChanges();
            graph.SubmitSuccess = true;
            return PartialView(graph);
        }

        public ActionResult GetGraphFuncs(int ID)
        {
            var first = DB.GraphTypeFuncs.FirstOrDefault(x => x.TypeID == ID) ?? new GraphTypeFunc();
            var list = DB.GraphTypeFuncs.Where(x => x.TypeID == ID).OrderBy(x => x.Name).ToList();
            return new ContentResult()
            {
                Content = list.Any()
                    ? list.
                        Select(
                            x =>
                                string.Format("<option value='{0}' {2}>{1}</option>", x.ID, x.Name,
                                    x.ID == first.ID ? "selected" : "")).ToList().JoinToString("")
                    : ""
            };
        }

        public ActionResult RegionFilter()
        {
            return PartialView(OrderFilter.Regions);
        }

        public ActionResult RegionFilterPopup()
        {
            return
                PartialView(DB.Orders.Where(x => x.OrderDelivery != null && (x.OrderDelivery.Region ?? "").Length > 0).Select(x => x.OrderDelivery.Region)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList());
        }
    }
}
