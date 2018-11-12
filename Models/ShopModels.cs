using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Services.Description;
using Trading.Extensions;
using WebMatrix.WebData;

namespace Trading.Models
{
    public partial class OrderedProduct
    {
        public void SaveChars(DataContext DB, List<ProductModelChar> chars)
        {
            var exist = new List<ProductChar>();
            foreach (var ch in chars)
            {
                var chd = ProductChars.FirstOrDefault(x => String.Equals(x.Name, ch.Name, StringComparison.CurrentCultureIgnoreCase));
                if (chd != null)
                {
                    exist.Add(chd);
                    chd.Value = ch.Value;
                }
                else
                {
                    var c = new ProductChar()
                    {
                        Name = ch.Name,
                        Value = ch.Value,
                        OrderedProductID = ID
                    };
                    exist.Add(c);
                    DB.ProductChars.InsertOnSubmit(c);
                }
            }
            DB.SubmitChanges();

            var foreDel = ProductChars.Where(x => exist.All(z => z.ID != x.ID));
            DB.ProductChars.DeleteAllOnSubmit(foreDel);
            DB.SubmitChanges();
        }

        private decimal? _priceWithoutNDS;
        public decimal PriceWithoutNDS
        {
            get
            {
                if (!_priceWithoutNDS.HasValue)
                    _priceWithoutNDS = Price / (decimal)1.18;
                return _priceWithoutNDS.Value;
            }
        }

        private decimal? _NDS;
        public decimal NDS
        {
            get
            {
                if (!_NDS.HasValue)
                {
                    _NDS = Price - Price / (decimal)1.18;
                }
                return _NDS.Value;
            }
        }

        public decimal? SumWeight
        {
            get { return Product.Weight.HasValue ? Product.Weight.Value * Amount : (decimal?)null; }
        }
        public decimal? SumVolume
        {
            get
            {
                return Product != null && Product.Width.HasValue && Product.Height.HasValue && Product.Length.HasValue ? (Product.Width.Value * Product.Height.Value * Product.Length.Value) :
                (decimal?)null;
            }
        }

        public decimal SumPrice
        {
            get { return Amount * Price; }
        }
    }

    public class ProductUnit
    {
        public string Name { get; set; }
        public string UnitCode { get; set; }

        public bool DefVal { get; set; }

        public static List<ProductUnit> CodeList = new List<ProductUnit>()
        {
            new ProductUnit() {Name = "шт", UnitCode = "796", DefVal = true},
            new ProductUnit() {Name = "кг", UnitCode = "166"},
            new ProductUnit() {Name = "м", UnitCode = "006"},
            new ProductUnit() {Name = "кв. м", UnitCode = "055"},
            new ProductUnit() {Name = "куб. м", UnitCode = "113"},
            new ProductUnit() {Name = "пог. метр", UnitCode = "018"}
            
        };
        public static string GetNameByCode(string code)
        {
            return CodeList.Any(x => x.UnitCode == code) ? CodeList.First(x => x.UnitCode == code).Name : "";
        }
        public static string GetCodeByName(string name)
        {
            return CodeList.Any(x => x.Name == name) ? CodeList.First(x => x.Name == name).UnitCode : "";
        }
    }

    public partial class Product : BaseDataObject
    {
        public string AmountUnitName
        {
            get { return ProductUnit.GetNameByCode(UnitCode); }
        }

        public void SaveShopRel(int shopID)
        {
            var rels = DB.ShopProducts.FirstOrDefault(x => x.ShopID == shopID && x.ProductID == this.ID);
            if (rels == null)
            {
                rels = new ShopProduct() { ProductID = this.ID, ShopID = shopID };
                DB.ShopProducts.InsertOnSubmit(rels);
                DB.SubmitChanges();
            }
        }
    }

