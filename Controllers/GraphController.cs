using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using Trading.Extensions;
using Trading.Models;

namespace Trading.Controllers
{
    public class GraphController : BaseController
    {
        [Authorize]
        public ActionResult SequenceEditor()
        {
            return PartialView(DB.GraphSerials.Where(x => x.UserID == CurrentUser.ID).ToList());
        }

        [Authorize]
        public ActionResult DeleteSerial(int ID)
        {
            var serial = DB.GraphSerials.FirstOrDefault(x => x.ID == ID);
            if (serial != null)
            {
                DB.GraphSerialsRels.DeleteAllOnSubmit(serial.GraphSerialsRels);
                DB.GraphSerialsOrders.DeleteAllOnSubmit(serial.GraphSerialsOrders);
                DB.GraphSerials.DeleteOnSubmit(serial);
                DB.SubmitChanges();
            }
            return new ContentResult();
        }

        [Authorize]
        public ActionResult ToggleSerial(int id)
        {
            var serial = DB.GraphSerialsRels.FirstOrDefault(x => x.ID == id);
            if (serial != null)
            {
                serial.IsHidden = !serial.IsHidden;
                DB.SubmitChanges();
            }
            return new ContentResult();
        }

        [Authorize]
        public ActionResult GetAllGraphsData(int? id)
        {
            var list = DB.Graphs.Where(x => x.UserID == CurrentUser.ID);
            if (id.HasValue)
            {
                list = list.Where(x => x.ID == id);
            }
            var graphs = list.OrderBy(x => x.OrderNum).ToList().Select(x => new
            {
                type = x.GraphTypeFunc.GraphType.Code,
                id = x.ID,
                data = createSubSeq(x)
            }).ToArray();
            return Json(graphs, JsonRequestBehavior.AllowGet);

        }

        public object createSubSeq(Graph x)
        {
            var period = new GraphPeriod(x.Period ?? 1);
            period.InitRange(x.MinDate, x.MaxDate, x.GraphSerialsRels.SelectMany(z => z.GraphSerial.GraphSerialsOrders).Select(z => z.Order).ToList());
            var array = new List<object>();
            
            foreach (var rel in x.GraphSerialsRels.Where(c=> !c.IsHidden))
            {
                array.Add(
                    new
                    {
                        Color = rel.Color,
                        Name = rel.GraphSerial.Name,
                        PointData = createPointData(period, x, rel)
                    }
                    );

            }
            return array;

        }

        public object createPointData(GraphPeriod period, Graph x, GraphSerialsRel rel)
        {
            if (period.Range.Count == 1)
            {
                period.Range.Insert(0, new GraphPeriod(period.Days, period.RangeOrders)
                {
                    MinDate = period.MinDate.Value.Date.AddDays(-2* period.Days),
                    MaxDate = period.MinDate.Value.Date.AddDays(-1 * period.Days),
                    
                });
                period.Range.Add(new GraphPeriod(period.Days, period.RangeOrders)
                {
                    MinDate = period.MaxDate.Value.Date,
                    MaxDate = period.MaxDate.Value.Date.AddDays(period.Days),

                });
            }

            return period.Range.Select(
                (z, index) =>
                    new
                    {
                        Label = GraphPeriod.CreateLiteralRange(z.MinDate.Value, z.MaxDate.Value),
                        Value =
                            period.Calculate(index, rel.SerialID, x.GraphTypeFunc.Func,
                                x.GraphTypeFunc.TypeID),

                    }).Where(z => z.Value != null).ToArray();
        }

