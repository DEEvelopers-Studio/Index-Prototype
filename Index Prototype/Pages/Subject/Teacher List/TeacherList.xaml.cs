using Index_Prototype.Directory;
using Index_Prototype.Pages.Add_User;
using Index_Prototype.Pages.Student_Info;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2;
using static Index_Prototype.Directory.DataTemplates;

namespace Index_Prototype.Pages.Subject.Teacher_List
{
    /// <summary>
    /// Interaction logic for TeacherList.xaml
    /// </summary>
    /// 
    public partial class TeacherList : Page, INotifyPropertyChanged
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
                for (int i = 0; i < teachers.Count; i++)
                {
                    teachers[i].isSelected = (bool)value;
                }
            }
        }

        private void ItemToggle(object sender, RoutedEventArgs e)
        {
            int itemChecked = 0;
            foreach (var item in teachers)
            {
                if (item.isSelected) itemChecked++;
            }
            if (itemChecked == teachers.Count) isAllSelected = true;
            else if (itemChecked != 0) isAllSelected = null;
            else isAllSelected = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public DataTemplates.Subject subject { get; set; }
        public ObservableCollection<UserListVM> teachers { get; set; } = new ObservableCollection<UserListVM>();
        public class Teacher
        {
            public string name;
            public Teacher(string name)
            {
                this.name = name;
            }
        }
        public class TeacherItem
        {
            public Teacher teacher;
            public bool isSelected;
            public TeacherItem(Teacher teacher)
            {
                this.teacher = teacher;
            }
        }
        public TeacherList()
        {
            InitializeComponent();
        }

        private void SelectedTeacher(object sender, MouseButtonEventArgs e)
        {
            StudentListVM selectedStudent = (sender as Border)?.Tag as StudentListVM;
            if (selectedStudent == null) return;
            //MainWindow.MainNavigationService.Navigate(new Subject.SubjectView(selectedSubj));
            StudentInfo.ShowStudent(selectedStudent);
        }
        public void LoadData()
        {
            subject = DatabaseHelper.getSubject(NavigationHelper.getParams(MainWindow.MainNavigationService)["SubjectId"]);
            teachers.Clear();
            DatabaseHelper.getTeachersInSubject(subject.id).ForEach((teacher) =>
            {
                teachers.Add(new UserListVM(teacher));
            });
            ItemToggle(null, null);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void AddNewStudentButton_Click(object sender, RoutedEventArgs e)
        {
            AddUser addUser = new AddUser("Teacher",DatabaseHelper.getTeachers(),new List<User>(teachers));
            bool? state = addUser.ShowDialog();
            if (state != null && state == true)
            {
                addUser.selectedUsers.ForEach((user) =>
                {
                    DatabaseHelper.AddTeachertoSubject(user.uid, subject.id);
                });
                LoadData();
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            List<User> selectedTeachers = new List<User>();
            foreach (UserListVM teacher in teachers)
            {
                if (teacher.isSelected) selectedTeachers.Add(teacher);
            }
            if (teachers.Count - selectedTeachers.Count < 1) { MessageBox.Show("Must have atleast 1 Teacher or else the Subject will cease to exist!"); return; }
            MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure you want to delete {selectedTeachers.Count} Teachers?", "Removing Teacher", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No) return;
            selectedTeachers.ForEach((teacher) =>
            {
                DatabaseHelper.RemoveTeachertoSubject(teacher.uid, subject.id);
            });
            LoadData();
        }
    }
}
