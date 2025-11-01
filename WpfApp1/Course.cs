using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class Course
    {
        [Key]
        public int CourseID { get; set; }
        public string CourseName { get; set; }

        [Column("TeacherID")]
        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]

        public User User { get; set; }
    }
}
