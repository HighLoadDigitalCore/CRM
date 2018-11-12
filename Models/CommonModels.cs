using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using Trading.Extensions;

namespace Trading.Models
{
    public class GraphPeriod
    {
        public GraphPeriod()
        {

        }
        public GraphPeriod(int day)
        {
            Days = day;
        }
        public GraphPeriod(int day, List<Order> orders)
            : this(day)
        {
            _orders = orders;
        }
        public int Days { get; set; }

        private List<Order> _orders;
        public List<Order> RangeOrders
        {
            get
            {
                if (_orders == null || !MinDate.HasValue || !MaxDate.HasValue)
                    return new List<Order>();
                return _orders.Where(x => x.CreateDate.Date > MinDate.Value.Date && x.CreateDate.Date <= MaxDate.Value.Date).ToList();
            }
        }

        public List<Order> GetRangeOrdersForSerial(int serialID, int index, ref DateTime minDate, ref DateTime maxDate, int typeid)
        {

            minDate = Range[index].MinDate.Value;
            maxDate = Range[index].MaxDate.Value;
            if (typeid == 3)
            {
                return RangeOrders.Where(x => x.GraphSerialsOrders.Any(z => z.SerialID == serialID)).ToList();
            }

            return
                Range[index].RangeOrders.Where(
                    x =>
                        x.GraphSerialsOrders.Any(z => z.SerialID == serialID)).ToList();
        }

        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }

        public DateTime? MinDateThresh { get; set; }
        public DateTime? MaxDateThresh { get; set; }

        public List<GraphPeriod> Range { get; set; }



        public List<GraphPeriod> InitRange(DateTime? minDate, DateTime? maxDate, List<Order> orders)
        {
            if (orders.Any())
            {
                MinDateThresh = orders.Min(x => x.CreateDate);
                MaxDateThresh = orders.Max(x => x.CreateDate);
            }
            if (!maxDate.HasValue)
                maxDate = DateTime.Now;
            if (!minDate.HasValue)
                minDate = DateTime.Now.AddDays(-30);

            if (minDate < MinDateThresh)
                minDate = MinDateThresh;
            if (MaxDateThresh < maxDate)
                maxDate = MaxDateThresh;

            MinDate = minDate;
            MaxDate = maxDate;
/*
            if (MinDate.Value.Date == MaxDate.Value.Date)
                MinDate = MaxDate.Value.AddDays(Days * -1);
*/

            var range = (int)MaxDate.Value.Subtract(MinDate.Value).TotalDays;
            AccessiblePeriods =
                AllPeriods.Select(x => new { Val = x, Can = (range / x) >= 2 })
                    .Where(x => x.Can)
                    .Select(x => x.Val)
                    .ToList();

            if (AccessiblePeriods.Count == 0)
                AccessiblePeriods = new List<int>() {1};

            _orders = orders;

            Range = new List<GraphPeriod>();
            var currentMaxPoint = MaxDate.Value;
            int counter = 0;
            while (currentMaxPoint.Date >= MinDate.Value.Date)
            {
                Range.Add(new GraphPeriod(Days, orders)
                {
                    MaxDate = currentMaxPoint,
                    MinDate = currentMaxPoint.AddDays(Days * -1),
                    AccessiblePeriods = AccessiblePeriods,

                });
                currentMaxPoint = currentMaxPoint.AddDays(Days * -1);
                counter++;
                if (counter > 60)
                    break;

            }
            Range.Reverse();
            return Range;
        }

        public List<int> AccessiblePeriods { get; set; }

        private List<int> _allPeriods;
        public List<int> AllPeriods
        {
            get { return _allPeriods ?? (_allPeriods = new List<int>() { 1, 7, 14, 30, 121, 182, 365 }); }
        }

        public string ToText(int perid)
        {
            return new GraphPeriod(perid).Text;
        }

        public string Text
        {
            get
            {
                switch (Days)
                {
                    default:
                    case 1:
                        return "День";
                    case 7:
                        return "Неделя";
                    case 14:
                        return "2 недели";
                    case 30:
                        return "Месяц";
                    case 121:
                        return "4 месяца";
                    case 182:
                        return "Полгода";
                    case 365:
                        return "Год";

                }
            }
        }

        public int ID { get; set; }
        public EntitySet<GraphSerialsRel> Serials { get; set; }
        public int TypeID { get; set; }

