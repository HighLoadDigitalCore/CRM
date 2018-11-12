using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trading.Extensions;
using Trading.Models;

namespace Trading.Controllers
{
    [Authorize(Roles = "ShopOwner")]
    public class ContractorController : BaseController
    {
        public ActionResult SavePercent(int id, decimal? percent)
        {
            var contractor = DB.Contractors.FirstOrDefault(x => x.ID == id);
            if (contractor != null)
            {
                contractor.Cost = percent;
                DB.SubmitChanges();
            }
            return new ContentResult();
        }
        
        public ActionResult Index(int? ShopID)
        {
            ViewBag.NoShops = !CurrentUser.ShopList.Any();
            return View(new MyContractors(ShopID));
        }

        public ActionResult Delete(int id)
        {
            var item = DB.Contractors.FirstOrDefault(x => x.ID == id);
            if (item != null)
            {
                DB.Contractors.DeleteOnSubmit(item);
                DB.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ContractorSearch()
        {
            return PartialView();
        }

        public ActionResult ContractorSearchContent(string Word)
        {
            if (Word.IsNullOrEmpty())
                return PartialView(new List<User>());

            Word = string.Format("%{0}%", Word.Replace("  ", " ").Replace("  ", " ").Replace(" ", "%")).ToLower();
            
            var users =
                DB.Users.Where(
                    x =>
                        (SqlMethods.Like(
                            ((x.UserSurname ?? "") + " " + (x.UserName ?? "") + " " + (x.UserPatrinomic ?? "")).ToLower(),
                            Word) || SqlMethods.Like(x.Email.ToLower(), Word)) &&
                        x.UsersInRoles.Any(z => z.Role.RoleName == "ShopOwner")).ToList();

            var shopsUsers =
                DB.Shops.Where(
                    x =>
                        SqlMethods.Like(x.Name.ToLower(), Word) ||
                        (x.ShopSettings.Any(z => z.ItemKey == "OrgName" && SqlMethods.Like(z.Value.ToLower(), Word))))
                    .ToList()
                    .Select(x => x.User);

            return PartialView(users.Concat(shopsUsers).DistinctBy(x => x.ID).OrderBy(x => x.FullName).ToList());

        }

        public ActionResult ContractorInvite(int ShopID, int UserID, string Message)
        {
            var exist =
                DB.Contractors.FirstOrDefault(
                    x => x.UserBy == CurrentUser.ID && x.UserFor == UserID && x.ShopID == ShopID);
            if (exist == null)
            {
                exist = new Contractor()
                {
                    UserBy = CurrentUser.ID,
                    UserFor = UserID,
                    ShopID = ShopID,
                    Message = Message,
                    Status = 0,
                    SendDate = DateTime.Now
                };
                DB.Contractors.InsertOnSubmit(exist);
                DB.SubmitChanges();
            }
            return new ContentResult();
        }

        public ActionResult StatusChange(int id, int status, int? rights)
        {
            var item = DB.Contractors.FirstOrDefault(x => x.ID == id);
            if (item != null)
            {
                item.Status = status;
                if (rights.HasValue)
                    item.AccessRight = rights.Value;
                DB.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
