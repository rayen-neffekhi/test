using StudentManager.Models;

namespace StudentManager.Interfaces;

public interface IAbsenceRepository
{
    List<Absence> GetByStudentId(int studentId);
    Absence? GetById(int id);
    void Add(Absence absence);
    bool Delete(int id);
    void DeleteByStudentId(int studentId);
    int GetNextId();
}