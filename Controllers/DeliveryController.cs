
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using NPOI.SS.Formula.Functions;
using Trading.Extensions;
using Trading.Models;

namespace Trading.Controllers
{
    [Authorize(Roles = "ShopOwner, Manager")]
    public class DeliveryController : BaseController
    {

        [HttpGet]
        public ActionResult OrderInfo(int id)
        {
            var order = DB.Orders.FirstOrDefault(x => x.ID == id);
            return PartialView(order);
        }

        [HttpGet]
        public ActionResult ChangeDelivererForGroup(int list, int group, int type)
        {
            var result =
                new object[]
                {
                    CurrentUser.AvailableCars.Where(x => type != (int)SectorTypes.CarSector || x.ID != @group).ToList(),
                    CurrentUser.AvailableCouriers.Where(x => type != (int)SectorTypes.CourierSector || x.ID != @group).ToList(),
                    list, group, type
                };
            return PartialView(result);

        }

        [HttpGet]
        public ActionResult ChangeWorkerGroup(int list, int car)
        {
            var c = DB.Cars.FirstOrDefault(x => x.ID == car) ?? new Car();
            var exist =
                DB.DeliveryListOrders.Where(x => x.ListID == list && x.Order != null && x.Order.Car != null)
                    .Select(x => x.Order.Car.WorkerGroupID)
                    .ToList();
            var result =
                new object[]
                {
                    CurrentUser.AvailableWorkerGroupsForCar(c).Where(x => !exist.Contains(x.ID)).ToList(),
                    car
                };
            return PartialView(result);

        }

        [HttpPost]
        public ActionResult SaveWorkerGroup(int car, int group)
        {
            var c = DB.Cars.FirstOrDefault(x => x.ID == car);
            if (c != null)
            {
                c.WorkerGroupID = group;
                DB.SubmitChanges();
            }
            return new ContentResult();
        }

        [HttpPost]
        public ActionResult ChangeDelivererForGroup(int list, int group, int type, int target, int ttype)
        {
            var source =
                DB.DeliveryListOrders.Where(
                    x =>
                        x.ListID == list && x.OrderID.HasValue && x.Order.OrderDelivery!=null && x.Order.OrderDelivery.DeliveryType == type &&
                        (type == (int) SectorTypes.CarSector ? x.Order.CarID == group : x.Order.WorkerID == group)
                    );
            var tgt =
                DB.DeliveryListOrders.Where(
                    x =>
                        x.ListID == list && x.OrderID.HasValue && x.Order.OrderDelivery!=null && x.Order.OrderDelivery.DeliveryType == ttype &&
                        (type == (int) SectorTypes.CarSector ? x.Order.CarID == target : x.Order.WorkerID == target)
                    );
            var max = 1;
            if (tgt.Any())
            {
                max = tgt.Max(x => x.OrderNum) + 1;
            }
            if (source.Any())
            {
                foreach (var order in source)
                {
                    order.Order.OrderDelivery.DeliveryType = ttype;
                    order.Order.WorkerID = null;
                    order.Order.CarID = null;
                    if (ttype == (int) SectorTypes.CarSector)
                    {
                        order.Order.CarID = target;
                    }
                    else
                    {
                        order.Order.WorkerID = target;
                    }
                    order.OrderNum = max;
                    max++;
                }
            }
            DB.SubmitChanges();
            return new ContentResult();
        }


        [HttpPost]
        public ActionResult AddPointsToList(DateTime date, int ShopID, int Type, int Group, string Stores, string Orders)
        {
            var list = DB.DeliveryLists.FirstOrDefault(x => x.Date.Date == date.Date);
            if (list == null)
            {
                return new ContentResult();
            }
            var stores = Stores.Split<int>(";").ToList();
            var max = list.GetMaxOrderNum(Group, Type);
            if (stores.Any())
            {
                var storeItems = stores.Select((x, index) => new DeliveryListOrder()
                {
                    StoreID = x,
                    Type = Type,
                    ListID = list.ID,
                    ReturnPoint = false,
                    ShowOnMap = true,
                    OrderNum = max + index + 1
                }).ToList();
                max += storeItems.Count;
                if (Type == (int) SectorTypes.CarSector)
                {
                    foreach (var item in storeItems)
                    {
                        item.CarID = Group;
                    }
                }
                if (Type == (int) SectorTypes.CourierSector)
                {
                    foreach (var item in storeItems)
                    {
                        item.WorkerID = Group;
                    }
                }
                DB.DeliveryListOrders.InsertAllOnSubmit(storeItems);
                DB.SubmitChanges();
            }

            var orders = Orders.Split<int>(";").ToList();
            if (orders.Any())
            {
                var orderItems = orders.Select((x, index) => new DeliveryListOrder()
                {
                    OrderID = x,
                    OrderNum = max + index + 1,
                    ListID = list.ID

                });
                var exist = DB.DeliveryListOrders.Where(x => orders.Contains(x.OrderID ?? 0));
                if (exist.Any())
                {
                    DB.DeliveryListOrders.DeleteAllOnSubmit(exist);
                }
                DB.SubmitChanges();
                DB.DeliveryListOrders.InsertAllOnSubmit(orderItems);
                DB.SubmitChanges();

                foreach (var o in orders.Select(order => DB.Orders.FirstOrDefault(x => x.ID == order)).Where(o => o != null))
                {
                    o.DeliveryDate = date.Date;
                    o.WorkerID = null;
                    o.CarID = null;
                    o.OrderDelivery.DeliveryType = Type;
                    if (Type == (int)SectorTypes.CarSector)
                    {
                        o.CarID = Group;
                    }
                    if (Type == (int)SectorTypes.CourierSector)
                    {
                        o.WorkerID = Group;
                    }
                }
                DB.SubmitChanges();
            }
            return  new ContentResult();
        }

