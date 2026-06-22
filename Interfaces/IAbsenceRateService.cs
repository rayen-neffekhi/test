namespace StudentManager.Interfaces;

public interface IAbsenceRateService
{
    double GetStudentAbsenceRate(int studentId);
    double GetAverageAbsenceRateForAllStudents();
    List<(int StudentId, string StudentName, double AbsenceRate)> GetAllStudentsWithAbsenceRate();
}
