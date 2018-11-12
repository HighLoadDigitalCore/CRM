using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Trading.Extensions;
using WebMatrix.WebData;

namespace Trading.Models
{


    public class UserFilter : BaseDataObject
    {
        public IEnumerable<SelectListItem> UserList { get; set; }
        public string LoginOrName { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public int? RoleID { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }

        public List<User> Result { get; set; }


        private string LoginOrNameLike
        {
            get { return string.Format("%{0}%", LoginOrName.Replace(" ", "%")).ToLower(); }
        }

        public UserFilter()
        {

            var roles =
                DB.Roles.AsEnumerable()
                    .Where(
                        x =>
                            CurrentUser.UsersInRoles.First().Role.UserAllowedRoles.Any(z => z.AllowedRoleID == x.RoleId))
                    .Select(x => new SelectListItem()
                    {
                        Selected = (x.RoleId == (RoleID ?? 0)),
                        Text = x.RoleDescription,
                        Value = x.RoleId.ToString()
                    }).ToList();


            roles.Insert(0, new SelectListItem() { Selected = !RoleID.HasValue, Text = "Все типы пользователей", Value = "" });

            RoleList = roles;

            Search(DB);

        }

        public void Search(DataContext db)
        {
            IQueryable<User> result = null;
            if (CurrentUser.UserRoles.Contains("Admin"))
            {
                result = db.Users.AsQueryable();
            }
            else if (CurrentUser.UserRoles.Contains("ShopOwner"))
            {
                var mans = DB.Managers.Where(x => x.ShopOwnerID == CurrentUser.ID).Select(x => x.ManagerUserID).ToList();
                mans.Add(CurrentUser.ID);
                result = db.Users.Where(x => mans.Contains(x.ID) && !(x.IsDeleted?? false));
            }
            else if (CurrentUser.UserRoles.Contains("Manager"))
            {
                var mans = DB.Managers.Where(x => x.ShopOwnerID == CurrentUser.ManagerProfiles.First().ShopOwnerID).Select(x => x.ManagerUserID).ToList();
                result = db.Users.Where(x => mans.Contains(x.ID) && !(x.IsDeleted ?? false));

            }

            if (MinDate.HasValue)
            {
                result =
                    result.Where(
                        x =>
                            x.Membership.CreateDate.HasValue && x.Membership.CreateDate.Value.Date >= MinDate.Value.Date);
            }
            if (MaxDate.HasValue)
            {
                result =
                    result.Where(
                        x =>
                            x.Membership.CreateDate.HasValue && x.Membership.CreateDate.Value.Date <= MaxDate.Value.Date);
            }
            if (LoginOrName.IsFilled())
            {
                result =
                    result.Where(
                        x =>
                            SqlMethods.Like(x.DigitID.ToLower(), LoginOrNameLike) ||
                            SqlMethods.Like(x.Email.ToLower(), LoginOrNameLike) ||
                            SqlMethods.Like(x.Name.ToLower(), LoginOrNameLike) ||
                            SqlMethods.Like(x.Phone, LoginOrNameLike) ||
                            SqlMethods.Like(
                                (x.UserSurname ?? "") + " " + (x.UserName ?? "") + " " + (x.UserPatrinomic ?? ""),
                                LoginOrNameLike));
            }

            if (RoleID.HasValue)
            {
                result = result.Where(x => x.UsersInRoles.Any(z => z.RoleId == RoleID));
            }

            /*
                        if (CurrentUser.UserRoles.Contains("ShopOwner"))
                        {
                            var managers = DB.Managers.Where(x => x.ShopOwnerID == CurrentUser.ID);
                            result = result.Where(x => managers.Any(z => z.ManagerUserID == x.ID));
                        }
            */

            Result = result.OrderByDescending(x => x.ID).Take(2000).ToList();
        }

    }

    public partial class User:BaseDataObject
    {

        public List<Manager> ManagerList
        {
            get
            {
                if (UserRoles.Contains("ShopOwner"))
                    return MyManagers.ToList();

                return ManagerProfiles.First().OwnerUser.MyManagers.Where(x=> !(x.User.IsDeleted?? false)).ToList();
            }
        }

