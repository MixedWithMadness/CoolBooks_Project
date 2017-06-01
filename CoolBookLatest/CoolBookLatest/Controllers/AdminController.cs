using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoolBookLatest.Models
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        CoolBooksEntities db = new CoolBooksEntities();
        //  [Authorize(Roles = "User")]
       
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
        [ActionName("Edit"), HttpPost]
        public async Task<ActionResult> EditConfirmed (AspNetUsers gotten)
        {
            if (ModelState.IsValid)
            {
                var user = db.AspNetUsers.Where(u => u.Id.Equals(gotten.Id)).FirstOrDefault();

                if (user != null)
                {
                    user.AccessFailedCount = gotten.AccessFailedCount;
                    user.EmailConfirmed = gotten.EmailConfirmed;

                    user.Email = gotten.Email;

                    user.LockoutEnabled = gotten.LockoutEnabled;

                    user.LockoutEndDateUtc = gotten.LockoutEndDateUtc; //problem

                    user.PhoneNumberConfirmed = gotten.PhoneNumberConfirmed;

                    user.TwoFactorEnabled = gotten.TwoFactorEnabled;

                    user.UserName = gotten.Email;

                    gotten.UserName = gotten.Email;

                    db.Entry(user).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    return RedirectToActionPermanent("Index", "Admin");
                    
                }
            
            else
            {
                return View();
            }
                
    }
            return View();
        }
       
        //public async Task<ActionResult> ChangeUserPassword()
        //{
        //    var model = new CoolBookLatest.Models.AdminChangePasswordModel();
        //    return View();
        //}
        
        //public async Task<ActionResult> ChangeUserPassword(AdminChangePasswordModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await Microsoft.AspNet.Identity.UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("ResetPasswordConfirmation", "Account");
        //        }
        //        return View();
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
}
}


