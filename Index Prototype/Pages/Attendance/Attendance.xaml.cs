using Index_Prototype.Directory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WpfApp2;
using static Index_Prototype.Directory.DataTemplates;

namespace Index_Prototype.Pages.Attendance
{
    /// <summary>
    /// Interaction logic for Attendance.xaml
    /// </summary>
    public partial class Attendance : Window, INotifyPropertyChanged
    {
        public Queue<Student> students { get; set; } = new Queue<Student>();
        public Student student { get; set; }
        public DataTemplates.Subject subject { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Attendance(DataTemplates.Subject subject)
        {
            this.subject = subject;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            subject = DatabaseHelper.getSubject(NavigationHelper.getParams(MainWindow.MainNavigationService)["SubjectId"]);
            DatabaseHelper.getStudentsInSubject(subject.id).ForEach((student) =>
            {
                students.Enqueue(student);
            });
            NextStudent();
        }
        public void NextStudent()
        {
            //add attenadnce record
            if(students.Count <= 0)
            {
                MessageBox.Show("Attendance Finished!");
                Close();
                return;
            }
            student = students.Dequeue();
        }
        private void AbsentBtn_Click(object sender, RoutedEventArgs e)
        {
            NextStudent();
        }

        private void PresentBtn_Click(object sender, RoutedEventArgs e)
        {
            NextStudent();
        }

        private void SkipBtn_Click(object sender, RoutedEventArgs e)
        {
            NextStudent();
        }
    }
}
