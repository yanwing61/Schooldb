using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schooldb.Models
{
    public class Teacher
    {
        public int TeacherId;
        public string TeacherFname;
        public string TeacherLname;
        public string TeacherEmployeeNumber;
        public DateTime TeacherHiredate;
        public decimal TeacherSalary;
        public string ClassCode;
        public string ClassName;
    }
}