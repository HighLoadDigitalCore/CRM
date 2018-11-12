using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebMatrix.WebData;

namespace Trading.Models
{

    public class OrderSearchTableOptions
    {
        public string Page { get; set; }
        public bool HideFilter { get; set; }
        public bool HideShopFilter { get; set; }
        public bool HideDeliveryRangeFilter { get; set; }
        public bool HideCreateRangeFilter { get; set; }

        public bool HideCheckBoxes { get; set; }
        public bool HideDeliveryExactFilter { get; set; }
        public bool HideCreateOrderBtn { get; set; }
        public string AddViewName { get; set; }
        public string AddControllerName { get; set; }
        public bool HideActionMenu { get; set; }
        public bool HideExportMenu { get; set; }
        public bool HideStatSave { get; set; }
        public bool HideSpamSend { get; set; }
        public bool HideStatusChange { get; set; }
        public bool HideContragent { get; set; }
        public bool HideRegionFilter { get; set; }
        public string SMSTemplate { get; set; }
        public string EmailTemplate { get; set; }
        public string AddViewNameLower { get; set; }
        public string AddControllerNameLower { get; set; }
        public bool HideBody { get; set; }
        public bool SkipAutoPost { get; set; }
    }

    public class OrderSearchBlock : BaseDataObject
    {
        public string Title { get; set; }
        public OrderSearchTableOptions Options { get; set; }
        private OrderFilter _model;

        public OrderFilter Model
        {
            get
            {
                if (_model == null)
                {
                    _model = new OrderFilter();
                }
                return _model;
            }
            set { _model = value; }
        }

        public bool HasPermission { get; set; }
        public string Page { get; set; }

