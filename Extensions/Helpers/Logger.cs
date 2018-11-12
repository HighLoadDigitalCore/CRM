using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Trading.Models;
using WebMatrix.WebData;

namespace Trading.Extensions.Helpers
{
    public class Logger
    {
       

        public static void WriteLogin(User user)
        {
            var db = new DataContext();
            db.LoginHistories.InsertOnSubmit(new LoginHistory()
            {
                LoginDate = DateTime.Now,
                UserID = user.ID,
                IPAddress = HttpContext.Current.Request.GetRequestIP().ToIPInt() ?? 0
            });
            db.SubmitChanges();
        }


        public enum EventType
        {
            UserAdding = 1,
            UserEdit = 2,
            UserLock = 3,
            UserUnLock = 9,
            UserAuthRelated = 10,
            UserAuth = 11,
            Debugging = 4, 
            ShopAdd = 5,
            ShopEdit = 6, 
            ShopCharEdit = 7,
            ShopCharCommonEdit = 8,
            ShopCharDelete = 12,
            ShopCharCommonDelete = 13,
            ShopCharAdd = 14,
            ShopCharCommonAdd = 15,
            ShopDelete = 16,
            OrderCreate = 17,
            UserRegister = 18,
            UserChangePass = 19,
            UserLogout = 20,
            UserDelete = 21
        }

        public static void WriteEvent(EventType type, string description, int? userId = null)
        {
            var db = new DataContext();
            db.LogEntries.InsertOnSubmit(new LogEntry()
            {
                ActionDate = DateTime.Now,
                ActionID = (int) type,
                Description = description,
                UserID = userId.HasValue ? userId.Value : WebSecurity.CurrentUserId
            });
            db.SubmitChanges();
        }
    }
}