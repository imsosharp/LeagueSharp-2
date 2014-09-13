using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using Newtonsoft.Json;

namespace AssemblyInstaller.Helpers
{
    public class Config
    {
        public static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string ConfigDirectory = Path.Combine(BaseDirectory, "Config");
        public static string LogDirectory = Path.Combine(BaseDirectory, "Log");
        public static string UpdateDirectory = Path.Combine(BaseDirectory, "Update");
        public static string RepositoriesDirectory = Path.Combine(BaseDirectory, "Repositories");
        public static string AssembliesDirectory = Path.Combine(BaseDirectory, "Assemblies");
        public static string SystemDirectory = Path.Combine(BaseDirectory, "Assemblies", "System");
        public static string SpritesDirectory = Path.Combine(BaseDirectory, "Assemblies", "Sprites");

        public static string LSharpConfigFile = Path.Combine(BaseDirectory, "Config", "LSharp.json");
        public static string InstallerConfigFile = Path.Combine(BaseDirectory, "Config", "Config.json");

        public static LSharpConfig LSharpConfig;
        public static InstallerConfig InstallerConfig;

        static Config()
        {
            if (!Directory.Exists(ConfigDirectory))
                Directory.CreateDirectory(ConfigDirectory);

            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);

            if (!Directory.Exists(RepositoriesDirectory))
                Directory.CreateDirectory(RepositoriesDirectory);

            if (!Directory.Exists(AssembliesDirectory))
                Directory.CreateDirectory(AssembliesDirectory);

            if (!Directory.Exists(SystemDirectory))
                Directory.CreateDirectory(SystemDirectory);

            if (!Directory.Exists(SpritesDirectory))
                Directory.CreateDirectory(SpritesDirectory);

            try
            {
                LSharpConfig = File.Exists(LSharpConfigFile) ?
                    JsonConvert.DeserializeObject<LSharpConfig>(File.ReadAllText(LSharpConfigFile)) :
                    new LSharpConfig
                    {
                        Afk = true,
                        Debug = false,
                        Tower = "None",
                        Zoom = false,
                        Version = new Version(0, 0, 0, 0)
                    };

                InstallerConfig = File.Exists(InstallerConfigFile) ?
                    JsonConvert.DeserializeObject<InstallerConfig>(File.ReadAllText(InstallerConfigFile)) :
                    new InstallerConfig();

                Console.WriteLine(SystemDirectory);
            }
            catch (Exception e)
            {
                LogFile.Write("Config", e.Message);
            }
        }

        public static void Save()
        {
            File.WriteAllText(LSharpConfigFile, JsonConvert.SerializeObject(LSharpConfig, Formatting.Indented));
            File.WriteAllText(InstallerConfigFile, JsonConvert.SerializeObject(InstallerConfig, Formatting.Indented));
        }
    }

    public class LSharpConfig
    {
        public bool Debug { get; set; }
        public bool Afk { get; set; }
        public bool Zoom { get; set; }
        public string Tower { get; set; }
        public Version Version { get; set; }
    }

    public class InstallerConfig
    {
        public List<string> Injected { get; set; }
        public List<Guid> Installed { get; set; }

        public InstallerConfig()
        {
            Injected = new List<string>();
            Installed = new List<Guid>();
        }
    }
}
