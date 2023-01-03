using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index_Prototype.Directory
{
    public static class DataTemplates 
    {

        public class UserListVM : User, INotifyPropertyChanged
        {
            public bool isSelected { get; set; } = false;

            public event PropertyChangedEventHandler PropertyChanged;
            public UserListVM(User user)
            {
                this.uid = user.uid;
                this.firstName = user.firstName;
                this.lastName = user.lastName;
                this.middleName = user.middleName;
            }
        }
        public class User
        {
            public enum NameType { FIRST, LAST, MIDDLE, COMMA };
            public string uid { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string middleName { get; set; }
            public string name { get { return getName(); }}
            public string profileLocation { get; set; }
            public string getName(NameType[] format)
            {

                string name = "";

                for (int i = 0; i < format.Length; i++)
                {
                    ///dont add space on start and when comma will be next
                    if (!(format.Length == 0 || format[i] == NameType.COMMA)) name += " ";
                    ///stitch the name according to the order
                    switch (format[i])
                    {
                        case NameType.FIRST:
                            name += firstName;
                            break;
                        case NameType.LAST:

                            name += lastName;
                            break;
                        case NameType.MIDDLE:

                            name += middleName;
                            break;

                        case NameType.COMMA:

                            name += ",";
                            break;
                    }
                }
                return name;
            }
            /// <summary>
            /// get the name of the account
            /// </summary>
            /// <returns>returns a name in LASTNAME, FIRSTNAME order</returns>
            public string getName()
            {
                return getName(new NameType[] { NameType.LAST, NameType.COMMA, NameType.FIRST });
            }
        }
    }
}
