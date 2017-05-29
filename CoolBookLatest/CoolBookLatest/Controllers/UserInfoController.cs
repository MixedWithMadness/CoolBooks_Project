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

            CoolBookLatest.Users  user =  db.Users.Where(u => u.UserId==loggedInUser).FirstOrDefault();

            return View(user);
        }
    }
}