using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Trading.Extensions;

namespace Trading.Models
{
    public partial class MapPoint
    {
        private static MapPoint _moscow;
        public static MapPoint Moscow
        {
            get { return _moscow ?? (_moscow = new MapPoint() {Lat = (decimal)55.75138181658962, Lng = (decimal)37.61545372387525}); }
        }
    }

    public enum SectorTypes : int
    {
        Undefined = 0,
        CarSector = 1,
        CourierSector = 2
    }

    public partial class DeliveryListOrder : BaseDataObject
    {
        public string Name
        {
            get
            {
                if (Store != null)
                {
                    return Store.Name.Contains("Склад") ? Store.Name : "Склад " + Store.Name;
                }
                if (Order != null)
                {
                    return "Заказ " + Order.OrderNumberOrID + " на сумму " + Order.TotalSum;
                }
                return "--??--";
            }

        }

        public string NeedPositionHtml
        {
            get
            {
                if (this.Order.OrderDelivery == null || this.Order.OrderDelivery.MapPoint == null)
                {
                    return "<b style=\"color:red\">(!)</b>";
                }
                return "";
            }
        }

        public int? CarUID
        {
            get
            {
                if (Order != null)
                {
                    return Order.CarID;
                }
                return CarID;
            }
        }
        public int? WorkerUID
        {
            get
            {
                if (Order != null)
                {
                    return Order.WorkerID;
                }
                return WorkerID;
            }
        }

        public string PointDataJson
        {
            get
            {
                if (StoreID.HasValue)
                {
                    return Store.PointDataJson;
                }
                if (OrderID.HasValue)
                {
                    return Order.PointDataJson;
                }
                return new JsonSerializable(new PointData()
                {
                    FullAdress = "[NoData]",
                    Name = "[NoName]",
                    Lat = MapPoint.Moscow.Lat,
                    Lng = MapPoint.Moscow.Lng
                }).ToString();
            }
        }
    }

    public class RoutePointList : BaseDataObject
    {
        public RoutePointList()
        {
            Param = new RoutePointParam();
        }
        public RoutePointParam Param { get; set; }
        public RoutePointList(RoutePointParam param)
        {
            Param = param;
        }


        private List<Store> _existStores;
        [ScriptIgnore]
        public List<Store> ExistStores
        {
            get
            {
                
                return _existStores ??
                       (_existStores =
                           DB.DeliveryListOrders.Where(
                               x =>
                                   x.Type == Param.Type && x.Store != null &&
                                   (Param.Type == (int)SectorTypes.CarSector ? x.CarID == Param.Group : x.WorkerID == Param.Group))
                               .Select(x => x.Store)
                               .ToList());
            }
        }

        private List<Store> _stores;
        [ScriptIgnore]
        public List<Store> Stores
        {
            get
            {
                return _stores ??
                       (_stores =
                           DB.Stores.Where(x => x.OwnerID == CurrentUser.ShopOwnerID)
                               .ToList()
                               .Where(x => ExistStores.All(z => z.ID != x.ID))
                               .ToList());
            }
        }

        private List<Order> _existOrders;
        [ScriptIgnore]
        public List<Order> ExistOrders
        {
            get
            {
                return _existOrders ??
                       (_existOrders = Param.Date.HasValue ?
                           DB.DeliveryListOrders.Where(x => x.DeliveryList.Date.Date == Param.Date.Value.Date && x.Order != null && x.DeliveryList.Approved)
                               .Select(x => x.Order)
                               .ToList() : new List<Order>());
            }
        }

        private List<Order> _orders;
        [ScriptIgnore]
        public List<Order> Orders
        {
            get
            {
                return _orders ??
                       (_orders = Param.Date.HasValue ?
                           DB.Orders.Where(
                               x =>
                                   x.DeliveryDate.HasValue && x.DeliveryDate.Value.Date == Param.Date.Value.Date &&
                                   x.ShopID == Param.ShopID && !ExistOrders.Select(z => z.ID).ToList().Contains(x.ID))
                                   .ToList() : new List<Order>());
            }
        }
        [ScriptIgnore]
        public bool Any
        {
            get { return Orders.Any() || Stores.Any(); }
        }

    }

    public class RoutePointParam
    {
        public int Group { get; set; }
        public int Type { get; set; }
        public DateTime? Date { get; set; }



        public int ShopID { get; set; }
      
    }

    public partial class DeliveryList : BaseDataObject
    {
        public List<DeliveryListOrder> GetStoresForGroup(IGrouping<int?, DeliveryListOrder> @group, SectorTypes type)
        {

            if (type == SectorTypes.CarSector)
            {
                var list = CarPoints.Where(x => x.Store != null && x.Type == (int)type);
                return list.Where(x => x.CarID == @group.Key).OrderBy(x => x.OrderNum).ToList();
            }
            if (type == SectorTypes.CourierSector)
            {
                var list = CourierPoints.Where(x => x.Store != null && x.Type == (int)type);
                return list.Where(x => x.WorkerID == @group.Key).OrderBy(x => x.OrderNum).ToList();
            }
            return new List<DeliveryListOrder>();
        }