        public OrderSearchBlock(string page)
        {
            var helper = new PermissionHelper();
            Page = page;
            switch (page)
            {
                case "Common":
                default:
                    Title = "Список заказов";
                    Options = new OrderSearchTableOptions()
                    {
                        HideCheckBoxes = false,
                        HideCreateRangeFilter = false,
                        HideDeliveryRangeFilter = false,
                        HideShopFilter = false,
                        HideFilter = false,
                        HideDeliveryExactFilter = true,
                        HideActionMenu = false,
                        AddViewName = "",
                        AddControllerName = "",
                        HideRegionFilter = false
                    };
                    HasPermission = helper.HasPermission(Permissions.OrderListView);
                    break;

                case "StatConstructor":
                    Title = "Поиск заказов";
                    Options = new OrderSearchTableOptions()
                    {
                        HideCheckBoxes = false,
                        HideCreateRangeFilter = false,
                        HideDeliveryRangeFilter = false,
                        HideShopFilter = false,
                        HideFilter = false,
                        HideDeliveryExactFilter = true,
                        HideActionMenu = false,
                        HideCreateOrderBtn = true,
                        AddViewName = "",
                        AddControllerName = "",
                        HideExportMenu = true,
                        HideSpamSend = true,
                        HideStatusChange = true,
                        HideStatSave = false,
                        HideRegionFilter = true

                    };
                    HasPermission = helper.HasPermission(Permissions.OrderListView);
                    break;


                case "Overage":

                    Title = "Заказы с оценкой за 2 недели";
                    Options = new OrderSearchTableOptions()
                    {
                        HideCheckBoxes = true,
                        HideCreateRangeFilter = false,
                        HideDeliveryRangeFilter = false,
                        HideShopFilter = false,
                        HideFilter = false,
                        HideDeliveryExactFilter = true,
                        HideActionMenu = true,
                        AddViewName = "Quality",
                        AddControllerName = "Graph",
                        HideRegionFilter = true
                    };

                    HasPermission = helper.HasPermission(Permissions.OrderListView);
                    break;

                case "StatCommon":

                    Title = "Общая статистика";
                    Options = new OrderSearchTableOptions()
                    {
                        HideCheckBoxes = true,
                        HideCreateRangeFilter = false,
                        HideDeliveryRangeFilter = true,
                        HideShopFilter = false,
                        HideFilter = false,
                        HideDeliveryExactFilter = true,
                        HideActionMenu = true,
                        AddViewName = "Common",
                        AddControllerName = "Graph",
                        HideRegionFilter = true
                    };

                    HasPermission = helper.HasPermission(Permissions.StatCommon);
                    break;

                case "StatQuality":

                    Title = "Общая статистика";
                    Options = new OrderSearchTableOptions()
                    {
                        HideCheckBoxes = true,
                        HideCreateRangeFilter = false,
                        HideDeliveryRangeFilter = true,
                        HideShopFilter = false,
                        HideFilter = false,
                        HideDeliveryExactFilter = true,
                        HideActionMenu = true,
                        AddViewName = "Quality",
                        AddControllerName = "Graph",
                        HideRegionFilter = true
                    };

                    HasPermission = helper.HasPermission(Permissions.StatQuality);
                    break;

                case "StatStatus":

                    Title = "Статистика по статусам заказов";
                    Options = new OrderSearchTableOptions()
                    {
                        HideCheckBoxes = true,
                        HideCreateRangeFilter = false,
                        HideDeliveryRangeFilter = true,
                        HideShopFilter = false,
                        HideFilter = false,
                        HideDeliveryExactFilter = true,
                        HideActionMenu = true,
                        AddViewName = "Status",
                        AddControllerName = "Graph",
                        HideRegionFilter = true
                    };

                    HasPermission = helper.HasPermission(Permissions.StatStatus);
                    break;
                case "StatOrderVolume":

                    Title = "Статистика по объему заказов";
                    Options = new OrderSearchTableOptions()
                    {
                        HideCheckBoxes = true,
                        HideCreateRangeFilter = false,
                        HideDeliveryRangeFilter = true,
                        HideShopFilter = false,
                        HideFilter = false,
                        HideDeliveryExactFilter = true,
                        HideActionMenu = true,
                        AddViewName = "OrderVolume",
                        AddControllerName = "Graph",
                        HideRegionFilter = true
                    };

                    HasPermission = helper.HasPermission(Permissions.StatOrderVolume);
                    break;
                case "StatOrderAmount":

                    Title = "Статистика по количеству заказов";
                    Options = new OrderSearchTableOptions()
                    {
                        HideCheckBoxes = true,
                        HideCreateRangeFilter = false,
                        HideDeliveryRangeFilter = true,
                        HideShopFilter = false,
                        HideFilter = false,
                        HideDeliveryExactFilter = true,
                        HideActionMenu = true,
                        AddViewName = "OrderAmount",
                        AddControllerName = "Graph",
                        HideRegionFilter = true
                    };

                    HasPermission = helper.HasPermission(Permissions.StatOrderAmount);
                    break;
                case "StatMiddlePrice":

                    Title = "Статистика по среднему чеку";
                    Options = new OrderSearchTableOptions()
                    {
                        HideCheckBoxes = true,
                        HideCreateRangeFilter = false,
                        HideDeliveryRangeFilter = true,
                        HideShopFilter = false,
                        HideFilter = false,
                        HideDeliveryExactFilter = true,
                        HideActionMenu = true,
                        AddViewName = "MiddlePrice",
                        AddControllerName = "Graph",
                        HideRegionFilter = true
                    };

                    HasPermission = helper.HasPermission(Permissions.StatMiddlePrice);
                    break;
                case "StatClientsCompare":

                    Title = "Стастистика удовлетворенности клиентов";
                    Options = new OrderSearchTableOptions()
                    {
                        HideCheckBoxes = true,
                        HideCreateRangeFilter = false,
                        HideDeliveryRangeFilter = true,
                        HideShopFilter = false,
                        HideFilter = false,
                        HideDeliveryExactFilter = true,
                        HideActionMenu = true,
                        AddViewName = "ClientsCompare",
                        AddControllerName = "Graph",
                        HideRegionFilter = true
                    };

                    HasPermission = helper.HasPermission(Permissions.StatClientsCompare);
                    break;

                case "Delivery":

                    Title = "Заказы для доставки";
                    Options = new OrderSearchTableOptions()
                    {
                        HideCheckBoxes = false,
                        HideCreateRangeFilter = true,
                        HideDeliveryRangeFilter = true,
                        HideShopFilter = false,
                        HideFilter = false,
                        HideDeliveryExactFilter = false,
                        HideActionMenu = false,
                        HideBody = true,
                        AddViewNameLower = "OrderMap",
                        AddControllerNameLower = "Delivery",
                        HideRegionFilter = true,
                        SMSTemplate = "",
                        EmailTemplate = "DeliveryLetter",
                        SkipAutoPost = true
                    };

                    HasPermission = helper.HasPermission(Permissions.StatClientsCompare);
                    break;
            }
            Options.Page = page;
        }
    }

    public class BaseDataObject
    {
        [ScriptIgnore]
        private List<Contractor> _availableContractors;
        public List<Contractor> AvailableContractors
        {
            get
            {
                return _availableContractors ??
                       (_availableContractors = DB.Contractors.Where(x => x.UserFor == CurrentUser.ShopOwnerID)
                           .ToList()
                           .OrderBy(x => x.Shop.OrgNameOrName)
                           .ToList());
            }
        }
        [ScriptIgnore]
        public bool SubmitSuccess { get; set; }
        [ScriptIgnore]
        public bool IsPost { get; set; }

        [ScriptIgnore]
        public bool HasCreated { get; set; }
        [ScriptIgnore]
        public string ErrorText { get; set; }

        private DataContext _db;
        [ScriptIgnore]
        public DataContext DB
        {
            get { return _db ?? (_db = new DataContext(ConfigurationManager.ConnectionStrings["TradingConnectionString"].ConnectionString)); }
        }
        [ScriptIgnore]
        public SimpleMembershipProvider MembershipProvider
        {
            get
            {
                return (SimpleMembershipProvider)System.Web.Security.Membership.Provider;

            }
        }
        [ScriptIgnore]
        public SimpleRoleProvider RoleProvider
        {
            get
            {
                return (SimpleRoleProvider)Roles.Provider;
            }
        }



        private User _currentUser;
        [ScriptIgnore]
        public User CurrentUser
        {
            get
            {
                return _currentUser ?? (_currentUser = DB.Users.FirstOrDefault(x => x.ID == WebSecurity.CurrentUserId));
            }
            set { _currentUser = value; }
        }
    }
}