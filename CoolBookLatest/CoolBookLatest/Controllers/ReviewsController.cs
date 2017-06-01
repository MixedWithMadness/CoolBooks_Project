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
using CoolBookLatest.Models;
using Microsoft.AspNet.Identity;

namespace CoolBookLatest.Controllers
{
    public class ReviewsController : Controller
    {
        private CoolBooksEntities db = new CoolBooksEntities();

        // GET: Reviews
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var reviews = db.Reviews.Include(r => r.AspNetUsers).Include(r => r.Books).Where(m => m.IsDeleted == false);
            return View(await reviews.ToListAsync());
        }

        // GET: Reviews/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = await db.Reviews.FindAsync(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            return View(reviews);
        }

        // GET: Reviews/Create
        [Authorize]
        public ActionResult Create()
        {
            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.BookId = new SelectList(db.Books, "Id", "UserId");

            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,BookId,Title,Text,Rating")] ReviewViewModel review)
        {
            Reviews reviews = new Reviews();
            reviews = review.VMToModel(reviews);

            reviews.UserId = HttpContext.User.Identity.GetUserId();
            reviews.Created = HttpContext.Timestamp;
            reviews.IsDeleted = false;

            if (ModelState.IsValid)
            {
                db.Reviews.Add(reviews);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Books", new { id=reviews.BookId, success = true });
            }

            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", reviews.UserId);
            //ViewBag.BookId = new SelectList(db.Books, "Id", "UserId", reviews.BookId);
            string error = "";
            if(review.Text == null)
            {
                error += "Please enter a text. ";
            }
            else if(review.Text.Count() < 5)
            {
                error += "Text too short. ";
            }

            if (review.Rating == null)
            {
                error += "Please enter a Rating. ";
            }
            else
            if (review.Rating >= 10 && reviews.Rating <= 1)
            {
                error += "Enter a rating between 1-10. ";
            }

            if (review.Title == null)
            {
                error += "Please enter a Title. ";
            }
            else
            if (review.Title.Count() < 5)
            {
                error += "Title too short. ";
            }


            return RedirectToAction("Details", "Books", new { id = reviews.BookId, errorMsg = error });
        }

        // GET: Reviews/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = await db.Reviews.FindAsync(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", reviews.UserId);
            ViewBag.BookId = new SelectList(db.Books, "Id", "UserId", reviews.BookId);
            return View(reviews);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,BookId,UserId,Title,Text,Rating,Created,IsDeleted")] ReviewViewModel reviews)
        {
            Reviews review = await db.Reviews.FindAsync(reviews.Id);
            review = reviews.VMToModel(review);
            
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            //Books books = await db.Books.FindAsync(vm.Id);
            //books = vm.VMToBooks(books);


            //if (ModelState.IsValid)
            //{
            //    db.Entry(books).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}


            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", reviews.UserId);
            ViewBag.BookId = new SelectList(db.Books, "Id", "UserId", reviews.BookId);
            return View(reviews);
        }

        // GET: Reviews/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = await db.Reviews.FindAsync(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            return View(reviews);
        }

        // POST: Reviews/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //Reviews reviews = await db.Reviews.FindAsync(id);
            //db.Reviews.Remove(reviews);
            //await db.SaveChangesAsync();
            //return RedirectToAction("Index");

            Reviews review = await db.Reviews.FindAsync(id);

            review.IsDeleted = true;

            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details","Books",new { id = review.BookId });
            }
            return RedirectToAction("Details", "Books", new { id = review.BookId });
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
