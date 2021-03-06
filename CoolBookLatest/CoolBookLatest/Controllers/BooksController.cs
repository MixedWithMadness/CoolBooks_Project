﻿using System;
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
using System.IO;

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

            
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name");

            var authors = db.Authors.Where(m => m.IsDeleted == false);
            List<object> newList = new List<object>();
            foreach (var author in authors)
                newList.Add(new
                {
                    Id = author.Id,
                    Name = author.FirstName + ", " + author.LastName
                });
            ViewData["AuthorId"] = new SelectList(newList, "Id", "Name", AuthorId);

            //ViewBag.Authors = new SelectList(authors, "Id", "Name");

            var books = from s in db.Books
                        where s.IsDeleted == false
                        select s;

            //if (!Request.IsAuthenticated)
            //{
            //    books = books.Where(b => b.IsDeleted != true);
            //}

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
        public async Task<ActionResult> Details(int? id, string errorMsg = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Books books =  (from u in db.Books
                                where u.Id == id
                                select u).SingleOrDefault();

            books.Reviews.Clear();
            

            if (books == null)
            {
                return RedirectToAction("Index", "Books");
            }
            if (!Request.IsAuthenticated && books.IsDeleted == true)
            {
                return RedirectToAction("Index", "Books");
            }

            books.Reviews =  db.Reviews.Where(b => b.BookId == id).Where(b => b.IsDeleted == false).ToList();
            

            if(books.Reviews != null)
            {


                foreach(var item in books.Reviews)
                {
                    item.UserId = (from s in db.AspNetUsers
                                  where s.Id == item.UserId
                                  select s.UserName).FirstOrDefault();
                                  
                }
            }

            ViewBag.BookId = books.Id;
            ViewBag.errorMsg = errorMsg;

            BookViewModel vm = books;

            return View(vm);
        }

        // GET: Books/Create
        [Authorize]
        public ActionResult Create()
        {
            
            //ViewBag.AuthorId = new SelectList(db.Authors.Where(m => m.IsDeleted == false), "Id", "FirstName");
            var authors = db.Authors.Where(m => m.IsDeleted == false);
            List<object> newList = new List<object>();
            foreach (var author in authors)
                newList.Add(new
                {
                    Id = author.Id,
                    Name = author.FirstName + ", " + author.LastName
                });
            ViewData["AuthorId"] = new SelectList(newList, "Id", "Name");

            ViewBag.GenreId = new SelectList(db.Genres.Where(m => m.IsDeleted == false), "Id", "Name");

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

            if (db.Books.Where(m => m.ISBN == newBook.ISBN).Count() != 0)
            {
                ModelState.AddModelError("ISBN", "This ISBN already exists.");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Books.Add(newBook);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            

            ViewBag.AuthorId = new SelectList(db.Authors.Where(m => m.IsDeleted == false), "Id", "FirstName", newBook.AuthorId);
            ViewBag.GenreId = new SelectList(db.Genres.Where(m => m.IsDeleted == false), "Id", "Name", newBook.GenreId);
            return View(vm);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
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
            //ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", books.AuthorId);
            var authors = db.Authors.Where(m => m.IsDeleted == false);
            List<object> newList = new List<object>();
            foreach (var author in authors)
                newList.Add(new
                {
                    Id = author.Id,
                    Name = author.FirstName + ", " + author.LastName
                });
            ViewData["AuthorId"] = new SelectList(newList, "Id", "Name");
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", books.GenreId);
            return View(vm);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

                List<Reviews> reviews = await db.Reviews.Where(m => m.BookId == id).ToListAsync();

                foreach(var item in reviews)
                {
                    item.IsDeleted = true;
                    if (ModelState.IsValid)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
        public async Task<ActionResult> FileUpload(HttpPostedFileBase file, int BookId)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Content/"), pic);
                // file is uploaded
                file.SaveAs(path);

                var book = await db.Books.FindAsync(BookId);
                book.ImagePath = pic;

                if (ModelState.IsValid)
                {
                    db.Entry(book).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                else { return RedirectToAction("Edit", "Books", new { id = BookId }); }

                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

                return RedirectToAction("Details", "Books", new { id = BookId });
            }
            // after successfully uploading redirect the user
            return RedirectToAction("Edit", "Books", new { id = BookId });
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
