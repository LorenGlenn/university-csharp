using Xunit;
using System;
using System.Collections.Generic;


namespace University
{
  public class CourseTest : IDisposable
  {
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_SaveToDataBase_GetAll()
    {
      List<Course> allCourses = new List<Course>{};
      List<Course> testList = new List<Course>{};
      Course newCourse = new Course("Math", "MTH 001");
      testList.Add(newCourse);

      newCourse.Save();
      allCourses = Course.GetAll();

      Assert.Equal(testList, allCourses);
    }
    [Fact]
    public void Test_GetStudentsAssociatedWithCourse()
    {
      List<Student> allStudents = new List<Student>{};
      List<Student> testStudents = new List<Student>{};

      Course newCourse = new Course("Math", "MTH 001");
      newCourse.Save();

      Student newStudent = new Student("John");
      newStudent.Save();

      newCourse.AddStudent(newStudent);
      allStudents = newCourse.GetStudents();
      testStudents.Add(newStudent);

      Assert.Equal(testStudents, allStudents);
    }
    public void Dispose()
    {
      Course.DeleteAll();
    }
  }
}
