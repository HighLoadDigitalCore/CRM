using System.Collections.Generic;
using Trading.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using DataContext = System.Data.Linq.DataContext;

namespace Trading.Extensions.Helpers
{


    public static class AccessHelper
    {

        public static Dictionary<string, object> Repository { get; set; }
        public static void AddToRepository(string key, object value)
        {
            if (Repository.ContainsKey(key))
                Repository[key] = value;
            else Repository.Add(key, value);
        }
        static AccessHelper()
        {
            Repository = new Dictionary<string, object>();
        }


        private static string _siteUrl;
        public static string SiteUrl
        {
            get
            {
                if (_siteUrl.IsNullOrEmpty())
                    try
                    {
                        _siteUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host;
                    }
                    catch
                    {
                        _siteUrl = "http://Syndicate.ru";
                    }
                return _siteUrl;
            }
        }
        public static string SiteName
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.Url.Host;
                }
                catch
                {
                    return "Trading.1gb.ru";
                }
            }
        }
    }
}

