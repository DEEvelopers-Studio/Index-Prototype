using Index_Prototype.Directory;
using Index_Prototype.Pages.Add_User;
using Index_Prototype.Pages.Student_Info;
using Index_Prototype.Pages.Subject.Add_Student;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
        private bool? _isAllSelected { get; set; } = false;
        public bool? isAllSelected
        {
            get { return _isAllSelected; }
            set
            {
                _isAllSelected = value;
                deleteBtn.Visibility = value != false ? Visibility.Visible : Visibility.Hidden;
                if (value == null) return;
                for (int i = 0; i < students.Count; i++)
                {
                    students[i].isSelected = (bool)value;
                }
            }
        }

        private void ItemToggle(object sender, RoutedEventArgs e)
        {
            int itemChecked = 0;
            foreach (var item in students)
            {
                if (item.isSelected) itemChecked++;
            }
            if (itemChecked == students.Count) isAllSelected = true;
            else if (itemChecked != 0) isAllSelected = null;
            else isAllSelected = false;
        }
        public DataTemplates.Subject subject { get; set; }
        public ObservableCollection<StudentListVM> students { get; set; } = new ObservableCollection<StudentListVM>();
        
        public StudentList()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private async void SelectedStudent(object sender, MouseButtonEventArgs e)
        {

            //lets the new window to be focused
            await Task.Delay(100);
            StudentListVM selectedStudent = (sender as Border)?.Tag as StudentListVM;
            if (selectedStudent == null) return;
            //MainWindow.MainNavigationService.Navigate(new Subject.SubjectView(selectedSubj));
            StudentInfo.ShowStudent(selectedStudent ,() => LoadData());
        }
        AddUser addUser;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<User> exclude = new List<User>(students);
            addUser = new AddUser("Student",DatabaseHelper.getStudents(), exclude, () => { 
                AddStudent addStudent = new AddStudent();
                bool? ste = addStudent.ShowDialog();
                if (ste != null && ste == true)
                {
                    addUser.users.Clear();
                    DatabaseHelper.getStudents().ForEach((user) =>
                    {
                        if (exclude.Find((usr) => { return usr.uid == user.uid; }) != null) return;
                        addUser.users.Add(new UserListVM(user));
                    });
                }
            });
            bool? state = addUser.ShowDialog();
            if (state != null && state == true)
            {
                addUser.selectedUsers.ForEach((user) =>
                {
                    DatabaseHelper.AddStudenttoSubject(user.uid, subject.id);
                });
                LoadData();
            }
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
            ItemToggle(null, null);
        }

        private void AttendanceBtn_Click(object sender, RoutedEventArgs e)
        {
            Attendance.Attendance attendance = new Attendance.Attendance(subject);
            attendance.ShowDialog();
            LoadData();
        }

        public String[] mode { get; set; } = new String[] { "Pick Next Student", "Pick Random Student", "Pick Next Random Student" };
        Queue<Student> studentQueue = new Queue<Student>();
        private void PickNextStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (students == null) return;
            if (students.Count <= 0) return;
            if (studentQueueModeBox.SelectedIndex == 1) {
                StudentInfo.ShowStudent(students[gacha.Next(students.Count)],()=>LoadData());
                return;
            };
            if (studentQueue.Count == 0) FillUpQueue();
            StudentInfo.ShowStudent(studentQueue.Dequeue(),()=>LoadData());
        }
        private static Random gacha = new Random();
        public void FillUpQueue()
        {
            switch (studentQueueModeBox.SelectedIndex)
            {
                case 2:
                    studentQueue = new Queue<Student>(students.OrderBy(a => gacha.Next()).ToList());
                    break;
                default:
                    studentQueue = new Queue<Student>(students); 
                    break;
            }
        }
        private void ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            StudentListVM selectedStudent = (sender as ComboBox)?.Tag as StudentListVM;
            if (selectedStudent == null) return;
            //MainWindow.MainNavigationService.Navigate(new Subject.SubjectView(selectedSubj));
            DatabaseHelper.PutAttendance(selectedStudent, subject.id);
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            List<User> selectedStudents = new List<User>();
            foreach (StudentListVM student in students)
            {
                if (student.isSelected) selectedStudents.Add(student);
            }
            MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure you want to remove {selectedStudents.Count} Students in {subject.title}?", "Removing student", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No) return;
            selectedStudents.ForEach((student) =>
            {
                DatabaseHelper.RemoveStudenttoSubject(student.uid, subject.id);
            });
            LoadData();
        }
    }
}
