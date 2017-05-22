using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoolBookLatest.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult CreateBook()
        {
            CoolBooksEntities ctx = new CoolBooksEntities();
            var model = new Books();
            return View();
        }
    }
}