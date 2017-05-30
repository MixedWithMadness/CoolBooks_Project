using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoolBookLatest.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        // GET: Author
        CoolBooksEntities db = new CoolBooksEntities();
        public async Task<ActionResult> Index()
        {
            var authorsList =  db.Authors.Where(a => a.IsDeleted== false);  // author which is not deleted
            
            // show only authors which are not deleted
            // conflict arises with books if we really remove the author from Database
              
            return View(authorsList);
        }

        public ActionResult Create()
        {
             return View();
        }

        
        [HttpPost]
        public async Task<ActionResult> Create (Authors gotten) // ModelBinder may also treat data returned with a form as a Author
        {
            gotten.Created = DateTime.Now;
            gotten.IsDeleted = false;

            if (ModelState.IsValid)
            {
                 // Need to remove Required constraint in AuthorsTable later on
                db.Authors.Add(gotten);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Author");
            }
            else
            {
                return RedirectToAction("Create", "Author");
            }
        } 

        public async Task<ActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            else
            {
                var author = await db.Authors.FindAsync(Id);
                if(author==null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }
                else
                {
                    return View(author);
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                
            }

            else

            {
                var author = await db.Authors.FindAsync(id);
                if(author!=null)
                {
                    author.IsDeleted = true;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Author");
                }
                else
                {
                    return RedirectToAction("Index", "Author");
                }
            }
        }

        public async Task<ActionResult> Edit(int?Id)
        {
            if(Id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var author = await db.Authors.FindAsync(Id);
                if(author==null)
                {
                    return RedirectToAction("Index", "Author");
                }
                else
                {
                    return View(author);
                }
            }
           
            
            
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Authors gotten)
        {
            if(ModelState.IsValid)
            {
                db.Entry(gotten).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Author");
            }
            else
            {
                return View("Edit", gotten.Id);
            }
        }

        public async Task<ActionResult> Details(int?Id)
        {
            Authors author = db.Authors.Find(Id);

            return View(author);
        }
    }
}