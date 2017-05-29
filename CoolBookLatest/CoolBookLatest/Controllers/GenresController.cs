using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CoolBookLatest.Models;
using System.Data.Entity;

namespace CoolBookLatest.Controllers
{
    [Authorize]
    public class GenresController : Controller
    {
        CoolBooksEntities db = new CoolBooksEntities();
        // GET: Generes
        public ActionResult Index()
        {
            var list = db.Genres.Where(g => g.IsDeleted == false).ToList();


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


        public async Task<ActionResult> Details(int?Id)
        {
            if(Id!=null)
            {
                var gen = db.Genres.Find(Id);
                if(gen!=null)
                {
                    return View(gen);
                }
                else
                {
                    return RedirectToAction("Index", "Genres");

                }
            }
            else
            {
                return RedirectToAction("Index", "Genres");
            }
            
        }

        public async Task<ActionResult> Edit(int? Id)
        {
            if(Id!=null)
            {
                var gen = db.Genres.Find(Id);
                if(gen!=null)
                {
                    return View(gen);
                }
                else
                {
                    return RedirectToAction("Index", "Genres");
                }
            }
            else
                {
                    return RedirectToAction("Index", "Genres");

                }
            
        }
        [HttpPost]
        public async Task<ActionResult> Edit (Genres gotten)
        {
            if(ModelState.IsValid)
            {
                db.Entry(gotten).State = EntityState.Modified;
                db.SaveChangesAsync();

                return RedirectToAction("Index", "Genres");

            }
            else
            {
                return View("Edit", gotten.Id);
            }

        }


        public async Task<ActionResult> Delete (int? Id)
        {
            if(Id!=null)
            {
                var gen = db.Genres.Find(Id);
                return View(gen);
            }
            else
            {
                return RedirectToAction("Index", "Genres");

            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int Id)
        {
            if(Id!=null)
            {
                var gen = db.Genres.Find(Id);

                if(gen!=null)
                {
                    gen.IsDeleted = true;
                    db.Entry(gen).State = EntityState.Modified;
                    db.SaveChangesAsync();
                    return RedirectToAction("Index", "Genres");


                }
                else
                {
                    return RedirectToAction("Index", "Genres");

                }
            }
            else
            { 
            return RedirectToAction("Index", "Genres");
            }
        }

    }
}