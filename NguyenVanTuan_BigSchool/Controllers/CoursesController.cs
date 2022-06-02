using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NguyenVanTuan_BigSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenVanTuan_BigSchool.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();
            Course objCourse = new Course();
            objCourse.ListCategory = context.Category.ToList();

            return View(objCourse);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(Course objCourse)
        {
            BigSchoolContext context = new BigSchoolContext();

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;   

            context.Course.Add(objCourse);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}