        [HttpPost]
        public ActionResult ChangeDate(DateTime? NewDate, int Notify, int ID)
        {
            var list = DB.DeliveryListOrders.FirstOrDefault(x => x.ID == ID);
            if (list != null && list.Order != null)
            {
                if (!NewDate.HasValue || NewDate.Value.Date <= list.Order.DeliveryDate.Value.Date)
                {
                    return new ContentResult() { Content = "Необходимо указать новую дату доставки." };
                }

                

                list.Order.DeliveryDate = NewDate;
                DB.DeliveryListOrders.DeleteOnSubmit(list);
                DB.SubmitChanges();
            }
            return new ContentResult();
        }
        [HttpPost]
        public ActionResult RemoveOrder(int id)
        {
            return PartialView(DB.DeliveryListOrders.FirstOrDefault(x => x.ID == id));
        }

        public ActionResult RoutePointsOrders(RoutePointList model)
        {
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult RoutePointsOrdersFilter(DateTime? Date, int Group, int Type, int ShopID)
        {
            return PartialView("RoutePointsOrders",
                new RoutePointList(new RoutePointParam() {Date = Date, Group = Group, ShopID = ShopID, Type = Type}));
        }

        [HttpPost]
        public ActionResult RoutePoints(int type, int group, int shopid)
        {
            return
                PartialView(new RoutePointList(new RoutePointParam()
                {
                    Type = type,
                    Date = DateTime.Now.AddDays(1).Date,
                    Group = group,
                    ShopID = shopid
                })
                    );
        }

        [HttpPost]
        public ActionResult RemoveStore(int id)
        {
            var item = DB.DeliveryListOrders.FirstOrDefault(x => x.ID == id && x.StoreID.HasValue);
            if (item != null)
            {
                DB.DeliveryListOrders.DeleteOnSubmit(item);
                DB.SubmitChanges();
            }
            return new ContentResult();
        }

        [HttpPost]
        public ActionResult SaveOrder(string list)
        {
            var ids = list.Split<int>(";").ToList();
            var items = DB.DeliveryListOrders.Where(x => ids.Contains(x.ID)).ToList();
            var ordered = ids.Join(items, x => x, y => y.ID, (x, y) => y).ToList();
            if (ordered.Any())
            {
                var min = ordered.Min(x => x.OrderNum);
                var counter = 0;
                foreach (var listOrder in ordered)
                {
                    listOrder.OrderNum = min + counter;
                    counter++;
                }
                DB.SubmitChanges();
            }
            return new ContentResult();
        }

        [HttpPost]
        public ActionResult SaveCoords(int id, decimal lat, decimal lng)
        {
            var order = DB.Orders.FirstOrDefault(x => x.ID == id);
            if (order != null)
            {
                if (order.OrderDelivery == null)
                {
                    var delivery = new DeliveryAddress();
                    DB.DeliveryAddresses.InsertOnSubmit(delivery);
                    order.OrderDelivery = delivery;
                }
                if (order.OrderDelivery.MapPoint == null)
                {
                    var point = new MapPoint() { Lat = lat, Lng = lng, Num = 0 };
                    DB.MapPoints.InsertOnSubmit(point);
                    order.OrderDelivery.MapPoint = point;
                }
                order.OrderDelivery.MapPoint.Lat = lat;
                order.OrderDelivery.MapPoint.Lng = lng;
                DB.SubmitChanges();
            }
            return new ContentResult();
        }

        [HttpPost]
        public ActionResult LocationEdit(int OrderID)
        {
            return PartialView(DB.Orders.FirstOrDefault(x => x.ID == OrderID));
        }

        [HttpPost]
        public ActionResult ChangeReturnPoint(int arg, int group, int? value, int type)
        {
            var list = DB.DeliveryLists.FirstOrDefault(x => x.ID == group);
            if (list != null)
            {
                IGrouping<int?, DeliveryListOrder> gr = null;
                if (type == (int)SectorTypes.CarSector)
                {
                    gr = list.DeliveryListOrders.GroupBy(x => x.CarID).FirstOrDefault(x => x.Key == arg);
                }
                if (type == (int)SectorTypes.CourierSector)
                {
                    gr = list.DeliveryListOrders.GroupBy(x => x.WorkerID).FirstOrDefault(x => x.Key == arg);
                }
                if (value.HasValue)
                {
                    var rp = gr.FirstOrDefault(x => x.ReturnPoint);
                    if (rp != null)
                    {
                        rp.StoreID = value.Value;
                    }
                    else
                    {
                        var max = gr.Max(x => x.OrderNum);
                        rp = new DeliveryListOrder()
                        {
                            StoreID = value.Value,
                            Type = type,
                            ReturnPoint = true,
                            OrderNum = max + 1,
                            ListID = list.ID
                        };
                        if (type == (int) SectorTypes.CarSector)
                            rp.CarID = arg;
                        if (type == (int)SectorTypes.CourierSector)
                            rp.WorkerID = arg;
                        DB.DeliveryListOrders.InsertOnSubmit(rp);
                    }
                }
                else
                {
                    var rp = gr.FirstOrDefault(x => x.ReturnPoint);
                    if (rp != null)
                    {
                        DB.DeliveryListOrders.DeleteOnSubmit(rp);
                    }
                }
                DB.SubmitChanges();
            }
            return new ContentResult();
        }

        [HttpPost]
        public ActionResult ChangeMapShow(int type, int arg, int group, int val)
        {
            var list = DB.DeliveryLists.FirstOrDefault(x => x.ID == group);
            if (list != null)
            {
                if (type == (int)SectorTypes.CarSector)
                {
                    var item =
                        list.CarPoints
                            .GroupBy(x => x.CarUID)
                            .FirstOrDefault(x => x.Key == arg);
                    if (item != null)
                    {
                        foreach (var order in item)
                        {
                            order.ShowOnMap = val == 1; 
                        }
                    }
                    DB.SubmitChanges();
                }
                if (type == (int)SectorTypes.CourierSector)
                {
                    var item =
                        list.CourierPoints
                            .GroupBy(x => x.WorkerUID)
                            .FirstOrDefault(x => x.Key == arg);
                    if (item != null)
                    {
                        foreach (var order in item)
                        {
                            order.ShowOnMap = val == 1;
                        }
                    }
                    DB.SubmitChanges();
                }
            }
            return new ContentResult();
        }


        [HttpPost]
        public ActionResult ChangeDeliverer(int target, int? uid, int type)
        {
            var dlo = DB.DeliveryListOrders.FirstOrDefault(x => x.ID == target);
            if (dlo != null)
            {
                dlo.Type = null;
                if (type == (int)SectorTypes.CarSector)
                {
                    dlo.Order.CarID = uid;
                }
                if (type == (int)SectorTypes.CourierSector)
                {
                    dlo.Order.WorkerID = uid;
                }
                dlo.WorkerID = null;
                dlo.CarID = null;
                DB.SubmitChanges();
            }
            return new ContentResult();
        }

        [HttpPost]
        public ActionResult ChangeType(int target, int type)
        {
            var dlo = DB.DeliveryListOrders.FirstOrDefault(x => x.ID == target);
            if (dlo != null)
            {
                dlo.Type = null;
                dlo.CarID = null;
                dlo.WorkerID = null;
                dlo.Order.OrderDelivery.DeliveryType = type;
                DB.SubmitChanges();
                if (dlo.Order.ShopID.HasValue)
                {
                    var poly = new PolygonList(dlo.Order.ShopID.Value);
                    if (type == (int)SectorTypes.CarSector)
                    {
                        var cars = poly.GetDefaultCars(dlo.Order);
                        if (cars.Any())
                        {
                            dlo.CarID = cars.First().ID;
                        }

                    }
                    if (type == (int)SectorTypes.CourierSector)
                    {
                        var cars = poly.GetDefaultCouriers(dlo.Order);
                        if (cars.Any())
                        {
                            dlo.CarID = cars.First().ID;
                        }

                    }
                    DB.SubmitChanges();
                }
            }
            return new ContentResult();
        }

        [HttpPost]
        public ActionResult LoadCar(int ID)
        {
            var car = DB.Cars.FirstOrDefault(x => x.ID == ID);
            return PartialView(car);
        }

        [HttpPost]
        public ActionResult SaveDriver(int ID, string Driver)
        {
            var order = DB.Orders.FirstOrDefault(x => x.ID == ID);
            if (order != null)
            {
                var car = DB.Cars.FirstOrDefault(x => x.Name == Driver && x.OwnerID == CurrentUser.ShopOwnerID);
                if (car != null)
                {
                    order.Car = car;
                }
                else
                {
                    order.CarID = null;
                }
                DB.SubmitChanges();
            }
            return new ContentResult();
        }
        public ActionResult OrdersData(string ids, int? ShopID)
        {
            var list = ids.Split<int>(";").ToList();
            var points = DB.Orders.Where(x => list.Contains(x.ID)).Select(x => x.OrderPointData).ToArray();


            var drivers =
                DB.MapSectors.Where(x => x.ShopID == (ShopID ?? DefShopID))
                    .SelectMany(x => x.MapSectorCars).Select(x => x.Car).Where(x => x != null)
                    .ToList()
                /*.Where(x => x.IsFilled())*/
                    .DistinctBy(x => x.ID).Select(x => x.Name)
                    .ToList();


            drivers =
                drivers.Concat(points.Select(x => x.Driver).ToList().Distinct().Where(x => x.IsFilled()))
                    .Distinct()
                    .Where(x => x.IsFilled())
                    .OrderBy(x => x)
                    .ToList();


            return Json(new { Points = points, Drivers = drivers }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderMap(int? ShopID)
        {
            var stores = DB.Stores.Where(x => x.ShopStores.Any(z => z.ShopID == (ShopID ?? DefShopID)));
            return PartialView(stores.ToList());
        }

        public ActionResult ApproveListChange(DateTime date, int val)
        {
            var list = DB.DeliveryLists.First(x => x.Date.Date == date.Date);
            if (list != null)
            {
                list.Approved = val == 1;
                list.ApprovedBy = CurrentUser.ID;
                DB.SubmitChanges();
                return new ContentResult() { Content = "" };
            }
            else
            {
                return new ContentResult() {Content = "Ошибка при утверждении листа. Лист не найден."};
            }
        }

        public ActionResult DeliveryMap(string List)
        {
            var uids = List.Split<int>(";").ToList();
            return View(DB.Orders.Where(x => uids.Contains(x.ID)));
        }

        public ActionResult Orders()
        {
            var route = Url.RouteUrl("DefaultOrderSearch",
                new
                {
                    ShopID = CurrentUser.DefShopID,
                    Page = "Delivery",
                    controller = "Common",
                    action = "OrderSearch",
                    Status = (int)OrderStatus.WaitingDelivery
                });
            return Redirect(route);
        }




        public ActionResult Zones(int? ShopID)
        {
            if (!ShopID.HasValue)
                ShopID = DefShopID;

            ViewBag.ShopID = ShopID;
            var zones = DB.MapSectors.Where(x => x.ShopID == ShopID && x.Type == (int)SectorTypes.CarSector).ToList();
            return View(zones);
        }

        public ActionResult SectorList(int ShopID, int? Type)
        {
            ViewBag.ShopID = ShopID;
            ViewBag.Type = Type;
            var list = DB.MapSectors.Where(x => x.ShopID == ShopID);
            if (Type.HasValue)
            {
                list = list.Where(x => x.Type == Type);
            }
            return PartialView(list.ToList());
        }

        [HttpGet]
        public ActionResult SectorEdit(int ShopID, int ID, int Type)
        {
            ViewBag.ShopID = ShopID;
            ViewBag.Type = Type;
            var sector = DB.MapSectors.FirstOrDefault(x => x.ID == ID);
            if (sector == null)
                sector = new MapSector() { ShopID = ShopID, Type = Type };
            ViewBag.StoreList =
                DB.Stores.Where(x => x.ShopStores.Any(z => z.ShopID == ShopID)).ToList()
                    .Select(
                        x =>
                            new SelectListItem()
                            {
                                Text = x.Name,
                                Value = x.ID.ToString(),
                                Selected = sector.StoreID == x.ID
                            }).AsEnumerable();
            return PartialView(sector);
        }

        [HttpPost]
        public ActionResult SectorEdit(int ShopID, int ID, string Name, int? StoreID, string Color, string PointList, string Driver, int? WorkerGroupID, int Type, FormCollection collection)
        {
            ViewBag.ShopID = ShopID;
            ViewBag.Type = Type;
            var sector = DB.MapSectors.FirstOrDefault(x => x.ID == ID);
            if (sector == null)
                sector = new MapSector() { ShopID = ShopID, Type = Type };


            sector.Name = Name;
            sector.StoreID = StoreID;
            sector.Color = Color;
            /*
                        sector.Driver = Driver;
                        sector.WorkerGroupID = WorkerGroupID;
            */
            ViewBag.StoreList =
                DB.Stores.Where(x => x.ShopStores.Any(z => z.ShopID == ShopID)).ToList()
                    .Select(
                        x =>
                            new SelectListItem()
                            {
                                Text = x.Name,
                                Value = x.ID.ToString(),
                                Selected = sector.StoreID == x.ID
                            }).AsEnumerable();


            var points = (List<MapPoint>)new JsonSerializable(new List<MapPoint>()).FromString(PointList);

            ViewBag.Message = "";
            if (sector.Name.IsNullOrEmpty())
            {
                ViewBag.Message += "Необходимо указать название<br>";
            }
            if (!points.Any() || points.Count < 3)
            {
                ViewBag.Message += "Необходимо указать область на карте<br>";
            }

            if (((string)ViewBag.Message).IsFilled())
                return PartialView(sector);


            if (sector.ID == 0)
            {
                DB.MapSectors.InsertOnSubmit(sector);
            }
            DB.SubmitChanges();
            DB.MapPoints.DeleteAllOnSubmit(sector.MapPoints);
            DB.SubmitChanges();

            foreach (var point in points)
            {
                point.MapSector = sector;
                DB.MapPoints.InsertOnSubmit(point);

            }
            DB.SubmitChanges();
            if (Type == (int)SectorTypes.CarSector)
            {
                var newCars =
                    collection.AllKeys.Where(x => x.StartsWith("Car_"))
                        .Select(x => new { Key = x.Replace("Car_", "").ToInt(), Value = true }).ToList();

                var forDel = sector.MapSectorCars.Where(x => newCars.All(z => z.Key != x.CarID)).ToList();
                var forAdd = newCars.Where(x => sector.MapSectorCars.All(z => z.CarID != x.Key)).ToList();

                if (forDel.Any())
                {
                    DB.MapSectorCars.DeleteAllOnSubmit(forDel);
                    DB.SubmitChanges();
                }

                if (forAdd.Any())
                {
                    DB.MapSectorCars.InsertAllOnSubmit(
                        forAdd.Select(x => new MapSectorCar() { CarID = x.Key, SectorID = sector.ID }));
                    DB.SubmitChanges();
                }
            }
            else if (Type == (int)SectorTypes.CourierSector)
            {
                var newCouriers =
                   collection.AllKeys.Where(x => x.StartsWith("Courier_"))
                       .Select(x => new { Key = x.Replace("Courier_", "").ToInt(), Value = true }).ToList();

                var forDel = sector.MapSectorCouriers.Where(x => newCouriers.All(z => z.Key != x.WorkerID)).ToList();
                var forAdd = newCouriers.Where(x => sector.MapSectorCouriers.All(z => z.WorkerID != x.Key)).ToList();

                if (forDel.Any())
                {
                    DB.MapSectorCouriers.DeleteAllOnSubmit(forDel);
                    DB.SubmitChanges();
                }

                if (forAdd.Any())
                {
                    DB.MapSectorCouriers.InsertAllOnSubmit(
                        forAdd.Select(x => new MapSectorCourier() { WorkerID = x.Key, SectorID = sector.ID }));
                    DB.SubmitChanges();
                }

            }
            return PartialView("SectorList", DB.MapSectors.Where(x => x.ShopID == ShopID && x.Type == Type).ToList());
        }

        [HttpPost]
        public ActionResult SectorDelete(int ID, int ShopID, int Type)
        {
            ViewBag.Type = Type;
            var poly = DB.MapSectors.FirstOrDefault(x => x.ID == ID);
            if (poly != null)
            {
                DB.MapPoints.DeleteAllOnSubmit(poly.MapPoints);
                DB.SubmitChanges();
                DB.MapSectors.DeleteOnSubmit(poly);
                DB.SubmitChanges();
            }
            return PartialView("SectorList", DB.MapSectors.Where(x => x.ShopID == ShopID && x.Type == Type).ToList());
        }


        public ActionResult Workers()
        {
            return View(DB.Workers.Where(x => x.OwnerID == CurrentUser.ShopOwnerID).OrderBy(x => x.Name).ToList());
        }

        public ActionResult WorkerGroups()
        {
            return View(DB.WorkerGroups.Where(x => x.OwnerID == CurrentUser.ShopOwnerID).OrderBy(x => x.Name).ToList());
        }

        [HttpGet]
        public ActionResult EditWorker(int? ID)
        {
            var worker = DB.Workers.FirstOrDefault(x => x.ID == (ID ?? 0)) ?? new Worker()
            {
                Job = DB.Jobs.FirstOrDefault()
            };
            return View(worker);
        }

        [HttpPost]
        public ActionResult SaveGroup(int ID, string Name, string Participants)
        {
            var group = DB.WorkerGroups.FirstOrDefault(x => x.ID == ID);
            if (group == null)
            {
                group = new WorkerGroup() { OwnerID = CurrentUser.ShopOwnerID };
                DB.WorkerGroups.InsertOnSubmit(group);
            }
            group.Name = Name;

            DB.SubmitChanges();
            var pIds = Participants.Split<int>(";").ToList();
            var forDel = group.WorkerGroupParticipants.ToList().Where(x => !pIds.Contains(x.WorkerID)).ToList();
            var forAdd = pIds.Where(x => @group.WorkerGroupParticipants.All(z => z.WorkerID != x)).ToList();
            if (forDel.Any())
            {
                DB.WorkerGroupParticipants.DeleteAllOnSubmit(forDel);
                DB.SubmitChanges();
            }
            if (forAdd.Any())
            {
                DB.WorkerGroupParticipants.InsertAllOnSubmit(forAdd.Select(x => new WorkerGroupParticipant() { GroupID = group.ID, WorkerID = x }));
                DB.SubmitChanges();
            }
            return new ContentResult() { Content = "1" };
        }


        [HttpPost]
        public ActionResult EditWorker(Worker model)
        {
            Worker worker;
            bool newCar = false;
            if (model.ID == 0)
            {
                worker = new Worker() { OwnerID = CurrentUser.ShopOwnerID };
                DB.Workers.InsertOnSubmit(worker);
            }
            else
            {
                worker = DB.Workers.FirstOrDefault(x => x.ID == model.ID);
                if (worker == null)
                {
                    worker = new Worker() { OwnerID = CurrentUser.ShopOwnerID };
                    DB.Workers.InsertOnSubmit(worker);
                }
            }
            worker.LoadPossibleProperties(model, new[] { "OwnerID" });
            worker.IsPost = true;
            var job = DB.Jobs.First(x => x.ID == worker.JobID);
            worker.Job = job;
            if (worker.Name.IsNullOrEmpty() || worker.Phone.IsNullOrEmpty())
                return View(worker);
            DB.SubmitChanges();
            return RedirectToAction("Workers");
        }

        public ActionResult DeleteWorker(int ID)
        {
            var worker = DB.Workers.FirstOrDefault(x => x.ID == ID);
            if (worker != null)
            {
                DB.Workers.DeleteOnSubmit(worker);
                DB.SubmitChanges();
            }
            return RedirectToAction("Workers");
        }

        [HttpGet]
        public ActionResult EditWorkerGroup(int? ID)
        {
            var group = DB.WorkerGroups.FirstOrDefault(x => x.ID == (ID ?? 0)) ?? new WorkerGroup();
            return View(group);
        }

        public ActionResult DeleteWorkerGroup(int ID)
        {
            var group = DB.WorkerGroups.FirstOrDefault(x => x.ID == ID);
            if (group != null)
            {
                if (group.WorkerGroupParticipants.Any())
                {
                    DB.WorkerGroupParticipants.DeleteAllOnSubmit(group.WorkerGroupParticipants);
                }
                DB.WorkerGroups.DeleteOnSubmit(group);
                DB.SubmitChanges();
            }
            return RedirectToAction("WorkerGroups");
        }

        [HttpGet]
        public ActionResult Cars()
        {
            return View(DB.Cars.OrderBy(x => x.Name).ToList());
        }

        [HttpGet]
        public ActionResult EditCar(int? id)
        {
            return View(DB.Cars.FirstOrDefault(x => x.ID == (id ?? 0)) ?? new Car());
        }

        [HttpPost]
        public ActionResult EditCar(Car model)
        {
            Car entity;
            model.IsPost = true;
            if (model.Name.IsNullOrEmpty() || !model.Passengers.HasValue || model.Number.IsNullOrEmpty())
            {
                return View(model);
            }

            if (model.ID > 0)
            {
                var exist = DB.Cars.Where(x => x.Name.ToLower().Trim() == model.Name.ToLower().Trim() && x.ID != model.ID);
                if (exist.Any())
                {
                    model.ErrorText = "Транспорт с таким названием уже существует. Укажите другое название.";
                    return View(model);
                }
                entity = DB.Cars.FirstOrDefault(x => x.ID == model.ID);
                if (entity == null)
                {
                    entity = model;
                    entity.ID = 0;
                    entity.OwnerID = CurrentUser.ShopOwnerID;
                    DB.Cars.InsertOnSubmit(entity);
                }
                else
                {
                    entity.LoadPossibleProperties(model, new[] { "ID", "OwnerID" });
                }
            }
            else
            {
                var exist = DB.Cars.Where(x => x.Name.ToLower().Trim() == model.Name.ToLower().Trim());
                if (exist.Any())
                {
                    model.ErrorText = "Транспорт с таким названием уже существует. Укажите другое название.";
                    return View(model);
                }
                entity = model;
                entity.OwnerID = CurrentUser.ShopOwnerID;
                DB.Cars.InsertOnSubmit(entity);
            }
            DB.SubmitChanges();
            return RedirectToAction("Cars");
        }

        public ActionResult DeleteCar(int id)
        {
            var car = DB.Cars.FirstOrDefault(x => x.ID == id);
            if (car != null)
            {
                DB.Cars.DeleteOnSubmit(car);
                DB.SubmitChanges();
            }
            return RedirectToAction("Cars");
        }

        public ActionResult CourierZones(int? ShopID)
        {
            if (!ShopID.HasValue)
                ShopID = DefShopID;

            ViewBag.ShopID = ShopID;
            var zones = DB.MapSectors.Where(x => x.ShopID == ShopID && x.Type == (int)SectorTypes.CourierSector).ToList();
            return View(zones);
        }

        public ActionResult CheckList(string Date, int ShopID)
        {
            var d = Date.ToDate();
            if (!d.HasValue) return new ContentResult();

            /*
                        var options = new DataLoadOptions();
                        options.LoadWith((DeliveryListOrder x) => x.Order);
                        DB.LoadOptions = options;
            */

            var list = DB.DeliveryLists.FirstOrDefault(x => x.Date.Date == d.Value.Date && x.ShopID == ShopID);

            /*
                        var dtl = (DateTime?)Session["CheckList_" + Date.Date.ToString("yy-mm-dd")];
                        if (dtl.HasValue && DateTime.Now.Subtract(dtl.Value).TotalSeconds < 2)
                        {
                            return list;
                        }
                        else
                        {
                            Session["CheckList_" + Date.Date.ToString("yy-mm-dd")] = null;
                        }
                        Session["CheckList_" + Date.Date.ToString("yy-mm-dd")] = DateTime.Now;

            */
            var orders =
                DB.Orders.Where(
                    x => x.DeliveryDate.HasValue && x.DeliveryDate.Value.Date == d.Value.Date && x.ShopID == ShopID)
                    .ToList();

            var poly = new PolygonList(ShopID);

            foreach (var order in orders)
            {
                if (order.OrderDelivery == null)
                {
                    var delivery = new DeliveryAddress() { DeliveryType = (int)SectorTypes.CarSector };
                    DB.DeliveryAddresses.InsertOnSubmit(delivery);
                    order.OrderDelivery = delivery;
                    DB.SubmitChanges();
                }

                if (order.OrderDelivery.DeliveryType == (int)SectorTypes.CarSector && !order.CarID.HasValue)
                {
                    var cars = poly.GetDefaultCars(order);
                    if (cars.Any())
                    {
                        order.CarID = cars.First().ID;
                    }
                }
                if (order.OrderDelivery.DeliveryType == (int)SectorTypes.CourierSector && !order.WorkerID.HasValue)
                {
                    var couriers = poly.GetDefaultCouriers(order);
                    if (couriers.Any())
                    {
                        order.WorkerID = couriers.First().ID;
                    }
                }
            }
            DB.SubmitChanges();
            var stores = DB.Stores.Where(x => x.ShopStores.Any(z => z.ShopID == ShopID)).ToList();
            if (list == null)
            {
                list = new DeliveryList() { Approved = false, Date = d.Value.Date, ShopID = ShopID };
                DB.DeliveryLists.InsertOnSubmit(list);
                DB.SubmitChanges();



                int add = 1;
                if (stores.Any() && orders.Any())
                {

                    var workers = orders.Where(x => x.WorkerID.HasValue).Select(x => x.WorkerID).Distinct().ToList();
                    foreach (var worker in workers)
                    {


                        DB.DeliveryListOrders.InsertAllOnSubmit(
                            stores.Select(
                                (x, index) =>
                                    new DeliveryListOrder() { ListID = list.ID, StoreID = x.ID, OrderNum = index + add, Type = (int)SectorTypes.CourierSector, WorkerID = worker })
                                .ToList());

                        DB.SubmitChanges();

                        add += stores.Count;

                        var workerOrders = orders.Where(x => x.WorkerID == worker).ToList();

                        DB.DeliveryListOrders.InsertAllOnSubmit(
                            workerOrders.Select(
                                (x, index) =>
                                    new DeliveryListOrder() { ListID = list.ID, OrderID = x.ID, OrderNum = index + add })
                                .ToList());
                        DB.SubmitChanges();

                        add += workerOrders.Count;

                    }




                    var cars = orders.Where(x => x.CarID.HasValue).Select(x => x.CarID).Distinct().ToList();
                    foreach (var car in cars)
                    {
                        DB.DeliveryListOrders.InsertAllOnSubmit(
                            stores.Select(
                                (x, index) =>
                                    new DeliveryListOrder()
                                    {
                                        ListID = list.ID,
                                        StoreID = x.ID,
                                        OrderNum = index + add,
                                        Type = (int)SectorTypes.CarSector,
                                        CarID = car
                                    })
                                .ToList());

                        add += stores.Count;

                        var carOrders = orders.Where(x => x.CarID == car).ToList();

                        DB.DeliveryListOrders.InsertAllOnSubmit(
                            carOrders.Select(
                                (x, index) =>
                                    new DeliveryListOrder() { ListID = list.ID, OrderID = x.ID, OrderNum = index + add })
                                .ToList());
                        DB.SubmitChanges();

                        add += carOrders.Count;

                    }


                    var another = orders.Where(x => !x.WorkerID.HasValue && !x.CarID.HasValue);
                    DB.DeliveryListOrders.InsertAllOnSubmit(
                        another.Select(
                            (x, index) =>
                                new DeliveryListOrder() { ListID = list.ID, OrderID = x.ID, OrderNum = index + add })
                            .ToList());
                    DB.SubmitChanges();


                }
            }
            else
            {
                var old = DB.DeliveryListOrders.Where(x => x.ListID == list.ID && x.Store == null && x.Order == null).ToList();
                if (old.Any())
                {
                    DB.DeliveryListOrders.DeleteAllOnSubmit(old);
                    DB.SubmitChanges();
                }
                var newOrders = orders.Where(x => list.DeliveryListOrders.All(z => z.OrderID != x.ID)).ToList();
                if (newOrders.Any())
                {
                    var min = list.DeliveryListOrders.Any() ? (list.DeliveryListOrders.Max(x => x.OrderNum) + 1) : 1;
                    DB.DeliveryListOrders.InsertAllOnSubmit(newOrders.Select((x, index) => new DeliveryListOrder() { ListID = list.ID, OrderID = x.ID, OrderNum = min + index }));
                    DB.SubmitChanges();
                }
                var orderList =
                    DB.DeliveryListOrders.Where(x => x.ListID == list.ID && (x.Order.CarID.HasValue || x.Order.WorkerID.HasValue)).ToList();
                foreach (var listOrder in orderList)
                {
                    if (listOrder.Order.OrderDelivery.DeliveryType == (int)SectorTypes.CarSector)
                    {
                        var storeCheck =
                            DB.DeliveryListOrders.Where(
                                x => x.StoreID.HasValue && x.ListID == list.ID && x.CarID.HasValue && !x.OrderID.HasValue && x.CarID == listOrder.Order.CarID);
                        if (!storeCheck.Any())
                        {
                            var stl = stores.Select(
                                (x, index) =>
                                    new DeliveryListOrder()
                                    {
                                        ListID = list.ID,
                                        StoreID = x.ID,
                                        OrderNum = listOrder.OrderNum - index - 1,
                                        Type = (int)SectorTypes.CarSector,
                                        CarID = listOrder.Order.CarID
                                    })
                                .ToList();
                            DB.DeliveryListOrders.InsertAllOnSubmit(stl);
                            DB.SubmitChanges();
                        }
                    }
                    if (listOrder.Order.OrderDelivery.DeliveryType == (int)SectorTypes.CourierSector)
                    {
                        var storeCheck =
                            DB.DeliveryListOrders.Where(
                                x => x.StoreID.HasValue && x.ListID == list.ID && x.WorkerID.HasValue && !x.OrderID.HasValue && x.WorkerID == listOrder.Order.WorkerID);
                        if (!storeCheck.Any())
                        {
                            var stl = stores.Select(
                                (x, index) =>
                                    new DeliveryListOrder()
                                    {
                                        ListID = list.ID,
                                        StoreID = x.ID,
                                        OrderNum = listOrder.OrderNum - index - 1,
                                        Type = (int)SectorTypes.CourierSector,
                                        WorkerID = listOrder.Order.WorkerID
                                    })
                                .ToList();
                            DB.DeliveryListOrders.InsertAllOnSubmit(stl);
                            DB.SubmitChanges();
                        }
                    }
                }


                var emptyCheck =
                    DB.DeliveryListOrders.Where(
                        x => !x.OrderID.HasValue && x.ListID == list.ID && x.StoreID.HasValue).ToList();

                foreach (var st in emptyCheck)
                {
                    var suitable =
                        DB.DeliveryListOrders.Any(
                            x =>
                                x.OrderID.HasValue && x.ListID == list.ID && x.Order.OrderDelivery != null && x.Order.OrderDelivery.DeliveryType > 0 &&
                                (x.Order.OrderDelivery.DeliveryType == (int)SectorTypes.CarSector
                                    ? x.Order.CarID == st.CarID
                                    : x.Order.WorkerID == st.WorkerID));
                    if (!suitable)
                    {
                        DB.DeliveryListOrders.DeleteOnSubmit(st);
                    }
                }
                DB.SubmitChanges();

            }


            return new ContentResult();
        }

        public ActionResult CarRoutes(string Date, int ShopID)
        {
            var d = Date.ToDate();
            var list = d.HasValue
                ? DB.DeliveryLists.FirstOrDefault(x => x.Date.Date == d.Value.Date && x.ShopID == ShopID)
                : null;
            return PartialView(list);
        }

        public ActionResult CourierRoutes(string Date, int ShopID)
        {
            var d = Date.ToDate();
            var list = d.HasValue
                ? DB.DeliveryLists.FirstOrDefault(x => x.Date.Date == d.Value.Date && x.ShopID == ShopID)
                : null;
            return PartialView(list);
        }

        public ActionResult UndefRoutes(string Date, int ShopID)
        {
            var d = Date.ToDate();
            var list = d.HasValue
                ? DB.DeliveryLists.FirstOrDefault(x => x.Date.Date == d.Value.Date && x.ShopID == ShopID)
                : null;
            return PartialView(list);
        }
    }
}
