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

			SqlParameter nameParameter = new SqlParameter();
			nameParameter.ParameterName = "@StudentName";
			nameParameter.Value = this.GetName();

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
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("Delete FROM students;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
