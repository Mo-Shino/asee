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
    public partial class TeacherPage : Window
    {
        public TeacherPage(int crruntTeacherId)
        {
            InitializeComponent();
            loudCoursStudentData(crruntTeacherId);
        }

        private void loudCoursStudentData(int crruntTeacherId)
        {
            using (var context = new db_Student_Gradebook_System())
            {
                var selectedCours = context.Course.Where(U => U.UserID == crruntTeacherId).ToList();
                Course_DropDown.ItemsSource = selectedCours;
            }
        }

        private void Course_DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StudentsassignmentsDataGrid.ItemsSource = null;
            txtStudentID.Text = string.Empty;
            txtStudentName.Text = string.Empty;
            txtStudentAvgScour.Text = string.Empty;

            if (Course_DropDown.SelectedItem is Course selected)
            {
                int selID = selected.CourseID;
                using (var context = new db_Student_Gradebook_System())
                {
                    var shit = context.Enrollment
                        .Include(e => e.User)
                        .Where(e => e.CourseID == selID)
                        .Select(e => new
                        {
                            studentID = e.UserID,
                            StudentName = e.User.UserName,
                            enrolmentID = e.EnrollmentID
                        }).ToList();

                    StudentsCourssDataGrid.ItemsSource = shit;
                }
            }
        }

        private void StudentsCourssDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectedIt = StudentsCourssDataGrid.SelectedItem;
            if (selectedIt != null)
            {
                txtStudentID.Text = selectedIt.studentID.ToString();
                txtStudentName.Text = selectedIt.StudentName;
            }
            else
            {
                txtStudentID.Text = string.Empty;
                txtStudentName.Text = string.Empty;
            }
            StudentsassignmentsDataGrid.ItemsSource = null;
            txtStudentAvgScour.Text = string.Empty;
            txtassignmentID.Text = string.Empty;
            txtAssignmentName.Text = string.Empty;
            txtstudentScour.Text = string.Empty;
        }

        private void LoadStudentAssignments(int enrollmentId)
        {
            using (var context = new db_Student_Gradebook_System())
            {
                var studentAssignments = context.Grade.Where(g => g.EnrollmentID == enrollmentId).Select(g => new
                    {
                        AssignmenteID = g.GradeID,
                        AssignmentName = g.AssignmentName,
                        Score = g.Score,
                        MaxScore = g.MaxScore
                    })
                    .ToList();

                if (studentAssignments.Any())
                {
                    StudentsassignmentsDataGrid.ItemsSource = studentAssignments;
                    decimal totalScore = studentAssignments.Sum(g => g.Score);
                    decimal totalMaxScore = studentAssignments.Sum(g => g.MaxScore);

                    if (totalMaxScore > 0)
                    {
                        decimal averagePercentage = (totalScore / totalMaxScore) * 100;
                        txtStudentAvgScour.Text = averagePercentage.ToString("F1");
                    }
                    else
                    {
                        txtStudentAvgScour.Text = "0.00";
                    }
                }
                else
                {
                    StudentsassignmentsDataGrid.ItemsSource = null;
                    txtStudentAvgScour.Text = string.Empty;
                }
            }
        }

        private void viwe_Click(object sender, RoutedEventArgs e)
        {
            if (Course_DropDown.SelectedItem == null || string.IsNullOrEmpty(txtStudentID.Text))
            {
                MessageBox.Show("select a cours and a student");
                return;
            }
            Course selectedCours = Course_DropDown.SelectedItem as Course;
            int coursID = selectedCours.CourseID;
            int studentID = int.Parse(txtStudentID.Text);

            using (var context = new db_Student_Gradebook_System())
            {
                var enrollment = context.Enrollment.FirstOrDefault(n => n.CourseID == coursID && n.UserID == studentID);
                if (enrollment == null)
                {
                    MessageBox.Show("nooooooo");
                    StudentsassignmentsDataGrid.ItemsSource = null;
                    return;
                }
                int enrolmentID = enrollment.EnrollmentID;
                LoadStudentAssignments(enrolmentID);
            }
        }

        private void StudentsassignmentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selected = StudentsassignmentsDataGrid.SelectedItem;
            if (selected != null)
            {
                txtassignmentID.Text = selected.AssignmenteID.ToString();
                txtAssignmentName.Text = selected.AssignmentName;
                txtstudentScour.Text = selected.Score.ToString();
            }
            else
            {
                txtassignmentID.Text = string.Empty;
                txtAssignmentName.Text = string.Empty;
                txtstudentScour.Text = string.Empty;
            }
        }

        private void AddassBTN_Click(object sender, RoutedEventArgs e)
        {
            if (Course_DropDown.SelectedItem == null || string.IsNullOrEmpty(txtStudentID.Text))
            {
                MessageBox.Show("select a course and a student.", "Error");
                return;
            }
            string assignmentName = txtAssignmentName.Text;
            string scoreText = txtstudentScour.Text;

            if (string.IsNullOrEmpty(assignmentName) || string.IsNullOrEmpty(scoreText))
            {
                MessageBox.Show("enter the assignment name and score.");
                return;
            }
            if (!int.TryParse(txtStudentID.Text, out int studentID) || !decimal.TryParse(scoreText, out decimal score) && score < 100)
            {
                MessageBox.Show("check the entered values.", "Error");
                return;
            }

            decimal maxScore = 100;
            Course selectedCours = Course_DropDown.SelectedItem as Course;
            int coursID = selectedCours.CourseID;

            using (var context = new db_Student_Gradebook_System())
            {
                var shit = context.Enrollment.FirstOrDefault(u => u.CourseID == coursID && u.UserID == studentID);
                
                if (shit == null)
                {
                    MessageBox.Show("errorrrrrrrr");
                    return;
                }
                int enrolmentID = shit.EnrollmentID;

                var grade = new Grade 
                {
                    EnrollmentID = enrolmentID,
                    AssignmentName = assignmentName,
                    Score = score,
                    MaxScore = maxScore
                };
                context.Grade.Add(grade);
                context.SaveChanges();

                txtAssignmentName.Text = string.Empty;
                txtstudentScour.Text = string.Empty;
                LoadStudentAssignments(enrolmentID);
            }
        }

        private void UpdatassBTN_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtassignmentID.Text) || string.IsNullOrEmpty(txtAssignmentName.Text) || string.IsNullOrEmpty(txtstudentScour.Text))
            {
                MessageBox.Show("select an assignment and enter all values", "Error");
                return;
            }

            if (!int.TryParse(txtassignmentID.Text, out int gradeId) || !decimal.TryParse(txtstudentScour.Text, out decimal newScore))
            {
                MessageBox.Show("Errorrrr", "Error");
                return;
            }


            string newAssignmentName = txtAssignmentName.Text;

            using (var context = new db_Student_Gradebook_System())
            {
                var gradeToUpdate = context.Grade.FirstOrDefault(g => g.GradeID == gradeId);

                if (gradeToUpdate != null)
                {
                    gradeToUpdate.AssignmentName = newAssignmentName;
                    gradeToUpdate.Score = newScore;

                    context.SaveChanges();

                    LoadStudentAssignments(gradeToUpdate.EnrollmentID);
                }
                else
                {
                    MessageBox.Show("errorrrr", "Error");
                }
            }
        }

        private void deleteassBTN_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtassignmentID.Text))
            {
                MessageBox.Show("select the assignment to deleted.", "Error");
                return;
            }

            if (!int.TryParse(txtassignmentID.Text, out int gradeId))
            {
                MessageBox.Show("Error reading assignment ID.", "Error");
                return;
            }

            using (var context = new db_Student_Gradebook_System())
            {
                var shit = context.Grade.FirstOrDefault(x => x.GradeID == gradeId);
                if (shit == null)
                {
                    MessageBox.Show("errorrrrr");
                    return;
                }
                int enrolmentToReloud = shit.EnrollmentID;
                context.Grade.Remove(shit);
                context.SaveChanges();
                txtAssignmentName.Text = string.Empty;
                txtStudentAvgScour.Text = string.Empty;
                LoadStudentAssignments(enrolmentToReloud);
                MessageBox.Show("remooooved");
            }
        }
    }
}