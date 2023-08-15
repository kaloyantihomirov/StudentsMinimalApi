namespace StudentsMinimalApi
{
    public class Student
    {
        public Student(
            string? firstName,
            string? lastName,
            string? favouriteSubject)
        {
            FirstName = firstName;
            LastName = lastName;
            FavouriteSubject = favouriteSubject;
        }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? FavouriteSubject { get; set; }
    }
}
