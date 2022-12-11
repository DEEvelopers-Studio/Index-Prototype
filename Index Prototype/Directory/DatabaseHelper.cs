using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Index_Prototype.Pages.Home.Accounts;
using System.Security;
using Index_Prototype.Pages.Home;

namespace WpfApp2
{
    static class DatabaseHelper
    {
        public static SqlConnection connection;
        public static void Initialize(Action<SqlConnection> onInit,Action<Exception> onFail = null)
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\Database.mdf\";Integrated Security=True");
            try
            {
                connection.Open();
            }
            catch(Exception e)
            {
                onFail?.Invoke(e);
            }
            finally
            {

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    onInit?.Invoke(connection);
                }

            }
        }
        public static List<User> getTeachers()
        {
            List<User> accounts = new List<User>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from Teachers", connection);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    accounts.Add(new User() { firstName = sdr?["firstName"] as string, lastName = sdr?["lastName"] as string, middleName = sdr?["middleName"] as string, uid = sdr?["uid"] as string});
                }
            }
            catch (Exception r)
            {
                MessageBox.Show("" + r);
            }
            finally
            {

                //insertData(myConn);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            return accounts;
        }
        public enum LoginState { SUCCESS,PASSWORDFAIL,UIDFAIL,DBFAIL}
        public static LoginState Login(String uid,string password)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select password from Teachers where uid = '"+uid+"'", connection);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (!sdr.Read()) return LoginState.UIDFAIL;
                return (sdr["password"] as string).Equals(password.ToString()) ? LoginState.SUCCESS : LoginState.PASSWORDFAIL;
            }
            catch (Exception r)
            {
                return LoginState.DBFAIL;
            }
            finally
            {

                //insertData(myConn);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }
    }
}
