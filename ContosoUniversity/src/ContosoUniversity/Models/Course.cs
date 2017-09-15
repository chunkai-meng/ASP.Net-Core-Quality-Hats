//Created for Week1 Exercise, 23/7/2017
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//namespace ContosoUniversity.Models
//{
//    public class Course
//    {
//        [DatabaseGenerated(DatabaseGeneratedOption.None)]
//        public int CourseID { get; set; }
//        public string Title { get; set; }
//        public int Credits { get; set; }
//        public ICollection<Enrollment> Enrollments { get; set; }
//    }
//}

//The above was replaced by the following in Week 4
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContosoUniversity.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int CourseID { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        [Range(0, 5)]
        public int Credits { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<CourseAssignment> Assignments { get; set; }
    }
}
