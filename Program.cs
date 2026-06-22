using StudentManager.Interfaces;
using StudentManager.Models;
using StudentManager.Repositories;
using StudentManager.Services;

var studentRepository = new StudentRepository();
var absenceRepository = new AbsenceRepository();
var studentService = new StudentService(studentRepository);
var absenceService = new AbsenceService(absenceRepository, studentRepository);

while (true)
{
    Console.WriteLine("Student Manager");
    Console.WriteLine("1. Add student");
    Console.WriteLine("2. List students");
    Console.WriteLine("3. Update student");
    Console.WriteLine("4. Delete student");
    Console.WriteLine("5. Manage absences");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");

    var choice = Console.ReadLine();

    if (choice == "0")
    {
        break;
    }

    if (choice == "1")
    {
        AddStudent(studentService);
        continue;
    }

    if (choice == "2")
    {
        ListStudents(studentService);
        continue;
    }

    if (choice == "3")
    {
        UpdateStudent(studentService);
        continue;
    }

    if (choice == "4")
    {
        DeleteStudent(studentService, absenceService);
        continue;
    }

    if (choice == "5")
    {
        ManageAbsences(studentService, absenceService);
        continue;
    }

    Console.WriteLine("Option invalide. Choisissez une option entre 0 et 5.");
}

void AddStudent(IStudentService studentService)
{
    var firstName = ReadNonEmptyString("Prénom");
    var lastName = ReadNonEmptyString("Nom");
    var dateOfBirth = ReadDate("Date de naissance (yyyy-MM-dd)");
    var email = ReadEmail();

    var student = new Student
    {
        FirstName = firstName,
        LastName = lastName,
        DateOfBirth = dateOfBirth,
        Email = email
    };

    if (studentService.AddStudent(student, out var errorMessage))
    {
        Console.WriteLine("Étudiant ajouté avec succès.");
        return;
    }

    Console.WriteLine(errorMessage);
}

void ListStudents(IStudentService studentService)
{
    var students = studentService.GetAllStudents();

    if (!students.Any())
    {
        Console.WriteLine("Aucun étudiant trouvé.");
        return;
    }

    foreach (var student in students)
    {
        Console.WriteLine($"{student.Id}: {student.FirstName} {student.LastName} / {student.Email} / {student.DateOfBirth:yyyy-MM-dd}");
    }
}

void UpdateStudent(IStudentService studentService)
{
    var id = ReadInt("ID de l'étudiant");
    var existing = studentService.GetStudentById(id);

    if (existing is null)
    {
        Console.WriteLine("Étudiant introuvable.");
        return;
    }

    Console.Write($"Prénom ({existing.FirstName}): ");
    var firstName = Console.ReadLine();
    Console.Write($"Nom ({existing.LastName}): ");
    var lastName = Console.ReadLine();
    Console.Write($"Date de naissance ({existing.DateOfBirth:yyyy-MM-dd}): ");
    var dateOfBirthInput = Console.ReadLine();
    Console.Write($"Email ({existing.Email}): ");
    var emailInput = Console.ReadLine();

    var updatedStudent = new Student
    {
        Id = existing.Id,
        FirstName = string.IsNullOrWhiteSpace(firstName) ? existing.FirstName : firstName.Trim(),
        LastName = string.IsNullOrWhiteSpace(lastName) ? existing.LastName : lastName.Trim(),
        DateOfBirth = ParseOptionalDate(dateOfBirthInput, existing.DateOfBirth),
        Email = string.IsNullOrWhiteSpace(emailInput) ? existing.Email : emailInput.Trim()
    };

    if (studentService.UpdateStudent(updatedStudent, out var errorMessage))
    {
        Console.WriteLine("Étudiant mis à jour.");
        return;
    }

    Console.WriteLine(errorMessage);
}

void DeleteStudent(IStudentService studentService, IAbsenceService absenceService)
{
    var id = ReadInt("ID de l'étudiant à supprimer");

    if (studentService.DeleteStudent(id, out var studentError))
    {
        absenceService.DeleteAbsencesByStudentId(id);
        Console.WriteLine("Étudiant supprimé.");
        return;
    }

    Console.WriteLine(studentError);
}

