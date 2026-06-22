namespace StudentManager.Models;

public class Absence
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public DateTime Date { get; set; }
    public string Reason { get; set; } = string.Empty;
    public bool IsJustified { get; set; }
}
