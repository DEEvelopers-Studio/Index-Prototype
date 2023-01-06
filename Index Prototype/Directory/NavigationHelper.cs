using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

namespace Index_Prototype.Directory
{
    static class NavigationHelper
    {
        public static Dictionary<string, string> getParams(NavigationService service) => getParams(service.Source.ToString());
        public static Dictionary<string,string> getParams(string uri)
        {
            Dictionary<string, string> parsedData = new Dictionary<string, string>();
            string[] url = uri.Split(new[] { '?' }, 2);
            string[] split = url[1].Split(new[] {'&'});
            foreach (string item in split)
            {
                string[] data = item.Split(new[] { '=' }, 2);
                if (data.Length < 2) continue;
                parsedData.Add(data[0], data[1]);
            }
            return parsedData;
        }
    }
}