        public static string CreateLiteralRange(DateTime min, DateTime max)
        {
            if (min.Year == max.Year)
            {
                if (min.Month == max.Month)
                {
                    if (min.AddDays(1).Date == max.Date)
                    {
                        return max.Date.ToString("d MMM");
                    }
                    return min.AddDays(1).Day + " - " + max.Day + " " + max.ToString("MMM");
                }
                else
                {
                    return min.AddDays(1).ToString("d MMM") + " - " + max.ToString("d MMM");
                }
            }
            else
            {
                return min.AddDays(1).ToString("dd.MM.yyyy") + " - " + max.ToString("dd.MM.yyyy");
            }
        }

        public object Calculate(int index, int serialID, string func, int typeid)
        {
            DateTime min = DateTime.Now;
            DateTime max = DateTime.Now;
            var orders = GetRangeOrdersForSerial(serialID, index, ref min, ref max, typeid).DistinctBy(x => x.ID).ToList();
            var cnt = orders.Count();
            if (cnt == 0)
                cnt = 1;

            switch (typeid)
            {
                case 3:
                    switch (func)
                    {
                        default:
                        case "TotalCost":
                            if (index > 0) return null;

                            var s = (UInt64)orders.Sum(x => x.OrderedProducts.Sum(z => z.Amount * z.Price));
                            string defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    orders.Count,
                                    CreateLiteralRange(min, max));

                            return
                                new
                                {
                                    Value = s,
                                    Tick =
                                        defText + string.Format(
                                            "<nobr>Общая стоимость: <b>" + s +
                                            "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>",
                                            orders.Select(x => x.ID).JoinToString("_"))
                                };

                    }
                    break;

                case 1:
                case 2:
                default:
                    switch (func)
                    {

                        case "Quality":


                            cnt = orders.Count(x => x.Marks.Any());


                            string defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    cnt,
                                    CreateLiteralRange(min, max));
                            var div = (decimal)orders.Count(x => x.Marks.Any());
                            if (div == 0)
                                div = 1;

                            var v = (UInt64)
                                (Math.Round(
                                    (decimal)orders.Where(x => x.Marks.Any()).Sum(z => z.Marks.First().Quality) /
                                    div,
                                    2));
                            return
                                new
                                {
                                    Value = v,
                                    Tick =
                                        defText + string.Format(
                                             "<nobr>Средняя оценка качества: <b>" + v +
                                            "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>"
                                            , orders.Select(x => x.ID).JoinToString("_"))
                                };
                        case "Operator":


                            cnt = orders.Count(x => x.Marks.Any());


                            defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    cnt,
                                    CreateLiteralRange(min, max));
                            div = (decimal)orders.Count(x => x.Marks.Any());
                            if (div == 0)
                                div = 1;


                            v = (UInt64)
                               (Math.Round(
                                   (decimal)orders.Where(x => x.Marks.Any()).Sum(z => z.Marks.First().Operator) /
                                   div,
                                   2));
                            return
                                new
                                {
                                    Value = v,
                                    Tick =
                                        defText + string.Format(
                                             "<nobr>Средняя оценка работы операторов: <b>" + v +
                                            "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>",
                                             orders.Select(x => x.ID).JoinToString("_"))
                                };
                        case "Delivery":
                            cnt = orders.Count(x => x.Marks.Any());


