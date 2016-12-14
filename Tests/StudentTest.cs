using Xunit;
using System;
using System.Collections.Generic;


namespace University
{
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_SaveToDataBase_GetAll()
    {
      List<Student> allStudents = new List<Student>{};
      List<Student> testList = new List<Student>{};
      Student newStudent = new Student("John");
      testList.Add(newStudent);

      newStudent.Save();
      allStudents = Student.GetAll();

      Assert.Equal(testList, allStudents);
    }
    [Fact]
    public void Test_GetCoursesAssociatedWithStudent()
    {
      List<Course> allCourses = new List<Course>{};
      List<Course> testCourses = new List<Course>{};

      Course newCourse = new Course("Math", "MTH 001");
      newCourse.Save();

      Student newStudent = new Student("John");
      newStudent.Save();

      newStudent.AddCourse(newCourse);
      allCourses = newStudent.GetCourses();
      testCourses.Add(newCourse);

      Assert.Equal(testCourses, allCourses);
    }

    public void Dispose()
    {
      Student.DeleteAll();
    }
  }
}
