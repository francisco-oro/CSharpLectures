using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UniversityManager universityManager = new UniversityManager();
            universityManager.MaleStudents();
            universityManager.FemaleStudents();
            universityManager.StudentsByUniversityId(1);
            universityManager.StudentsByUniversityId(2);
            Console.ReadLine();
        }
    }

    class UniversityManager
    {
        public List<University> universities;
        public List<Student> students;

        public UniversityManager()
        {
            universities = new List<University>();
            students = new List<Student>();

            universities.Add(new University(){ Id = 1, Name = "Yale"});
            universities.Add(new University(){ Id = 2, Name = "Beijing Tech"});
                
            students.Add(new Student() { Id = 1, Name = "Carla", Gender = "female", Age = 17, UniversityId = 1 });
            students.Add(new Student() { Id = 2, Name = "Toni", Gender = "male", Age = 21, UniversityId = 1 });
            students.Add(new Student() { Id = 2, Name = "Frank", Gender = "male", Age = 22, UniversityId = 2 });
            students.Add(new Student() { Id = 3, Name = "Leyla", Gender = "female", Age = 19, UniversityId = 2 });
            students.Add(new Student() { Id = 4, Name = "James", Gender = "trans-gender", Age = 25, UniversityId = 2 });
            students.Add(new Student() { Id = 5, Name = "Linda", Gender = "female", Age = 22, UniversityId = 2 });
        }

        public void MaleStudents()
        {
            IEnumerable<Student> maleStudents = from student in students where student.Gender == "male" select student;
            Console.WriteLine("Male - students : ");

            foreach (Student student in maleStudents)
            {
                student.Print();
            }

        }
        public void FemaleStudents()
        {
            IEnumerable<Student> femaleStudents = from student in students where student.Gender == "female" select student;
            Console.WriteLine("Female - students : ");

            foreach (Student student in femaleStudents)
            {
                student.Print();
            }

        }

        public void StudentsByUniversityId(int universityId)
        {
            IEnumerable<Student> _students = from student in students 
                                             join university in universities 
                                             on student.UniversityId equals university.Id
                                             where university.Id == universityId
                                             select student;
            Console.WriteLine("Students in univeristy with id {0}", universityId);
            foreach (Student student in _students)
            {
                student.Print();
            }
        }
    }

    class University
    {
        public int Id { get; set; } 
        public string Name { get; set; } 

        public void Print()
        {
            Console.WriteLine("Univeristy {0} with id {1}", Name, Id);
        }
    }

    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }

        /* Foreign Key*/
        public int UniversityId { get; set; }

        public void Print()
        {
            Console.WriteLine("Student {0} with id {1}, Gender {2} and Age {3} from university with the Id {4}",
                Name, Id, Gender, Age, UniversityId);
        }
    }
}
