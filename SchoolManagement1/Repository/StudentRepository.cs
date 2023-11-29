using Microsoft.EntityFrameworkCore;
using SchoolManagement1.Data.Interfaces;
using SchoolManagement1.Models;
using System.Reflection.Metadata.Ecma335;

namespace SchoolManagement1.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolManagementContext context;

        public StudentRepository(SchoolManagementContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<StudentDetail>> GetAllStudents()
        {
            return await context.StudentDetails.Include(c => c.Class.FeeStructure).ToListAsync();
        }

        public async Task<Class> GetClassId(string standard)
        {
            return await context.Classes.Include(f=>f.FeeStructure).FirstOrDefaultAsync(c => c.ClassName == standard);
        }

        public async Task<IEnumerable<StudentDetail>> GetStudentDetailsByClass(string _class)
        {
            return await context.StudentDetails.Where(c => c.Class.ClassName.Contains(_class)).ToListAsync();
        }

        public async Task<StudentDetail> GetStudentDetailsById(int id)
        {
            return await context.StudentDetails.Include(c => c.Class.FeeStructure).Include(t => t.StudentTransportation.Transportation).FirstOrDefaultAsync(Id => Id.StudentId == id);
        }
        public async Task<IEnumerable<Transportation>> GetAllTransportations()
        {
            return await context.Transportations.ToListAsync();
        }
        public bool AddAttendance(List<Attendance> attendanceList)
        {
            foreach(Attendance att in attendanceList)
            {
                context.Attendances.Add(att);
            }
            return SaveChanges();
        }
        public bool AddStudent(StudentDetail student)
        {
            context.StudentDetails.Add(student);
            return SaveChanges();
        }
        public bool UpdateStudent(StudentDetail student)
        {
            context.StudentDetails.Update(student);
            return SaveChanges();
        }

        public bool RemoveStudent(StudentDetail student)
        {
            if (student.StudentTransportation!=null)
            {
                context.Remove(student.StudentTransportation);
            }
            context.StudentDetails.Remove(student);
            return SaveChanges();
        }
        public bool addStudentTransportation(StudentTransportation transportation)
        {
            context.Add(transportation);
            return SaveChanges();
        }
        public bool removeStudentTransportation(StudentTransportation transportation)
        {
            context.Remove(transportation);
            return SaveChanges();
        }
        public bool SaveChanges()
        {
            var saved=context.SaveChanges();
            return saved > 0 ? true : false;
        }

        
    }
}
