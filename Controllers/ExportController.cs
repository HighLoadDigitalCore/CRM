using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.HSSF.Record;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PawnHunter.Numerals;
using Trading.Extensions;
using Trading.Extensions.Helpers;
using Trading.Models;


namespace Trading.Controllers
{
    public class ExportController : BaseController
    {
        public ActionResult XLS(string List)
        {

            var stream = new MemoryStream();
            var workbook = new HSSFWorkbook();

            var ids = List.Split<int>(";");
            var orders = DB.Orders.Where(x => ids.Contains(x.ID) && x.Shop != null && x.Shop.Owner == CurrentUser.ShopOwnerID).ToList();
            foreach (var order in orders)
            {
                var worksheet = workbook.CreateSheet("Заказ №" + order.OrderNumberOrID.Replace("[", "@").Replace("]", "@"));
                var header = worksheet.CreateRow(0);
                header.CreateCell(0).SetCellValue("Информация о заказе:");

                header = worksheet.CreateRow(1);
                header.CreateCell(0).SetCellValue("Номер заказа");
                header.CreateCell(1).SetCellValue(order.OrderNumberOrID);

                header = worksheet.CreateRow(2);
                header.CreateCell(0).SetCellValue("Статус заказа");
                header.CreateCell(1).SetCellValue(order.StatusText);

                header = worksheet.CreateRow(3);
                header.CreateCell(0).SetCellValue("Пометка \"Важный\"");
                header.CreateCell(1).SetCellValue(order.IsImportant.ToYesNoStatus());

                header = worksheet.CreateRow(4);
                header.CreateCell(0).SetCellValue("Дата создания");
                header.CreateCell(1).SetCellValue(order.CreateDate.ToString("dd.MM.yyyy HH:mm:ss"));

                header = worksheet.CreateRow(5);
                header.CreateCell(0).SetCellValue("Комментарий");
                header.CreateCell(1).SetCellValue(order.Warning);

                header = worksheet.CreateRow(6);
                header.CreateCell(0).SetCellValue("");

                if (order.Consumer != null)
                {

                    header = worksheet.CreateRow(7);
                    header.CreateCell(0).SetCellValue("Информация о покупателе:");

                    header = worksheet.CreateRow(8);
                    header.CreateCell(0).SetCellValue("ФИО");
                    header.CreateCell(1).SetCellValue(order.Consumer.FullName);

                    header = worksheet.CreateRow(9);
                    header.CreateCell(0).SetCellValue("Телефон");
                    header.CreateCell(1).SetCellValue(order.Consumer.Phone);

                    header = worksheet.CreateRow(10);
                    header.CreateCell(0).SetCellValue("");

                }

                if (order.OrderedProducts.Any())
                {
                    header = worksheet.CreateRow(11);
                    header.CreateCell(0).SetCellValue("Заказанные товары:");


                    header = worksheet.CreateRow(12);
                    header.CreateCell(0).SetCellValue("Название");
                    header.CreateCell(1).SetCellValue("Артикул");
                    header.CreateCell(2).SetCellValue("Цена");
                    header.CreateCell(3).SetCellValue("Количество");
                    header.CreateCell(4).SetCellValue("Вес");
                    int index = 5;

                    foreach (var ch in order.CharList)
                    {
                        header.CreateCell(index).SetCellValue(ch.Name);
                        index++;
                    }


                    int msi = 13;
                    foreach (var product in order.OrderedProducts)
                    {
                        header = worksheet.CreateRow(msi);
                        header.CreateCell(0).SetCellValue(product.Product.Name);
                        header.CreateCell(1).SetCellValue(product.Product.Article);
                        header.CreateCell(2).SetCellValue(product.Price.ToString());
                        header.CreateCell(3).SetCellValue(product.Amount);
                        header.CreateCell(4).SetCellValue(product.Product.Weight.ToString());
                        index = 5;
                        foreach (var ch in order.CharList)
                        {
                            header.CreateCell(index)
                                .SetCellValue(product.ProductChars.Any(x => x.Name == ch.Name)
                                    ? product.ProductChars.First(x => x.Name == ch.Name).Value
                                    : "");
                            index++;
                        }


                        msi++;
                    }
                }
            }


            workbook.Write(stream);

            return File(stream.ToArray(), MIMETypeWrapper.GetMIME("xls"),
                        "Order_" + List.Trim(';').Replace(";", "_") + ".xls");


        }


