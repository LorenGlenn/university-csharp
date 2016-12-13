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
    public void Dispose()
    {
      Course.DeleteAll();
    }
  }
}
