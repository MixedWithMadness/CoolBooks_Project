using CoolBookLatest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoolBookLatest.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        CoolBooksEntities db = new CoolBooksEntities();
        // GET: Roles
        public ActionResult Index()
        {

            return View();
        }
        
        public async Task<ActionResult> Create()
        {
            //RoleModel model = new RoleModel();

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(RoleModel model)
        {
            //if (ModelState.IsValid)

            //{
            AspNetRoles role = new AspNetRoles();
                role.Name = model.Name;
           // role.Id = db.AspNetRoles.ToList().Count();
                db.AspNetRoles.Add(role);
                db.SaveChangesAsync();
                return View("Index");
            //}
            //else
            //{
            //    return View();
            //}
        }
    }
}