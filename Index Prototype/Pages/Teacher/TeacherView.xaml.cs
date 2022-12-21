﻿using System;
using System.Collections.Generic;
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

namespace Index_Prototype.Pages.Teacher
{
    /// <summary>
    /// Interaction logic for TeacherView.xaml
    /// </summary>
    public partial class TeacherView : Page
    {
        public TeacherView()
        {
            InitializeComponent();
            SubjectNavButton.navigationService = routerView.NavigationService;
            SettingsNavButton.navigationService = routerView.NavigationService;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
        }
    }
}
