using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Schooldb.Models;
using MySql.Data.MySqlClient;
using Microsoft.Ajax.Utilities;

namespace Schooldb.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Return a list of teachers in the database.
        /// </summary>
        /// <example>
        /// GET: api/TeacherData/ListTeachers/{SearchKey}
        /// </example>
        /// <returns>
        /// A list of teachers (id, first names and last names)
        /// </returns>
        [HttpGet]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * FROM teachers WHERE lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                //string TeacherEmployeeNumber = ResultSet["employeenumber"].ToString();
                //DateTime TeacherHiredate = (DateTime)ResultSet["hiredate"];
                //decimal TeacherSalary = (decimal)ResultSet["salary"];
                //string ClassCode = ResultSet["classcode"].ToString();
                //string ClassName = ResultSet["classname"].ToString();


                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                //NewTeacher.TeacherEmployeeNumber = TeacherEmployeeNumber;
                //NewTeacher.TeacherHiredate = TeacherHiredate;
                //NewTeacher.TeacherSalary = TeacherSalary;
                //NewTeacher.ClassCode = ClassCode;
                //NewTeacher.ClassName = ClassName;

                //Add the Teacher Name to the List
                Teachers.Add(NewTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of Teacher names
            return Teachers;



        }

        /// <summary>
        /// Returns an individual teacher from the database by specifying the primary key teacherid
        /// </summary>
        /// <example>
        /// GET: api/TeacherData/FindTeachers/{id}
        /// </example>
        /// <param name="id">The teacherid in the database</param>
        /// <returns>information about the teacher</returns>
        [HttpGet]
        public Teacher FindTeachers(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * FROM teachers LEFT JOIN classes ON teachers.teacherid=classes.teacherid WHERE teachers.teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };


            Teacher NewVar = new Teacher();
            NewVar.TeacherId = Teachers[0].TeacherId;
            NewVar.TeacherFname = Teachers[0].TeacherFname;
            NewVar.TeacherLname = Teachers[0].TeacherLname;
            NewVar.TeacherEmployeeNumber = Teachers[0].TeacherEmployeeNumber;
            NewVar.TeacherHiredate = Teachers[0].TeacherHiredate;
            NewVar.TeacherSalary = Teachers[0].TeacherSalary;
            NewVar.ClassCode = Teachers[0].ClassCode;
            NewVar.ClassName = Teachers[0].ClassName;

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string TeacherEmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime TeacherHiredate = (DateTime)ResultSet["hiredate"];
                decimal TeacherSalary = (decimal)ResultSet["salary"];
                string ClassCode = ResultSet["classcode"].ToString();
                string ClassName = ResultSet["classname"].ToString();

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherEmployeeNumber = TeacherEmployeeNumber;
                NewTeacher.TeacherHiredate = TeacherHiredate;
                NewTeacher.TeacherSalary = TeacherSalary;
                NewTeacher.ClassCode = ClassCode;
                NewTeacher.ClassName = ClassName;

                //Add the Teacher Name to the List
                Teachers.Add(NewTeacher);
            }

            if (Teachers.Count > 1)
            {
                NewVar.ClassName = Teachers[0].ClassName + Teachers[1].ClassName;
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of Teacher names
            return NewVar;



        }
    }
}
