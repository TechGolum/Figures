using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Figures
{
    class Plugins
    {
        static FieldInfo fi;
        static string folder = Directory.GetCurrentDirectory() + @"\Plugins";
        static string[] files = Directory.GetFiles(folder);
        static Assembly assembly = Assembly.LoadFile(new FileInfo(files[0]).FullName);
        static Type[] types = assembly.GetTypes();
        static public Type[] getClasses()
        {
            return types;
        }
    }
}
