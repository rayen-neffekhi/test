using StudentManager.Interfaces;
using StudentManager.Models;

namespace StudentManager.Services;

public class AbsenceService : IAbsenceService
{
    private readonly IAbsenceRepository _absenceRepository;
    private readonly IStudentRepository _studentRepository;

    public AbsenceService(IAbsenceRepository absenceRepository, IStudentRepository studentRepository)
    {
        _absenceRepository = absenceRepository;
        _studentRepository = studentRepository;
    }

    public List<Absence> GetAbsencesByStudent(int studentId)
    {
        return _absenceRepository.GetByStudentId(studentId);
    }

    public bool AddAbsence(Absence absence, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (_studentRepository.GetById(absence.StudentId) is null)
        {
            errorMessage = "Étudiant introuvable.";
            return false;
        }

        if (absence.Date > DateTime.Today)
        {
            errorMessage = "La date de l'absence ne peut pas être future.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(absence.Reason))
        {
            errorMessage = "Le motif est requis.";
            return false;
        }

        _absenceRepository.Add(absence);
        return true;
    }

    public bool DeleteAbsence(int id, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (!_absenceRepository.Delete(id))
        {
            errorMessage = "Absence introuvable.";
            return false;
        }

        return true;
    }

    public void DeleteAbsencesByStudentId(int studentId)
    {
        _absenceRepository.DeleteByStudentId(studentId);
    }
}
