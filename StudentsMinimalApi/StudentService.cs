namespace StudentsMinimalApi
{
    public class StudentService
    {
        private static readonly IDictionary<string, Student> _allStudents
            = new Dictionary<string, Student>();

        public static IDictionary<string, Student> AllStudents { get { return _allStudents; } }

        public bool AddStudent(string id, Student student)
        {            
            return _allStudents.TryAdd(id, student);
        }

        public Student? GetStudent(string id)
        {
            if (_allStudents.TryGetValue(id, out var student))
            {
                return student;
            }

            return null;
        }

        public bool DeleteStudent(string id)
        {
            if (_allStudents.TryGetValue(id, out _))
            {
                return _allStudents.Remove(id);
            }

            return false;
        }

        public void UpdateStudent(string id, Student newStudent)
        {
            if (_allStudents.TryGetValue(id, out _))
            {
                _allStudents[id] = newStudent;
                return;
            }
            
            AddStudent(id, newStudent);
        }
    }
}
