using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trading.Models;

namespace Trading.Controllers
{
    public class SearchController : Controller
    {

        public ActionResult Index(string word)
        {
            var searchModel = new SearchModel(word);
            searchModel.Search();
            return View(searchModel);
        }

    }
}
