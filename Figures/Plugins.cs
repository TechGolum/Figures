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
        static string folder = Directory.GetCurrentDirectory() + @"\Plugins";
        static string[] files = Directory.GetFiles(folder);
        static Assembly assembly;
        static List<Type[]> types = new List<Type[]>();
        static public List<Type[]> getClasses()
        {
            for (int i = 0; i < files.Length; i++)
            {
                assembly = Assembly.LoadFile(new FileInfo(files[i]).FullName);
                types.Add(assembly.GetTypes());
            }
            return types;
        }
        static public string[] getLibs()
        {
            string[] libs = new string[files.Length];
            for(int i = 0; i < libs.Length; i++)
            {
                libs[i] = new FileInfo(files[i]).Name.Replace(".dll", "");
            }
            return libs;
        }
    }
}