        public List<Shop> ShopList
        {
            get
            {
                if (UserRoles.Contains("Admin"))
                {
                    return new List<Shop>();
                }
                if (UserRoles.Contains("ShopOwner"))
                {
                    return Shops.ToList();
                }
                if (UserRoles.Contains("Operator"))
                {
                    return OperatorShops.Select(x => x.Shop).ToList();
                }
                if (UserRoles.Contains("Manager"))
                {
                    return ManagerProfiles.First().ShopManagers.Select(x => x.Shop).ToList();
                }
                return new List<Shop>();
            }
        }
        public List<Shop> AvailShopList
        {
            get
            {
                if (UserRoles.Contains("Admin"))
                {
                    return new DataContext().Shops.ToList();
                }
                if (UserRoles.Contains("ShopOwner"))
                {
                    return Shops.ToList();
                }
                if (UserRoles.Contains("Operator"))
                {
                    return OperatorShops.Select(x => x.Shop).ToList();
                }
                if (UserRoles.Contains("Manager"))
                {
                    return ManagerProfiles.First().ShopManagers.Select(x => x.Shop).ToList();
                }
                return new List<Shop>();
            }
        }

        public string PermissionList { get; set; }
        public int UserRoleRadio { get; set; }
        public string Password { get; set; }
        public bool IsPost { get; set; }
     

        public string RedirectURL { get; set; }

        public string[] UserRoles
        {
            get
            {
                if (Name.IsNullOrEmpty()) return new string[] { };
                return RoleProvider.GetRolesForUser(Name);
            }
        }
        public string FullName
        {
            get
            {
                var name = "";
                if (UserSurname.IsFilled())
                {
                    name += UserSurname;
                }
                if (name.IsFilled())
                    name += " ";
                if (UserName.IsFilled())
                {
                    name += UserName;
                }
                if (name.IsNullOrEmpty())
                {
                    name = "[Anonimous]";
                }
                return name;
            }
        }

        public int ShopOwnerID
        {
            get
            {
                if (UserRoles.Contains("ShopOwner"))
                    return ID;
                else if (UserRoles.Contains("Manager")) return ManagerProfiles.First().ShopOwnerID;
                return 0;
            }

        }

        protected User _shopOwner;
        public User ShopOwner
        {
            get
            {
                if (_shopOwner == null)
                {
                    if (UserRoles.Contains("ShopOwner"))
                        _shopOwner = this;
                    else if (UserRoles.Contains("Manager"))
                        _shopOwner =
                            new DataContext().Users.FirstOrDefault(x => x.ID == ManagerProfiles.First().ShopOwnerID);
                    else _shopOwner = null;
                    
                }
                return _shopOwner;
            }

        }

        private List<Store> _storeList;
        public List<Store> StoreList
        {
            get
            {
                return _storeList ??
                       (_storeList = DB.Stores.Where(x => x.ShopStores.Any() && x.ShopStores.First().Shop.Owner == CurrentUser.ShopOwnerID).ToList());
            }
        }

        public string FullUrl
        {
            get { return "/Users/Edit/" + ID; }
        }

        private int? _defShopID;
        public int DefShopID
        {
            get
            {
                if (!_defShopID.HasValue)
                {
                    _defShopID = ((UserRoles.Contains("ShopOwner")
                        ? (DB.Shops.FirstOrDefault(x => x.Owner == ID)?? new Shop(){ID = 0}).ID
                        : DB.ShopManagers.First(x => x.Manager.ManagerUserID == ID).Shop.ID));
                }
                return _defShopID.Value;
            }
        }

        public int ShopIDOrDef
        {
            get { return HttpContext.Current.Request["ShopID"].ToNullInt() ?? CurrentUser.DefShopID; }
        }
        private int? _defStoreID;
        public int DefStoreID
        {
            get
            {
                if (!_defStoreID.HasValue)
                {

                    _defStoreID =
                        (DB.Stores.FirstOrDefault(x => x.OwnerID == this.ShopOwnerID) ?? new Store() {ID = 0}).ID;
                }
                return _defStoreID.Value;
                
            }
        }

        private List<Worker> _availableWorkers;
        public List<Worker> AvailableWorkers
        {
            get {
                return _availableWorkers ??
                       (_availableWorkers = DB.Workers.Where(x => x.OwnerID == CurrentUser.ShopOwnerID).ToList());
            }
        }

        public List<WorkerGroup> _availableWorkerGroups;
        public List<WorkerGroup> AvailableWorkerGroups
        {
            get {
                return _availableWorkerGroups ??
                       (_availableWorkerGroups =
                           DB.WorkerGroups.Where(x => x.OwnerID == CurrentUser.ShopOwnerID)
                               .OrderBy(x => x.Name)
                               .ToList());
            }
        }

        private List<Car> _availableCars;
        public List<Car> AvailableCars
        {
            get
            {
                return _availableCars ??
                       (_availableCars =
                           DB.Cars.Where(x => x.OwnerID == CurrentUser.ShopOwnerID).OrderBy(x => x.Name).ToList());
            }
        }

        public Car GetCarByID(int ID)
        {
            return AvailableCars.FirstOrDefault(x => x.ID == ID) ?? new Car();
        }

