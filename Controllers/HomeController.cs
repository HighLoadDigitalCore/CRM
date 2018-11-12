using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trading.Extensions;
using Trading.Extensions.Helpers;
using Trading.Extensions.Mail;
using Trading.Models;
using WebMatrix.WebData;

namespace Trading.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LoginBlock()
        {
            return PartialView(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginBlock(LoginModel model)
        {
            //WebSecurity.ResetPassword(WebSecurity.GeneratePasswordResetToken("admin"), "admin");

            if (model.Login.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
            {
                model.Message = "Неверно указан логин или пароль";
                return PartialView(model);
            }

            var user =
                DB.Users.FirstOrDefault(
                    x =>
                        x.Email.ToLower() == model.Login.ToLower() || x.Name.ToLower() == model.Login.ToLower() ||
                        x.Phone.ToLower() == model.Login.ClearPhone());

            if (user == null || (user.IsLocked ?? false) || (user.IsDeleted ?? false))
            {
                model.Message = "Неверно указан логин";
            }
            else
            {

                if (WebSecurity.Login(user.Name, model.Password, model.RememberMe))
                {
                    Logger.WriteLogin(user);
                    Logger.WriteEvent(Logger.EventType.UserAuth, "Авторизация в системе", user.ID);
                    model.RedirectURL = Url.Action("Index", "Home");
                }
                else
                {
                    model.Message = "Неверно указан логин или пароль";
                }
            }
            return PartialView(model);
        }
        public ActionResult RegisterBlock()
        {
            return PartialView();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult CheckKey(string key)
        {
            try
            {
                var uid = new Guid(key);
                var user = DB.Users.FirstOrDefault(x => x.ConfirmKey == uid && x.RegStep == 1);
                if (user == null)
                {
                    return RedirectToAction("NotFound");
                }
                else
                {
                    user.RegStep = 2;
                    DB.SubmitChanges();
                    string pass = new Random(DateTime.Now.Millisecond).GeneratePassword();
                    WebSecurity.ResetPassword(WebSecurity.GeneratePasswordResetToken(user.Name), pass);
                    WebSecurity.Login(user.Name, pass);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("NotFound");
            }
        }

        [HttpGet]
        public ActionResult RegBlockPartner()
        {
            return PartialView(new RegisterModelPartnerStep1() { Type = 1 });
        }

        [HttpGet]
        public ActionResult RegBlockCustomer()
        {
            return PartialView(new RegisterModelCustomerStep1() { Type = 1 });
        }
        [HttpPost]
        public ActionResult RegBlockPartner(RegisterModelPartnerStep1 model)
        {
            model.IsPost = true;
            if (!model.Agree || model.Name.IsNullOrEmpty() || model.Surname.IsNullOrEmpty() || !model.Email.IsMailAdress() || model.Phone.IsNullOrEmpty())
                return PartialView(model);


            var rand = new Random(DateTime.Now.Millisecond);
            var pass = rand.GeneratePassword();
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


            var confirmKey = Guid.NewGuid();


            string link = Url.Action("CheckKey", "Home", new { key = confirmKey });

            var message = NotifyMail.SendNotify("Register", model.Email,
                format => string.Format(format, HostName),
                format => string.Format(format, HostName, link)
                );

            if (message.IsNullOrEmpty())
            {
                var dict = new Dictionary<string, object>();
                dict.Add("UserName", model.Name);
                dict.Add("UserSurname", model.Surname);
                dict.Add("Email", model.Email);
                dict.Add("UserPatrinomic", model.Patrinomic);
                dict.Add("Phone", model.Phone);
                dict.Add("RegStep", 1);
                dict.Add("IsPhoneConfirmed", false);
                dict.Add("DigitID", digitID);
                dict.Add("ConfirmKey", confirmKey);

                try
                {
                    MembershipProvider.CreateUserAndAccount(model.Email, pass, false, dict);
                }
                catch
                {
                    model.Email = "";
                    model.Message = "Пользователь с таким E-mail уже зарегистрирован.";
                    return PartialView(model);
                }
                RoleProvider.AddUsersToRoles(new[] { model.Email }, new[] { "ShopOwner" });
                var user = DB.Users.FirstOrDefault(x => x.Email == model.Email);
                if (user != null)
                {
                    Logger.WriteEvent(Logger.EventType.UserRegister, "Регистрация в системе", user.ID);
                }
                model.Message =
                    "На указанный Вами электронный адрес было выслано письмо<br>Пожалуйста, перейдите по ссылке из письма для продолжения регистрации в системе";
            }
            else
            {
                model.Message = message;
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult RegBlockCustomer(RegisterModelCustomerStep1 model)
        {
            model.IsPost = true;
            if (model.Nick.IsFilled())
                model.HasNick = DB.Users.Any(x => x.Nick.ToLower() == model.Nick.ToLower());

            if (model.Nick.IsFilled())
            {
                model.NickCSS = "nick-short";
            }


            if (!model.Agree || model.HasNick || model.Name.IsNullOrEmpty() || model.Surname.IsNullOrEmpty() || !model.Email.IsMailAdress() || (model.Password ?? "").Length < 6)
                return PartialView(model);


            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CheckNick(string nick)
        {
            var exist = DB.Users.FirstOrDefault(x => x.Nick.ToLower() == nick.ToLower());
            if (exist == null)
            {
                return new ContentResult() { Content = "1" };
            }
            return new ContentResult() { Content = "0" };
        }

        [Authorize]
        [HttpGet]
        public ActionResult RegBlockPartnerPassword()
        {
            return PartialView();
        }
        [Authorize]
        [HttpGet]
        public ActionResult RegBlockPartnerPasswordPartial()
        {
            var model = new SavePassModel() { DigitID = CurrentUser.DigitID };
            return PartialView(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult RegBlockPartnerPasswordPartial(SavePassModel model)
        {
            if (model.Password.IsNullOrEmpty() || model.Password.Length < 6)
            {
                model.Message = "Длина пароля должна быть не менее 6 символов";
            }
            if (model.PasswordAgain.IsNullOrEmpty() || model.Password != model.PasswordAgain)
            {
                model.Message = "Пароли не совпадают";
            }
            if (model.Message.IsFilled())
            {
                return PartialView(model);
            }
            WebSecurity.ResetPassword(WebSecurity.GeneratePasswordResetToken(CurrentUser.Name), model.Password);
            var u = DB.Users.First(x => x.ID == CurrentUser.ID);
            u.RegStep = null;
            DB.SubmitChanges();


            Logger.WriteEvent(Logger.EventType.UserChangePass, "Изменение пароля", CurrentUser.ID);

            model.RedirectURL = Url.Action("Index", "Home");
            return PartialView(model);
        }

        [Authorize]
        public ActionResult Menu()
        {
            return PartialView();
        }

        [Authorize]
        public ActionResult LogOut()
        {
            Logger.WriteEvent(Logger.EventType.UserLogout, "Выход из системы", CurrentUser.ID);
            WebSecurity.Logout();

            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult UserMenu()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult RestoreBlock()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult RestoreBlock(string login)
        {
            var user = DB.Users.FirstOrDefault(
                x =>
                    x.Email.ToLower() == login.ToLower() || x.Name.ToLower() == login.ToLower() ||
                    x.Phone == login.ToLower() || x.Nick.ToLower() == login.ToLower());

            if (user == null)
            {
                ViewBag.Error = "Такой пользователь не зарегистрирован в системе.";
            }
            else
            {


                var confirmKey = WebSecurity.GeneratePasswordResetToken(user.Name);
                string link = Url.Action("ChangePass", "Home", new { key = confirmKey });

                NotifyMail.SendNotify("ForgotPassword", user.Email,
                    format => string.Format(format, HostName),
                    format => string.Format(format, HostName, link)
                    );


                ViewBag.Message = "На вашу электронную почту отправлено письмо с инструкцией по восстановлению пароля";
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult ChangePass(string key)
        {
            var user = WebSecurity.GetUserIdFromPasswordResetToken(key);
            if (user > 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("NotFound");
            }
        }

        [HttpPost]
        public ActionResult ChangePass(string key, string pass)
        {
            if (pass.IsNullOrEmpty() || pass.Length < 6)
            {
                ViewBag.Error = "Минимальная длина пароля - 6 символов";
                return View();
            }
            var user = WebSecurity.GetUserIdFromPasswordResetToken(key);
            if (user > 0)
            {
                WebSecurity.ResetPassword(key, pass);
                var u = DB.Users.First(x => x.ID == user);
                Logger.WriteEvent(Logger.EventType.UserChangePass, "Изменение пароля", u.ID);
                WebSecurity.Login(u.Name, pass);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("NotFound");
            }
        }

        public ActionResult WelcomeBlock()
        {
            var shops = CurrentUser.ShopList;
            return PartialView(shops);
        }

        public ActionResult NotifyBar()
        {
            return PartialView(new MyContractors());
        }
    }

}
