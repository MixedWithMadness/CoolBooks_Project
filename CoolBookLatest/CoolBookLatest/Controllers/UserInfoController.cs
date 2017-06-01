using CoolBookLatest.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoolBookLatest.Controllers
{
    public class UserInfoController : Controller
    {
        // GET: UserInfo


        CoolBooksEntities db = new CoolBooksEntities();


        [Authorize]
        public ActionResult Edit()
        {
            var loggedInUser = User.Identity.GetUserId();

            CoolBookLatest.Users  user =  db.Users.Where(u => u.UserId.Equals(loggedInUser)).FirstOrDefault();

            UserEditModel model = new UserEditModel();

            if(user.Gender=="1")
            {
                model.SelectedGender = (Enums.Gender)Enum.Parse(typeof(Enums.Gender), "Female");

            }
            else
            {
                model.SelectedGender = (Enums.Gender)Enum.Parse(typeof(Enums.Gender), "Male");
            }

            TransportUserDataTOModel(user, model);
            //string test = model.Country.
            //model.SelectedGender = (Enums.Gender)Enum.Parse(typeof(Enums.Gender), user.Gender);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit( UserEditModel gotten)
        {
            

            if(ModelState.IsValid)
            {
                
                CoolBookLatest.Users user = db.Users.Where(u => u.UserId.Equals(gotten.UserId)).FirstOrDefault();

                user.Country = gotten.Country.ToString();
                   // gotten.Country.ToString();
                user.FirstName = gotten.FirstName;
                user.LastName = gotten.LastName;
                user.Phone = gotten.Phone;
                user.ZipCode = gotten.ZipCode;
                user.Info = gotten.Info;
                user.Birthdate = gotten.Birthdate;
                //user.Picture = gotten.Picture;

                user.City = gotten.City;
               
               
                user.Address = gotten.Address;

                if(gotten.SelectedGender==(Enums.Gender)(Enum.Parse(typeof(Enums.Gender), "Female")))
                {
                    user.Gender = "1";
                }
                else
                {
                    user.Gender = "0";
                }

                CoolBookLatest.AspNetUsers aspUser = db.AspNetUsers.Where(a => a.UserName.Equals(user.Email)).FirstOrDefault();

                aspUser.PhoneNumber = gotten.Phone;
                aspUser.PhoneNumberConfirmed = false;
                db.Entry(aspUser).State = EntityState.Modified;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChangesAsync();
                return RedirectToAction("Index", "Manage");

            }
            return View();
        }

        private static void TransportUserDataTOModel(CoolBookLatest.Users user, UserEditModel model)
        {
            model.UserId = user.UserId;
            model.Address = user.Address;
            model.Birthdate = user.Birthdate;
            model.City = user.City;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Phone = user.Phone;
           
            model.ZipCode = user.ZipCode;

            model.Country = (Enums.Countries)(Enum.Parse(typeof(Enums.Countries), user.Country));
            model.Info = user.Info;
                
            
        }

    }



}