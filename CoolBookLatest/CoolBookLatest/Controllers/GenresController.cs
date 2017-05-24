using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CoolBookLatest.Models;

namespace CoolBookLatest.Controllers
{
    public class GenresController : Controller
    {
        CoolBooksEntities db = new CoolBooksEntities();
        // GET: Generes
        public ActionResult Index()
        {
            var list = db.Genres.ToList();

            return View(list);
        }


        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create (GenresModel model)
        {
            Genres gen = new Genres();

            if (ModelState.IsValid)
            {
                gen.Created = model.Created;
                gen.Description = model.Description;
                gen.IsDeleted = model.IsDeleted;
                gen.Name = model.Name;

                db.Genres.Add(gen);
                db.SaveChangesAsync();
                return RedirectToAction("Index", "Genres");
            }
            else
            {
                return RedirectToAction("Create");
            }
        }
    }
}