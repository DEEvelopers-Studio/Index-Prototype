using ControlzEx.Standard;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfApp2;

namespace Index_Prototype.Pages.Subject.Data_Importer
{
    /// <summary>
    /// Interaction logic for ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : System.Windows.Window, INotifyPropertyChanged
    {
        public string _filename { get; set; }
        public string filename { get { return _filename; } set { _filename = value; importBtn.Visibility = !string.IsNullOrWhiteSpace(value) ? Visibility.Visible : Visibility.Hidden; } }
        public string subjectId { get; set; }
        public ImportWindow()
        {
            InitializeComponent();
        }
        public ImportWindow(string subjectId)
        {
            
            this.subjectId = subjectId;

            InitializeComponent();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void openFileDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog { Filter = "Excel File|*.xlsx;*.xls" };
            if(fileDialog.ShowDialog() == true)
            {
                filename = fileDialog.FileName;
            }
        }

        private void importBtn_Click(object sender, RoutedEventArgs e)
        {
            Workbook wb = null;
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                wb = excel.Workbooks.Open(filename, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Problem Opening the file");

                wb?.Close();
                Close();
                return;
            }
                Dictionary<string, string> studentsRaw = new Dictionary<string, string>();
                Worksheet xlWorksheet = wb.Sheets[1];
                string tst = wb.Sheets[1].Name;
                Range namesColumn = xlWorksheet.UsedRange.Columns[NameColumnTargetBox.Text];
                Range uidColumn = xlWorksheet.UsedRange.Columns[StudIDColumnTargetBox.Text];
                bool isSectionIncluded = !string.IsNullOrWhiteSpace(SectionColBox.Text);
                Range sectionColumn = isSectionIncluded?xlWorksheet.UsedRange.Columns[SectionColBox.Text]:null;
                string[] namesArray = ((System.Array)namesColumn.Cells.Value).OfType<object>().Select(o => o.ToString()).ToArray();
                string[] uidArray = ((System.Array)uidColumn.Cells.Value).OfType<object>().Select(o => o.ToString()).ToArray();
                string[] sectionArray = sectionColumn != null?((System.Array)sectionColumn.Cells.Value).OfType<object>().Select(o => o.ToString()).ToArray():null;
                int shortestRow = uidArray.Length > namesArray.Length ? namesArray.Length : uidArray.Length;
                int failedImports = 0;
                int startReadingat = int.Parse(StartReadingRow.Text) -1;
            if (startReadingat > shortestRow || startReadingat < 0) { MessageBox.Show("Invalid row start count"); wb?.Close(); return; }
                for (int i = startReadingat; i < shortestRow; i++)
                {
                string uid = uidArray[i];
                string name = namesArray[i];
                string seciton = i < (sectionArray?.Length??0) ? sectionArray?[i] : null ;
                    if (name == null || uid == null) { failedImports++; continue; }
                if (uid.Length == 10) uid = "0"+uid;
                    if (uid.Length != 11) { failedImports++; continue; }
                string[] parsedName = name.Split(new char[] {','}, 2);
                if(parsedName.Length < 2) { failedImports++; continue; }
                    DatabaseHelper.PutStudent(new Directory.DataTemplates.Student() { firstName = parsedName[1].Trim(),lastName = parsedName[0].Trim(), uid=uid,section= seciton });
                    if(subjectId != null) DatabaseHelper.AddStudenttoSubject( uid, subjectId);
                }

            MessageBox.Show(failedImports == 0 ? "Data Imported Successfully" : $"Data Imported with {failedImports} failure");
            wb?.Close();
            Close();
            //Range xlRange = xlWorksheet.UsedRange;
            //Range bColumn = xlWorksheet.get_Range("B", null);
            
            

            
            
        }
        private void StartReadingRow_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
    }
}
