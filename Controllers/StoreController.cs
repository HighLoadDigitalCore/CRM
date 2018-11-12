using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trading.Extensions;
using Trading.Models;

namespace Trading.Controllers
{
    public class StoreController : BaseController
    {
        public ActionResult Stores(/*int? ShopID*/)
        {
/*
            if (!ShopID.HasValue)
                ShopID = DefShopID;
*/
            var stores = DB.Stores.Where(x => x.OwnerID == CurrentUser.ShopOwnerID).OrderBy(x => x.Name);
            return View(stores.ToList());
        }


        public ActionResult StoreDelete(int id)
        {
            var store = DB.Stores.FirstOrDefault(x => x.ID == id);
            int? ShopID = null;
            if (store != null)
            {
                /*ShopID = store.ShopID;*/
                DB.Stores.DeleteOnSubmit(store);
                DB.SubmitChanges();
            }
            return RedirectToAction("Stores", new { ShopID });
        }

        [HttpGet]
        public ActionResult StoreEdit(int? ID/*, int? ShopID*/)
        {
            var store = new Store() { DeliveryAddress = new DeliveryAddress() };
            if (ID.HasValue)
            {
                store = DB.Stores.FirstOrDefault(x => x.ID == ID) ??
                        new Store()
                        {
                            OwnerID = CurrentUser.ShopOwnerID,
                            DeliveryAddress =
                                new DeliveryAddress()
                        };
            }

/*
            if (!ShopID.HasValue)
            {
                ShopID = store.ID > 0 ? store.ShopID : DefShopID;
            }
            store.ShopID = ShopID.Value;
            ViewBag.ShopID = store.ShopID;
*/
            return View(store);
        }

        [HttpPost]
        public ActionResult StoreEdit(Store store, FormCollection collection/*, int? ShopID*/)
        {
            store.IsPost = true;

            if (store.Name.IsNullOrEmpty() || store.DeliveryAddress.Region.IsNullOrEmpty() ||
                store.DeliveryAddress.District.IsNullOrEmpty() || store.DeliveryAddress.Town.IsNullOrEmpty() ||
                store.DeliveryAddress.Street.IsNullOrEmpty() || store.DeliveryAddress.House.IsNullOrEmpty())
                return View(store);

/*
            if (ShopID.HasValue)
                store.ShopID = ShopID.Value;
*/
            if (store.ID == 0)
            {

                DB.Stores.InsertOnSubmit(store);
            }
            else
            {
                var dbs = DB.Stores.First(x => x.ID == store.ID);
                dbs.Name = store.Name;
                dbs.DeliveryAddress.LoadPossibleProperties(store.DeliveryAddress, new[] { "ID" });
                dbs.DeliveryAddress.MapPoint.Lat = store.DeliveryAddress.Lat;
                dbs.DeliveryAddress.MapPoint.Lng = store.DeliveryAddress.Lng;

/*
                dbs.ShopID = store.ShopID;
*/
            }

            DB.SubmitChanges();
            return RedirectToAction("Stores");
            //return View(store);
        }

        public ActionResult StoreProducts(int id)
        {
            var products =
                DB.StoredProducts.Where(x => x.StoreID == id)
                    .Select(x => x.Product)
                    .DistinctBy(x => x.ID)
                    .OrderBy(x => x.Name)
                    .ToList();
            return View(products);
        }
        public ActionResult AdressEditor(PointSettings pointSettings)
        {

            return PartialView(pointSettings);
        }
    }
}
