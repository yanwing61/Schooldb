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
        public ActionResult Create(string TeacherFname, string TeacherLname)
        {
            TeacherDataController controller = new TeacherDataController();

            Teacher NewTeacher = new Teacher();

            NewTeacher.TeacherFname = TeacherFname;

            NewTeacher.TeacherLname = TeacherLname;
            
            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }

    }
}