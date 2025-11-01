using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class Grade
    {
        [Key]
        public int GradeID { get; set; }
        public int EnrollmentID { get; set; }
        public string AssignmentName { get; set; }
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }

        [ForeignKey(nameof(EnrollmentID))]
        public Enrollment enrollment { get; set; }
    }
}
