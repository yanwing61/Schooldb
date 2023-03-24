using Schooldb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Schooldb.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Student/List
        public ActionResult List()
        {
            StudentDataController controller = new StudentDataController();
            IEnumerable<Student> Students = controller.ListStudents();

            return View(Students);
        }

        //GET : /Author/Show/{id}
        public ActionResult Show(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student NewStudent = controller.FindStudents(id);

            return View(NewStudent);
        }
    }
}