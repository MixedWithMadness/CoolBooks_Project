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
        [Authorize]
        public async Task<ActionResult> Create (GenresModel model)
        {
            Genres gen = new Genres();

            model.IsDeleted = false;
            model.Created = HttpContext.Timestamp;

            if (ModelState.IsValid)
            {
                gen.Created = model.Created;
                gen.Description = model.Description;
                gen.IsDeleted = model.IsDeleted;
                gen.Name = model.Name;

                db.Genres.Add(gen);
                await db.SaveChangesAsync();
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
        [Authorize]
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
        [Authorize]
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


        [Authorize]
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
        [Authorize]
        public async Task<ActionResult> Delete(int Id)
        {
            if(Id!=null)
            {
                var gen = db.Genres.Find(Id);

                if(gen!=null)
                {
                    gen.IsDeleted = true;

                    db.Entry(gen).State = EntityState.Modified;

                    List<Books> books = await db.Books.Where(m => m.GenreId == Id).ToListAsync();

                    foreach (var item in books)
                    {
                        item.IsDeleted = true;

                        if (ModelState.IsValid)
                        {
                            List<Reviews> reviews = await db.Reviews.Where(m => m.BookId == item.Id).ToListAsync();

                            foreach (var item2 in reviews)
                            {
                                item2.IsDeleted = true;
                                if (ModelState.IsValid)
                                {
                                    db.Entry(item2).State = EntityState.Modified;
                                }
                            }

                            db.Entry(item).State = EntityState.Modified;
                        }
                    }

                    await db.SaveChangesAsync();
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