    public partial class DeliveryAddress
    {
        public string FullAddress
        {
            get
            {

                string[] names = new[] { "Region", "Town", "Street", "House", "Building", "Section", "Doorway", "Code", "Floor", "Flat" };
                string[] prefix = new[] { "", "", "", "д. ", "стр. ", "корп. ", "подъезд ", "код домофона ", "этаж ", "кв. " };

                var value =
                    names.Select((x, index) => new { Val = this.GetPropertyValue(x), Index = index })
                        .Where(x => x.Val != null)
                        .Select(x => string.Format("{0}{1}", prefix[x.Index], x.Val))
                        .JoinToString(", ");

                return value;
            }
        }
    }

    public partial class Order : BaseDataObject
    {

        public string StatusText
        {
            get
            {
                if (Status == (int)OrderStatus.Deleted)
                {
                    return "Удален";
                }
                if (Status == (int)OrderStatus.Filling)
                {
                    return "Заполнение";
                }
                if (Status == (int)OrderStatus.WaitingDelivery)
                {
                    return "Ожидает отгрузки";
                }
                if (Status == (int)OrderStatus.Payed)
                {
                    return "Оплачен";
                }
                if (Status == (int)OrderStatus.Delivered)
                {
                    return "Доставлен";
                }
                if (Status == (int)OrderStatus.Marked)
                {
                    return "Оценен";
                }
                return "";
            }
        }

