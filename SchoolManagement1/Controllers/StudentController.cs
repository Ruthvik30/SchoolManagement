using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement1.Data.Enum;
using SchoolManagement1.Data.Interfaces;
using SchoolManagement1.Models;
using SchoolManagement1.ViewModels;
using System.Linq;

namespace SchoolManagement1.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Index()
        {
            var models = await studentRepository.GetAllStudents();
            return View(models);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> StudentDetails(int studentId)
        {
            var student = await studentRepository.GetStudentDetailsById(studentId);
            if (student == null)
            {
                ViewBag.ErrorMessage = $"Student with id {studentId} not found!";
                return View("NotFound");
            }
            return View(student);
        }

        [Authorize(Roles = "Admin,Faculty")]
        [HttpGet]
        public IActionResult StudentRegistration()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost]
        public async Task<IActionResult> StudentRegistration(RegisterStudent model)
        {
            if (ModelState.IsValid)
            {
                var classId = await studentRepository.GetClassId(model.Standard.ToString());
                var student = new StudentDetail()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FatherName = model.FatherName,
                    DateOfBirth = model.DateOfBirth,
                    Phone = model.Phone,
                    Gender = model.Gender.ToString(),
                    Address = model.Address,
                    ClassId = classId.ClassId,
                };
                studentRepository.AddStudent(student);
                return RedirectToAction("Index", "Student");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> EditTransportationInStudent(int studentId)
        {
            ViewBag.StudentId = studentId;
            var student = await studentRepository.GetStudentDetailsById(studentId);
            if (student is null)
            {
                ViewBag.ErrorMessage = $"Student with id {studentId} not found!";
                return View("NotFound");
            }
            else
            {
                var model = new List<TransportationStudentViewModel>();
                var transportations = await studentRepository.GetAllTransportations();
                foreach (var item in transportations)
                {
                    var transportationStudentViewModel = new TransportationStudentViewModel()
                    {
                        TransportationId = item.TransportationId,
                        RouteName = item.RouteName,
                        Amount = Convert.ToDecimal(item.Amount),
                    };
                    if (student.StudentTransportation != null && student.StudentTransportation.Transportation.TransportationId == item.TransportationId)
                    {
                        transportationStudentViewModel.IsSelected = true;
                    }
                    else
                    {
                        transportationStudentViewModel.IsSelected = false;
                    }
                    model.Add(transportationStudentViewModel);
                }
                return View(model);
            }
        }

        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost]
        public async Task<IActionResult> EditTransportationInStudent(List<TransportationStudentViewModel> models, int studentId)
        {
            var student = await studentRepository.GetStudentDetailsById(studentId);
            if (student is null)
            {
                ViewBag.ErrorMessage = $"Student with id {studentId} not found!";
                return View("NotFound");
            }
            else
            {
                if (student.StudentTransportation != null)
                {
                    studentRepository.removeStudentTransportation(student.StudentTransportation);
                }
                StudentTransportation studentTransportation = null;
                foreach (var model in models)
                {
                    if (model.IsSelected)
                    {
                        studentTransportation = new StudentTransportation();
                        studentTransportation.StudentId = student.StudentId;
                        studentTransportation.TransportationId = model.TransportationId;
                    }
                }
                if (studentTransportation != null)
                {
                    studentRepository.addStudentTransportation(studentTransportation);
                }
            }
            return RedirectToAction("StudentDetails", new { studentId = student.StudentId });
        }

        [Authorize(Roles = "Admin,Faculty")]
        [HttpGet]
        public async Task<IActionResult> EditStudent(int studentId)
        {
            var student = await studentRepository.GetStudentDetailsById(studentId);
            if (student is null)
            {
                ViewBag.ErrorMessage = $"Student with id {studentId} not found!";
                return View("NotFound");
            }
            var studentmodel = new RegisterStudent()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Address = student.Address ?? "Address",
                FatherName = student.FatherName,
                Phone = student.Phone,
                Gender = (Gender)Enum.Parse(typeof(Gender), student.Gender),
                DateOfBirth = (DateTime)student.DateOfBirth,
                Standard = (Standard)Enum.Parse(typeof(Standard), student.Class.ClassName)
            };
            ViewBag.studentId = student.StudentId;
            return View(studentmodel);
        }

        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost]
        public async Task<IActionResult> EditStudent(RegisterStudent model, int studentId)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //var student = await studentRepository.GetStudentDetailsById(studentId);
            //if (student is null)
            //{
            //    ViewBag.ErrorMessage = $"Student with id {studentId} not found!";
            //    return View("NotFound");
            //}
            var classId = await studentRepository.GetClassId(model.Standard.ToString());
            var studentDetails = new StudentDetail()
            {
                StudentId = studentId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FatherName = model.FatherName,
                DateOfBirth = model.DateOfBirth,
                Phone = model.Phone,
                Gender = model.Gender.ToString(),
                Address = model.Address,
                ClassId = classId.ClassId,
            };
            studentRepository.UpdateStudent(studentDetails);
            return RedirectToAction("StudentDetails", new { studentId = studentId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var student = await studentRepository.GetStudentDetailsById(studentId);
            if (student == null)
            {
                ViewBag.ErrorMessage = $"Student with id {studentId} not found!";
                return View("NotFound");
            }
            studentRepository.RemoveStudent(student);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles ="Faculty")]
        public async Task<IActionResult> StudentAttendance()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Faculty")]
        public async Task<IActionResult> StudentAttendance(IFormCollection formData)
        {
            int standardId;
            int.TryParse(formData["standard"], out standardId);
            var date = formData["date"];
            var attendancelist = new List<Attendance>();
            foreach (var model in formData)
            {
                if (model.Key.Contains("student_"))
                {
                    var attendance = new Attendance()
                    {
                        StudentId=Convert.ToInt32(model.Key.Substring(8)),
                        ClassId= standardId,
                        AttendanceDate=DateTime.Parse(date.ToString()),
                        IsPresent=true
                    };
                    attendancelist.Add(attendance);
                }
            }
            studentRepository.AddAttendance(attendancelist);
            ViewBag.SuccessMessage = "Attendance successfully registered";
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Faculty")]
        public async Task<IActionResult> GetStudentsByClass(int classId)
        {
            var standardName = (Standard)classId;
            var model = await studentRepository.GetStudentDetailsByClass(standardName.ToString());
            return Json(model);
            //var studentList = new List<StudentAttendanceViewModel>();
            //foreach(var student in model)
            //{
            //    var studentAttendance = new StudentAttendanceViewModel()
            //    {
            //        StudentId = student.StudentId,
            //        StudentName=student.FirstName+" "+student.LastName,
            //    };
            //    studentList.Add(studentAttendance);
            //}
            //return Json(studentList);
        }
    }
}
