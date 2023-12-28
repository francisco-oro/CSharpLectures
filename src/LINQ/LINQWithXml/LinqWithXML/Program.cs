using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinqWithXML
{
    class Program
    {
        static void Main(string[] args)
        {

            //We simply apply our Student-Structure to XML. 
            string studentsXML =
                        @"<Students>
                            <Student>
                                <Name>Toni</Name>
                                <Age>21</Age>
                                <University>Yale</University>
                                <Semester>6</Semester>
                                <GPA>1.4</GPA>
                            </Student>
                            <Student>
                                <Name>Carla</Name>
                                <Age>17</Age>
                                <University>Yale</University>
                                <Semester>1</Semester>
                                <GPA>1.4</GPA>
                            </Student>
                            <Student>
                                <Name>Leyla</Name>
                                <Age>19</Age>
                                <University>Beijing Tech</University>
                                <Semester>3</Semester>
                                <GPA>1.4</GPA>
                            </Student>
                            <Student>
                                <Name>Frank</Name>
                                <Age>25</Age>
                                <University>Beijing Tech</University>
                                <Semester>10</Semester>
                                <GPA>1.4</GPA>
                            </Student>                           
                            <Student>
                                <Name>Francisco</Name>
                                <Age>20</Age>
                                <University>UNAM</University>
                                <Semester>4</Semester>
                                <GPA>3.8</GPA>
                            </Student>
                        </Students>";

            XDocument studentsXdoc = new XDocument();
            studentsXdoc = XDocument.Parse(studentsXML);

            var students = from student in studentsXdoc.Descendants("Student")
                           select new
                           {
                               Name = student.Element("Name").Value,
                               Age = student.Element("Age").Value,
                               University = student.Element("University").Value,
                               Semester = student.Element("Semester").Value,
                               GPA = student.Element("GPA").Value
                           };

            foreach (var student in students)
            {
                Console.WriteLine("Student {0} with age {1} from University {2} is in his/her {3} Semester with GPA: {4}", student.Name, student.Age, student.University, student.Semester, student.GPA);
            }

            var sortedStudents = from student in students
                                 orderby student.Age
                                 select new
                                 {
                                     Name = student.Name,
                                     Age = student.Age,
                                     University = student.University,
                                     Semester = student.Semester, 
                                     GPA = student.GPA
                                 };
            foreach (var student in sortedStudents) 
            {
                Console.WriteLine("Student {0} with age {1} from University {2} is in his/her {3} Semester with GPA: {4}", student.Name, student.Age, student.University, student.Semester, student.GPA);
            }
            Console.ReadKey();  
        }
    }
}
