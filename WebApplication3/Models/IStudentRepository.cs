using System.Collections;
using System.Collections.Generic;

namespace WebApplication3.Models
{
    public interface IStudentRepository
    {
        IEnumerable<Student> getAllStudents();
        Student createStudent(Student student);
    }
}