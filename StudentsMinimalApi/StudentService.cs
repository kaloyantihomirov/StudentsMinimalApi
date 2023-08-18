using System.Collections.Concurrent;

namespace StudentsMinimalApi
{
    public class StudentService
    {
        private static readonly ConcurrentDictionary<string, Student> _allStudents
            = new ConcurrentDictionary<string, Student>();

        public static ConcurrentDictionary<string, Student> AllStudents { get { return _allStudents; } }

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
            return _allStudents.TryRemove(id, out _);
        }

        public void UpdateStudent(string id, Student newStudent)
        {
            _allStudents[id] = newStudent;
        }
    }
}
