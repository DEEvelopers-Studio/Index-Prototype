using Index_Prototype.Directory;
using Index_Prototype.Pages.Student_Info;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp2;
using static Index_Prototype.Directory.DataTemplates;

namespace Index_Prototype.Pages.Subject.Student_List
{
    /// <summary>
    /// Interaction logic for StudentList.xaml
    /// </summary>
    public partial class StudentList : Page,INotifyPropertyChanged
    {
        public String[] mode { get; set; } = new String[] { "Pick Next Student", "Pick Random Student", "Pick Next Random Student" };
        public DataTemplates.Subject subject { get; set; }
        public ObservableCollection<StudentListVM> students { get; set; } = new ObservableCollection<StudentListVM>();
        
        public StudentList()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ItemToggle(object sender, RoutedEventArgs e)
        {

        }

        private void SelectedStudent(object sender, MouseButtonEventArgs e)
        {
            StudentListVM selectedStudent = (sender as Border)?.Tag as StudentListVM;
            if (selectedStudent == null) return;
            //MainWindow.MainNavigationService.Navigate(new Subject.SubjectView(selectedSubj));
            StudentInfo.ShowStudent(selectedStudent);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add_Student.AddStudent form = new Add_Student.AddStudent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        public void LoadData()
        {
            subject = DatabaseHelper.getSubject(NavigationHelper.getParams(MainWindow.MainNavigationService)["SubjectId"]);
            students.Clear();
            DatabaseHelper.getStudentsInSubject(subject.id).ForEach((student) =>
            {
                students.Add(new StudentListVM(student));
            });
        }

        private void AttendanceBtn_Click(object sender, RoutedEventArgs e)
        {
            Attendance.Attendance attendance = new Attendance.Attendance(subject);
            attendance.ShowDialog();
            LoadData();
        }

        private void PickNextStudentBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            StudentListVM selectedStudent = (sender as ComboBox)?.Tag as StudentListVM;
            if (selectedStudent == null) return;
            //MainWindow.MainNavigationService.Navigate(new Subject.SubjectView(selectedSubj));
            DatabaseHelper.PutAttendance(selectedStudent, subject.id);
        }
    }
}
