using Index_Prototype.Directory;
using Index_Prototype.Pages.Subject.Data_Importer;
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
namespace Index_Prototype.Pages.Subject.Configuration
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    /// 
    public partial class Configuration : Page, INotifyPropertyChanged
    {
        public bool isChanged { get; set; } = false;
        public DataTemplates.Subject prevSubject { get; set; }
        private DataTemplates.Subject _subject { get; set; }
        public DataTemplates.Subject subject {
            get { return _subject; } set {
                _subject = value;
                //defaultStdntSlctnBox.SelectedIndex = (int)value.defaultStudentSelection;
            } }
        //public string[] menu { get; } = DataTemplates.StuddentSelectStr;

        public String[] mode { get; set; } = new String[] { "Pick Next Student", "Pick Random Student", "Pick Next Random Student" };
        public event PropertyChangedEventHandler PropertyChanged;

        public Configuration()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSubject();
        }

        public void LoadSubject()
        {
            subject = DatabaseHelper.getSubject(NavigationHelper.getParams(MainWindow.MainNavigationService)["SubjectId"]);
            subject.PropertyChanged += change;
        }
        public void change(object sender, PropertyChangedEventArgs args)
        {
            isChanged = true;
        }
        private void applyChangesBtn_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper.PutSubject(subject);
            isChanged = false;
            SubjectView.Instance.subject = DatabaseHelper.getSubject(NavigationHelper.getParams(MainWindow.MainNavigationService)["SubjectId"]);
        }

        private void revertChanges_Click(object sender, RoutedEventArgs e)
        {
            LoadSubject();
            isChanged = false;
        }

        private void importBtn_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow import = new ImportWindow(subject.id);
            import.ShowDialog();
        }

        private void exportBtn_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper.ExportAttendance(subject);
        }
        public void ExportData()
        {
            
        }
    }
}