        //
        // GET: /Graph/
        public ActionResult Common()
        {
            return PartialView();
        }
        public ActionResult Quality()
        {
            return PartialView();
        }
        public ActionResult Status()
        {
            return PartialView();
        }
        public ActionResult ClientsCompare()
        {
            return PartialView();
        }
        public ActionResult OrderAmount()
        {
            return PartialView();
        }
        public ActionResult MiddlePrice()
        {
            return PartialView();
        }
        public ActionResult OrderVolume()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult QualityPoints(string points, int period = 24)
        {
            var pids = points.Split<int>(";").ToList();
            var orders = DB.Orders.Where(x => pids.Contains(x.ID) && x.Marks.Any()).ToList();

            var pointList = new List<KeyValuePair<string, List<int>>>();
            DateTime? minDate = null;
            DateTime? maxDate = null;
            if (orders.Any())
            {
                minDate = orders.Min(x => x.Marks.First().MarkDate);
                maxDate = orders.Max(x => x.Marks.First().MarkDate);
            }
            else
            {
                minDate = DateTime.Now.AddDays(-1);
                maxDate = DateTime.Now;
            }

            var periods = new List<int> { 1, 24, 168, 336, 720, 2880, 4320, 8640 };
            var limits = new int[] { 24, 30, 12, 12, 12, 12, 5, 5 };

            var currentDate = maxDate;
            for (int i = 0; i <= limits[periods.IndexOf(period)]; i++)
            {
                var start = currentDate.Value;
                var end = start.AddHours(-1 * period);
                var os = orders.Where(x => x.Marks.First().MarkDate > end && x.Marks.First().MarkDate <= start).ToList();
                var cnt = os.Count();
                if (cnt == 0)
                {
                    cnt = 1;
                }
                var point = new KeyValuePair<string, List<int>>(
                    (period == 1
                        ? (end.ToString("dd.MM HH:mm") + " - " + start.ToString("dd.MM HH:mm"))
                        : (end.ToString("d MMM yyyy") + " - " + start.ToString("d MMM yyyy"))),
                    new List<int>()
                    {
                        (int) (Math.Round((decimal) os.Sum(z => z.Marks.First().Total)/(decimal) cnt, 2)*1000),
                        (int)(Math.Round((decimal) os.Sum(z => z.Marks.First().Operator)/(decimal) cnt, 2)*1000),
                        (int)(Math.Round((decimal) os.Sum(z => z.Marks.First().Delivery)/(decimal) cnt, 2)* 1000),
                        (int)(Math.Round((decimal) os.Sum(z => z.Marks.First().Quality)/(decimal) cnt, 2)*1000),
                        (int)(Math.Round((decimal) os.Sum(z => z.Marks.First().Price)/(decimal) cnt)* 1000),
                    }
                    );
                pointList.Add(point);

                currentDate = end;

            }


            pointList.Reverse();

            var result = new
            {
                Points =
                    pointList.Select(
                        (x, index) =>
                            new
                            {
                                period = (index + 1) + ". " + x.Key,
                                Total = x.Value.ElementAt(0),
                                Operator = x.Value.ElementAt(1),
                                Delivery = x.Value.ElementAt(2),
                                Quality = x.Value.ElementAt(3),
                                Price = x.Value.ElementAt(4),
                            }).ToArray(),
                YKeys = new[] { "Total", "Operator", "Delivery", "Quality", "Price" },
                Labels = new[] { "Общая оценка", "Работа оператора", "Доставка", "Качество обслуживания", "Цена" }
            };
            return Json(result, JsonRequestBehavior.AllowGet);

            /*
                        [
                          { period: '2010 Q1', iphone: 2666, ipad: null, itouch: 2647 },
                          { period: '2010 Q2', iphone: 2778, ipad: 2294, itouch: 2441 },
                          { period: '2010 Q3', iphone: 4912, ipad: 1969, itouch: 2501 },
                          { period: '2010 Q4', iphone: 3767, ipad: 3597, itouch: 5689 },
                          { period: '2011 Q1', iphone: 6810, ipad: 1914, itouch: 2293 },
                          { period: '2011 Q2', iphone: 5670, ipad: 4293, itouch: 1881 },
                          { period: '2011 Q3', iphone: 4820, ipad: 3795, itouch: 1588 },
                          { period: '2011 Q4', iphone: 15073, ipad: 5967, itouch: 5175 },
                          { period: '2012 Q1', iphone: 10687, ipad: 4460, itouch: 2028 },
                          { period: '2012 Q2', iphone: 8432, ipad: 5713, itouch: 1791 }
                        ]
            */
        }

