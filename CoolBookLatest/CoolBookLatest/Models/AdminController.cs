using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoolBookLatest.Models
{
    public class AdminController : Controller
    {
        // GET: Admin
        CoolBooksEntities db = new CoolBooksEntities();

        public async Task<ActionResult> Index()
        {
            var list =  db.AspNetUsers.ToList();
            //var tempList = new List<AspNetRoles>();

            for( int i=0; i<list.Count;i++)
            {
                string temp = list[i].Id;

                var user = db.Users.Where(u => u.UserId.Equals(temp)).FirstOrDefault();
                if (user != null)
                {

                    if (user.IsDeleted == true)
                    {
                        list.RemoveAt(i);

                    }
                }
            }
              
            
                
            
            return View(list);
        }

        public async Task<ActionResult> Delete(String Id)
        {
          if(Id!=null)
            {
                var user = db.AspNetUsers.Where(u => u.Id.Equals(Id.ToString())).FirstOrDefault();
                return View(user);
            }

            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }
        
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed (string Id)
        {
           if(Id!=null)
            {
                var user = db.Users.Where(u => u.UserId.Equals(Id.ToString())).FirstOrDefault();
                if(user!=null)
                {
                    user.IsDeleted = true;
                    db.Entry(user).State = EntityState.Modified;

                    db.SaveChangesAsync();
                    return RedirectToActionPermanent("Index", "Admin");
                }
                
                return RedirectToActionPermanent("Index", "Admin");
            }
           else
            {
                return View();
            }

        }

        public async Task<ActionResult> Create()
        {
            return RedirectToAction("Register", "Account");
        }
        public async Task<ActionResult> Edit(string Id)
        {
            if(Id!=null)
            {
                var user = db.AspNetUsers.Where(u => u.Id.Equals(Id)).FirstOrDefault();
                if(user!=null)
                {
                    return View(user);
                }

            }

            return View();

        }
    }
}