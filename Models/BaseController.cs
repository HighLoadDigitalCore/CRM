using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace Trading.Models
{
    public class BaseController : Controller
    {

        public int DefShopID
        {
            get { return CurrentUser.DefShopID; }
        }

        public static string HostName = string.Empty;
        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.HttpContext.Request.Url != null)
            {
                HostName = requestContext.HttpContext.Request.Url.Authority;
            }
            base.Initialize(requestContext);
        }


        private DataContext _db;
        public DataContext DB
        {
            get { return _db ?? (_db = new DataContext(ConfigurationManager.ConnectionStrings["TradingConnectionString"].ConnectionString)); }
        }

        public SimpleMembershipProvider MembershipProvider
        {
            get
            {
                return (SimpleMembershipProvider)System.Web.Security.Membership.Provider;
                
            }
        }

        public SimpleRoleProvider RoleProvider
        {
            get
            {
                return (SimpleRoleProvider)Roles.Provider;
            }
        }



        private User _currentUser;
        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = DB.Users.FirstOrDefault(x => x.ID == WebSecurity.CurrentUserId);
                }
                return _currentUser;
            }
            set { _currentUser = value; }
        }
    }
}