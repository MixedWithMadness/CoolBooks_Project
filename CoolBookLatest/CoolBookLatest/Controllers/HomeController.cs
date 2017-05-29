using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoolBookLatest.Controllers
{
    public class HomeController : Controller
    {
        private CoolBooksEntities db = new CoolBooksEntities();

        public ActionResult Index()
        {
            List<Books> listofbooks = db.Books.ToList();
            int count = listofbooks.Count();

            Random rand = new Random();
            Books mainbook = new Books();

            if(listofbooks != null)
            {
                mainbook = listofbooks[rand.Next(0, count)];

                return View(mainbook);
            }
            else
            {
                return View();
            }
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Coolbooks - The modern review system.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}