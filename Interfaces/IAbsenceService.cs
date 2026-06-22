using StudentManager.Models;

namespace StudentManager.Interfaces;

public interface IAbsenceService
{
    List<Absence> GetAbsencesByStudent(int studentId);
    bool AddAbsence(Absence absence, out string errorMessage);
    bool DeleteAbsence(int id, out string errorMessage);
    void DeleteAbsencesByStudentId(int studentId);
}
