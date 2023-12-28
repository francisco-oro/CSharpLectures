using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LINQToSQL
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinqToSqlDataClassesDataContext dataContext;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["LINQToSQL.Properties.Settings.FranciscoDBConnectionString"].ConnectionString;
            dataContext = new LinqToSqlDataClassesDataContext(connectionString);
            /*InsertStudentLectureAssociations();*/
            /*GetUniversityOfToni();*/
            /*GetTonisLectures();*/
            /*GetAllStudentsFromYale();*/
            /*GetAllUniversitiesWithTransgenders();*/
            /*GetAllLecturesTaughtAtUNAM();*/
            /*UpdateToni();*/
            DeleteJame();
        }

        public void InsertUniversities()
        {
            dataContext.ExecuteCommand("delete from University");

            University yale = new University();
            yale.Name = "Yale";
            dataContext.University.InsertOnSubmit(yale);

            University unam = new University();
            unam.Name = "UNAM";

            dataContext.University.InsertOnSubmit(unam);

            dataContext.SubmitChanges();

        }

        public void InsertStudents()
        {
            University yale = dataContext.University.First(un => un.Name.Equals("Yale"));
            University unam = dataContext.University.First(un => un.Name.Equals("UNAM"));

            List<Student> students = new List<Student>();
            students.Add(new Student { Name = "Carla", Gender = "female", UniversityId = yale.Id });
            students.Add(new Student { Name = "Toni", Gender = "male", University = yale });
            students.Add(new Student { Name = "Leyle", Gender = "female", University = unam });
            students.Add(new Student { Name = "Jame", Gender = "trans-gender", University = unam });

            dataContext.Student.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();

        }

        public void InsertLectures()
        {
            List<Lecture> lectures = new List<Lecture>();
            lectures.Add(new Lecture { Name = "Math" });
            lectures.Add(new Lecture { Name = "History" });

            dataContext.Lecture.InsertAllOnSubmit(lectures);
            dataContext.SubmitChanges();
        }

        public void InsertStudentLectureAssociations()
        {
            Student Carla = dataContext.Student.First(st => st.Name.Equals("Carla"));
            Student Toni = dataContext.Student.First(st => st.Name.Equals("Toni"));
            Student Leyle = dataContext.Student.First(st => st.Name.Equals("Leyle"));
            Student Jame = dataContext.Student.First(st => st.Name.Equals("Jame"));

            Lecture Math = dataContext.Lecture.First(le => le.Name.Equals("Math"));
            Lecture History = dataContext.Lecture.First(le => le.Name.Equals("History"));

            dataContext.StudentLecutre.InsertOnSubmit(new StudentLecutre { Student = Carla, Lecture = Math });
            dataContext.StudentLecutre.InsertOnSubmit(new StudentLecutre { Student = Toni, Lecture = Math });

            StudentLecutre slToni = new StudentLecutre();
            slToni.Student = Toni;
            slToni.LectureId = History.Id;
            dataContext.StudentLecutre.InsertOnSubmit(slToni);

            dataContext.StudentLecutre.InsertOnSubmit(new StudentLecutre() { Student = Leyle, Lecture = History });

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.StudentLecutre;
        }

        public void GetUniversityOfToni()
        {
            Student toni = dataContext.Student.First(st => st.Name.Equals("Toni"));

            University university = toni.University;
            List<University> universities = new List<University> { university };

            MainDataGrid.ItemsSource = universities;
        }

        public void GetTonisLectures()
        {
            Student toni = dataContext.Student.First(st => st.Name.Equals("Toni"));

            var lectures = from sl in toni.StudentLecutre select sl.Lecture;
            MainDataGrid.ItemsSource = lectures;
        }

        public void GetAllStudentsFromYale()
        {
            University yale = dataContext.University.First(un => un.Name.Equals("Yale"));
            var studentsFromYale = from st in yale.Student select st;
            MainDataGrid.ItemsSource = studentsFromYale;
        }

        public void GetAllUniversitiesWithTransgenders()
        {
            var universitiesWithTransgenders = from student in dataContext.Student
                                               join university in dataContext.University
                                               on student.University equals university
                                               where student.Gender == "trans-gender"
                                               select university;
            MainDataGrid.ItemsSource= universitiesWithTransgenders;
        }

        public void GetAllLecturesTaughtAtUNAM()
        {
            var LecturesAtUNAM = from lecture in dataContext.StudentLecutre
                                 join student in dataContext.Student
                                 on lecture.Student equals student
                                 where student.University.Name == "UNAM"
                                 select lecture.Lecture;

            MainDataGrid.ItemsSource = LecturesAtUNAM;
                                 
        }

        public void UpdateToni()
        {
            Student Toni = dataContext.Student.FirstOrDefault(st => st.Name.Equals("Toni"));

            Toni.Name = "Antonio";

            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Student;
        }

        public void DeleteJame()
        {
            Student jame = dataContext.Student.FirstOrDefault(st => st.Name.Equals("Jame"));
            dataContext.Student.DeleteOnSubmit(jame);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Student;
        }
    }
}