        private List<DeliveryListOrder> _carPoints;
        public List<DeliveryListOrder> CarPoints
        {
            get
            {
                return _carPoints ??
                       (_carPoints =
                           DeliveryListOrders.Where(
                               x =>
                                   (x.Store != null && x.Type == (int)SectorTypes.CarSector) ||
                                   (x.Order != null && x.Order.OrderDelivery.DeliveryType == (int)SectorTypes.CarSector && x.Order.CarID.HasValue && x.Order.OrderDelivery.MapPoint != null))
                               .OrderBy(x => x.OrderNum)
                               .ToList());


            }
        }

        private List<DeliveryListOrder> _carPointsUndef;
        public List<DeliveryListOrder> CarPointsUndef
        {
            get
            {
                return _carPointsUndef ??
                       (_carPointsUndef =
                           DeliveryListOrders.Where(
                               x =>
                                   (x.Order != null && x.Order.OrderDelivery.DeliveryType == (int)SectorTypes.CarSector && (!x.Order.CarID.HasValue || x.Order.OrderDelivery.MapPoint == null)))
                               .OrderBy(x => x.OrderNum)
                               .ToList());
            }
        }
        private List<DeliveryListOrder> _courierPoints;
        public List<DeliveryListOrder> CourierPoints
        {
            get
            {
                return _courierPoints ??
                       (_courierPoints =
                           DeliveryListOrders.Where(
                               x =>
                                   (x.Store != null && x.Type == (int)SectorTypes.CourierSector) ||
                                   (x.Order != null && x.Order.OrderDelivery.DeliveryType == (int)SectorTypes.CourierSector && x.Order.WorkerID.HasValue && x.Order.OrderDelivery.MapPoint != null))
                               .OrderBy(x => x.OrderNum)
                               .ToList());
            }
        }


        private List<DeliveryListOrder> _courierPointsUndef;
        public List<DeliveryListOrder> CourierPointsUndef
        {
            get
            {
                return _courierPointsUndef ??
                       (_courierPointsUndef =
                           DeliveryListOrders.Where(
                               x =>

                                   (x.Order != null &&
                                    x.Order.OrderDelivery.DeliveryType == (int)SectorTypes.CourierSector &&
                                    (!x.Order.WorkerID.HasValue || x.Order.OrderDelivery.MapPoint == null)))
                               .OrderBy(x => x.OrderNum)
                               .ToList());
            }
        }

        private List<DeliveryListOrder> _carOrders;
        public List<DeliveryListOrder> CarOrders
        {
            get
            {
                return _carOrders ??
                       (_carOrders =
                           DeliveryListOrders.Where(
                               x => x.Order != null && x.Order.OrderDelivery.DeliveryType == (int)SectorTypes.CarSector && x.Order.OrderDelivery.MapPoint != null)
                               .OrderBy(x => x.OrderNum)
                               .ToList());
            }
        }
        private List<DeliveryListOrder> _courierOrders;
        public List<DeliveryListOrder> CourierOrders
        {
            get
            {
                return _courierOrders ??
                       (_courierOrders =
                           DeliveryListOrders.Where(
                               x =>
                                   x.Order != null &&
                                   x.Order.OrderDelivery.DeliveryType == (int)SectorTypes.CourierSector && x.Order.OrderDelivery.MapPoint != null)
                               .OrderBy(x => x.OrderNum)
                               .ToList());
            }
        }
        private List<DeliveryListOrder> _undefOrders;
        public List<DeliveryListOrder> UndefOrders
        {
            get
            {
                return _undefOrders ??
                       (_undefOrders =
                           DeliveryListOrders.Where(
                               x =>
                                   x.Order != null &&
                                   (x.Order.OrderDelivery.DeliveryType == (int)SectorTypes.Undefined))
                               .ToList());
            }
        }



        public bool HasReturnPoint(List<DeliveryListOrder> stores)
        {
            return stores.Any(x => x.Store != null && x.ReturnPoint);
        }
        public DeliveryListOrder GetReturnPoint(List<DeliveryListOrder> stores)
        {
            return stores.FirstOrDefault(x => x.Store != null && x.ReturnPoint) ?? new DeliveryListOrder() { StoreID = 0 };
        }


        public bool IsShowOnMap(IGrouping<int?, DeliveryListOrder> group)
        {
            return group.Any(x => x.ShowOnMap);
        }

        public int GetMaxOrderNum(int @group, int type)
        {
            if (type == (int) SectorTypes.CarSector)
            {
                var g = CarPoints;
                return g.Any() ? g.Max(x => x.OrderNum) : 0;
            }
            if (type == (int) SectorTypes.CarSector)
            {
                var g = CourierPoints;
                return g.Any() ? g.Max(x => x.OrderNum) : 0;
            }
            return 0;
        }

