using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Net;
using System.Text;
using LeagueSharp;
using Microsoft.CSharp;

namespace OrbwalkerPluginLoader
{
    internal class PluginCompiler
    {
        private readonly CSharpCodeProvider _provider = new CSharpCodeProvider();
        private readonly CompilerParameters _parameters = new CompilerParameters();

        public PluginCompiler()
        {
            _parameters.ReferencedAssemblies.Add("System.dll");
            _parameters.ReferencedAssemblies.Add("System.Core.dll");
            _parameters.ReferencedAssemblies.Add("System.Data.dll");
            _parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            _parameters.ReferencedAssemblies.Add("System.Reflection.dll");
            _parameters.ReferencedAssemblies.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"LeagueSharp.dll"));
            _parameters.ReferencedAssemblies.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"LeagueSharp.Common.dll"));
            _parameters.ReferencedAssemblies.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"OrbwalkerPlugin.dll"));
            _parameters.ReferencedAssemblies.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"SharpDX.dll"));
            _parameters.ReferencedAssemblies.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"SharpDX.Toolkit.dll"));
            _parameters.ReferencedAssemblies.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"SharpDX.Toolkit.Graphics.dll"));
            _parameters.ReferencedAssemblies.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"SharpDX.Direct3D9.dll"));

            _parameters.GenerateInMemory = false;
            _parameters.GenerateExecutable = false;
            _parameters.OutputAssembly = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OrbwalkerPlugins", "Cache", ObjectManager.Player.ChampionName + ".dll");
        }

        public CompilerResults Compile(Uri uri)
        {
            using (var client = new WebClient())
            {
                return Compile(client.DownloadString(uri));
            }
        }

        public CompilerResults Compile(FileInfo file)
        {
            return Compile(File.ReadAllText(file.FullName));
        }

        public CompilerResults Compile(string code)
        {
            var result = _provider.CompileAssemblyFromSource(_parameters, code);

            if (result.Errors.HasErrors)
            {
                var error = new StringBuilder();
                foreach (CompilerError e in result.Errors)
                {
                    error.AppendLine(string.Format("Compile Error ({0}): {1}", e.ErrorNumber, e.ErrorText));
                }
                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OrbwalkerPlugins", "Compiler.log"), error.ToString());

                throw new InvalidOperationException(error.ToString());
            }

            return result;
        }
    }
}
