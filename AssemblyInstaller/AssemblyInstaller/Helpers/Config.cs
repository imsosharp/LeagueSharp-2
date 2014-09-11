using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyInstaller.Helpers
{
    public class Config
    {
        public static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string RepositoriesDirectory = Path.Combine(BaseDirectory, "Repositories");
        public static string AssembliesDirectory = Path.Combine(BaseDirectory, "Assemblies");
        public static string SystemDirectory = Path.Combine(BaseDirectory, "Assemblies", "System");
        public static string SpritesDirectory = Path.Combine(BaseDirectory, "Assemblies", "Sprites");
    }
}
