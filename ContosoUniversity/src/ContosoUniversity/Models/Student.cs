using System; //Created for Week1 Practical, 23/7/2017 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;//Added for Week 4
using System.ComponentModel.DataAnnotations.Schema;//Week 4
namespace ContosoUniversity.Models
{
    public class Student
    {
        //public int ID { get; set; }
        //[StringLength(50)] //Week 4 
        //public string LastName { get; set; }
        //[StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")] //Week4
        //[Column("FirstName")] //Week 4
        //public string FirstMidName { get; set; }
        //[DataType(DataType.Date)] //Week 4
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] //Week 4
        //public DateTime EnrollmentDate { get; set; }
        //public ICollection<Enrollment> Enrollments { get; set; }

        //The above was replaced by the following
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }
        public string PathOfFile { get; set; } //Added in Week 6
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
