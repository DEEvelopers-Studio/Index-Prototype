using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using Index_Prototype.Pages.Subject;
using static Index_Prototype.Directory.DataTemplates;
using Index_Prototype.Pages.Home;
using System.Web.UI.WebControls;
using FastMember;
using System.Linq;
using System.Security.Cryptography;

public enum LoginExeption { PASSWORDFAIL, UIDFAIL, DBFAIL }
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
        public static Subject getSubject(string id)
        {
            //   84cd2e3a - f898 - 4bbf - ade4 - 921ee59abe37
            Subject subject = null;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * from Subjects where id='{id}'", connection);
                SqlDataReader sdr = cmd.ExecuteReader();
                //sdr.Read();
                subject = ConvertToObject<Subject>(sdr);                
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
            return subject;
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
                    accounts.Add(ConvertToObject<User>(sdr));
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
        public static bool BitToBool(int? num) => num == 1;
        public static List<Subject> getSubjects() => getDocumentsInTable<Subject>("Subjects");
        public static List<User> getStudents() => getDocumentsInTable<User>("Students");

        public static User getStudent(string uid) => getDocument<User>(uid, "Students");
        public static User getTeacher(string uid) => getDocument<User>(uid, "Teachers");
        public enum UserType { Teacher ,Student};
        private static string[] _userType = { "Teachers", "Students" };
        public static User getUser(string uid,UserType userType) => getDocument<User>(uid, _userType[(int)userType]);
        public static List<T> getDocumentsInTable<T>(string Table, string query = "") where T : class, new()
        {
            List<T> items = new List<T>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * from {Table} {query}", connection);
                SqlDataReader sdr = cmd.ExecuteReader();
                //sdr.Read();

                while (sdr.Read())
                {
                    items.Add(ConvertToObject<T>(sdr));
                }
                return items;
                //user = new User() { firstName = sdr?["firstName"] as string, lastName = sdr?["lastName"] as string, middleName = sdr?["middleName"] as string, uid = sdr?["uid"] as string };

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
            return null;
        }
        public static T getDocument<T>(string id, string Table) where T : class, new()
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * from {Table} where id='{id}'", connection);
                SqlDataReader sdr = cmd.ExecuteReader();
                //sdr.Read();

                return ConvertToObject<T>(sdr);
                //user = new User() { firstName = sdr?["firstName"] as string, lastName = sdr?["lastName"] as string, middleName = sdr?["middleName"] as string, uid = sdr?["uid"] as string };

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
            return null;
        }
        
        public static void Login(string uid, string password,Action<User> OnSuccess,Action<LoginExeption> OnFailure = null)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from Teachers where uid = '" + uid + "'", connection);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (!sdr.Read()) OnFailure?.Invoke(LoginExeption.UIDFAIL);
                if (!(sdr["password"] as string).Equals(password.ToString())){ OnFailure?.Invoke(LoginExeption.PASSWORDFAIL);return; }
                OnSuccess.Invoke(new User() { firstName = sdr?["firstName"] as string, lastName = sdr?["lastName"] as string, middleName = sdr?["middleName"] as string, uid = sdr?["uid"] as string });
            }
            catch (Exception r)
            {
                OnFailure?.Invoke(LoginExeption.DBFAIL);
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
        public static T ConvertToObject<T>(this SqlDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[t, fieldName] = rd.GetValue(i);
                    }
                }
            }

            return t;
        }
        //public class Data
        //{
        //    public void ConvertToObject( SqlDataReader rd)
        //    {
        //        Type type = typeof(T);
        //        var accessor = TypeAccessor.Create(type);
        //        var members = this.GetType().GetFields();
        //        var t = new T();

        //        for (int i = 0; i < rd.FieldCount; i++)
        //        {
        //            if (!rd.IsDBNull(i))
        //            {
        //                string fieldName = rd.GetName(i);

        //                if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
        //                {
        //                    accessor[t, fieldName] = rd.GetValue(i);
        //                }
        //            }
        //        }

        //        return t;
        //    }
        //}
        //public static LoginState Login(string uid,string password)
        //{
        //    try
        //    {
        //        connection.Open();
        //        SqlCommand cmd = new SqlCommand("Select password from Teachers where uid = '"+uid+"'", connection);
        //        SqlDataReader sdr = cmd.ExecuteReader();
        //        if (!sdr.Read()) return LoginState.UIDFAIL;
        //        return (sdr["password"] as string).Equals(password.ToString()) ? LoginState.SUCCESS : LoginState.PASSWORDFAIL;
        //    }
        //    catch (Exception r)
        //    {
        //        return LoginState.DBFAIL;
        //    }
        //    finally
        //    {

        //        //insertData(myConn);
        //        if (connection.State == ConnectionState.Open)
        //        {
        //            connection.Close();
        //        }

        //    }
        //}
    }
}
