using CoolBookLatest.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoolBookLatest.Controllers
{
    public class UserInfoController : Controller
    {
        // GET: UserInfo


        CoolBooksEntities db = new CoolBooksEntities();
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
       

            model.SelectedGender = (Enums.Gender)Enum.Parse(typeof(Enums.Gender), user.Gender);
            return View(model);
        }




    }



}