        public ActionResult Torg12(string List)
        {
            var stream = new MemoryStream();
            var ids = List.Split<int>(";");
            var orders = DB.Orders.Where(x => ids.Contains(x.ID) && x.Shop!=null && x.Shop.Owner == CurrentUser.ShopOwnerID).ToList();
            var workbook = new HSSFWorkbook();


            using (var fs = new FileStream(Server.MapPath("~/Content/TORG12.xls"), FileMode.Open))
            {
                var example = new HSSFWorkbook(fs);
                int index = 0;
                foreach (var order in orders)
                {
                    var sheet = (HSSFSheet)example.GetSheetAt(0);

                    sheet.CopyTo(workbook, "Заказ №" + order.OrderNumberOrID.Replace("'", "").Replace("[", "@").Replace("]", "@"), true, false);

                    var copied = (HSSFSheet)workbook.GetSheetAt(index);
                    copied.GetRow(2).GetCell(1).SetCellValue(order.Requisites);
                    copied.GetRow(9).GetCell(3).SetCellValue(order.Requisites);
                    copied.GetRow(7).GetCell(3).SetCellValue(order.Receiver);
                    copied.GetRow(11).GetCell(3).SetCellValue(order.Receiver);
                    copied.GetRow(3).GetCell(37).SetCellValue(order.Shop.GetSetting("OKPO"));
                    copied.GetRow(8).GetCell(37).SetCellValue(order.Shop.GetSetting("OKPO"));
                    copied.GetRow(16).GetCell(10).SetCellValue(order.OrderNumberOrID);
                    copied.GetRow(16).GetCell(14).SetCellValue(DateTime.Now.ToString("dd.MM.yyyy"));
                    
                    if (order.OrderedProducts.Count > 1)
                    {
                        for (int i = 1; i < order.OrderedProducts.Count; i++)
                        {
                            copied.GetRow(22).CopyRowTo(23);
                        }
                    }
                    int oi = 0;
                    foreach (var product in order.OrderedProducts)
                    {
                        var row = copied.GetRow(22 + oi);
                        row.GetCell(1).SetCellValue(oi+1);
                        row.GetCell(2).SetCellValue(product.Product.Name);
                        row.GetCell(6).SetCellValue(product.Product.Article);
                        row.GetCell(7).SetCellValue(product.Product.AmountUnitName);
                        row.GetCell(11).SetCellValue(product.Product.UnitCode);
                        row.GetCell(21).SetCellValue(product.Amount);
                        row.GetCell(23).SetCellValue((product.PriceWithoutNDS).ToString("f2"));
                        row.GetCell(25).SetCellValue((product.PriceWithoutNDS * product.Amount).ToString("f2"));
                        row.GetCell(30).SetCellValue("18%");
                        row.GetCell(33).SetCellValue((product.NDS*product.Amount).ToString("f2"));
                        row.GetCell(36).SetCellValue((product.Price*product.Amount).ToString("f2"));
                        
                        oi++;
                    }
                    
                    var rws1 = copied.GetRow(22 + oi);
                    var rws2 = copied.GetRow(22 + oi + 1);
                    rws1.GetCell(21).SetCellValue(order.OrderedProducts.Sum(x=> x.Amount));
                    rws2.GetCell(21).SetCellValue(order.OrderedProducts.Sum(x=> x.Amount));

                    rws1.GetCell(23).SetCellValue("X");
                    rws2.GetCell(23).SetCellValue("X");

                    rws1.GetCell(25).SetCellValue(order.OrderedProducts.Sum(x => x.Amount * x.PriceWithoutNDS).ToString("f2"));
                    rws2.GetCell(25).SetCellValue(order.OrderedProducts.Sum(x => x.Amount * x.PriceWithoutNDS).ToString("f2"));

                    rws1.GetCell(30).SetCellValue("X");
                    rws2.GetCell(30).SetCellValue("X");

                    rws1.GetCell(33).SetCellValue(order.OrderedProducts.Sum(x => x.Amount * x.NDS).ToString("f2"));
                    rws2.GetCell(33).SetCellValue(order.OrderedProducts.Sum(x => x.Amount * x.NDS).ToString("f2"));

                    rws1.GetCell(36).SetCellValue(order.OrderedProducts.Sum(x => x.Amount * x.Price).ToString("f2"));
                    rws2.GetCell(36).SetCellValue(order.OrderedProducts.Sum(x => x.Amount * x.Price).ToString("f2"));

                    
                    copied.GetRow(22 + oi + 3)
                        .GetCell(5)
                        .SetCellValue(Russian.ToString(order.OrderedProducts.Count, Gender.Masculine));


                    var all = order.OrderedProducts.Sum(x => x.Price*x.Amount);
                    var allStr = Russian.ToString((int) all, Gender.Masculine, true) + " рублей " +
                                 ((int) ((all - (int) all)*100)).ToString("d2") + " копеек";
                    copied.GetRow(22 + oi + 13)
                        .GetCell(1)
                        .SetCellValue(allStr);

                    index++;
                }

                workbook.Write(stream);

            }
            return File(stream.ToArray(), MIMETypeWrapper.GetMIME("xls"),
                        "Torg12_" + List.Trim(';').Replace(";", "_") + ".xls");
        }

    
    }
}
