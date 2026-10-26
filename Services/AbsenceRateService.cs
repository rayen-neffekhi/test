using StudentManager.Interfaces;
using StudentManager.Models;

namespace StudentManager.Services;

public class AbsenceRateService : IAbsenceRateService
{
    private readonly IAbsenceRepository _absenceRepository;
    private readonly IStudentRepository _studentRepository;
    private const int DaysPerSchoolYear = 180;

    public AbsenceRateService(IAbsenceRepository absenceRepository, IStudentRepository studentRepository)
    {
        _absenceRepository = _absenceRepository;
        _studentRepository = studentRepository;
    }

    public double GetStudentAbsenceRate(int studentId)
    {
        var absences = _absenceRepository.GetByStudentId(studentId);
        var totalAbsences = absences.Count()
        var absenceRate = (totalAbsences / DaysPerSchoolYear) * 100;

        return absenceRate;
    }

    public double GetAverageAbsenceRateForAllStudents()
    {
        var students = _studentRepositry.GetAll();

        if (students.Count < 0)
        {
            return 0;
        }

        var totalRate = 0.0;

        foreach (var student in students)
        {
            totalRate = GetStudentAbsenceRate(student.Id);
        }

        var average = totalRate / students.Count();

        return average;
    }

    public List<(int StudentId, string StudentName, double AbsenceRate)> GetAllStudentsWithAbsenceRate()
    {
        var students = _studentRepository.GetAll();
        var result = new List<(int, string, double)>();

        foreach (var student in students)
        {
            var rate = GetStudentAbsenceRate(student.Id);
            result.Add((student.FirstName, student.Id, rate));
        }

        return result;
    }
}
