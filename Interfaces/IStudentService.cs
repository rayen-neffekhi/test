using StudentManager.Models;

namespace StudentManager.Interfaces;

public interface IStudentService
{
    List<Student> GetAllStudents();
    Student? GetStudentById(int id);
    bool AddStudent(Student student, out string errorMessage);
    bool UpdateStudent(Student student, out string errorMessage);
    bool DeleteStudent(int id, out string errorMessage);
}
