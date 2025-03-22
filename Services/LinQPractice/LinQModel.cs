namespace Services.LinQPractice;

public class LinQModel
{
    static List<Student> students = new()
    {
        new() { ID = 1, FirstName = "John", LastName = "Doe", Year = GradeLevel.FirstYear, Scores = new List<int> { 85, 90, 78 }, DepartmentID = 1 },
        new() { ID = 2, FirstName = "Jane", LastName = "Smith", Year = GradeLevel.FirstYear, Scores = new List<int> { 92, 88, 84 }, DepartmentID = 2 },
        new() { ID = 3, FirstName = "Alice", LastName = "Johnson", Year = GradeLevel.FirstYear, Scores = new List<int> { 79, 95, 87 }, DepartmentID = 3 },
        new() { ID = 4, FirstName = "Bob", LastName = "Brown", Year = GradeLevel.FourthYear, Scores = new List<int> { 88, 91, 93 }, DepartmentID = 1 },
        new() { ID = 5, FirstName = "Charlie", LastName = "Davis", Year = GradeLevel.ThirdYear, Scores = new List<int> { 72, 81, 77 }, DepartmentID = 2 },
        new() { ID = 6, FirstName = "Diana", LastName = "Miller", Year = GradeLevel.ThirdYear, Scores = new List<int> { 89, 90, 94 }, DepartmentID = 3 },
        new() { ID = 7, FirstName = "Ethan", LastName = "Wilson", Year = GradeLevel.SecondYear, Scores = new List<int> { 91, 85, 88 }, DepartmentID = 1 },
        new() { ID = 8, FirstName = "Fiona", LastName = "Taylor", Year = GradeLevel.SecondYear, Scores = new List<int> { 76, 84, 79 }, DepartmentID = 2 },
        new() { ID = 9, FirstName = "George", LastName = "Anderson", Year = GradeLevel.FourthYear, Scores = new List<int> { 83, 89, 90 }, DepartmentID = 3 },
        new() { ID = 10, FirstName = "Hannah", LastName = "Thomas", Year = GradeLevel.FourthYear, Scores = new List<int> { 95, 92, 98 }, DepartmentID = 1 }
    };

    static List<Teacher> teachers = new()
    {
        new() { ID = 1, First = "Michael", Last = "Scott", City = "Scranton" },
        new() { ID = 2, First = "Pam", Last = "Beesly", City = "Scranton" },
        new() { ID = 3, First = "Jim", Last = "Halpert", City = "Philadelphia" },
        new() { ID = 4, First = "Dwight", Last = "Schrute", City = "Scranton" },
        new() { ID = 5, First = "Stanley", Last = "Hudson", City = "Florida" },
        new() { ID = 6, First = "Angela", Last = "Martin", City = "Scranton" },
        new() { ID = 7, First = "Oscar", Last = "Martinez", City = "Scranton" },
        new() { ID = 8, First = "Kevin", Last = "Malone", City = "Scranton" },
        new() { ID = 9, First = "Toby", Last = "Flenderson", City = "Scranton" },
        new() { ID = 10, First = "Meredith", Last = "Palmer", City = "Scranton" }
    };

    static List<Department> departments = new()
    {
        new() { ID = 1, Name = "Mathematics", TeacherID = 1 },
        new() { ID = 2, Name = "Science", TeacherID = 2 },
        new() { ID = 3, Name = "History", TeacherID = 3 },
        new() { ID = 4, Name = "English", TeacherID = 4 },
        new() { ID = 5, Name = "Physics", TeacherID = 5 },
        new() { ID = 6, Name = "Chemistry", TeacherID = 6 },
        new() { ID = 7, Name = "Biology", TeacherID = 7 },
        new() { ID = 8, Name = "Computer Science", TeacherID = 8 },
        new() { ID = 9, Name = "Physical Education", TeacherID = 9 },
        new() { ID = 10, Name = "Art", TeacherID = 10 }
    };

    public void ExecuteLinQOperators()
    {
        var filter = students.Where(s => s.Year == GradeLevel.FourthYear).Select(s => new { s.ID, s.DepartmentID }).ToArray();
        Console.WriteLine(filter);
    }
}

public enum GradeLevel
{
    FirstYear = 1,
    SecondYear,
    ThirdYear,
    FourthYear
};

public class Student
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required int ID { get; init; }
    public required GradeLevel Year { get; init; }
    public required List<int> Scores { get; init; }
    public required int DepartmentID { get; init; }
}

public class Teacher
{
    public required string First { get; init; }
    public required string Last { get; init; }
    public required int ID { get; init; }
    public required string City { get; init; }
}

public class Department
{
    public required string Name { get; init; }
    public int ID { get; init; }
    public required int TeacherID { get; init; }
}


