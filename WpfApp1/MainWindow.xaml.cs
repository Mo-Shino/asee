using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string pass = txtPass.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
            {
                Error_mas.Content = "enter all the values !!!!!";
            }

            else
            {
                using (var context = new db_Student_Gradebook_System())
                {
                    var user = context.User.FirstOrDefault(u => u.Email == email && u.Password == pass);
                    if (user != null)
                    {
                        if (user.UserRole == "Teacher")
                        {
                            TeacherPage teacherPage = new TeacherPage(user.UserID);
                            teacherPage.Show();
                            this.Close();
                        }

                        else if (user.UserRole == "Student")
                        {
                            studentPage studentpage = new studentPage(user.UserID);
                            studentpage.Show();
                            this.Close();
                        }
                    }
                    else Error_mas.Content = "wrong email and password_-_-_-_-";
                }
            }
        }
    }
}