                            defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    cnt,
                                    CreateLiteralRange(min, max));

                            div = (decimal)orders.Count(x => x.Marks.Any());
                            if (div == 0)
                                div = 1;


                            v = (UInt64)
                               (Math.Round(
                                   (decimal)orders.Where(x => x.Marks.Any()).Sum(z => z.Marks.First().Delivery) /
                                   div,
                                   2));
                            return
                                new
                                {
                                    Value = v,
                                    Tick =
                                        defText + string.Format(
                                             "<nobr>Средняя оценка работы доставки: <b>" + v +
                                            "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>",
                                        orders.Select(x => x.ID).JoinToString("_"))
                                };
                        case "Price":
                            cnt = orders.Count(x => x.Marks.Any());


                            defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    cnt,
                                    CreateLiteralRange(min, max));

                            div = (decimal)orders.Count(x => x.Marks.Any());
                            if (div == 0)
                                div = 1;


                            v = (UInt64)
                                (Math.Round(
                                    (decimal)orders.Where(x => x.Marks.Any()).Sum(z => z.Marks.First().Price) /
                                    div,
                                    2));
                            return
                                new
                                {
                                    Value = v,
                                    Tick =
                                        defText + string.Format(
                                            "<nobr>Средняя оценка цены: <b>" + v +
                                            "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>",
                                            orders.Select(x => x.ID).JoinToString("_"))
                                };
                        default:
                        case "Overage":

                            cnt = orders.Count(x => x.Marks.Any());


                            defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    cnt,
                                    CreateLiteralRange(min, max));
                            div = (decimal)orders.Count(x => x.Marks.Any());
                            if (div == 0)
                                div = 1;

                            v = (UInt64)
                               (Math.Round(
                                   (decimal)orders.Where(x => x.Marks.Any()).Sum(z => z.Marks.First().Total) /
                                   div,
                                   2));
                            return
                                new
                                {
                                    Value = v,
                                    Tick =
                                        defText + string.Format(
                                             "<nobr>Средняя оценка качества: <b>" + v +
                                            "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>",
                                             orders.Select(x => x.ID).JoinToString("_"))
                                };
                        case "ClientsCompare":
                            v = (UInt64)orders.Count(z => z.Marks.Any() && z.Marks.First().Total >= ((decimal)7 / 2));



                            defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    v,
                                    CreateLiteralRange(min, max));


                            return
                                new
                                {

                                    Value = v,
                                    Tick =
                                        defText + string.Format(
                                            "<nobr>Кол-во удовлетворенных клиентов: <b>" + v +
                                            "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>",
                                            orders.Select(x => x.ID).JoinToString("_"))
                                };
                        case "ClientsCompareBad":
                            v = (UInt64)orders.Count(z => z.Marks.Any() && z.Marks.First().Total < ((decimal)7 / 2));

                            defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    v,
                                    CreateLiteralRange(min, max));
                            return
                                new
                                {

                                    Value = v,
                                    Tick =
                                        defText + string.Format(
                                             "<nobr>Кол-во неудовлетворенных клиентов: <b>" + v +
                                            "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>",
                                             orders.Select(x => x.ID).JoinToString("_"))
                                };
                        case "OrderAmount":

                            v = (UInt64)orders.Count(x => x.Status != (int)OrderStatus.Filling);

                            defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    v,
                                    CreateLiteralRange(min, max));


                            return
                                new
                                {
                                    Value = v,
                                    Tick =
                                        defText + string.Format(
                                             "<nobr>Кол-во заказов: <b>" + v +
                                            "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>",
                                             orders.Select(x => x.ID).JoinToString("_"))
                                };
                        case "MiddlePrice":
                            defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    orders.Count,
                                    CreateLiteralRange(min, max));

                            v =
                                (UInt64)
                                    ((orders.Sum(z => z.OrderedProducts.Sum(c => (decimal)c.Amount * c.Price)) /
                                      (decimal)(!orders.Any() ? 1 : orders.Count())) * 1);
                            return
                                new
                                {
                                    Value = v,
                                    Tick =
                                        defText + string.Format(
                                            "<nobr>Средний чек: <b>" + v +
                                            "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>",
                                            orders.Select(x => x.ID).JoinToString("_"))
                                };
                        case "OrderVolume":
                            defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    orders.Count,
                                    CreateLiteralRange(min, max));


                            v = (UInt64)orders.Sum(z => z.OrderedProducts.Sum(c => c.Amount * c.Price));
                            return
                               new
                               {
                                   Value = v,
                                   Tick =
                                       defText + string.Format(
                                            "<nobr>Стоимость заказа: <b>" + v +
                                           "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>",
                                           orders.Select(x => x.ID).JoinToString("_"))
                               };
                        case "Status":
                            v = (UInt64)orders.Count(x => x.Status == (int)OrderStatus.Delivered);
                            defText =
                                string.Format("<nobr>Найдено <b>{0}</b> заказ(ов) за период <b>{1}</b></nobr><br>",
                                    v,
                                    CreateLiteralRange(min, max));

                            return
                                new
                                {
                                    Value = v,
                                    Tick =
                                        defText + string.Format(
                                            "<nobr>Кол-во выполненных заказов: <b>" + v +
                                            "</b></nobr><br><a href=\"#search-head\" onclick=\"viewPeriod('{0}')\">Смотреть детально</a>",
                                            orders.Select(x => x.ID).JoinToString("_"))
                                };

                    }
            }
        }
    }

    public class SaveOrdersModel : BaseDataObject
    {
        public string OrderList { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public int Count { get; set; }

        public bool CanAddOnGraph { get { return GraphList.Any(); } }
        public List<Graph> GraphList { get; set; }
        public List<Graph> MyGraphList { get; set; }
        public List<Order> Orders { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public string Result { get; set; }

        public bool IsPost { get; set; }
        public List<GraphSerial> MySerialList { get; set; }
        public string GraphListPlane { get; set; }

        public SaveOrdersModel()
        {
            MyGraphList = CurrentUser != null
                ? DB.Graphs.Where(x => x.UserID == CurrentUser.ID).OrderBy(x => x.OrderNum).ToList()
                : new List<Graph>();
            MySerialList = CurrentUser != null
                ? DB.GraphSerials.Where(x => x.UserID == CurrentUser.ID).ToList()
                : new List<GraphSerial>();


        }
        public SaveOrdersModel(int ID)
            : this()
        {
            if (ID > 0)
            {
                var serial = DB.GraphSerials.FirstOrDefault(x => x.ID == ID);
                if (serial != null)
                {
                    this.Name = serial.Name;
                    this.ID = serial.ID;
                    GraphListPlane = MyGraphList.Where(x => x.GraphSerialsRels.Any(z => z.SerialID == serial.ID)).Select(x => x.ID).JoinToString(",");
                }
            }

        }

        public SaveOrdersModel(string uids)
            : this(0)
        {

            var uid = uids.Split<int>().ToList();
            Orders = DB.Orders.Where(x => uid.Contains(x.ID)).ToList();
            OrderList = uids;
            if (Orders.Any())
            {
                MinDate = Orders.Min(x => x.CreateDate);
                MaxDate = Orders.Max(x => x.CreateDate);
            }
            Count = Orders.Count;
        }
        public SaveOrdersModel(int ID, string uids, string Name, string graphs)
            : this(uids)
        {
            this.Name = Name;
            var uid = graphs.Split<int>().ToList();
            GraphList = DB.Graphs.Where(x => uid.Contains(x.ID)).ToList();
        }

        public string Save()
        {

            if (Name.IsNullOrEmpty())
                Result = "Необходимо указать название последовательности";
            else if (Count == 0)
                Result = "Необходимо добавить заказы в последовательность";
            else Result = "";

            if (Result.IsFilled())
                return Result;

            var serial = DB.GraphSerials.FirstOrDefault(x => x.ID == ID);
            if (serial == null || ID == 0)
            {
                serial = new GraphSerial()
                {
                    Name = Name,
                    UserID = CurrentUser.ID,

                };
                DB.GraphSerials.InsertOnSubmit(serial);
            }
            else
            {
                serial.Name = Name;
            }

            DB.SubmitChanges();
            var uids = Orders.Select(x => x.ID).ToList();
            var forDel = serial.GraphSerialsOrders.Where(x => !uids.Contains(x.OrderID)).ToList();
            if (forDel.Any())
            {
                DB.GraphSerialsOrders.DeleteAllOnSubmit(forDel);
                DB.SubmitChanges();
            }
            var forAdd = uids.Where(x => serial.GraphSerialsOrders.All(z => z.OrderID != x)).ToList();
            if (forAdd.Any())
            {
                var add = forAdd.Select(x => new GraphSerialsOrder() { GraphSerial = serial, OrderID = x }).ToList();
                DB.GraphSerialsOrders.InsertAllOnSubmit(add);
                DB.SubmitChanges();
            }

            uids = GraphList.Select(x => x.ID).ToList();
            var forDelG = serial.GraphSerialsRels.Where(x => !uids.Contains(x.GraphID)).ToList();
            if (forDelG.Any())
            {
                DB.GraphSerialsRels.DeleteAllOnSubmit(forDelG);
                DB.SubmitChanges();
            }
            var forAddG = uids.Where(x => serial.GraphSerialsRels.All(z => z.GraphID != x)).ToList();
            if (forAddG.Any())
            {
                var add = forAddG.Select(x => new GraphSerialsRel() { GraphSerial = serial, GraphID = x, IsHidden = false }).ToList();
                DB.GraphSerialsRels.InsertAllOnSubmit(add);
                DB.SubmitChanges();
            }
            return Result;
        }
    }
}