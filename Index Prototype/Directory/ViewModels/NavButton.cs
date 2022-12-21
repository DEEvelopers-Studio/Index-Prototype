using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace Index_Prototype.Directory.ViewModels
{
    internal class NavButton:ToggleButton
    {
        public Uri Navlink
        {
            get { return (Uri)GetValue(NavlinkProperty); }
            set { SetValue(NavlinkProperty, value); }
        }
        public static readonly DependencyProperty NavlinkProperty = DependencyProperty.Register("Navlink", typeof(Uri), typeof(NavButton), new PropertyMetadata(null));

        public NavigationService navigationService
        {
            get { return (NavigationService)GetValue(Navigationservice); }
            set { SetValue(Navigationservice, value);
                navigationService.LoadCompleted += (s, e) => { IsChecked = e.Uri.ToString().Replace("%20"," ").Insert(0,"/").Equals(Navlink.ToString()); };
            }
        }
        public static readonly DependencyProperty Navigationservice = DependencyProperty.Register("navigationService", typeof(NavigationService), typeof(NavButton));
        public NavButton()
        {
            Click += (s, e) => { navigationService.Navigate(Navlink); };
        }
    }
}
