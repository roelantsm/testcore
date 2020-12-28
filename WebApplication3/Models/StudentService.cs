using System.Collections.Generic;
using System.Linq;
using WebApplication3.Models.DB;

namespace WebApplication3.Models
{
    public class StudentService :IStudentRepository
    {
        private readonly StudentContext _context;


        public StudentService(StudentContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Student> getAllStudents()
        {
           return _context.Students.ToList();
        }

        public Student createStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }
    }
}