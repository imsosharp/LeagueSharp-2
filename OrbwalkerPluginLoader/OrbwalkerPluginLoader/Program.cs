using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.OrbwalkerPlugins;

namespace OrbwalkerPluginLoader
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Loading...");
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OrbwalkerPlugins"));
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OrbwalkerPlugins", "Cache"));

                var pluginRepositoryPath = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OrbwalkerPlugins", "Repository.txt"));
                var pluginLocalPath = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OrbwalkerPlugins", ObjectManager.Player.ChampionName + ".cs"));
                var pluginCachePath = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OrbwalkerPlugins", "Cache", ObjectManager.Player.ChampionName + ".dll"));
                var compiler = new PluginCompiler();

                // TODO: Version check / Autoupdate

                if (pluginCachePath.Exists)
                {
                    Game.PrintChat("Loading Local Plugin");
                    LoadAssembly(pluginCachePath);
                    return;
                }

                if (pluginLocalPath.Exists)
                {
                    if (pluginLocalPath.Exists)
                    {
                        Game.PrintChat("Loading Source Plugin");
                        Console.WriteLine("Start Plugin Compile: " + pluginLocalPath.Name);
                        compiler.Compile(pluginLocalPath);
                        LoadAssembly(pluginCachePath);
                        return;
                    }
                }

                if (pluginRepositoryPath.Exists)
                {
                    Game.PrintChat("Loading Repository Plugin");
                    var line = File.ReadAllLines(pluginRepositoryPath.FullName).First();
                    var uri = new Uri(line + "/OrbwalkerPlugins/" + ObjectManager.Player.ChampionName + ".cs");
                    Console.WriteLine("Start Plugin Compile: " + uri);

                    Task.Factory.StartNew(() =>
                    {
                        compiler.Compile(uri);
                        LoadAssembly(pluginCachePath);
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void LoadAssembly(FileInfo path)
        {
            Console.WriteLine("Loading Assembly into AppDomain");

            var assembly = Assembly.LoadFrom(path.FullName);

            foreach (var type in assembly.GetTypes())
            {
                if (typeof(OrbwalkerPluginBase).IsAssignableFrom(type) && type.IsClass)
                {
                    Console.WriteLine("Found Plugin: " + type.AssemblyQualifiedName);
                    var constructor = type.GetConstructor(new Type[] { });

                    if (constructor == null)
                        throw new InvalidOperationException("No default constructor for plugin found");

                    var plugin = (OrbwalkerPluginBase)constructor.Invoke(new object[] { });

                    if (plugin.ChampionName != ObjectManager.Player.ChampionName)
                    {
                        Console.WriteLine("Unload Plugin: " + type.AssemblyQualifiedName);
                        Console.WriteLine("ChampionName: {0} != {1}", ObjectManager.Player.ChampionName, plugin.ChampionName);
                        plugin.OnUnload(new EventArgs());
                        plugin = null;
                    }
                }
            }
        }
    }
}
