using System;
using System.ComponentModel.DataAnnotations;
namespace ContosoUniversity.Models.SchoolViewModels
{
    //Created for Week 3, 6/8/2017
    public class EnrollmentDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }
        public int StudentCount { get; set; }
    }
}
