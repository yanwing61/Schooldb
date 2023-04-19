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

            //Create new variable to store the first item of the Teachers array
            Teacher NewVar = new Teacher();
            NewVar.TeacherId = Teachers[0].TeacherId;
            NewVar.TeacherFname = Teachers[0].TeacherFname;
            NewVar.TeacherLname = Teachers[0].TeacherLname;
            NewVar.TeacherEmployeeNumber = Teachers[0].TeacherEmployeeNumber;
            NewVar.TeacherHiredate = Teachers[0].TeacherHiredate;
            NewVar.TeacherSalary = Teachers[0].TeacherSalary;
            NewVar.ClassCode = Teachers[0].ClassCode;
            NewVar.ClassName = Teachers[0].ClassName;

            //check if the teacher has teach one or more courses
            if (Teachers.Count > 1)
            {
                NewVar.ClassCode = Teachers[0].ClassCode + " , " + Teachers[1].ClassCode;
                NewVar.ClassName = Teachers[0].ClassName + " , " + Teachers[1].ClassName;
            }


            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of Teacher names
            return NewVar;

        }

        /// <summary>
        /// Add a teacher to the mySQL database.
        /// </summary>
        /// <param name="NewTeacher">
        /// An object with fields that map to the columns of the teachers table. Non-Deterministic.
        /// </param>
        /// <example>
        /// POST: api/TeacherData/AddTeacher 
        /// {
        ///	"TeacherFname":"Yan Wing",
        ///	"TeacherLname":"Pang",
        ///	"TeacherEmployeeNumber": "T203",
        ///	"TeacherHiredate": 2023-04-05 00:00:00,
        ///	"TeacherSalary": 50.7;
        /// }
        /// </example>
        [HttpPost]
        [Route("api/TeacherData/AddTeacher")]
        public string AddTeacher([FromBody] Teacher NewTeacher)

        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();


            MySqlCommand Cmd = Conn.CreateCommand();

            string query = "INSERT INTO Teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) VALUES(@fname, @lname, @employeeno, CURDATE(), @salary)";

            Cmd.CommandText = query;
            Cmd.Parameters.AddWithValue("@fname", NewTeacher.TeacherFname);
            Cmd.Parameters.AddWithValue("@lname", NewTeacher.TeacherLname);
            Cmd.Parameters.AddWithValue("@employeeno", NewTeacher.TeacherEmployeeNumber);
            Cmd.Parameters.AddWithValue("@salary", NewTeacher.TeacherSalary);

            Cmd.ExecuteNonQuery();

            Conn.Close();

            return "add Teacher";

        }

        /// <summary>
        /// Deletes a teacher from the Database if the ID of that teacher exists.
        /// </summary>
        /// <param name="Teacherid">The ID of the teacher.</param>
        /// <example>
        /// POST: /api/TeacherData/DeleteTeacher/5
        /// </example>
        [HttpPost]
        [Route("api/Teacherdata/deleteTeacher/{Teacherid}")]
        public void DeleteTeacher(int Teacherid)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand Cmd = Conn.CreateCommand();

            string query = "DELETE from Teachers WHERE Teacherid=@id";

            Cmd.CommandText = query;
            Cmd.Parameters.AddWithValue("id", Teacherid);
            Cmd.ExecuteNonQuery();

            Conn.Close();


        }


        /// <summary>
        /// Update an teacher in the system.
        /// </summary>
        /// <example>
        /// POST: api/teacherdata/updateteacher/{teacherid}
        /// POST DATA/ FORM DATA / REQUEST BODY
        /// {
        /// teacher first name: 'Dana',
        /// teacher last name: 'Ford',
        /// teacher id: '8',
        /// teacher employee number: 'T401',
        /// teacher salary: '71.15';
        /// }
        /// communicate through get and post
        /// curl -H "Content-Type: application/json" -d @teacher.json http://localhost:51326/api/teacherdata/updateteacher/78
        /// </example>
        [HttpPost]
        [Route("api/teacherdata/updateteacher/{teacherid}")]
        public void Updateteacher(int teacherid, [FromBody] Teacher Updatedteacher)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand Cmd = Conn.CreateCommand();

            string query = "update teachers set teacherfname=@fname, teacherlname=@lname, employeenumber=@employeeno, hiredate=@hiredate, salary=@salary where teacherid=@id";

            Cmd.CommandText = query;

            Cmd.Parameters.AddWithValue("@fname", Updatedteacher.TeacherFname);
            Cmd.Parameters.AddWithValue("@lname", Updatedteacher.TeacherLname);
            Cmd.Parameters.AddWithValue("@employeeno", Updatedteacher.TeacherEmployeeNumber);
            Cmd.Parameters.AddWithValue("@hiredate", Updatedteacher.TeacherHiredate);
            Cmd.Parameters.AddWithValue("@salary", Updatedteacher.TeacherSalary);
            Cmd.Parameters.AddWithValue("@id", teacherid);

            Cmd.ExecuteNonQuery();

            Conn.Close();

        }



    }
}
