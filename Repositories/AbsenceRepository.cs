using StudentManager.Interfaces;
using StudentManager.Models;

namespace StudentManager.Repositories;

public class AbsenceRepository : IAbsenceRepository
{
    private readonly List<Absence> _absences = new();
    private int _nextId = 1;

    public List<Absence> GetByStudentId(int studentId)
    {
        return _absences.Where(absence => absence.StudentId == studentId).ToList();
    }

    public Absence? GetById(int id)
    {
        return _absences.FirstOrDefault(absence => absence.Id == id);
    }

    public void Add(Absence absence)
    {
        absence.Id = GetNextId();
        _absences.Add(absence);
    }

    public bool Delete(int id)
    {
        var absence = GetById(id);

        if (absence is null)
        {
            return false;
        }

        return _absences.Remove(absence);
    }

    public void DeleteByStudentId(int studentId)
    {
        _absences.RemoveAll(absence => absence.StudentId == studentId);
    }

    public int GetNextId()
    {
        return _nextId++;
    }
}
