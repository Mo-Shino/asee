using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class db_Student_Gradebook_System : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<Grade> Grade { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=Mo-Elshinawy;Initial Catalog=db_Student_Gradebook_System;Integrated Security=True;Trust Server Certificate=True");
        }
    }
}
