using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace University
{
  public class Course
  {
    private int _id;
    private string _name;
    private string _number;

    public Course(string courseName, string courseNumber, int id = 0)
    {
      _id = id;
      _name = courseName;
      _number = courseNumber;
    }
    public override bool Equals(System.Object otherCourse)
    {

      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = (this.GetId() == newCourse.GetId());
        bool nameEquality = (this.GetName() == newCourse.GetName());
        bool numberEquality = (this.GetNumber() == newCourse.GetNumber());
        return (idEquality && nameEquality && numberEquality);
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
      public string GetNumber()
      {
        return _number;
      }

    //GETALL METHOD BEGINS
    public static List<Course> GetAll()
		{
			List<Course> allCourses = new List<Course>{};

			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
			SqlDataReader rdr = cmd.ExecuteReader();

			while(rdr.Read())
			{
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        string courseNumber = rdr.GetString(2);

        Course newCourse = new Course(courseName, courseNumber, courseId);
        allCourses.Add(newCourse);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allCourses;
    }

    public void Save()
		{
			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("INSERT INTO courses (name, number) OUTPUT INSERTED.id VALUES (@CourseName, @CourseNumber);", conn);

			SqlParameter nameParameter = new SqlParameter("@CourseName", this.GetName());
			SqlParameter numberParameter = new SqlParameter("@CourseNumber", this.GetNumber());

			cmd.Parameters.Add(nameParameter);
			cmd.Parameters.Add(numberParameter);

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
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("Delete FROM courses;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