        [HttpGet]
        public ActionResult StatusPoints(string points, int period = 24)
        {
            var pids = points.Split<int>(";").ToList();
            var orders = DB.Orders.Where(x => pids.Contains(x.ID)).ToList();

            var pointList = new List<KeyValuePair<string, List<int>>>();
            DateTime? minDate = null;
            DateTime? maxDate = null;
            if (orders.Any())
            {
                minDate = orders.Min(x => x.CreateDate);
                maxDate = orders.Max(x => x.CreateDate);
            }
            else
            {
                minDate = DateTime.Now.AddDays(-1);
                maxDate = DateTime.Now;
            }

            var periods = new List<int> { 1, 24, 168, 336, 720, 2880, 4320, 8640 };
            var limits = new int[] { 24, 30, 12, 12, 12, 12, 5, 5 };

            var currentDate = maxDate;
            for (int i = 0; i <= limits[periods.IndexOf(period)]; i++)
            {
                var start = currentDate.Value;
                var end = start.AddHours(-1 * period);
                var os = orders.Where(x => x.CreateDate > end && x.CreateDate <= start).ToList();
                var cnt = os.Count();
                if (cnt == 0)
                {
                    cnt = 1;
                }
                var point = new KeyValuePair<string, List<int>>(
                    (period == 1
                        ? (end.ToString("dd.MM HH:mm") + " - " + start.ToString("dd.MM HH:mm"))
                        : (end.ToString("d MMM yyyy") + " - " + start.ToString("d MMM yyyy"))),
                    new List<int>()
                    {
                        os.Count(x=> x.Status==(int)OrderStatus.WaitingDelivery),
                        os.Count(x=> x.Status==(int)OrderStatus.Delivered),
                        os.Count(x=> x.Status==(int)OrderStatus.Filling),
                        os.Count(x=> x.Status==(int)OrderStatus.Deleted)
                    }
                    );
                pointList.Add(point);

                currentDate = end;

            }


            pointList.Reverse();

            var result = new
            {
                Points =
                    pointList.Select(
                        (x, index) =>
                            new
                            {
                                period = (index + 1) + ". " + x.Key,
                                WaitingDelivery = x.Value.ElementAt(0),
                                Delivered = x.Value.ElementAt(1),
                                Filling = x.Value.ElementAt(2),
                                Deleted = x.Value.ElementAt(3)
                            }).ToArray(),
                YKeys = new[] { "WaitingDelivery", "Delivered", "Filling", "Deleted" },
                Labels = new[] { "Ожидает отгрузки", "Доставлен", "Заполнение", "Удален" }
            };
            return Json(result, JsonRequestBehavior.AllowGet);

          
        }

