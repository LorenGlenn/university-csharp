using Nancy;
using System;
using System.Collections.Generic;

namespace University
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ =>
      {
        return View["index.cshtml"];
      };
      Get["/add-new-student"] = _ =>
      {
        return View["add-new-student.cshtml"];
      };
      Post["/added-student"] = _ =>
      {
        string studentName = Request.Form["student-name"];

        Student newStudent = new Student(studentName);
        newStudent.Save();
        return View["added-student.cshtml", studentName];
      };
      Get["/add-new-course"] = _ =>
      {
        return View["add-new-course.cshtml"];
      };
      Post["/added-course"] = _ =>
      {
        string courseName = Request.Form["course-name"];
        string courseNumber = Request.Form["course-number"];
        Course newCourse = new Course(courseName, courseNumber);
        newCourse.Save();
        return View["added-course.cshtml", newCourse];
      };
      Get["/view-all-courses"] = _ =>
      {
        List<Course> allCourses = new List<Course>{};
        allCourses = Course.GetAll();
        return View["view-all-courses.cshtml", allCourses];
      };
      Get["/view-all-students"] = _ =>
      {
        List<Student> allStudents = new List<Student>{};
        allStudents = Student.GetAll();
        return View["view-all-students.cshtml", allStudents];
      };
      Get["/course/{id}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Course selectedCourse = Course.Find(parameters.id);
        List<Student> CourseStudents = selectedCourse.GetStudents();
        List<Student> allStudents = Student.GetAll();
        model.Add("course", selectedCourse);
        model.Add("CourseStudents", CourseStudents);
        model.Add("allStudents", allStudents);
        return View["course.cshtml", model];
      };
      Get["/student/{id}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Student selectedStudent = Student.Find(parameters.id);
        List<Course> CourseStudents = selectedStudent.GetCourses();
        List<Course> allCourses = Course.GetAll();
        model.Add("student", selectedStudent);
        model.Add("CourseStudents", CourseStudents);
        model.Add("allCourses", allCourses);
        return View["student.cshtml", model];
      };
      Post["/student/add_course"] = _ =>
      {
        Student student = Student.Find(Request.Form["student-id"]);
        Course course = Course.Find(Request.Form["course-id"]);
        student.AddCourse(course);
        List<Student> allStudents = new List<Student>{};
        allStudents = Student.GetAll();
        return View["view-all-students.cshtml", allStudents];
      };
      Post["/course/add_student"] = _ =>
      {
        Course course = Course.Find(Request.Form["course-id"]);
        Student student = Student.Find(Request.Form["student-id"]);
        course.AddStudent(student);
        List<Course> allCourses = new List<Course>{};
        allCourses = Course.GetAll();
        return View["view-all-courses.cshtml", allCourses];
      };
    }
  }
}
