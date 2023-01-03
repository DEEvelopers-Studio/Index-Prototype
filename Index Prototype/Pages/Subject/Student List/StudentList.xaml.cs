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
using WpfApp2;
using static Index_Prototype.Directory.DataTemplates;

namespace Index_Prototype.Pages.Subject.Student_List
{
    /// <summary>
    /// Interaction logic for StudentList.xaml
    /// </summary>
    public partial class StudentList : Page,INotifyPropertyChanged
    {
        public String[] mode { get; set; }
        public List<UserListVM> students { get; set; } = new List<UserListVM>();
        
        public StudentList()
        {
            DatabaseHelper.getStudents().ForEach((student)=>
            {
                students.Add(new UserListVM(student));
            });
            InitializeComponent();
            mode = new String[] { "Pick Next Student", "Pick Random Student", "Pick Next Random Student" };
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ItemToggle(object sender, RoutedEventArgs e)
        {

        }

        private void SelectedSubject(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