        private List<Worker> _availableCouriers;
        public List<Worker> AvailableCouriers
        {
            get {
                return _availableCouriers ??
                       (_availableCouriers = AvailableWorkers.Where(x => x.Job.Code == "Courier").ToList());
            }
        }


        public void SavePermissions(DataContext db)
        {
            var pl = PermissionList.Split<int>(",");

            var forDel = db.UserPermissions.Where(x => x.UserID == this.ID && !pl.Contains(x.PermissionID));
            if (forDel.Any())
            {
                db.UserPermissions.DeleteAllOnSubmit(forDel);
                db.SubmitChanges();
            }
            foreach (var pid in pl)
            {
                var exist = db.UserPermissions.FirstOrDefault(x => x.UserID == ID && x.PermissionID == pid);
                if (exist == null)
                {
                    db.UserPermissions.InsertOnSubmit(new UserPermission() { UserID = ID, PermissionID = pid });
                }
            }
            db.SubmitChanges();
        }

        public void SaveShops(DataContext DB, FormCollection collection)
        {
            var shops = (from key in collection.AllKeys
                         where key.StartsWith("Shop_")
                         select (int)collection.GetValue(key).ConvertTo(typeof(int))).ToList();

            if (this.UserRoles.Contains("Manager"))
            {
                var forDel =
                    DB.ShopManagers.Where(x => x.Manager.ManagerUserID == ID && !shops.Contains(x.ShopID ?? 0));
                DB.ShopManagers.DeleteAllOnSubmit(forDel);
                DB.SubmitChanges();

                var forAdd =
                    shops.Where(x => !DB.ShopManagers.Any(z => z.Manager.ManagerUserID == ID && z.ShopID == x));

                foreach (var shid in forAdd)
                {
                    var shop = DB.Shops.FirstOrDefault(x => x.ID == shid);
                    if (shop != null)
                    {
                        var manager =
                            DB.Managers.FirstOrDefault(
                                x => x.ManagerUserID == ID && x.ShopOwnerID == shop.Owner) ??
                            new Manager() { ManagerUserID = ID, ShopOwnerID = shop.Owner };

                        DB.ShopManagers.InsertOnSubmit(new ShopManager() { Manager = manager, Shop = shop });
                    }
                }


                DB.SubmitChanges();

            }

            if (UserRoles.Contains("Operator"))
            {
                var forDel =
                    DB.OperatorShops.Where(x => x.UserID == ID && !shops.Contains(x.ShopID));
                DB.OperatorShops.DeleteAllOnSubmit(forDel);
                DB.SubmitChanges();

                var forAdd =
                    shops.Where(x => !DB.OperatorShops.Any(z => z.UserID == ID && z.ShopID == x));

                foreach (var shid in forAdd)
                {
                    var shop = DB.Shops.FirstOrDefault(x => x.ID == shid);
                    if (shop != null)
                    {
                        var op = new OperatorShop() { Shop = shop, UserID = ID };
                        DB.OperatorShops.InsertOnSubmit(op);
                    }
                }
                DB.SubmitChanges();
            }

        }

        public bool HasInvitesToBeContractorFromMe(int shopID)
        {
            return HasInvitesToBeContractorFrom(CurrentUser.ID, shopID);
        }
        public bool HasInvitesToBeContractorFromMe()
        {
            return HasInvitesToBeContractorFrom(CurrentUser.ID, null);
        }
        public bool HasInvitesToBeContractorFrom(int id, int? shopID)
        {
            if (!shopID.HasValue)
                return DB.Contractors.Any(x => x.UserBy == id && x.UserFor == ID);
            return DB.Contractors.Any(x => x.UserBy == id && x.UserFor == ID && x.ShopID == shopID);
        }

        public IEnumerable<WorkerGroup> AvailableWorkerGroupsForCar(Car car)
        {
            var groups = AvailableWorkerGroups;
            if (car.Passengers.HasValue)
            {
                return
                    groups.Where(
                        x =>
                            x.WorkerGroupParticipants.Any(z => z.Worker.Job.HasCar) &&
                            x.WorkerGroupParticipants.Count <= car.Passengers.Value).ToList();
            }
            return groups;
        }

        public Worker GetWorkerByID(int ID)
        {
            return AvailableWorkers.FirstOrDefault(x=> x.ID == ID) ?? new Worker();
        }
    }

    public partial class Contractor:BaseDataObject
    {
        public User UserAnotherEntity
        {
            get
            {
                if (UserBy != CurrentUser.ID)
                    return UserByEntity;
                return UserForEntity;
            }
        }
    }
}