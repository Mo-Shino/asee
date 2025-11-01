    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace WpfApp1
    {
        internal class Enrollment
        {
            [Key]
            public int EnrollmentID { get; set; }
            public int UserID { get; set; }
            [ForeignKey(nameof(UserID))]
            public User User { get; set; }

            public int CourseID { get; set; }
            [ForeignKey(nameof(CourseID))]
            public Course Course { get; set; }
        }
    }
