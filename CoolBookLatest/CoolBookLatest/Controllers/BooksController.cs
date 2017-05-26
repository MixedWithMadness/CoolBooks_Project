using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoolBookLatest;
using Microsoft.AspNet.Identity;
using CoolBookLatest.Models;

namespace CoolBookLatest.Controllers
{
    public class BooksController : Controller
    {
        private CoolBooksEntities db = new CoolBooksEntities();
        /// <summary>
        /// This will show the list of Books added by the logged in user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> MyBooks()
        {

            String userId = User.Identity.GetUserId();


            var myBooks = db.Books.Where(b => b.UserId.Equals(userId));
            return View(await myBooks.ToListAsync());
        }
        // GET: Books
        public async Task<ActionResult> Index(string sortOrder,int AuthorId = 0)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", "LastName");
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name");

            
            var books = from s in db.Books
                        select s;

            if (!Request.IsAuthenticated)
            {
                books = books.Where(b => b.IsDeleted != true);
            }

            if(AuthorId != 0)
            {
                books = books.Where(b => b.AuthorId == AuthorId);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    books = books.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    books = books.OrderBy(s => s.PublishDate);
                    break;
                case "date_desc":
                    books = books.OrderByDescending(s => s.PublishDate);
                    break;
                default:
                    books = books.OrderBy(s => s.Title);
                    break;
            }

            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.BookId = id;

            Books books = await db.Books.FindAsync(id);
            books.Reviews = await db.Reviews.Where(b => b.BookId == id).ToListAsync();
            List<AspNetUsers> Users = await db.AspNetUsers.ToListAsync(); 

            if(books.Reviews != null)
            {
                foreach(var item in books.Reviews)
                {
                    item.UserId = (from s in Users
                                  where s.Id == item.UserId
                                  orderby s
                                  select s.UserName).FirstOrDefault();
                }
            }
            

            if (books == null)
            {
                return RedirectToAction("Index", "Books");
            }
            if (!Request.IsAuthenticated && books.IsDeleted == true)
            {
                return RedirectToAction("Index", "Books");
            }

            BookViewModel vm = books;

            return View(vm);
        }

        // GET: Books/Create
        [Authorize]
        public ActionResult Create()
        {
            
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName");
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AuthorId,GenreId,Title,AlternativeTitle,Part,Description,ISBN,PublishDate,ImagePath")] BookViewModel vm)
        {
            //Books newBook = vm;
            Books newBook = new Books();
            newBook = vm.VMToBooks(newBook);

            //    AuthorId = books.AuthorId,
            //    GenreId = books.GenreId,
            //    Title = books.Title,
            //    AlternativeTitle = books.AlternativeTitle,
            //    Part = books.Part,
            //    Description = books.Description,
            //    ISBN = books.ISBN,
            //    PublishDate = books.PublishDate,
            //    ImagePath = books.ImagePath,
            //    Authors = books.Authors,
            //    Genres = books.Genres

            newBook.UserId = HttpContext.User.Identity.GetUserId();
            newBook.Created = HttpContext.Timestamp;
            newBook.IsDeleted = false;

            if (ModelState.IsValid)
            {
                db.Books.Add(newBook);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", newBook.AuthorId);
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", newBook.GenreId);
            return View(newBook);
        }

        // GET: Books/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = await db.Books.FindAsync(id);

            if (books == null)
            {
                return HttpNotFound();
            }

            BookViewModel vm = new BookViewModel();
            vm = books;


            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", books.UserId);
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", books.AuthorId);
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", books.GenreId);
            return View(vm);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AuthorId,GenreId,Title,AlternativeTitle,Part,Description,ISBN,PublishDate,ImagePath")] BookViewModel vm)
        {
            Books books = await db.Books.FindAsync(vm.Id);
            books = vm.VMToBooks(books);


            if (ModelState.IsValid)
            {
                db.Entry(books).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", books.UserId);
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", books.AuthorId);
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", books.GenreId);
            return View(books);
        }

        // GET: Books/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = await db.Books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // POST: Books/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Books books = await db.Books.FindAsync(id);
            //db.Books.Remove(books);
            //await db.SaveChangesAsync();
            //return RedirectToAction("Index");

            books.IsDeleted = true;

            if (ModelState.IsValid)
            {
                db.Entry(books).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
