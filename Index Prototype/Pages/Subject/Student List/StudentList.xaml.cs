﻿using Index_Prototype.Directory;
using Index_Prototype.Pages.Attendance;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2;
using static Index_Prototype.Directory.DataTemplates;
using static Index_Prototype.Pages.Subject_List.SubjectList;

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
            subject = DatabaseHelper.getSubject(NavigationHelper.getParams(MainWindow.MainNavigationService)["SubjectId"]);
            DatabaseHelper.getStudentsInSubject(subject.id).ForEach((student) =>
            {
                students.Add(new StudentListVM(student));
            });
        }

        private void AttendanceBtn_Click(object sender, RoutedEventArgs e)
        {
            Attendance.Attendance attendance = new Attendance.Attendance(subject);
            attendance.ShowDialog();
        }

        private void PickNextStudentBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
