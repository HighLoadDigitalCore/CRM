using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using Trading.Extensions;
using Trading.Models;

namespace Trading.Controllers
{
    [Authorize(Roles = "ShopOwner,Manager")]
    public class ProductsController : BaseController
    {
        
        public ActionResult ViewProduct(int ID)
        {
            return View(DB.Products.FirstOrDefault(x=> x.ID == ID));
        }

        public ActionResult StoresList(int id)
        {
            ViewBag.ProductID = id;
            return
                PartialView(
                    DB.StoredProducts.Where(x => x.StoreProductRefillings.Any(z => z.StoredProduct.ProductID == id))
                        .Select(x => x.Store)
                        .OrderBy(x => x.Name)
                        .ToList());
        }

        public ActionResult ShopList(int id)
        {
            ViewBag.ProductID = id;
            return PartialView(DB.ShopProducts.Where(x => x.ProductID == id).Select(x => x.Shop).OrderBy(x=> x.Name).ToList());
        }

        public ActionResult RefillingList(int id)
        {
            ViewBag.ProductID = id;
            return
                PartialView(
                    DB.StoreProductRefillings.Where(x => x.StoredProduct.ProductID == id)
                        .OrderByDescending(x => x.Date)
                        .ToList());
        }

        public ActionResult ProductFormView(int id)
        {
            return PartialView(DB.Products.FirstOrDefault(x => x.ID == id));
        }

        public ActionResult EditProduct(int? id)
        {
            ViewBag.ProductID = id;
            return View();
        }

        public ActionResult DeleteProduct(int id)
        {
            return null;
        }

        public ActionResult SaveShops(string ids, int id)
        {
            var shids = ids.Split<int>(",").ToList();
            var exist = DB.ShopProducts.Where(x => x.ProductID == id).ToList();
            var forAdd = shids.Where(x => exist.All(z => z.ShopID != x)).ToList();
            var forDel = exist.Where(x => !shids.Contains(x.ShopID)).ToList();
            if (forDel.Any())
            {
                DB.ShopProducts.DeleteAllOnSubmit(forDel);
                DB.SubmitChanges();
            }
            if (forAdd.Any())
            {
                DB.ShopProducts.InsertAllOnSubmit(forAdd.Select(x => new ShopProduct() {ProductID = id, ShopID = x}));
                DB.SubmitChanges();
            }
            return new ContentResult();
        }

        public ActionResult EditShopList(int id)
        {
            ViewBag.ProductID = id;
            return PartialView(DB.ShopProducts.Where(x=> x.ProductID == id).ToList());
        }

        [HttpGet]
        public ActionResult ProductForm(int? id)
        {

            var product = DB.Products.FirstOrDefault(x => x.ID == (id ?? 0)) ?? new Product();
            var model = new ProductModel()
            {
                ID = product.ID,
                Name = product.Name,
                Article = product.Article,
                SelfCost = product.SelfCost,
                OptCost = product.OptCost,
                Price = product.Price
            };
/*
            model.Chars = new List<ProductModelChar>();

            model.Chars.AddRange(
                DB.ShopProductChars.Where(
                    x =>
                        ((!x.ShopID.HasValue && x.UserID == CurrentUser.ID)) && x.Type <= 2)
                    .Select(x => new ProductModelChar() { Name = x.Name, Value = x.DefValue }));
*/

            return
                PartialView(model);
        }
        
        [HttpPost]
        public ActionResult ProductForm(ProductModel model)
        {
            if (model.Name.IsNullOrEmpty() || model.Article.IsNullOrEmpty() || !model.Price.HasValue)
            {
                model.IsPost = true;
                return
                    PartialView(model);
            }

            var product = DB.Products.FirstOrDefault(x => x.ID == model.ID);
            bool added = false;
            if (product == null)
            {
                product = new Product(){AddedDate = DateTime.Now};
                DB.Products.InsertOnSubmit(product);
                added = true;
            }
            
            product.LoadPossibleProperties(model, new[] {"ID"});
            if (product.OwnerID == 0)
                product.OwnerID = CurrentUser.ShopOwnerID;
            DB.SubmitChanges();
            model.IsPost = true;
            model.IsSuccess = true;
            if (added)
                model.RedirectURL = Url.Action("EditProduct", new {ID = product.ID});
            return
                PartialView(model);
        }

        public ActionResult RefillForm(int id)
        {
            ViewBag.ProductID = id;
            return PartialView(DB.Products.FirstOrDefault(x => x.ID == id));
        }

        [HttpPost]
        public ActionResult Refill(int? ID, int? StoreID, DateTime? Date, decimal? Price, decimal? Amount, int? Unit)
        {
            if (ID.HasValue && StoreID.HasValue && Date.HasValue && Price.HasValue && Amount.HasValue && Unit.HasValue)
            {
                var storedProduct =
                    DB.StoredProducts.FirstOrDefault(x => x.ProductID == ID.Value && x.StoreID == StoreID.Value);

                if (storedProduct == null)
                {
                    storedProduct = new StoredProduct()
                    {
                        ProductID = ID.Value,
                        StoreID = StoreID.Value,
                       
                    };
                    DB.StoredProducts.InsertOnSubmit(storedProduct);
                }

                var refilling = new StoreProductRefilling()
                {
                    StoredProduct = storedProduct,
                    Date = Date.Value,
                    Price = Price.Value,
                    Amount = Amount.Value,
                    UnitCode = Unit.Value.ToString(),
                };
                DB.StoreProductRefillings.InsertOnSubmit(refilling);
                DB.SubmitChanges();
                return new ContentResult();
            }
            return new ContentResult(){Content = "Необходимо корректно заполнить все поля"};
        }

