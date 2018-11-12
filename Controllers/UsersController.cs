using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Trading.Extensions;
using Trading.Extensions.Helpers;
using Trading.Models;
using WebMatrix.WebData;

namespace Trading.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {

        [HttpGet]
        public ActionResult Index()
        {
            var filter = new UserFilter();
            return View(filter);
        }

        public ActionResult UserSearch(UserFilter filter)
        {
            filter.Search(DB);
            return PartialView(filter);
        }

        public ActionResult Edit(int? ID)
        {
            var myRoles = DB.Roles.Where(x => CurrentUser.UserRoles.Contains(x.RoleName)).Select(x => x.RoleId).ToList();
            var allowedRoles =
                DB.UserAllowedRoles.Where(x => myRoles.Contains(x.RoleID))
                    .Select(x => DB.Roles.FirstOrDefault(z => z.RoleId == x.AllowedRoleID))
                    .Where(x => x != null)
                    .Distinct()
                    .ToList();
            ViewBag.AllowedRoles = allowedRoles;

            User user = null;
            if (ID.HasValue)
            {
                user = DB.Users.FirstOrDefault(x => x.ID == ID);
                if (user == null || ((user.IsDeleted ?? false) && RoleProvider.IsUserInRole(CurrentUser.Name, "Admin")))
                    return RedirectToAction("Index");
            }
            else
            {
                user = new User() { Password = new Random(DateTime.Now.Millisecond).GeneratePassword() };
            }
            if (user.ID > 0)
                user.UserRoleRadio = DB.Roles.First(x => x.RoleName == user.UserRoles.First()).RoleId;
            return View(user);
        }

        [HttpGet]
        public ActionResult EditPartial(int? ID)
        {
            var myRoles = DB.Roles.Where(x => CurrentUser.UserRoles.Contains(x.RoleName)).Select(x => x.RoleId).ToList();
            var allowedRoles =
                DB.UserAllowedRoles.Where(x => myRoles.Contains(x.RoleID))
                    .Select(x => DB.Roles.FirstOrDefault(z => z.RoleId == x.AllowedRoleID))
                    .Where(x => x != null)
                    .Distinct()
                    .ToList();
            ViewBag.AllowedRoles = allowedRoles;

            User user = null;
            if (ID.HasValue)
            {
                user = DB.Users.FirstOrDefault(x => x.ID == ID);
                if (user == null || ((user.IsDeleted ?? false) && RoleProvider.IsUserInRole(CurrentUser.Name, "Admin")))
                {
                    user = new User();
                    user.RedirectURL = Url.Action("Index");
                    return PartialView(user);
                }
            }
            else
            {
                user = new User() { Password = new Random(DateTime.Now.Millisecond).GeneratePassword() };
            }
            if (user.ID > 0)
                user.UserRoleRadio = DB.Roles.First(x => x.RoleName == user.UserRoles.First()).RoleId;
            return PartialView(user);
        }


