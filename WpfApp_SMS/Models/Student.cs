using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Model
{
    public class Student
    {

        [Key]
        public string StudentID { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB { get; set; }
        public float GPA { get; set; }
        public byte[] Img { get; set; }
        private static int _count = 1;

        public Student(string firstName, string lastName, DateOnly dob, float gpa, byte[] img)
        {
            _count++;
            StudentID = GenerateStudentID();
            FirstName = firstName;
            LastName = lastName;
            DOB = dob;
            GPA = gpa;
            Img = img;
        }

        Student() {
            _count++;
            StudentID = GenerateStudentID();
        }
        private string GenerateStudentID()
        {

            string countString = _count.ToString("000");

            return $"S{countString}";
        }
    }
}