using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trading.Extensions;

namespace Trading.Models
{

    public enum Permissions
    {
        ShopAdding = 1,
        ShopEditing = 2,
        ShopListView = 4,
        UserAdd = 5,
        UserEdit = 6,
        ShopCharEdit = 7,
        UserDelete = 8,
        UserLock = 9,
        ShopDelete = 10,
        UserListView = 11,
        OrderAdd = 12,
        OrderEdit = 13,
        OrderStatusChange = 14,
        OrderListView = 15,
        OrderDelete = 16,
        UserAuth = 17,
        MarkList = 18,
        MarkCreate = 19,
        OrderStatusPayed = 20,
        StatClientsCompare = 1021,
        StatQuality = 1022,
        StatOrderAmount = 1024,
        StatMiddlePrice = 1027,
        StatOrderVolume = 1028,
        StatCommon = 1029,
        OrderSMSSend = 1030,
        OrderMailSend = 1031,
        StatStatus = 1033,
        DeliveryStores = 1034,
        DeliveryZones = 1036,
        DeliveryOrders = 1037,
        StatConstructor = 1038,
        ContragentEdit = 1039,
        Import = 1041,
        SelfCostWrite = 1042,
        SelfCostRead = 1043,
        OptCostWrite = 1045,
        OptCostRead = 1046,
        DeliveryEdit = 1047,
        ProductsEdit = 1048,
        StoreProducts = 1049,
        Workers = 1050,
        WorkersGroups = 1051,
        MailSettings = 1052,
        Cars = 1053,
        SMSSettings = 1054,
        DeliveryApprove = 1055,
        DeliveryApprovedEdit = 1056
    }

    public class PermissionHelper : BaseDataObject
    {
        public bool HasPermission(Permissions p)
        {

            CurrentUser.UsersInRoles.Load();
            if (!CurrentUser.UsersInRoles.Any())
                return false;
            if (CurrentUser.UsersInRoles.Any(x => x.Role.RoleName == "Admin"))
                return true;
            return
                DB.PermissionsForRoles.Any(
                    x => x.PermissionID == (int)p && x.RoleID == CurrentUser.UsersInRoles.First().RoleId) ||
                DB.UserPermissions.Any(x => x.UserID == CurrentUser.ID && x.PermissionID == (int)p);
        }

    }

    public class PermissionGroupModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public List<UserPermissionModel> AllowedPermissions { get; set; }


    }

    public class UserPermissionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public bool IsAdded { get; set; }


    }

    public class PermissionModel : BaseDataObject
    {


        public string TargetPermissions { get; set; }

        public PermissionModel(int? TargetUserID)
        {
            Groups = new List<PermissionGroupModel>();
            var user = DB.Users.First(x => x.ID == CurrentUser.ID);
            if (!user.UsersInRoles.Any())
                return;

            var role = user.UsersInRoles.First()
                .Role;
            Groups =
                role.PermissionsForRoles.Select(x => x.Permission.PermissionGroup)
                    .Concat(
                        DB.UserPermissions.Where(x => x.UserID == CurrentUser.ID)
                            .Select(x => x.Permission.PermissionGroup))
                    .Distinct()
                    .Select(x => new PermissionGroupModel() {ID = x.ID, Name = x.Name})
                    .OrderBy(x => x.Name)
                    .ToList();


            var targetPermissions = DB.UserPermissions.Where(x => x.UserID == TargetUserID).ToList();
            TargetPermissions = targetPermissions.Select(x => x.PermissionID).JoinToString(",");

            foreach (var g in Groups)
            {
                g.AllowedPermissions = new List<UserPermissionModel>();

                var permissions =
                    DB.PermissionsForRoles.Where(x => x.RoleID == role.RoleId && x.Permission.GroupID == g.ID).Select(x=> x.Permission).
                    Concat(DB.UserPermissions.Where(x=> x.Permission.GroupID == g.ID && x.UserID == CurrentUser.ID).Select(x=> x.Permission)
                    ).Distinct()
                        .OrderBy(x => x.Name).ToList()
                        .Select(
                            x =>
                                new UserPermissionModel()
                                {
                                    ID = x.ID,
                                    Name = x.Name,
                                    IsAdded = targetPermissions.Any(z => z.PermissionID == x.ID)
                                });
                g.AllowedPermissions = permissions.ToList();
            }
        }

        public List<PermissionGroupModel> Groups { get; set; }
    }
}