        public ActionResult ViewTradingHistoryPartial(int ShopID, int ProductID)
        {

            ViewBag.ProductID = ProductID;
            var history =
                DB.Orders.Where(x => x.ShopID == ShopID && x.OrderedProducts.Any(z => z.ProductID == ProductID));
            return PartialView(history);
        }

        public ActionResult ViewTradingHistory()
        {
            return PartialView();
        }
        public ActionResult Search(string query, int? id)
        {
            var arts =
                DB.Products.Where(
                    x => x.OwnerID == CurrentUser.ShopOwnerID &&
                         SqlMethods.Like(x.Name.ToLower(), "{0}%".FormatWith(query.Trim().ToLower())) ||
                         SqlMethods.Like(x.Article.ToLower(), "{0}%".FormatWith(query.Trim().ToLower())));
            if (id.HasValue)
                arts = arts.Where(x => x.ID != id);
            return Json(arts.ToList().Select(x => new {label = /*x.MapCountry.Name + ", " +*/ x.Name, value = x.ID}),
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ProductComponents(int? id)
        {
            ViewBag.ProductID = id;
            var product = DB.Products.FirstOrDefault(x => x.ID == (id ?? 0));
            if (product != null && product.ComplexProduct != null
                )
            {
                return PartialView(DB.ComplexProductComponents.Where(x=> x.ComplexID == product.ComplexProduct.ID).ToList());
            }
            return PartialView(new List<ComplexProductComponent>());
        }

        [HttpPost]
        public ActionResult DeleteComponent(int ID, int ProductID)
        {
            var component = DB.ComplexProductComponents.FirstOrDefault(x => x.ID == ID);
            if(component==null)
                return new ContentResult(){Content = "0"};
            bool another = DB.ComplexProductComponents.Any(x => x.ID != ID && x.ComplexID == component.ComplexID);
            
            if (!another)
            {
                var complex = DB.ComplexProducts.FirstOrDefault(x => x.ID == component.ComplexID);
                if (complex != null)
                {
                    DB.ComplexProducts.DeleteOnSubmit(complex);
                }
                var prod = DB.Products.FirstOrDefault(x => x.ID == ProductID);
                if (prod != null)
                {
                    prod.ComplexID = null;
                }
            }
            DB.ComplexProductComponents.DeleteOnSubmit(component);
            DB.SubmitChanges();
            return new ContentResult(){Content = "1"};
        }

        [HttpPost]
        public ActionResult ProductComponents(int ProductID, int? NewComponentID, string ComponentName, int Amount)
        {
            ViewBag.ProductID = ProductID;
            var product = DB.Products.FirstOrDefault(x => x.ID == ProductID);
            if (product != null)
            {
                if (!NewComponentID.HasValue)
                {
                    var comp = DB.Products.FirstOrDefault(x => (x.Name == ComponentName || x.Article == ComponentName) && x.OwnerID == CurrentUser.ShopOwnerID);
                    if (comp != null)
                    {
                        NewComponentID = comp.ID;
                    }
                    if (!NewComponentID.HasValue)
                    {
                        ViewBag.Message = "Товар не найден в базе данных";
                        return PartialView(new List<ComplexProductComponent>());    
                    }
                }
               
                var target = DB.Products.FirstOrDefault(x => x.ID == NewComponentID.Value);
                if (target != null)
                {
                    if (product.ComplexProduct == null)
                    {
                        var cp = new ComplexProduct();
                        DB.ComplexProducts.InsertOnSubmit(cp);
                        product.ComplexProduct = cp;
                        DB.SubmitChanges();
                    }
                    var component = new ComplexProductComponent()
                    {
                        Amount = Amount,
                        ComplexID = product.ComplexProduct.ID,
                        Product = target
                    };
                    DB.ComplexProductComponents.InsertOnSubmit(component);
                    DB.SubmitChanges();
                    return PartialView(DB.ComplexProductComponents.Where(x => x.ComplexID == product.ComplexProduct.ID).ToList());

                }
                else
                {
                    ViewBag.Message = "Товар не найден в базе данных";
                    return PartialView(new List<ComplexProductComponent>());    
                }
            }
            else
            {
                ViewBag.Message = "Товар не найден в базе данных";
                return PartialView(new List<ComplexProductComponent>());    
            }
        }

        public ActionResult EditPriceFormula(int? productid, string targetid)
        {
            return null;
        }

        [HttpGet]
        public ActionResult ProductSize(int ID)
        {
            return PartialView(DB.Products.FirstOrDefault(x => x.ID == ID));
        }

        [HttpPost]
        public ActionResult ProductSize(int ID, decimal? Weight, decimal? Height, decimal? Length, decimal? Width)
        {
            var p = DB.Products.FirstOrDefault(x => x.ID == ID);
            if (p != null)
            {
                p.Width = Width;
                p.Height = Height;
                p.Length = Length;
                p.Weight = Weight;
                p.SubmitSuccess = true;
            }
            DB.SubmitChanges();
            
            return PartialView(p);
        }
    }
}
