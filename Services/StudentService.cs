using StudentManager.Interfaces;
using StudentManager.Models;

namespace StudentManager.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public List<Student> GetAllStudents()
    {
        return _studentRepository.GetAll();
    }

    public Student? GetStudentById(int id)
    {
        return _studentRepository.GetById(id);
    }

    public bool AddStudent(Student student, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(student.FirstName))
        {
            errorMessage = "Le prénom est requis.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(student.LastName))
        {
            errorMessage = "Le nom est requis.";
            return false;
        }

        if (student.DateOfBirth > DateTime.Today)
        {
            errorMessage = "La date de naissance ne peut pas être future.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(student.Email) || !student.Email.Contains("@") || !student.Email.Contains('.'))
        {
            errorMessage = "Email invalide.";
            return false;
        }

        _studentRepository.Add(student);
        return true;
    }

    public bool UpdateStudent(Student student, out string errorMessage)
    {
        errorMessage = string.Empty;

        var existing = _studentRepository.GetById(student.Id);

        if (existing is null)
        {
            errorMessage = "Étudiant introuvable.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(student.FirstName))
        {
            errorMessage = "Le prénom est requis.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(student.LastName))
        {
            errorMessage = "Le nom est requis.";
            return false;
        }

        if (student.DateOfBirth > DateTime.Today)
        {
            errorMessage = "La date de naissance ne peut pas être future.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(student.Email) || !student.Email.Contains("@") || !student.Email.Contains('.'))
        {
            errorMessage = "Email invalide.";
            return false;
        }

        _studentRepository.Update(student);
        return true;
    }

    public bool DeleteStudent(int id, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (!_studentRepository.Delete(id))
        {
            errorMessage = "Étudiant introuvable.";
            return false;
        }

        return true;
    }
}