        public string GetColor(IGrouping<int?, DeliveryListOrder> @group, SectorTypes sectorType)
        {
            if (sectorType == SectorTypes.CarSector)
            {
                var car = group.FirstOrDefault(x => x.CarID.HasValue);
                if (car == null)
                    return "#000000";
                return car.Car.Color;
            }
            if (sectorType == SectorTypes.CourierSector)
            {
                var car = group.FirstOrDefault(x => x.WorkerID.HasValue);
                if (car == null)
                    return "#000000";
                return car.Worker.Color;
            }
            return "#000000";
        }

        public decimal GetWeightForGroup(IGrouping<int?, DeliveryListOrder> @group)
        {
            return group.Where(x=> x.Order!=null).Sum(x => x.Order.TotalWeight);
        }
        public bool IsWeightCorrectForGroup(IGrouping<int?, DeliveryListOrder> @group)
        {
            return !group.Where(x=> x.Order!=null).Any(x=> x.Order.OrderedProducts.Any(z=> !z.Product.Weight.HasValue));
        }
    }

    public partial class Car : BaseDataObject
    {
        public bool IsSizesCorrect()
        {
            return Width.HasValue && Height.HasValue && Length.HasValue;
        }

        public bool IsOverSized(IGrouping<int?, DeliveryListOrder> @group)
        {
            return
                group.Where(x => x.Order != null)
                    .Any(x => x.Order.OrderedProducts.Any(z => (z.Product.Width ?? 0) > this.Width) || x.Order.OrderedProducts.Any(z => (z.Product.Height ?? 0) > this.Height) || x.Order.OrderedProducts.Any(z => (z.Product.Length ?? 0) > this.Length));
        }

        public string DriverPhone
        {
            get
            {
                var driver = WorkerGroup.WorkerGroupParticipants.FirstOrDefault(x => x.Worker.Job.HasCar);
                if (driver == null)
                    return "--не указан--";
                return driver.Worker.Phone;
            }
        }
    }



    public partial class Worker : BaseDataObject
    {
        private List<Job> _availableJobs;
        public List<Job> AvailableJobs
        {
            get { return _availableJobs ?? (_availableJobs = DB.Jobs.OrderBy(x => x.Name).ToList()); }
        }

    }


    public partial class MapSector
    {
        public string PointList
        {
            get
            {
                var result =
                    MapPoints.OrderBy(x => x.Num).Select(x => new { Lat = x.Lat, Lng = x.Lng, Num = x.Num }).ToArray();

                return new JsonSerializable(result).ToString();
            }
        }
    }



    public class JsonSerializable
    {
        private object _serial;

        public object Object
        {
            get { return _serial; }
        }
        public JsonSerializable()
        {
            _serial = this;
        }
        public JsonSerializable(object obj)
        {
            _serial = obj;
        }
        public override string ToString()
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(_serial);
        }

        public object FromString(string value)
        {
            var des = new JavaScriptSerializer();
            var obj = des.Deserialize(value, _serial.GetType());
            _serial.LoadPossibleProperties(obj);
            return obj;
        }
    }

    public class PointSettings
    {
        public string LatInputName { get; set; }
        public string LngInputName { get; set; }
        public string RegionName { get; set; }
        public string TownName { get; set; }
        public string StreetName { get; set; }
        public string HouseName { get; set; }

        public int AddressDetectionEnabled { get; set; }
        public string DistrictName { get; set; }

        public override string ToString()
        {
            return new JsonSerializable(this).ToString();
        }
    }


    public class PointData
    {
        public string Name { get; set; }
        public string FullAdress { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }

    public partial class Store
    {
        public bool IsPost { get; set; }

        public string PointDataJson
        {
            get
            {

                var pointData = new PointData();
                pointData.Name = Name;

                if (DeliveryAddress != null)
                {
                    pointData.FullAdress = DeliveryAddress.FullAddress;
                }
                if (DeliveryAddress != null && DeliveryAddress.MapPoint != null)
                {
                    pointData.Lat = DeliveryAddress.MapPoint.Lat;
                    pointData.Lng = DeliveryAddress.MapPoint.Lng;
                }
                return new JsonSerializable(pointData).ToString();
            }
        }
    }

    public partial class DeliveryAddress
    {
        public decimal FullCost
        {
            get
            {
                decimal sum = 0;
                if (Cost.HasValue)
                    sum += Cost.Value;
                if (LiftCost.HasValue)
                    sum += LiftCost.Value;
                return sum;
            }
        }
        public bool HasPoint
        {
            get { return MapPoint != null; }
        }
        public decimal Lat
        {
            get { return MapPoint == null ? (decimal)55.75222 : MapPoint.Lat; }
            set
            {
                if (MapPoint == null)
                {
                    MapPoint = new MapPoint();
                }
                MapPoint.Lat = value;
            }
        }
        public decimal Lng
        {
            get { return MapPoint == null ? (decimal)37.61556 : MapPoint.Lng; }
            set
            {
                if (MapPoint == null)
                {
                    MapPoint = new MapPoint();
                }
                MapPoint.Lng = value;

            }
        }


    }
}