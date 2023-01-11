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
using static Index_Prototype.Pages.Subject.Teacher_List.TeacherList;
using Index_Prototype.Directory;
using Microsoft.Win32;

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
        public static List<Subject> getSubjects() => getDocumentsInTable<Subject>(new SqlCommand($"select Subjects.* from Subjects LEFT JOIN TeacherSubjectData ON Subjects.id = TeacherSubjectData.subjectId where TeacherSubjectData.uid = '{AccountAuth.account.uid}'"));
        public static List<User> getTeachers() => getDocumentsInTable<User>("Teachers");
        public static List<User> getStudents() => getDocumentsInTable<User>("Students");
        public static List<Student> getStudentsInSubject(string subjectId) => getDocumentsInTable<Student>(new SqlCommand($"select Students.*,ISNULL(Attendance.status, -1) as attendanceStatus from Students LEFT JOIN StudentSubjectData ON Students.uid = StudentSubjectData.uid LEFT JOIN Attendance ON Students.uid=Attendance.studentId AND Attendance.date = CAST(GETDATE() as DATE) AND Attendance.subjectId = '{subjectId}'  where StudentSubjectData.subjectId = '{subjectId}'  Order By lastName ASC"));
        public static List<Student> getTeachersInSubject(string subjectId) => getDocumentsInTable<Student>(new SqlCommand($"select Teachers.uid, Teachers.firstName,Teachers.lastName,Teachers.middleName,Teachers.lastName from Teachers LEFT JOIN TeacherSubjectData ON Teachers.uid = TeacherSubjectData.uid where TeacherSubjectData.subjectId = '{subjectId}' Order By Teachers.lastName ASC"));
        public static User getStudent(string uid) => getDocument<User>(uid, "Students","uid");
        public static Subject getSubject(string uid) => getDocument<Subject>(uid, "Subjects");
        public static User getTeacher(string uid) => getDocument<User>(uid, "Teachers", "uid");
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
        public static T getDocument<T>(string id, string Table,string where = "id") where T : class, new()
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * from {Table} where {where}='{id}'", connection);
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
        public static void ExportAttendance(Subject subject)
        {  
            SaveFileDialog saveFileDialog= new SaveFileDialog() { Filter="CSV File|*.csv",Title=$"Export {subject.title} Data",FileName = $"{subject.title}-{DateTime.Now:MMM-dd-yyyy}-{subject?.section}"};
            if (saveFileDialog.ShowDialog() == false) return ;
            SQLToCSV($@"select Students.*,ISNULL(PRESENT,0) AS PRESENT,ISNULL(LATE,0) AS LATE,ISNULL(ABSENT,0) AS ABSENT from Students 
LEFT JOIN StudentSubjectData ON Students.uid = StudentSubjectData.uid Left Join (SELECT COUNT(CASE WHEN status = 0 THEN 1 END) as PRESENT,COUNT(CASE WHEN status = 1 THEN 1 END) as LATE,COUNT(CASE WHEN status = 2 THEN 1 END) as ABSENT, 
studentId FROM Attendance WHERE subjectId = '{subject.id}' GROUP BY studentId) att on Students.uid = att.studentId", saveFileDialog.FileName);
        }
        private static void SQLToCSV(string query, string Filename)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dr = cmd.ExecuteReader();

                using (System.IO.StreamWriter fs = new System.IO.StreamWriter(Filename))
                {
                    // Loop through the fields and add headers
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string name = dr.GetName(i);
                        if (name.Contains(","))
                            name = "\"" + name + "\"";

                        fs.Write(name + ",");
                    }
                    fs.WriteLine();

                    // Loop through the rows and output the data
                    while (dr.Read())
                    {
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(","))
                                value = " \"" + value + "\"";

                            fs.Write($"=\"{value}\"" + ",");
                        }
                        fs.WriteLine();
                    }

                    fs.Close(); dr.Close(); connection.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public static void PutStudent(Student student)
        {
            string table = "Students";
            try
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand() {Connection = connection, CommandText = $@"IF EXISTS (SELECT * FROM {table} WHERE uid = '{student.uid}')
 BEGIN
     UPDATE {table} SET firstName = '{student.firstName}', middleName = '{student.middleName}',lastName = '{student.lastName}', section = '{student.section}' WHERE uid = '{student.uid}';
      RETURN
 END
INSERT INTO {table} (uid,firstName,middleName,lastName,section) VALUES ('{student.uid}','{student.firstName}','{student.middleName}','{student.lastName}','{student.section}')
" };
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                sqlrdr.Close();
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
        public static void PutAttendance(Student student,string subjectId)
        {
            string table = "Attendance";
            try
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand() { Connection = connection, CommandText = $@"IF EXISTS (SELECT * FROM {table} WHERE studentId = '{student.uid}' AND date = CAST(GETDATE() as DATE) AND subjectId = '{subjectId}')
 BEGIN
     UPDATE {table} SET subjectId = '{subjectId}', date = GETDATE() ,status = {(int)student.attendanceStatus}, studentId = '{student.uid}' WHERE studentId = '{student.uid}' AND date = CAST(GETDATE() as DATE) AND subjectId = '{subjectId}'
      RETURN
 END
INSERT INTO {table} (studentId,date,status,subjectId) VALUES ('{student.uid}',GETDATE(),{(int)student.attendanceStatus},'{subjectId}')
" };
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                sqlrdr.Close();
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
        public static void PutTeacher(User teacher,string password)
        {
            string table = "Teachers";
            try
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand() { Connection = connection, CommandText = $@"IF EXISTS (SELECT * FROM {table} WHERE uid = '{teacher.uid}')
 BEGIN
     UPDATE {table}  SET firstName = '{teacher.firstName}', middleName = '{teacher.middleName}',lastName = '{teacher.lastName}', password = '{password}' WHERE uid = '{teacher.uid}';
      RETURN
 END
INSERT INTO {table}  (uid,firstName,middleName,lastName,password) VALUES ('{teacher.uid}','{teacher.firstName}','{teacher.middleName}','{teacher.lastName}','{password}')
" };
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                sqlrdr.Close();
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
        public static void PutSubject(Subject subject)
        {
            string table = "Subjects";
            try
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand() { Connection = connection, CommandText = $@"IF EXISTS (SELECT * FROM {table} WHERE id = '{subject.id}')
      BEGIN UPDATE {table} SET title='{subject.title}',section='{subject.section}',defaultStudentSelection={subject.defaultStudentSelection},attendanceOnStart='{subject.attendanceOnStart}' WHERE id = '{subject.id}';
      RETURN END INSERT INTO {table} (id,title,section,defaultStudentSelection,attendanceOnStart) VALUES ('{subject.id}','{subject.title}','{subject.section}','{subject.defaultStudentSelection}','{subject.attendanceOnStart}')" };
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                sqlrdr.Close();
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
                SqlCommand sqlcmd = new SqlCommand() { Connection = connection, CommandText = $"IF EXISTS (SELECT * FROM {table} WHERE uid = '{studentUid}') BEGIN RETURN END insert into {table} (subjectId,uid) values('{subjectUid}','{studentUid}')" };
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                sqlrdr.Close();
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
        public static void AddTeachertoSubject(string teacherUid,string subjectId)
        {
            string table = "TeacherSubjectData";
            try
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand() { Connection = connection, CommandText = $"insert into {table} (subjectId,uid) values('{subjectId}','{teacherUid}')" };
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                sqlrdr.Close();
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

        public static void RemoveTeachertoSubject(string teacherUid, string subjectId)
        {
            string table = "TeacherSubjectData";
            try
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand() { Connection = connection, CommandText = $"DELETE FROM {table} WHERE uid = '{teacherUid}' AND subjectId = '{subjectId}'" };
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                sqlrdr.Close();
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
        public static void RemoveStudenttoSubject(string studentUid, string subjectId)
        {
            string table = "StudentSubjectData";
            try
            {
                connection.Open();
                SqlCommand sqlcmd = new SqlCommand() { Connection = connection, CommandText = $"DELETE FROM {table} WHERE uid = '{studentUid}' AND subjectId = '{subjectId}'" };
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                sqlrdr.Close();
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