        [HttpGet]
        public ActionResult ClientsComparePoints(string points, int period = 24)
        {
            var pids = points.Split<int>(";").ToList();
            var orders = DB.Orders.Where(x => pids.Contains(x.ID) && x.Marks.Any()).ToList();

            var pointList = new List<KeyValuePair<string, List<int>>>();
            DateTime? minDate = null;
            DateTime? maxDate = null;
            if (orders.Any())
            {
                minDate = orders.Min(x => x.Marks.First().MarkDate);
                maxDate = orders.Max(x => x.Marks.First().MarkDate);
            }
            else
            {
                minDate = DateTime.Now.AddDays(-1);
                maxDate = DateTime.Now;
            }

            var periods = new List<int> { 1, 24, 168, 336, 720, 2880, 4320, 8640 };
            var limits = new int[] { 24, 10, 12, 12, 12, 12, 5, 5 };

            var currentDate = maxDate;
            for (int i = 0; i <= limits[periods.IndexOf(period)]; i++)
            {
                var start = currentDate.Value;
                var end = start.AddHours(-1 * period);
                var os = orders.Where(x => x.Marks.First().MarkDate > end && x.Marks.First().MarkDate <= start).ToList();
                var point = new KeyValuePair<string, List<int>>(
                    (period == 1
                        ? (end.ToString("HH:mm") + "-" + start.ToString("HH:mm"))
                        : (end.ToString("d MMM") + "-" + start.ToString("d MMM"))),
                    new List<int>()
                    {
                        os.Count(z => z.Marks.First().Total >= ((decimal)7/2)),
                        os.Count(z => z.Marks.First().Total < ((decimal)7/2))
                    });
                pointList.Add(point);

                currentDate = end;

            }


            pointList.Reverse();
            pointList = pointList.Where(x => x.Value.ElementAt(0) > 0 || x.Value.ElementAt(1) > 0).ToList();
            var result =
                pointList.Select(
                    (x, index) =>
                        new
                        {
                            period = x.Key,
                            good = x.Value.ElementAt(0),
                            bad = x.Value.ElementAt(1),
                        }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult OrderVolumePoints(string points, int period = 24)
        {
            var pids = points.Split<int>(";").ToList();
            var orders = DB.Orders.Where(x => pids.Contains(x.ID) && x.Marks.Any()).ToList();

            var pointList = new List<KeyValuePair<string, List<int>>>();
            DateTime? minDate = null;
            DateTime? maxDate = null;
            if (orders.Any())
            {
                minDate = orders.Min(x => x.Marks.First().MarkDate);
                maxDate = orders.Max(x => x.Marks.First().MarkDate);
            }
            else
            {
                minDate = DateTime.Now.AddDays(-1);
                maxDate = DateTime.Now;
            }

            var periods = new List<int> { 1, 24, 168, 336, 720, 2880, 4320, 8640 };
            var limits = new int[] { 24, 10, 12, 12, 12, 12, 5, 5 };

            var currentDate = maxDate;
            for (int i = 0; i <= limits[periods.IndexOf(period)]; i++)
            {
                var start = currentDate.Value;
                var end = start.AddHours(-1 * period);
                var os = orders.Where(x => x.Marks.First().MarkDate > end && x.Marks.First().MarkDate <= start).ToList();
                var point = new KeyValuePair<string, List<int>>(
                    (period == 1
                        ? (end.ToString("HH:mm") + "-" + start.ToString("HH:mm"))
                        : (end.ToString("d MMM") + "-" + start.ToString("d MMM"))),
                    new List<int>()
                    {
                        (int)os.Where(z => z.Consumer.Sex).Sum(z=> z.OrderedProducts.Sum(c=> c.Amount * c.Price)),
                        (int)os.Where(z => !z.Consumer.Sex).Sum(z=> z.OrderedProducts.Sum(c=> c.Amount * c.Price))
                    });
                pointList.Add(point);

                currentDate = end;

            }


            pointList.Reverse();
            pointList = pointList.Where(x => x.Value.ElementAt(0) > 0 || x.Value.ElementAt(1) > 0).ToList();
            var result =
                pointList.Select(
                    (x, index) =>
                        new
                        {
                            period = x.Key,
                            man = x.Value.ElementAt(0),
                            woman = x.Value.ElementAt(1),
                        }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult OrderAmountPoints(string points, int period = 24)
        {
            var pids = points.Split<int>(";").ToList();
            var orders = DB.Orders.Where(x => pids.Contains(x.ID) && x.Marks.Any()).ToList();

            var pointList = new List<KeyValuePair<string, List<int>>>();
            DateTime? minDate = null;
            DateTime? maxDate = null;
            if (orders.Any())
            {
                minDate = orders.Min(x => x.Marks.First().MarkDate);
                maxDate = orders.Max(x => x.Marks.First().MarkDate);
            }
            else
            {
                minDate = DateTime.Now.AddDays(-1);
                maxDate = DateTime.Now;
            }

            var periods = new List<int> { 1, 24, 168, 336, 720, 2880, 4320, 8640 };
            var limits = new int[] { 24, 10, 12, 12, 12, 12, 5, 5 };

            var currentDate = maxDate;
            var managers = CurrentUser.ManagerList.Select(x => x.User).ToList();
            managers = managers.Where(x => orders.Any(z => z.AddedByID == x.ID)).ToList();
            var hasOwner = orders.Any(x => x.AddedByID == CurrentUser.ShopOwnerID);
            if (hasOwner)
            {
                managers.Insert(0, CurrentUser.ShopOwner);
            }

            for (int i = 0; i <= limits[periods.IndexOf(period)]; i++)
            {
                var start = currentDate.Value;
                var end = start.AddHours(-1 * period);
                var os = orders.Where(x => x.Marks.First().MarkDate > end && x.Marks.First().MarkDate <= start).ToList();
                var point = new KeyValuePair<string, List<int>>(
                    (period == 1
                        ? (end.ToString("HH:mm") + "-" + start.ToString("HH:mm"))
                        : (end.ToString("d MMM") + "-" + start.ToString("d MMM"))),
                    new List<int>()
                    {
                        os.Count
                    });

                foreach (var manager in managers)
                {
                    var mos = os.Where(x => x.AddedByID == manager.ID).ToList();
                    point.Value.Add(mos.Count);
                }

                pointList.Add(point);

                currentDate = end;

            }


            pointList.Reverse();

            var result =
                pointList.Select(
                    (x, index) =>
                        new
                        {
                            period = (index + 1) + ". " + x.Key,
                            details = x.Value.Select((z, i) => new { manager = (i == 0 ? "A" : "M" + managers.ElementAt(i - 1).ID), value = z }).ToArray()
                        }).ToArray();


            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult MiddlePricePoints(string points, int period = 24)
        {
            var pids = points.Split<int>(";").ToList();
            var orders = DB.Orders.Where(x => pids.Contains(x.ID) && x.Marks.Any()).ToList();

            var pointList = new List<KeyValuePair<string, List<UInt64>>>();
            DateTime? minDate = null;
            DateTime? maxDate = null;
            if (orders.Any())
            {
                minDate = orders.Min(x => x.Marks.First().MarkDate);
                maxDate = orders.Max(x => x.Marks.First().MarkDate);
            }
            else
            {
                minDate = DateTime.Now.AddDays(-1);
                maxDate = DateTime.Now;
            }

            var periods = new List<int> { 1, 24, 168, 336, 720, 2880, 4320, 8640 };
            var limits = new int[] { 24, 10, 12, 12, 12, 12, 5, 5 };

            var currentDate = maxDate;
            for (int i = 0; i <= limits[periods.IndexOf(period)]; i++)
            {
                var start = currentDate.Value;
                var end = start.AddHours(-1 * period);
                var os = orders.Where(x => x.Marks.First().MarkDate > end && x.Marks.First().MarkDate <= start).ToList();
                var point = new KeyValuePair<string, List<UInt64>>(
                    (period == 1
                        ? (end.ToString("HH:mm") + "-" + start.ToString("HH:mm"))
                        : (end.ToString("d MMM") + "-" + start.ToString("d MMM"))),
                    new List<UInt64>()
                    {
                       (UInt64)((os.Sum(z=> z.OrderedProducts.Sum(c=> (decimal)c.Amount * c.Price)) / (decimal)(!os.Any() ? 1: os.Count()))* 1)
                    });
                pointList.Add(point);

                currentDate = end;

            }


            pointList.Reverse();

            var result =
                pointList.Select(
                    (x, index) =>
                        new
                        {
                            period = (index + 1) + ". " + x.Key,
                            point = x.Value.ElementAt(0),

                        }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult GraphIndex(string type)
        {
            ViewBag.StatGraph = type;
            var filter = new OrderFilter();
            return View(filter);
        }


        public ActionResult GraphIndexSearch(OrderFilter filter, string type)
        {
            ViewBag.StatGraph = type;
            filter.Search(DB);
            return PartialView(filter);
        }

        [Authorize]
        public ActionResult Constructor()
        {
            var graps = DB.Graphs.Where(x => x.UserID == CurrentUser.ID).OrderBy(x => x.OrderNum).ToList();
            return View(graps);
        }

        public ActionResult DeleteGraph(int id)
        {
            var graph = DB.Graphs.FirstOrDefault(x => x.ID == id);
            if (graph != null)
            {
                DB.GraphSerialsRels.DeleteAllOnSubmit(graph.GraphSerialsRels);
                DB.SubmitChanges();
                DB.Graphs.DeleteOnSubmit(graph);
                DB.SubmitChanges();
            }
            return RedirectToAction("Constructor");
        }

        [Authorize]
        [HttpPost]
        public ActionResult GraphFilter(GraphPeriod period, FormCollection collection)
        {
            var graph = DB.Graphs.FirstOrDefault(x => x.ID == period.ID);
            if (graph != null)
            {
                graph.Period = period.Days;
                if (period.MinDateThresh.HasValue && period.MinDate.HasValue)
                {
                    if (period.MinDate < period.MinDateThresh)
                    {
                        period.MinDate = period.MinDateThresh;
                    }
                }
                if (period.MaxDateThresh.HasValue && period.MaxDate.HasValue)
                {
                    if (period.MaxDate > period.MaxDateThresh)
                    {
                        period.MaxDate = period.MaxDateThresh;
                    }
                }
                if (period.MinDate.HasValue && period.MaxDate.HasValue)
                {
                    if (period.MinDate > period.MaxDate)
                    {
                        period.MinDate = period.MaxDate.Value.AddDays(-1);
                    }
                    var days = (int) (period.MaxDate.Value.Subtract(period.MinDate.Value).TotalDays/2);
                    if (days < 1)
                        days = 1;
                    var allowed = period.AllPeriods.OrderBy(x=> x).Last(x => x <= days);
                    if (allowed < period.Days)
                        graph.Period = allowed;
                }
                
                graph.MinDate = period.MinDate;
                graph.MaxDate = period.MaxDate;
                DB.SubmitChanges();
                return GraphFilter(graph.ID);
            }
            return PartialView(null);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GraphFilter(int id)
        {
            var graph = DB.Graphs.FirstOrDefault(x => x.ID == id);
            if (graph != null)
            {
                var range = new GraphPeriod(graph.Period ?? 1);
                var orders =
                    graph.GraphSerialsRels.SelectMany(x => x.GraphSerial.GraphSerialsOrders)
                        .Select(x => x.Order)
                        .ToList();

                range.InitRange(
                    graph.MinDate.HasValue
                        ? graph.MinDate
                        : (orders.Any() ? orders.Min(x => x.CreateDate) : (DateTime?) null),
                    graph.MaxDate.HasValue
                        ? graph.MaxDate
                        : (orders.Any() ? orders.Max(x => x.CreateDate) : (DateTime?) null), orders);

                

                range.Serials = graph.GraphSerialsRels;

                range.ID = id;
                range.TypeID = graph.GraphTypeFunc.TypeID;
                return PartialView(range);
            }
            return PartialView(null);
        }
    }
}
