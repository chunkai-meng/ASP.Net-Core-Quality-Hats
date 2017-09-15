//Created for Week1 Parctical, 23/7/2017
//namespace ContosoUniversity.Models
//{
//    public enum Grade
//    {
//        A, B, C, D, F
//    }
//    public class Enrollment
//    {
//        public int EnrollmentID { get; set; }
//        public int CourseID { get; set; }
//        public int StudentID { get; set; }
//        public Grade? Grade { get; set; }
//        public Course Course { get; set; }
//        public Student Student { get; set; }
//    }
//}

//The above was replaced by the following in Week 4
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
