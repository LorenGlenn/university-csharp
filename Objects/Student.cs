using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace University
{
  public class Student
  {
    private int _id;
    private string _name;

    public Student(string studentName, int id = 0)
    {
      _id = id;
      _name = studentName;
    }
    public override bool Equals(System.Object otherStudent)
    {

      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = (this.GetId() == newStudent.GetId());
        bool nameEquality = (this.GetName() == newStudent.GetName());
        return (idEquality && nameEquality);
      }
    }
      public int GetId()
      {
        return _id;
      }
      public string GetName()
      {
        return _name;
      }

    //GETALL METHOD BEGINS
    public static List<Student> GetAll()
		{
			List<Student> allStudents = new List<Student>{};

			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
			SqlDataReader rdr = cmd.ExecuteReader();

			while(rdr.Read())
			{
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);

        Student newStudent = new Student(studentName, studentId);
        allStudents.Add(newStudent);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allStudents;
    }

    public void Save()
		{
			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("INSERT INTO students (name) OUTPUT INSERTED.id VALUES (@StudentName);", conn);

			SqlParameter nameParameter = new SqlParameter("@StudentName", this.GetName());

			cmd.Parameters.Add(nameParameter);

			SqlDataReader rdr = cmd.ExecuteReader();

			while(rdr.Read())
			{
				this._id = rdr.GetInt32(0);
			}
			if (rdr != null)
			{
				rdr.Close();
			}
			if (conn != null)
			{
				conn.Close();
			}
    }

    public static Student Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);
      SqlParameter courseIdParameter = new SqlParameter("@StudentId", id.ToString());
      cmd.Parameters.Add(courseIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundStudentId = 0;
      string foundStudentName = null;
      while(rdr.Read())
      {
        foundStudentId = rdr.GetInt32(0);
        foundStudentName = rdr.GetString(1);
      }
      Student foundStudent = new Student(foundStudentName, foundStudentId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundStudent;
    }

    public void AddCourse(Course newCourse)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(studentIdParameter);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = newCourse.GetId();
      cmd.Parameters.Add(courseIdParameter);

      cmd.ExecuteNonQuery();

      if(conn!= null)
      {
        conn.Close();
      }
    }

    public List<Course> GetCourses()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT courses.* FROM courses JOIN courses_students ON (courses_students.course_id = courses.id) JOIN students ON (students.id = courses_students.student_id) WHERE student_id = @StudentId;", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(studentIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Course> allCourses = new List<Course> {};
      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        string courseNumber = rdr.GetString(2);
        Course newCourse = new Course(courseName, courseNumber, courseId);
        allCourses.Add(newCourse);
      }
      if (rdr != null)
      {
        rdr.Close();
      }

      return allCourses;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("Delete FROM students; DELETE FROM courses_students;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
