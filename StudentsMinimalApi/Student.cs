namespace StudentsMinimalApi
{
    public class Student
    {
        public Student(
            string? firstName,
            string? lastName,
            string? favouriteSubject)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FavouriteSubject = favouriteSubject;
        }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? FavouriteSubject { get; set; }
    }
}