        private static IEnumerable<SelectListItem> _statusList;
        public static IEnumerable<SelectListItem> StatusList
        {
            get
            {
                return _statusList ?? (_statusList = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Text = "Заполнение",
                        Value = "-1",
                    },
                    new SelectListItem()
                    {
                        Text = "В обработке",
                        Value = "0",
                    },
                    new SelectListItem()
                    {
                        Text = "Все статусы",
                        Value = ""
                    }

                });
            }
        }

        public decimal TotalSum
        {
            get { return OrderedProducts.Sum(x => x.Price * x.Amount); }
        }

        public string FullUrl
        {
            get { return "/Shop/ViewOrder/" + ID; }
        }

        public string StatusCSS
        {
            get
            {
                if (Status == -10)
                {
                    return "gradeU";
                }
                if (Status == -1)
                {
                    return "gradeC";
                }
                if (IsImportant)
                {
                    return "gradeX";
                }
                return "gradeA";

            }
        }

        public string StatusCSSForOper
        {
            get
            {
                if (Marks.Any())
                {
                    return "gradeG";
                }
                else
                {
                    return "gradeY";
                }
            }
        }

        public string OrderNumberOrID
        {
            get
            {
                if (OrderNumber.IsFilled())
                    return OrderNumber;
                return "[{0}]".FormatWith(ID.ToString("d8"));
            }
        }

        public Mark OrderMark
        {
            get
            {
                if (Marks.Any())
                    return Marks.First();
                return new Mark();
            }
        }

        public string Requisites
        {
            get
            {
                if (Shop == null)
                    return "";
                return string.Format("{0}, ИНН {1}, {2}, {3}, р/с {4}, в банке {5}, БИК {6}, к/с {7}",
                    Shop.GetSetting("OrgName"), Shop.GetSetting("INN"), Shop.GetSetting("Address"),
                    Shop.GetSetting("Phones"), Shop.GetSetting("OrgAccount"), Shop.GetSetting("Bank"),
                    Shop.GetSetting("Bik"), Shop.GetSetting("Korr"));
            }
        }

        public string Receiver
        {
            get
            {
                if (Consumer == null)
                    return "";
                return string.Format("{0}, {1}, тел.: {2}", Consumer.FullName, DeliveryAddress, Consumer.Phone);
            }
        }

        public void SaveChars(DataContext DB, List<OrderChar> charList)
        {
            var exist = new List<OrderChar>();
            foreach (var ch in charList)
            {
                var chd = OrderChars.FirstOrDefault(x => String.Equals(x.Name, ch.Name, StringComparison.CurrentCultureIgnoreCase));
                if (chd != null)
                {
                    exist.Add(chd);
                    chd.Value = ch.Value;
                }
                else
                {
                    var c = new OrderChar()
                    {
                        Name = ch.Name,
                        Value = ch.Value,
                        OrderID = ID
                    };
                    exist.Add(c);
                    DB.OrderChars.InsertOnSubmit(c);
                }
            }
            DB.SubmitChanges();

            var foreDel = OrderChars.Where(x => exist.All(z => z.ID != x.ID));
            DB.OrderChars.DeleteAllOnSubmit(foreDel);
            DB.SubmitChanges();
        }

        private User _currentUser;
        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    var db = new DataContext();
                    _currentUser = db.Users.FirstOrDefault(x => x.ID == WebSecurity.CurrentUserId);
                }
                return _currentUser;
            }
            set { _currentUser = value; }
        }



        private List<ShopProductChar> _charList;
        public List<ShopProductChar> CharList
        {
            get
            {
                if (_charList == null)
                {
                    _charList = new List<ShopProductChar>();
                    foreach (var ch in from product in OrderedProducts from ch in product.ProductChars where _charList.All(z => z.Name.ToLower() != ch.Name.ToLower()) select ch)
                    {
                        _charList.Add(new ShopProductChar() { Name = ch.Name });
                    }
                    _charList = _charList.OrderBy(x => x.Name).ToList();
                }
                return _charList;

            }
        }

        private List<ShopProductChar> _orderCharList;
        public List<ShopProductChar> OrderCharList
        {
            get
            {
                if (_charList == null)
                {
                    var db = new DataContext();
                    _orderCharList = db.ShopProductChars.Where(
                        x => x.IsMain && x.Type >= 2 && (
                            (x.UserID == CurrentUser.ShopOwnerID && !x.ShopID.HasValue) ||
                            (x.ShopID == (ShopID ?? 0) && x.UserID == CurrentUser.ShopOwnerID)))
                        .OrderBy(x => x.Name).ToList();
                }
                return _orderCharList;

            }
        }

        public string OrderedProductsList
        {
            get { return OrderedProducts.Select(x => x.Product.Name).JoinToString(", "); }
        }

        public DeliveryAddress OrderDeliveryOrNull
        {
            get
            {
                if (OrderDelivery == null)
                    return new DeliveryAddress();
                return OrderDelivery;
            }
        }

        public string Artikuls
        {
            get
            {
                return OrderedProducts.Where(x => x.Product != null).Select(x => x.Product.Article).Distinct().JoinToString(", ");
            }
        }
        public string Products
        {
            get
            {
                return OrderedProducts.Where(x => x.Product != null).Select(x => x.Product.Name).Distinct().JoinToString(", ");
            }
        }

        public string PointDataJson
        {
            get
            {
                var data = new PointData()
                {
                    Name = OrderNumberOrID,
                    FullAdress = OrderDeliveryOrNull.FullAddress,
                    Lat = OrderDeliveryOrNull.Lat,
                    Lng = OrderDeliveryOrNull.Lng
                };
                return new JsonSerializable(data).ToString();
            }
        }

        private OrderPointData _orderPointData;
        public OrderPointData OrderPointData
        {
            get
            {
                return _orderPointData ?? (_orderPointData =
                    new OrderPointData()
                    {
                        StoreID = /*(Shop.ShopStores.FirstOrDefault() ?? new Store()).ID*/ 0,
                        ID = ID,
                        Name = OrderNumberOrID,
                        Adress = OrderDeliveryOrNull.FullAddress,
                        Driver = (Car ?? new Car() { Name = "" }).Name,
                        DriverID = (Car ?? new Car() { ID = 0 }).ID,
                        HasPoint = OrderDeliveryOrNull.HasPoint,
                        PointData = new PointData()
                        {
                            FullAdress = OrderDeliveryOrNull.FullAddress,
                            Lat = OrderDeliveryOrNull.Lat,
                            Lng = OrderDeliveryOrNull.Lng
                        }
                    });
            }
        }

        private Contractor _contractor;
        public Contractor Contractor
        {
            get
            {
                if (_contractor == null)
                {
                    if (ContractedOrder.ID > 0)
                    {
                        _contractor = ContractedOrder.Contractor;
                    }
                    else
                    {
                        _contractor = new Contractor() { Shop = new Shop() { } };
                    }
                }
                return _contractor;
            }
        }

        private ContractedOrder _contractedOrder;
        public ContractedOrder ContractedOrder
        {
            get
            {
                if (_contractedOrder == null)
                {
                    if (ContractedOrders.Any(x => x.SenderID == CurrentUser.ShopOwnerID))
                    {
                        _contractedOrder = ContractedOrders.First(x => x.SenderID == CurrentUser.ShopOwnerID);
                    }
                    else
                    {
                        _contractedOrder = new ContractedOrder() { Contractor = new Contractor() { Shop = new Shop() } };
                    }
                }
                return _contractedOrder;
            }
        }

        public decimal TotalCostForContractor
        {
            get
            {
                return
                    OrderedProducts.Where(x => x.Product != null)
                        .Sum(
                            x =>
                                (x.Product.OptCost.HasValue
                                    ? x.Product.OptCost.Value
                                    : x.Price) * x.Amount /*(x.Product.SelfCost.HasValue
                                        ? x.Product.SelfCost.Value * ((100 + ProfitPercent) / 100)
                                        : x.Price)*/);
            }
        }

        public decimal ProfitPercent
        {
            get
            {
                if (TotalSum == 0)
                    return 0;
                return TotalProfit * 100 / TotalSum;
            }
        }
        public decimal ProfitPercentOpt
        {
            get
            {
                if (OptSum == 0)
                    return 0;
                return TotalProfit * 100 / OptSum;
            }
        }

        public decimal TotalSelfCost
        {
            get
            {
                return
                    OrderedProducts.Where(x => x.Product != null && x.Product.SelfCost.HasValue)
                        .Sum(x => x.Product.SelfCost.Value * x.Amount) +
                    OrderedProducts.Where(
                        x => x.Product != null && !x.Product.SelfCost.HasValue && x.Product.OptCost.HasValue)
                        .Sum(x => x.Product.OptCost.Value * x.Amount);
            }
        }

        public decimal ContractorPercent
        {
            get
            {
                if (ContractedOrder.ID > 0)
                {
                    return ContractedOrder.ContractedCost / (TotalSum == 0 ? 1 : TotalSum);
                }
                else
                {
                    return 0;
                }
            }
        }

        public decimal TotalProfit
        {
            get
            {
                return TotalSum - ContractedOrder.ContractedCost - TotalSelfCost - DeliveryCost;
            }
        }
        public decimal OptSum
        {
            get
            {
                return
                    OrderedProducts.Where(x => x.Product != null && x.Product.OptCost.HasValue)
                        .Sum(x => x.Product.OptCost.Value * x.Amount) +
                    OrderedProducts.Where(x => x.Product != null && !x.Product.OptCost.HasValue)
                        .Sum(x => x.Price * x.Amount);
            }
        }

        public decimal DeliveryCost
        {
            get { return (OrderDeliveryOrNull.Cost ?? 0) + (OrderDeliveryOrNull.LiftCost ?? 0); }
        }

        public int TotalCount
        {
            get { return OrderedProducts.Sum(x => x.Amount); }
        }

        public decimal TotalWeight
        {
            get { return OrderedProducts.Select(x => (x.Product.Weight ?? 0)).Sum(); }
        }
        public decimal TotalVolume
        {
            get
            {
                return
                    OrderedProducts.Select(x => ((x.Product.Width ?? 0) * (x.Product.Height ?? 0) * (x.Product.Length ?? 0)))
                        .Sum();
            }
        }

        private Customer _customer;
        public Customer Customer
        {
            get
            {
                if (_customer != null) return _customer;
                if (Consumer != null)
                {
                    _customer = Consumer.ToCustomer(this);
                }
                else
                {
                    _customer = new Customer();
                    _customer.Message = "Данные покупателя не указаны!";
                }
                _customer.OrderID = ID;
                return _customer;
            }
        }

        private bool? _isDeliveryApproved;
        public bool IsDeliveryApproved
        {
            get
            {
                if (!_isDeliveryApproved.HasValue)
                {
                    _isDeliveryApproved = DeliveryListOrders.Any(x => x.DeliveryList.Approved);
                }
                return _isDeliveryApproved.Value;
            }
        }


        public decimal GetPercent(int ProductID)
        {
            var pCost = OrderedProducts.Where(x => x.ProductID == ProductID).Sum(x => x.Price * x.Amount);
            var sum = TotalSum;
            if (sum == 0)
                return 0;
            return pCost * 100 / sum;
        }
    }


    public class OrderPointData
    {
        public PointData PointData { get; set; }
        public string Name { get; set; }
        public string Driver { get; set; }
        public bool HasPoint { get; set; }
        public int ID { get; set; }
        public int StoreID { get; set; }
        public int DriverID { get; set; }
        public string Adress { get; set; }
    }

    public partial class Mark
    {
        public decimal OverageTotal
        {
            get { return (OveragePrice + OverageOperator + OverageDelivery + OverageQuality) / 4; }
        }
        public decimal Total
        {
            get { return (decimal)(Price + Operator + Delivery + Quality) / 4; }
        }
        public decimal OveragePrice { get; set; }
        public decimal OverageOperator { get; set; }
        public decimal OverageDelivery { get; set; }
        public decimal OverageQuality { get; set; }

        public string GetMarkColor(decimal overageTotal)
        {
            if (overageTotal == 0)
                return "";
            if (overageTotal < 2)
                return "m-red";
            if (overageTotal >= 2 & overageTotal < 4)
                return "m-orange";
            return "m-green";
        }
    }

    public partial class Shop : BaseDataObject
    {

        public string GetSetting(string key)
        {
            var setting = ShopSettings.FirstOrDefault(x => String.Equals(x.ItemKey, key, StringComparison.CurrentCultureIgnoreCase));
            if (setting != null)
                return setting.Value;
            return "";
        }

        public const int CalcPeriod = -14;
        public bool Selected { get; set; }
        public bool IsPost { get; set; }

        private Mark _overageMark;
        public Mark OverageMark
        {
            get
            {
                if (_overageMark == null)
                {
                    var marks = Orders.Where(x => x.Marks.Any() && x.Marks.First().MarkDate > DateTime.Now.AddDays(CalcPeriod)).Select(x => x.Marks.FirstOrDefault()).Where(x => x != null).ToList();
                    _overageMark = new Mark();

                    var cnt = marks.Count();
                    if (cnt == 0)
                        cnt = 1;
                    _overageMark.OveragePrice = (decimal)marks.Sum(x => x.Price) / cnt;
                    _overageMark.OverageOperator = (decimal)marks.Sum(x => x.Operator) / cnt;
                    _overageMark.OverageDelivery = (decimal)marks.Sum(x => x.Delivery) / cnt;
                    _overageMark.OverageQuality = (decimal)marks.Sum(x => x.Quality) / cnt;
                }
                return _overageMark;
            }
        }

        public string OrgNameOrName
        {
            get
            {
                var res = GetSetting("OrgName");
                if (res.IsNullOrEmpty())
                    res = Name;
                if (res.IsNullOrEmpty())
                    res = "<<Без имени>>";
                return res;
            }
        }
        public string OrgNameOrNameOrEmpty
        {
            get
            {
                var res = GetSetting("OrgName");
                if (res.IsNullOrEmpty())
                    res = Name;
                if (res.IsNullOrEmpty())
                    res = "";
                return res;
            }
        }

        public bool IsCharExist(string name, int? ShopID)
        {
            if (!ShopID.HasValue)
            {
                return
                    DB.ShopProductChars.Any(
                        x => x.UserID == CurrentUser.ShopOwnerID && x.Name.ToLower() == name.ToLower() && x.Type >= 2 && (x.ShopID == this.ID || !x.ShopID.HasValue));

            }
            else
            {

                return
                    DB.ShopProductChars.Any(
                        x => x.UserID == CurrentUser.ShopOwnerID && x.Name.ToLower() == name.ToLower() && x.Type >= 2 && (x.ShopID == ShopID || !x.ShopID.HasValue));

            }

        }
    }

    public partial class ShopProductChar
    {
        public bool IsPost { get; set; }
    }

    public class Customer
    {
        public bool IsPost { get; set; }
        public string Message { get; set; }
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patrinomic { get; set; }
        public bool? Sex { get; set; }
        public string Phone { get; set; }
        public string AddPhone { get; set; }
        public string Email { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryAdress { get; set; }
        public bool ForPopup { get; set; }
        public Order Order { get; set; }
    }

    public class ProductModelChar
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int ID { get; set; }

    }

    public class ProductModel : BaseDataObject
    {
        public int OrderID { get; set; }
        public int? OrderedProductID { get; set; }
        public bool IsPost { get; set; }

        public bool IsSuccess { get; set; }

        public string RedirectURL { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? Amount { get; set; }
        public decimal? Weight { get; set; }

        public string UnitCode { get; set; }
        public List<ProductModelChar> Chars { get; set; }
        public decimal? SelfCost { get; set; }
        public decimal? OptCost { get; set; }
        public int? ID { get; set; }

        public OrderedProduct ToOrderedProduct()
        {
            var p = new OrderedProduct()
            {
                Amount = Amount ?? 0,
                Price = Price ?? 0,
            };

            /*
                        var chars =
                            Chars.Select(x => new ProductChar() { Name = x.Name.ToNiceForm(), Value = x.Value, OrderedProduct = p }).Where(x => x.Name.IsFilled() && x.Value.IsFilled()).ToList();
            */

            return p;
        }

        public bool IsCharExist(string name, int? ShopID)
        {
            if (!ShopID.HasValue)
            {
                if (OrderID == 0)
                    return false;
                var order = DB.Orders.FirstOrDefault(x => x.ID == OrderID);
                if (order == null || order.Shop == null)
                    return false;
                ShopID = order.ShopID;
            }
            return
                DB.ShopProductChars.Any(
                    x => x.UserID == CurrentUser.ShopOwnerID && x.Name.ToLower() == name.ToLower() && x.Type <= 2 && (!x.ShopID.HasValue || x.ShopID == ShopID));


        }

        public bool CanCreateFormula(string priceName)
        {
            return true;
        }

        public bool IsFormulaExist(string priceName)
        {
            return false;
        }
    }


    public partial class Consumer
    {
        public string FullName
        {
            get { return Surname + " " + Name + " " + Patrinomic; }
        }

        public Customer ToCustomer(Order order)
        {
            var model = new Customer();
            model.CustomerID = ID;
            model.Order = order;
            model.Patrinomic = Patrinomic;
            model.Phone = Phone;
            model.Surname = Surname;
            model.Name = Name;
            model.Email = Email;
            model.AddPhone = AddPhone;
            model.DeliveryAdress = order.OrderDeliveryOrNull.FullAddress;
            model.DeliveryDate = order.DeliveryDate;
            model.Sex = Sex;
            return model;

        }
    }

    public class OrderFilter : BaseDataObject
    {
        public static List<string> Regions
        {
            get
            {
                var cook = HttpContext.Current.Request.Cookies["Regions"];
                if (cook != null && cook.Value.IsFilled())
                {
                    return HttpUtility.UrlDecode(cook.Value).Split<string>(";").Where(x => x.IsFilled()).OrderBy(x => x).ToList();
                }
                return new List<string>();
            }
        }
        public bool Overage { get; set; }
        public List<SelectListItem> ShopList { get; set; }
        public string LoginOrName { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public int? Status { get; set; }
        public int? ShopID { get; set; }

        public List<Order> Result { get; set; }

        public List<ShopProductChar> ShopChars { get; set; }

        private string LoginOrNameLike
        {
            get { return string.Format("%{0}%", LoginOrName.Replace(" ", "%")).ToLower(); }
        }

        public DateTime? MaxDateDelivery { get; set; }
        public DateTime? MinDateDelivery { get; set; }

        public bool HasForMark
        {
            get { return Result.Any(x => !x.Marks.Any()); }
        }

        public DateTime? ExactDateDelivery { get; set; }
        public List<int> ListOverride { get; set; }

        public OrderFilter(int Status, DateTime? DeliveryDate)
        {
            this.ExactDateDelivery = DeliveryDate;
            this.Status = Status;
            InitFields();
            Search(DB);

        }

        public OrderFilter()
        {
            InitFields();
            Search(DB);

        }

        private void InitFields()
        {
            if (!ShopID.HasValue)
            {
                ShopID = HttpContext.Current.Request.QueryString["ShopID"].ToNullInt();
            }
            if (!ShopID.HasValue)
            {
                var index = HttpContext.Current.Request.RawUrl.IndexOf("?");
                if (index < 0)
                    index = HttpContext.Current.Request.RawUrl.Length;
                var parts = HttpContext.Current.Request.RawUrl.Substring(0, index).Split<string>("/").ToList();
                if (parts.Contains("ShopID"))
                {
                    ShopID = parts.ElementAt(parts.IndexOf("ShopID") + 1).ToNullInt();
                }
            }


            if (CurrentUser.UserRoles.Contains("ShopOwner"))
            {

                ShopList =
                    DB.Shops.Where(x => x.Owner == CurrentUser.ID)
                        .Select(
                            x =>
                                new SelectListItem()
                                {
                                    Text = x.Name,
                                    Value = x.ID.ToString(),
                                    Selected = (ShopID ?? 0) == x.ID
                                }).ToList();
            }
            else if (CurrentUser.UserRoles.Contains("Manager"))
            {
                ShopList =
                    DB.ShopManagers.Where(x => x.Manager.ManagerUserID == CurrentUser.ID).Select(x => x.Shop).OrderBy(x => x.Name)
                        .Select(
                            x =>
                                new SelectListItem()
                                {
                                    Text = x.Name,
                                    Value = x.ID.ToString(),
                                    Selected = (ShopID ?? 0) == x.ID
                                }).ToList();

            }
            else if (CurrentUser.UserRoles.Contains("Operator"))
            {
                ShopList =
                    DB.OperatorShops.Where(x => x.UserID == CurrentUser.ID).Select(x => x.Shop).OrderBy(x => x.Name)
                        .Select(
                            x =>
                                new SelectListItem()
                                {
                                    Text = x.Name,
                                    Value = x.ID.ToString(),
                                    Selected = (ShopID ?? 0) == x.ID
                                }).ToList();

            }
            Overage = HttpContext.Current.Request.QueryString["Overage"].ToBool();
        }

        public void Search(DataContext db)
        {

            if (!ShopList.Any())
            {
                Result = new List<Order>();
                return;

            }



            IQueryable<Order> result = null;

            if (ListOverride != null && ListOverride.Any())
            {
                Result = db.Orders.Where(x => ListOverride.Contains(x.ID)).ToList();
                return;
            }
            if (!ShopID.HasValue)
            {
                ShopID = HttpContext.Current.Request.QueryString["ShopID"].ToNullInt();
            }
            if (!ShopID.HasValue)
            {
                ShopID = ShopList.First().Value.ToInt();
            }
            if (CurrentUser.UserRoles.Contains("ShopOwner"))
            {
                var orderAuthors =
                    db.ShopManagers.Where(x => x.Manager.ShopOwnerID == CurrentUser.ID)
                        .Select(x => x.Manager.ManagerUserID)
                        .ToList();
                orderAuthors.Add(CurrentUser.ID);

                result = db.Orders.Where(x => orderAuthors.Contains(x.AddedByID));
                result =
                    result.Where(x => x.ShopID == ShopID.Value || (!x.ShopID.HasValue && x.AddedByID == CurrentUser.ID));
                var contracted =
                    db.ContractedOrders.Where(
                        x => x.Contractor.UserBy == CurrentUser.ShopOwnerID && x.Contractor.ShopID == ShopID).Select(x => x.Order);
                result = result.Concat(contracted);

            }
            else if (CurrentUser.UserRoles.Contains("Manager"))
            {
                var orderAuthors =
                    db.ShopManagers.Where(x => x.Manager.ShopOwnerID == CurrentUser.ManagerProfiles.First().ShopOwnerID)
                        .Select(x => x.Manager.ManagerUserID)
                        .ToList();

                orderAuthors.Add(CurrentUser.ManagerProfiles.First().ShopOwnerID);

                var shids = db.ShopManagers.Where(x => x.Manager.ManagerUserID == CurrentUser.ID).Select(x => x.ShopID).ToList();

                result = db.Orders.Where(x => orderAuthors.Contains(x.AddedByID) && shids.Contains(x.ShopID));
                result =
                    result.Where(x => x.ShopID == ShopID.Value || (!x.ShopID.HasValue && x.AddedByID == CurrentUser.ID));
                var contracted =
                    db.ContractedOrders.Where(
                        x => x.Contractor.UserFor == CurrentUser.ShopOwnerID && x.Contractor.ShopID == ShopID)
                        .Select(x => x.Order);
                result = result.Concat(contracted);


            }
            else if (CurrentUser.UserRoles.Contains("Operator"))
            {
                var shds = db.OperatorShops.Where(x => x.UserID == CurrentUser.ID).Select(x => x.ShopID).ToList();
                result = db.Orders.Where(x => shds.Contains(x.ShopID ?? 0));
            }
            else
            {
                result = db.Orders.Where(x => x.AddedByID == CurrentUser.ID);
            }
            if (MinDate.HasValue)
            {
                result =
                    result.Where(
                        x =>
                            x.CreateDate.Date >= MinDate.Value);
            }
            if (MaxDate.HasValue)
            {
                result =
                    result.Where(
                        x =>
                            x.CreateDate <= MaxDate.Value);
            }

            if (MinDateDelivery.HasValue)
            {
                result =
                    result.Where(x => x.DeliveryDate.HasValue && x.DeliveryDate.Value.Date >= MinDateDelivery.Value.Date);
            }

            if (MaxDateDelivery.HasValue)
            {
                result =
                    result.Where(x => x.DeliveryDate.HasValue && x.DeliveryDate.Value.Date <= MaxDateDelivery.Value.Date);
            }

            if (LoginOrName.IsFilled())
            {
                result =
                    result.Where(
                        x => x.Consumer != null && (
                            SqlMethods.Like(x.Consumer.Name.ToLower(), LoginOrNameLike) ||
                            SqlMethods.Like(x.Consumer.Surname.ToLower(), LoginOrNameLike) ||
                            SqlMethods.Like(x.Consumer.Phone.ToLower(), LoginOrNameLike)));
            }

            if (Status.HasValue)
            {
                result = result.Where(x => x.Status == Status.Value);
            }


            var chars =
                DB.ShopProductChars.Where(
                    x => x.UserID == CurrentUser.ShopOwnerID && (!x.ShopID.HasValue || x.ShopID == ShopID) && x.IsMain)
                    .OrderBy(x => x.Name).ToList();

            ShopChars = chars.ToList();


            if (Overage)
            {
                result = result.Where(x => x.Marks.Any() && x.Marks.First().MarkDate > DateTime.Now.AddDays(-14));
            }

            if (ExactDateDelivery.HasValue)
            {
                result =
                    result.Where(
                        x => x.DeliveryDate.HasValue && x.DeliveryDate.Value.Date == ExactDateDelivery.Value.Date);
            }

            if (Regions.Any())
            {

                IQueryable<Order> filtered = null;
                foreach (var region in Regions)
                {
                    if (filtered == null)
                        filtered =
                            result.Where(
                                x => x.OrderDelivery != null && x.OrderDelivery.Region.ToLower() == region.ToLower());
                    else
                        filtered = filtered.Concat(result.Where(
                            x => x.OrderDelivery != null && x.OrderDelivery.Region.ToLower() == region.ToLower()));
                }
                result = filtered;
            }

            Result = result.OrderByDescending(x => x.Status).ThenByDescending(x => x.ID).Take(2000).ToList();



            if (CurrentUser.UserRoles.Contains("Operator"))
            {
                Result = Result.Where(x => x.Status == 100 && x.DeliveryDate.HasValue).OrderBy(x => x.Marks.Any()).ThenBy(x => x.DeliveryDate).ToList();
            }


        }

    }

    public enum OrderStatus
    {
        Deleted = -10,
        Filling = -1,
        WaitingDelivery = 0,
        Payed = 50,
        Delivered = 100,
        Marked = 200
    }
}