void ManageAbsences(IStudentService studentService, IAbsenceService absenceService)
{
    var studentId = ReadInt("ID de l'étudiant");
    var student = studentService.GetStudentById(studentId);

    if (student is null)
    {
        Console.WriteLine("Étudiant introuvable.");
        return;
    }

    while (true)
    {
        Console.WriteLine($"Absences de {student.FirstName} {student.LastName}");
        Console.WriteLine("1. Ajouter une absence");
        Console.WriteLine("2. Lister les absences");
        Console.WriteLine("3. Supprimer une absence");
        Console.WriteLine("0. Retour");
        Console.Write("Choisissez une option : ");

        var choice = Console.ReadLine();

        if (choice == "0")
        {
            break;
        }

        if (choice == "1")
        {
            AddAbsence(studentId, absenceService);
            continue;
        }

        if (choice == "2")
        {
            ListAbsences(studentId, absenceService);
            continue;
        }

        if (choice == "3")
        {
            DeleteAbsence(absenceService);
            continue;
        }

        Console.WriteLine("Option invalide. Choisissez une option entre 0 et 3.");
    }
}

void AddAbsence(int studentId, IAbsenceService absenceService)
{
    var date = ReadDate("Date de l'absence (yyyy-MM-dd)");
    var reason = ReadNonEmptyString("Motif");
    Console.Write("Justifiée ? (o/n) : ");
    var justifiedInput = Console.ReadLine()?.Trim().ToLowerInvariant();
    var isJustified = justifiedInput == "o" || justifiedInput == "oui";

    var absence = new Absence
    {
        StudentId = studentId,
        Date = date,
        Reason = reason,
        IsJustified = isJustified
    };

    if (absenceService.AddAbsence(absence, out var errorMessage))
    {
        Console.WriteLine("Absence ajoutée.");
        return;
    }

    Console.WriteLine(errorMessage);
}

void ListAbsences(int studentId, IAbsenceService absenceService)
{
    var absences = absenceService.GetAbsencesByStudent(studentId);

    if (!absences.Any())
    {
        Console.WriteLine("Aucune absence trouvée.");
        return;
    }

    foreach (var absence in absences)
    {
        Console.WriteLine($"{absence.Id}: {absence.Date:yyyy-MM-dd} / {absence.Reason} / Justifiée: {absence.IsJustified}");
    }
}

void DeleteAbsence(IAbsenceService absenceService)
{
    var id = ReadInt("ID de l'absence à supprimer");

    if (absenceService.DeleteAbsence(id, out var errorMessage))
    {
        Console.WriteLine("Absence supprimée.");
        return;
    }

    Console.WriteLine(errorMessage);
}

string ReadNonEmptyString(string fieldName)
{
    while (true)
    {
        Console.Write($"{fieldName} : ");
        var value = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(value))
        {
            return value.Trim();
        }

        Console.WriteLine($"{fieldName} est requis.");
    }
}

DateTime ReadDate(string prompt)
{
    while (true)
    {
        Console.Write($"{prompt} : ");
        var input = Console.ReadLine();

        if (DateTime.TryParse(input, out var date) && date <= DateTime.Today)
        {
            return date.Date;
        }

        Console.WriteLine("Date invalide ou future. Utilisez le format yyyy-MM-dd.");
    }
}

DateTime ParseOptionalDate(string? input, DateTime fallback)
{
    if (string.IsNullOrWhiteSpace(input))
    {
        return fallback;
    }

    if (DateTime.TryParse(input, out var date) && date <= DateTime.Today)
    {
        return date.Date;
    }

    Console.WriteLine("Date invalide ou future. La valeur précédente est conservée.");
    return fallback;
}

string ReadEmail()
{
    while (true)
    {
        Console.Write("Email : ");
        var email = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(email) && email.Contains("@") && email.Contains('.'))
        {
            return email.Trim();
        }

        Console.WriteLine("Email invalide. Entrez une adresse email valide.");
    }
}

int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write($"{prompt} : ");
        var input = Console.ReadLine();

        if (int.TryParse(input, out var value) && value > 0)
        {
            return value;
        }

        Console.WriteLine("Valeur numérique positive requise.");
    }
}