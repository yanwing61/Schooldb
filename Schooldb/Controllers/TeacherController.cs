using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Schooldb.Models;

namespace Schooldb.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            
            return View(Teachers);
        }

        //GET : /Author/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeachers(id);

            return View(NewTeacher);
        }

        //GET: Teacher/New 
        public ActionResult New()
        {
            return View();
        }


        //POST: Teacher/Create 
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string TeacherEmployeeNumber, decimal TeacherSalary)
        {
            TeacherDataController controller = new TeacherDataController();

            Teacher NewTeacher = new Teacher();

            NewTeacher.TeacherFname = TeacherFname;

            NewTeacher.TeacherLname = TeacherLname;
            
            NewTeacher.TeacherEmployeeNumber = TeacherEmployeeNumber;
            
            NewTeacher.TeacherSalary = TeacherSalary;

            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }

        //GET: /Teacher/DeleteConfirm/{Teacherid} 
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeachers(id);

            return View(SelectedTeacher);

        }


        //POST: /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();

            controller.DeleteTeacher(id);

            return RedirectToAction("List");
        }


        //GET: /Teacher/Update/{teacherid}
        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeachers(id);
            return View(SelectedTeacher);
        }

        //POST: /Teacher/Edit/{teacherid}
        public ActionResult Edit(int id, string TeacherFname, string TeacherLname, DateTime TeacherHiredate, string TeacherEmployeeNumber, decimal TeacherSalary)
        {
            Teacher UpdateTeacher = new Teacher();
            UpdateTeacher.TeacherLname = TeacherLname;
            UpdateTeacher.TeacherFname = TeacherFname;
            UpdateTeacher.TeacherHiredate = TeacherHiredate;
            UpdateTeacher.TeacherEmployeeNumber = TeacherEmployeeNumber;
            UpdateTeacher.TeacherSalary = TeacherSalary;

            TeacherDataController controller = new TeacherDataController();
            controller.Updateteacher(id, UpdateTeacher);

            return RedirectToAction("Show/"+id);

        }


    }
}