using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for studentPage.xaml
    /// </summary>
    public partial class studentPage : Window
    {
        private readonly int _currentStudentId;
        public studentPage(int studentID)
        {
            InitializeComponent();
            _currentStudentId = studentID;
            comboboxLoud(_currentStudentId);
        }

        private void comboboxLoud(int studentID)
        {
            using (var context = new db_Student_Gradebook_System())
            {
                var enrolledCourses = context.Enrollment
                    .Include(e => e.Course)
                    .Where(e => e.UserID == _currentStudentId)
                    .Select(e => new
                    {
                        e.EnrollmentID,
                        CourseName = e.Course.CourseName
                    })
                    .ToList();

                StudentsPageCombo.ItemsSource = enrolledCourses;
                StudentsPageCombo.DisplayMemberPath = "CourseName";
                StudentsPageCombo.SelectedValuePath = "EnrollmentID";
            }
        }


        private void LoadStudentGrades(int enrollmentId)
        {
            using (var context = new db_Student_Gradebook_System())
            {
                var studentGrades = context.Grade
                    .Where(g => g.EnrollmentID == enrollmentId)
                    .Select(g => new
                    {
                        Assignment = g.AssignmentName,
                        Score = g.Score,
                        Max_Score = g.MaxScore,
                        Percentage = (g.MaxScore > 0)
                        ? (g.Score / g.MaxScore * 100) : (decimal?)null
                    })
                    .ToList();
                StudentsPageGrid.ItemsSource = studentGrades;
            }
        }

        private void StudentsPageCombo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (StudentsPageCombo.SelectedValue is int enrollmentId)
            {
                LoadStudentGrades(enrollmentId);
            }
            else
            {
                StudentsPageGrid.ItemsSource = null;
            }
        }
    }
}