        [HttpPost]
        public ActionResult EditPartial(User user, FormCollection collection)
        {
            var myRoles = DB.Roles.Where(x => CurrentUser.UserRoles.Contains(x.RoleName)).Select(x => x.RoleId).ToList();
            var allowedRoles =
                DB.UserAllowedRoles.Where(x => myRoles.Contains(x.RoleID))
                    .Select(x => DB.Roles.FirstOrDefault(z => z.RoleId == x.AllowedRoleID))
                    .Where(x => x != null)
                    .Distinct()
                    .ToList();
            ViewBag.AllowedRoles = allowedRoles;

            user.IsPost = true;
            if (!user.Email.IsMailAdress() || user.UserName.IsNullOrEmpty() || user.UserSurname.IsNullOrEmpty() ||
                (user.UserRoleRadio == 0 && user.ID!=CurrentUser.ID))
            {
                return PartialView(user);
            }

            if (user.ID > 0)
            {
                var dbu = DB.Users.First(x => x.ID == user.ID);
                dbu.LoadPossibleProperties(user);
                if (dbu.Email != dbu.Name)
                    dbu.Name = dbu.Email;

                DB.SubmitChanges();

                if (user.ID != CurrentUser.ID)
                {


                    if (dbu.UsersInRoles.All(x => x.RoleId != user.UserRoleRadio))
                    {
                        DB.UsersInRoles.DeleteAllOnSubmit(dbu.UsersInRoles);
                        DB.SubmitChanges();
                        DB.UsersInRoles.InsertOnSubmit(new UsersInRole() {RoleId = user.UserRoleRadio, UserId = dbu.ID});
                        DB.SubmitChanges();
                    }

                    if (user.Password.IsFilled())
                    {
                        Logger.WriteEvent(Logger.EventType.UserChangePass,
                            "Изменение пароля для пользователя " + user.Email);
                        WebSecurity.ResetPassword(WebSecurity.GeneratePasswordResetToken(dbu.Name), user.Password);
                    }
                }
                Logger.WriteEvent(Logger.EventType.UserEdit, "Редактирование пользователя, Email пользователя - " + user.Email);
            }
            else
            {

                var dict = new Dictionary<string, object>();
                dict.Add("UserName", user.UserName);
                dict.Add("UserSurname", user.UserSurname);
                dict.Add("Email", user.Email);
                dict.Add("UserPatrinomic", user.UserPatrinomic);
                dict.Add("Phone", user.Phone);


                var rand = new Random(DateTime.Now.Millisecond);
                if (user.UserRoleRadio == DB.Roles.First(x => x.RoleName == "ShopOwner").RoleId)
                {
                    string digitID = "";

                    for (int i = 0; i < 500; i++)
                    {
                        var digit = rand.Next(100000000, 999999999);
                        if (!DB.Users.Any(x => x.DigitID == digit.ToString()))
                        {
                            digitID = digit.ToString();
                            break;
                        }
                    }
                    dict.Add("DigitID", digitID);
                }
                var pass = rand.GeneratePassword();

                var roleName = DB.Roles.First(x => x.RoleId == user.UserRoleRadio).RoleName;

                MembershipProvider.CreateUserAndAccount(user.Email, pass, false, dict);
                RoleProvider.AddUsersToRoles(new[] { user.Email }, new[] { roleName });

                if (CurrentUser.UserRoles.Contains("ShopOwner"))
                {
                    DB.Managers.InsertOnSubmit(new Manager()
                    {
                        ManagerUserID = WebSecurity.GetUserId(user.Email),
                        ShopOwnerID = CurrentUser.ID
                    });
                    DB.SubmitChanges();
                }
                Logger.WriteEvent(Logger.EventType.UserAdding, "Создание пользователя, Email пользователя - " + user.Email);
            }

            if (user.ID != CurrentUser.ID)
            {
                user.RedirectURL = Url.Action("Index");

                user.SavePermissions(DB);

                user.SaveShops(DB, collection);
            }
            else
            {
                ViewBag.Message = "Данные успешно сохранены";
            }

            return PartialView(user);
        }

        public ActionResult Permissions(int TargetUserID)
        {
            return PartialView(new PermissionModel(TargetUserID));
        }

        public ActionResult Lock(int id)
        {
            var user = DB.Users.FirstOrDefault(x => x.ID == id);
            if (user != null)
            {
                var nl = !(user.IsLocked ?? false);
                user.IsLocked = nl;
                DB.SubmitChanges();
                Logger.WriteEvent(nl ? Logger.EventType.UserLock : Logger.EventType.UserUnLock,
                     (nl ? "Блокирование пользователя" : "Разблокирование пользователя") + "Email пользователя - " + user.Email);
            }

            return RedirectToAction("Index", "Users");
        }

        public ActionResult Enter(int id)
        {
            var user = DB.Users.FirstOrDefault(x => x.ID == id);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Name, false);
                Logger.WriteEvent(Logger.EventType.UserAuthRelated, "Вход под другим логином, Email пользователя - " + user.Email);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Edit", "Users", new { ID = id });
        }

        public ActionResult Delete(int ID)
        {
            var user = DB.Users.FirstOrDefault(x => x.ID == ID);
            if (user != null)
            {
                user.IsDeleted = true;
                DB.SubmitChanges();
                Logger.WriteEvent(Logger.EventType.UserDelete, "Удаление пользователя, Email пользователя - " + user.Email);
                return RedirectToAction("Index", "Users");
            }
            return RedirectToAction("Index", "Users");
        }
    }


}
