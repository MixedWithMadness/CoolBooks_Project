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
            List<Books> listofbooks = db.Books.Where(m => m.IsDeleted == false).OrderByDescending(m => m.Created).ToList();
            int count = listofbooks.Count();

            Random rand = new Random();
            List<Books> mainbook = new List<Books>();

            

            if (listofbooks.Count() > 7)
            {
                mainbook.Add(listofbooks[rand.Next(0, count)]);

                for (int i = 1; i < 7; i++)
                {
                    mainbook.Add(listofbooks[i]);
                }
                //mainbook = listofbooks[rand.Next(0, count)];

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