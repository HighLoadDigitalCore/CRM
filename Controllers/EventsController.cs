using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trading.Extensions;
using Trading.Extensions.Helpers;
using Trading.Models;

namespace Trading.Controllers
{
    [Authorize]
    public class EventsController : BaseController
    {
        
        [HttpGet]
        public ActionResult Actions()
        {

            var filter = new EventsFilter();
            return View(filter);
        }

        
        public ActionResult ActionSearch(EventsFilter filter)
        {
            filter.Search(DB);
            return PartialView(filter);
        }    
        
        
        [HttpGet]
        public ActionResult Logins()
        {

            var filter = new LoginsFilter();
            return View(filter);
        }

        public ActionResult LoginsSearch(LoginsFilter filter)
        {
            filter.Search(DB);
            return PartialView(filter);
        }
    }
}
