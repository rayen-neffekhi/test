using StudentManager.Models;

namespace StudentManager.Interfaces;

public interface IStudentRepository
{
    List<Student> GetAll();
    Student? GetById(int id);
    void Add(Student student);
    void Update(Student student);
    bool Delete(int id);
    int GetNextId();
}