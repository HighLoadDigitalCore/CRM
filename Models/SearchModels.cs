using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trading.Extensions;

namespace Trading.Models
{
    public class SearchResult
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Group { get; set; }
    }

    public class SearchModel : BaseDataObject
    {
        protected string Word { get; set; }
        public List<SearchResult> Results { get; set; }
        public SearchModel(string word)
        {
            Word = word.Replace("  ", " ").Replace(" ", "%").ToLower();
            Results = new List<SearchResult>();
        }

        public void Search()
        {
            if(Word.IsNullOrEmpty())
                return;
            
            IQueryable<User> result = null;
            if (CurrentUser.UserRoles.Contains("Admin"))
            {
                result = DB.Users.AsQueryable();
            }
            else if (CurrentUser.UserRoles.Contains("ShopOwner"))
            {
                var mans = DB.Managers.Where(x => x.ShopOwnerID == CurrentUser.ID).Select(x => x.ManagerUserID).ToList();
                mans.Add(CurrentUser.ID);
                result = DB.Users.Where(x => mans.Contains(x.ID) && !(x.IsDeleted ?? false));
            }
            else if (CurrentUser.UserRoles.Contains("Manager"))
            {
                var mans = DB.Managers.Where(x => x.ShopOwnerID == CurrentUser.ManagerProfiles.First().ShopOwnerID).Select(x => x.ManagerUserID).ToList();
                result = DB.Users.Where(x => mans.Contains(x.ID) && !(x.IsDeleted ?? false));

            }
            if (result != null)
            {
                result =
                    result.Where(
                        x =>
                            SqlMethods.Like(x.Name.ToLower(), "%{0}%".FormatWith(Word)) ||
                            SqlMethods.Like(x.Email.ToLower(), "%{0}%".FormatWith(Word)) ||
                            SqlMethods.Like(x.UserSurname.ToLower(), "%{0}%".FormatWith(Word)) ||
                            SqlMethods.Like(x.Phone.ToLower(), "%{0}%".FormatWith(Word)));

                Results.AddRange(
                    result.ToList().Select(
                        x => new SearchResult() { Group = "Пользователи системы", Name = x.FullName, Link = x.FullUrl }));
            }

            IQueryable<Order> orders = null;
            if (CurrentUser.UserRoles.Contains("ShopOwner"))
            {
                var orderAuthors =
                    DB.ShopManagers.Where(x => x.Manager.ShopOwnerID == CurrentUser.ID)
                        .Select(x => x.Manager.ManagerUserID)
                        .ToList();
                orderAuthors.Add(CurrentUser.ID);

                orders = DB.Orders.Where(x => orderAuthors.Contains(x.AddedByID));
            }
            else if (CurrentUser.UserRoles.Contains("Manager"))
            {
                var orderAuthors =
                    DB.ShopManagers.Where(x => x.Manager.ShopOwnerID == CurrentUser.ManagerProfiles.First().ShopOwnerID)
                        .Select(x => x.Manager.ManagerUserID)
                        .ToList();

                orderAuthors.Add(CurrentUser.ManagerProfiles.First().ShopOwnerID);

                var shids = DB.ShopManagers.Where(x => x.Manager.ManagerUserID == CurrentUser.ID).Select(x => x.ShopID).ToList();

                orders = DB.Orders.Where(x => orderAuthors.Contains(x.AddedByID) && shids.Contains(x.ShopID));

            }
            else if (CurrentUser.UserRoles.Contains("Operator"))
            {
                var shds = DB.OperatorShops.Where(x => x.UserID == CurrentUser.ID).Select(x => x.ShopID).ToList();
                orders = DB.Orders.Where(x => shds.Contains(x.ShopID ?? 0));
            }
            else
            {
                orders = DB.Orders.Where(x => x.AddedByID == CurrentUser.ID);
            }
            if (orders != null)
            {
                orders =
                    orders.Where(
                        x =>
                            SqlMethods.Like(x.Warning.ToLower(), Word) || SqlMethods.Like(x.ID.ToString(), Word) ||
                            x.OrderedProducts.Any(z => SqlMethods.Like(z.Product.Name.ToLower(), Word)));
                Results.AddRange(
                    orders.ToList()
                        .Select(x => new SearchResult() { Group = "Заказы", Name = string.Format("Заказ №{0}", x.ID), Link = x.FullUrl}));
            }
        }
    }
}