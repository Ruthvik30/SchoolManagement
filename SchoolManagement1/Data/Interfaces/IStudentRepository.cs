using SchoolManagement1.Models;

namespace SchoolManagement1.Data.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<StudentDetail>> GetAllStudents();
        Task<IEnumerable<StudentDetail>> GetStudentDetailsByClass(string _class);
        Task<StudentDetail> GetStudentDetailsById(int id);
        Task<Class> GetClassId(string standard);
        Task<IEnumerable<Transportation>> GetAllTransportations();
        bool AddStudent(StudentDetail student);
        bool UpdateStudent(StudentDetail student);
        bool RemoveStudent(StudentDetail student);
        bool SaveChanges();
        bool addStudentTransportation(StudentTransportation transportation);
        bool removeStudentTransportation(StudentTransportation transportation);
        bool AddAttendance(List<Attendance> attendanceList);

    }
}
