using StudentManager.Interfaces;
using StudentManager.Models;

namespace StudentManager.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly List<Student> _students = new();
    private int _nextId = 1;

    public List<Student> GetAll()
    {
        return _students.ToList();
    }

    public Student? GetById(int id)
    {
        return _students.FirstOrDefault(student => student.Id == id);
    }

    public void Add(Student student)
    {
        student.Id = GetNextId();
        _students.Add(student);
    }

    public void Update(Student student)
    {
        var existing = GetById(student.Id);

        if (existing is null)
        {
            return;
        }

        existing.FirstName = student.FirstName;
        existing.LastName = student.LastName;
        existing.DateOfBirth = student.DateOfBirth;
        existing.Email = student.Email;
    }

    public bool Delete(int id)
    {
        var student = GetById(id);

        if (student is null)
        {
            return false;
        }

        return _students.Remove(student);
    }

    public int GetNextId()
    {
        return _nextId++;
    }
}
