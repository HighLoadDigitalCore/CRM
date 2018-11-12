using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trading.Extensions;

namespace Trading.Models
{
    public class ProductSearchTableOptions
    {
        public string Page { get; set; }
        public bool HideFilter { get; set; }
        public bool HideShopFilter { get; set; }
        public bool HideCreateRangeFilter { get; set; }

        public bool HideCheckBoxes { get; set; }
        public bool HideCreateProductBtn { get; set; }
        public string AddViewName { get; set; }
        public string AddControllerName { get; set; }
        public bool HideActionMenu { get; set; }
        public bool HideStoreFilter { get; set; }
    }
    public class ProductSearchBlock : BaseDataObject
    {
        public string Title { get; set; }
        public ProductSearchTableOptions Options { get; set; }
        private ProductFilter _model;

        public ProductFilter Model
        {
            get
            {
                if (_model == null)
                {
                    _model = new ProductFilter();
                }
                return _model;
            }
            set { _model = value; }
        }

        public bool HasPermission { get; set; }
        public string Page { get; set; }

        public ProductSearchBlock(string page)
        {
            var helper = new PermissionHelper();
            Page = page;
            switch (page)
            {
                case "Common":
                default:
                    Title = "Список товаров";
                    Options = new ProductSearchTableOptions()
                    {
                        HideCheckBoxes = true,
                        HideCreateRangeFilter = false,
                        HideActionMenu = true,
                        HideShopFilter = false,
                        HideStoreFilter = true,
                        AddViewName = "",
                        AddControllerName = "",
                    };
                    HasPermission = helper.HasPermission(Permissions.ProductsEdit);
                    break;

                case "Stores":

                    Title = "Товары на складах";
                    Options = new ProductSearchTableOptions()
                    {
                        HideCheckBoxes = true,
                        HideCreateRangeFilter = false,
                        HideActionMenu = true,
                        HideShopFilter = true,
                        HideStoreFilter = false,
                        AddViewName = "",
                        AddControllerName = "",
                    };
                    HasPermission = helper.HasPermission(Permissions.StoreProducts);
                    break;
            }
            Options.Page = page;
        }
    }
    public class ProductFilter : BaseDataObject
    {
        public List<SelectListItem> ShopList { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public int? ShopID { get; set; }
        public int? StoreID { get; set; }

        public List<Product> Result { get; set; }


        public ProductFilter()
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


            if (!StoreID.HasValue)
            {
                StoreID = HttpContext.Current.Request.QueryString["StoreID"].ToNullInt();
            }
            if (!StoreID.HasValue)
            {
                var index = HttpContext.Current.Request.RawUrl.IndexOf("?");
                if (index < 0)
                    index = HttpContext.Current.Request.RawUrl.Length;
                var parts = HttpContext.Current.Request.RawUrl.Substring(0, index).Split<string>("/").ToList();
                if (parts.Contains("StoreID"))
                {
                    StoreID = parts.ElementAt(parts.IndexOf("StoreID") + 1).ToNullInt();
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

            ShopList.Insert(0,
                new SelectListItem() {Selected = !ShopID.HasValue, Text = "Без привязки к магазину", Value = "0"});

            StoreList =
                CurrentUser.StoreList.Select(
                    x =>
                        new SelectListItem() {Text = x.Name, Value = x.ID.ToString(), Selected = (StoreID ?? -1) == x.ID})
                    .ToList();

            StoreList.Insert(0,
                new SelectListItem()
                {
                    Selected = !StoreID.HasValue || StoreID.Value == 0,
                    Text = "Без привязки к складу",
                    Value = "0"
                });
        }

        public void Search(DataContext db)
        {

            if (!ShopList.Any())
            {
                Result = new List<Product>();
                return;

            }
            IQueryable<Product> result = null;
            if (StoreID.HasValue && StoreID.Value > 0)
            {
                result = db.Products.Where(x => x.StoredProducts.Any(z=> z.StoreID == StoreID));
            }
            else if (ShopID.HasValue && ShopID > 0)
            {
                result = db.Products.Where(x => x.ShopProducts.Any(z=> z.ShopID == ShopID));
            }
            else if ((!StoreID.HasValue || StoreID.Value == 0) || (!ShopID.HasValue || ShopID.Value == 0))
            {
                var profiles = CurrentUser.ManagerList.Select(x => x.User.ID).ToList();
                result =
                    db.Products.Where(x => x.OwnerID == CurrentUser.ShopOwnerID || profiles.Contains(CurrentUser.ID));
            }
            //
            if (MinDate.HasValue)
            {
                result =
                    result.Where(
                        x =>
                            x.AddedDate.Date >= MinDate.Value);
            }
            if (MaxDate.HasValue)
            {
                result =
                    result.Where(
                        x =>
                            x.AddedDate <= MaxDate.Value);
            }


            Result = result.OrderByDescending(x => x.Name).ThenByDescending(x => x.ID).Take(5000).ToList();


        }

    }
}