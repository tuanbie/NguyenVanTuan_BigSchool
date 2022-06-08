﻿using Microsoft.AspNet.Identity;
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
        BigSchoolContext context = new BigSchoolContext();
        public ActionResult Create()
        {   
           
            Course objCourse = new Course();
            objCourse.ListCategory = context.Categories.ToList();

            return View(objCourse);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            
            // Không xét valid LectureId vì bằng user đăng nhập
            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = context.Categories.ToList();
                return View("Create", objCourse);
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;   

            context.Courses.Add(objCourse);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Attending()
        {

            
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendancces = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance temp in listAttendancces)
            {
                Course objCourse = temp.Course;
                objCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }

        public ActionResult Mine()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());


            var courses = context.Courses.Where(c => c.LecturerId == currentUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach (Course i in courses)
            {
                i.LectureName = currentUser.Name;
            }
            return View(courses);
        }

        public void setViewBag(int? selectedId = null)
        {
            var model = new Course();
            ViewBag.CategoryId = new SelectList(model.ListAll(), "Id", "Name", selectedId);
        }

        public ActionResult EditMine(int Id)
        {

            var model = context.Courses.Find(Id);
            setViewBag(model.CategoryId);
            return View(model);
        }
     
        [Authorize]
        [HttpPost]
        public ActionResult EditMine(Course model)
        {

            setViewBag(model.CategoryId);
            var updateCourses = context.Courses.Find(model.Id);
            updateCourses.DateTime = model.DateTime;
            updateCourses.LectureName = model.LectureName;
            updateCourses.CategoryId = model.CategoryId;
            var id = context.SaveChanges();
            if (id > 0)
                return RedirectToAction("Mine");
            else
            {
                ModelState.AddModelError("", "Can't save to database");
                return View(model);
            }
        }




        public ActionResult DeleteMine(int Id)
        {
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.Courses.Find(Id);
            context.Courses.Remove(courses);
            context.SaveChanges();
            return RedirectToAction("Mine");
            //return BadRequest("the attendance already exists");
        }


        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser =
            System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            //danh sách giảng viên được theo dõi bởi người dùng (đăng nhập) hiện tại
            var listFollwee = context.Followings.Where(p => p.FollowerId == currentUser.Id).ToList();

            //danh sách các khóa học mà người dùng đã đăng ký
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();

            var courses = new List<Course>();
            foreach (var course in listAttendances)
            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeId == course.Course.LecturerId)
                    {
                        Course objCourse = course.Course;
                        objCourse.LectureName =
                        System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                        .FindById(objCourse.LecturerId).Name;
                        courses.Add(objCourse);
                    }
                }
            }
            return View(courses);
        }
    }
}