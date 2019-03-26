using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionsExample
{
    class Program
    {
        public class Student
        {
            public string FirstName { get; }
            public string LastName { get; }
            public int ClassId { get; }

            public Student(string firstName, string lastName, int classId)
            {
                FirstName = firstName;
                LastName = lastName;
                ClassId = classId;
            }
        }

        public class StudentClass
        {
            public string Teacher { get; set; }
            public int ClassId { get; set; }
        }

        static void Main(string[] args)
        {
            var namesOfStudentsWithBlackHair = new List<string> { "athan Monzales", "Austin", "Marty mcfly" };

            var classes = new List<StudentClass> { new StudentClass { ClassId = 1, Teacher = "Steve" } };

            var studentsWithBlackHair = namesOfStudentsWithBlackHair
                // Where is for filtering data in collections like:
                // .Where(name => name.StartsWith("P"))
                // Select is for transforming data.  You can pass a lambda (anonymous function) like this:
                // .Select(name => new Student(name.split(" ")[0],name.split(" ")[1],1))
                // or you can pass in a reference to a function with the right signature, as seen below.
                .Select(NameToStudent);

            // All verifies that all items in the collection return true for a boolean expression.
            // Any verifies that at least one thing does.
            // Any can also be used to verify that the collection has at least one item in it like:
            // if (studentsWithBlackHair.Any()) {....}
            if (studentsWithBlackHair.All(student => student.FirstName.StartsWith("M")))
            {
                Console.WriteLine("Not getting in here");
            }

            // GroupBy returns an IEnumerable<IGrouping<TKey, TValue>>.
            // This is very similar to a List of Dictionaries of Lists.
            // I'll give a more thorough example in the notes.
            // Here we are grouping by the first letter of a student's name
            var groupedStudentNames = namesOfStudentsWithBlackHair.GroupBy(name => name.First());

            // Iterating over the groups of student names.
            // groupOfStudentNames is an IGrouping<char,string>.
            foreach (var groupOfStudentNames in groupedStudentNames)
            {
                //the Key property is a part of IGrouping<char,string>, and it is a char
                var key = groupOfStudentNames.Key;
                var numberOfPeople = groupOfStudentNames.Count();

                // groupOfStudentNames is also an IEnumerable<string>
                foreach (var name in groupOfStudentNames)
                {
                    Console.WriteLine($"{name} starts with {key}");
                }
            }

            // Join lets us combine two related but different sets of data.
            // Here we are relating students with their classes.  Students and
            // Classes both have ClassIds.  That is what we are joining on.
            var studentsWithTeachersName =
                // the outer collection we are joining
                studentsWithBlackHair
                .Join(
                    // the inner collection we are joining
                    classes,
                    //select the related property for the student
                    student => student.ClassId,
                    //select the related property for the class
                    studentClass => studentClass.ClassId,
                    //combine the two sets of data into one new data structure
                    (student, studentClass) =>
                        // this is an anonymous type.  it has readonly properties
                        // FirstName,LastName and Teacher.
                        new { student.FirstName, student.LastName, studentClass.Teacher });

            //each student is of our new anonymous type... 'a
            foreach (var student in studentsWithTeachersName)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName} has {student.Teacher} as their teacher.");
            }

            var studentsByHairColor = new Dictionary<string, List<string>>
            {
                {"Black", namesOfStudentsWithBlackHair}
            };

            studentsByHairColor.Add("Bald", new List<string> { "Martin" });

            //This throws an exception:
            //studentsByHairColor.Add("Bald", new List<string> { "Adam" });

            var theBlackHairedStudents = studentsByHairColor["Black"];

            if (theBlackHairedStudents == namesOfStudentsWithBlackHair)
            {
                Console.WriteLine("They are the same");
            }

            studentsByHairColor["Black"].Add("new person");

            foreach (var (hairColor, students) in studentsByHairColor)
            {
                Console.WriteLine($"The following students have {hairColor}");

                students.Add("new person");

                foreach (var student in students)
                {
                    Console.WriteLine(student);
                }
            }

            Console.ReadLine();
        }

        private static Student NameToStudent(string name)
        {
            return new Student(name.Split(" ")[0], name.Split(" ")[1], 1);
        }
    }
}
