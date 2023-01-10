using Index_Prototype.Directory;
using Index_Prototype.Pages.Student_Info;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class TeacherList : Page
    {

        public DataTemplates.Subject subject { get; set; }
        public ObservableCollection<User> students { get; set; } = new ObservableCollection<User>();
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
        public static TeacherItem[] teacherList = { new TeacherItem(new Teacher("Carl")) };
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
            students.Clear();
            DatabaseHelper.getTeachersInSubject(subject.id).ForEach((teacher) =>
            {
                students.Add(teacher);
            });
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void ItemToggle(object sender, RoutedEventArgs e)
        {

        }
    }
}
