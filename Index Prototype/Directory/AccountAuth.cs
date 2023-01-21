using Index_Prototype.Pages.Home;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using WpfApp2;
using static Index_Prototype.Directory.DataTemplates;
using static WpfApp2.DatabaseHelper;

namespace Index_Prototype.Directory
{
    public class AccountAuth: INotifyPropertyChanged
    {
        public static User account {  get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static void Login(string uid, string password, Action<User> OnSuccess, Action<LoginExeption> OnFailure = null)
        {
            DatabaseHelper.Login(uid, password, (User account) =>
            {
                AccountAuth.account = account;
                OnSuccess.Invoke(account);
            }, OnFailure);
        }
        public static void ChangePassword(string oldPassword,string newPassword,Action OnSuccess,Action<LoginExeption> OnFailure)
        {
            DatabaseHelper.Login(account.uid, oldPassword, (account) =>
            {
                UpdatePassword(AccountAuth.account.uid, newPassword);
                OnSuccess?.Invoke();
            }, OnFailure);
        }
        public static void Logout()
        {
            if (account == null) return;
            account = null;
            //TODO: handle navigation
            MainWindow.MainNavigationService.Navigate(new Home());
            //
        }
        public static void ClearHistory(NavigationService navigationService)
        {
            if (!navigationService.CanGoBack && !navigationService.CanGoForward)
            {
                return;
            }

            var entry = navigationService.RemoveBackEntry();
            while (entry != null)
            {
                entry = navigationService.RemoveBackEntry();
            }
            navigationService.Navigate(new PageFunction<string>() { RemoveFromJournal = true });
        }
    }
}
