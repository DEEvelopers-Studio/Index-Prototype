using Index_Prototype.Directory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
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

namespace Index_Prototype.Pages.Student_Info
{
    /// <summary>
    /// Interaction logic for StudentInfo.xaml
    /// </summary>
    public partial class StudentInfo : Window, INotifyPropertyChanged
    {
        public static StudentInfo Instance;

        public event PropertyChangedEventHandler PropertyChanged;
        public static void ShowStudent(DataTemplates.Student student, Action action = null)
        {
            if (Instance == null) { 
                Instance = new StudentInfo(); 
            }
            Instance.refresh = action;
            Instance.student = student;
            Instance.Show();
            Instance.Focus();
        }
        public DataTemplates.Student _student { get; set; }
        public DataTemplates.Student student { get { return _student; } set { _student = value;if(value!=null)
                    LoadData();
            }}
        public StudentInfo()
        {
            InitializeComponent();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
        }
        SqlDataAdapter adapter;
        Index_Prototype.DatabaseDataSet2 databaseDataSet2;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Instance.Focus();
            
        }
        public void LoadData()
        {
            string subjectId = NavigationHelper.getParams(MainWindow.MainNavigationService)["SubjectId"];
            Index_Prototype.DatabaseDataSet databaseDataSet = ((Index_Prototype.DatabaseDataSet)(this.FindResource("databaseDataSet")));
            // Load data into the table Grades. You can modify this code as needed.
            Index_Prototype.DatabaseDataSetTableAdapters.GradesTableAdapter databaseDataSetGradesTableAdapter = new Index_Prototype.DatabaseDataSetTableAdapters.GradesTableAdapter();
            databaseDataSetGradesTableAdapter.Fill(databaseDataSet.Grades);
            System.Windows.Data.CollectionViewSource gradesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("gradesViewSource")));
            gradesViewSource.View.MoveCurrentToFirst();
            databaseDataSet2 = ((Index_Prototype.DatabaseDataSet2)(this.FindResource("databaseDataSet2")));
            // TODO: Add code here to load data into the table Attendance.
            // This code could not be generated, because the databaseDataSet2AttendanceTableAdapter.Fill method is missing, or has unrecognized parameters.
            Index_Prototype.DatabaseDataSet2TableAdapters.AttendanceTableAdapter databaseDataSet2AttendanceTableAdapter = new Index_Prototype.DatabaseDataSet2TableAdapters.AttendanceTableAdapter();
            adapter = databaseDataSet2AttendanceTableAdapter.Adapter;
            databaseDataSet2AttendanceTableAdapter.Fill(databaseDataSet2.Attendance, student.uid, subjectId);
            System.Windows.Data.CollectionViewSource attendanceViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("attendanceViewSource")));
            attendanceViewSource.View.MoveCurrentToFirst();
            Index_Prototype.DatabaseDataSet3 databaseDataSet3 = ((Index_Prototype.DatabaseDataSet3)(this.FindResource("databaseDataSet3")));
            // TODO: Add code here to load data into the table Attendance.
            // This code could not be generated, because the databaseDataSet3AttendanceTableAdapter.Fill method is missing, or has unrecognized parameters.

            Index_Prototype.DatabaseDataSet3TableAdapters.AttendanceTableAdapter databaseDataSet3AttendanceTableAdapter = new Index_Prototype.DatabaseDataSet3TableAdapters.AttendanceTableAdapter();
            databaseDataSet3AttendanceTableAdapter.Fill(databaseDataSet3.Attendance, subjectId, student.uid);
            System.Windows.Data.CollectionViewSource attendanceViewSource1 = ((System.Windows.Data.CollectionViewSource)(this.FindResource("attendanceViewSource1")));
            attendanceViewSource1.View.MoveCurrentToFirst();
            Instance.Focus();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        public Action refresh;
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EditStudentInfo.EditStudentInfo editStudent = new EditStudentInfo.EditStudentInfo(student);
            if (editStudent.ShowDialog() == false) return;
            Instance.student = DatabaseHelper.getStudent(student.uid);
            refresh?.Invoke();
        }
    }
}
