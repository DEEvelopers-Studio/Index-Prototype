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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Index_Prototype.Pages.Student_Info
{
    /// <summary>
    /// Interaction logic for StudentInfo.xaml
    /// </summary>
    public partial class StudentInfo : Window, INotifyPropertyChanged
    {
        public static StudentInfo Instance;

        public event PropertyChangedEventHandler PropertyChanged;

        public static StudentInfo getInstance()
        {
            if (Instance == null) Instance = new StudentInfo(); 
            return Instance;
        }
        public static void ShowStudent(DataTemplates.Student student)
        {
            if (Instance == null) { Instance = new StudentInfo(); Instance.Show(); }
            Instance.student =  student;

            Instance.Focus();

        }
        public DataTemplates.Student student { get; set; }
        public StudentInfo()
        {
            InitializeComponent();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Instance.Focus();
            Index_Prototype.DatabaseDataSet databaseDataSet = ((Index_Prototype.DatabaseDataSet)(this.FindResource("databaseDataSet")));
            // Load data into the table Grades. You can modify this code as needed.
            Index_Prototype.DatabaseDataSetTableAdapters.GradesTableAdapter databaseDataSetGradesTableAdapter = new Index_Prototype.DatabaseDataSetTableAdapters.GradesTableAdapter();
            databaseDataSetGradesTableAdapter.Fill(databaseDataSet.Grades);
            System.Windows.Data.CollectionViewSource gradesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("gradesViewSource")));
            gradesViewSource.View.MoveCurrentToFirst();
        }
    }
}
