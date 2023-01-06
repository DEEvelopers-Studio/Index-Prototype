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
using System.Drawing;

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
        public static bool BitToBool(int? num) => num == 1;
        public static List<Subject> getSubjects() => getDocumentsInTable<Subject>("Subjects");
        public static List<User> getTeachers() => getDocumentsInTable<User>("Teachers");
        public static List<User> getStudents() => getDocumentsInTable<User>("Students");
        public static List<Student> getStudentsInSubject(string subjectId) => getDocumentsInTable<Student>(new SqlCommand($"select Students.* from Students LEFT JOIN StudentSubjectData ON Students.uid = StudentSubjectData.uid where StudentSubjectData.subjectId = '{subjectId}'"));
        public static User getStudent(string uid) => getDocument<User>(uid, "Students");
        public static Subject getSubject(string uid) => getDocument<Subject>(uid, "Subjects");
        public static User getTeacher(string uid) => getDocument<User>(uid, "Teachers");
        public enum UserType { Teacher ,Student};
        private static string[] _userType = { "Teachers", "Students" };
        public static User getUser(string uid,UserType userType) => getDocument<User>(uid, _userType[(int)userType]);

        public static List<T> getDocumentsInTable<T>(string Table, string query = "") where T : class, new() => getDocumentsInTable<T>(new SqlCommand($"SELECT * from {Table} {query}"));
        public static List<T> getDocumentsInTable<T>(SqlCommand command) where T : class, new()
        {
            List<T> items = new List<T>();
            try
            {
                connection.Open();
                command.Connection = connection;
                SqlDataReader sdr = command.ExecuteReader();
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
                if(!sdr.Read())return null;
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
        class SqllErrorNumbers
        {
            public const int BadObject = 208;
            public const int DupKey = 2627;
        }
        public static void AddStudent(Student student)
        {
            string table = "Students";
            try
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand() {Connection = connection, CommandText = $"insert into {table} (uid,firstName,lastName,middleName,section) values('{student.uid}','{student.firstName}','{student.lastName}','{student.middleName}','{student.section}')" };
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
            }
            catch (SqlException r)
            {
                switch (r.Number)
                {
                    case SqllErrorNumbers.DupKey:
                        MessageBox.Show("Duplicate Key");
                        break;
                    case SqllErrorNumbers.BadObject:
                        MessageBox.Show("Bad Obj");
                        break;
                    default:

                        MessageBox.Show("" + r);
                        break;
                }
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
        public static void AddStudenttoSubject(string studentUid,string subjectUid)
        {
            string table = "StudentSubjectData";
            try
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand() { Connection = connection, CommandText = $"insert into {table} (subjectId,uid) values('{studentUid}','{subjectUid}')" };
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
            }
            catch (SqlException r)
            {
                switch (r.Number)
                {
                    case SqllErrorNumbers.DupKey:
                        MessageBox.Show("Duplicate Key");
                        break;
                    case SqllErrorNumbers.BadObject:
                        MessageBox.Show("Bad Obj");
                        break;
                    default:

                        MessageBox.Show("" + r);
                        break;
                }
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
