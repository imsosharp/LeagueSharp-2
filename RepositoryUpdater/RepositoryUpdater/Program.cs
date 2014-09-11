using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpSvn;

namespace LSharpRepoUpdater
{
    class Program
    {
        static void Main()
        {
            Console.WindowWidth = 130;
            Console.WindowHeight = 50;
            Directory.CreateDirectory("Cache");

            Console.WriteLine("Starting Repository Update");

            using (var client = new WebClient())
            {
                var server = JsonConvert.DeserializeObject<List<AssemblyEntity>>(client.DownloadString("https://raw.githubusercontent.com/h3h3/LeagueSharp/master/RepositoryUpdater/RepositoryUpdater/Repository.json"));
                var list = new List<AssemblyEntity>();
                var repos = File.ReadAllLines("repos.txt");

                Console.WriteLine("{0,-30}{1,-20}{2,-25}{3,-15}{4}", "Name", "Developer", "Repositroy", "Category", "ProjectFile");

                //foreach (var url in repos)
                Parallel.ForEach(repos, url =>
                {
                    var match = Regex.Match(url, @"(?i:https://)(?<server>[^\s/]*)/(?<user>[^\s/]*)/(?<repo>[^\s/]*)");
                    var user = match.Groups["user"].Value;
                    var repo = match.Groups["repo"].Value;
                    var projects = UpdateRepo(url);

                    foreach (var project in projects)
                    {
                        var file = new FileInfo(project);
                        var fileContent = File.ReadAllText(file.FullName);

                        var regexOutputType = Regex.Match(fileContent, @"<OutputType>(?'OutputType'.*?)</OutputType>");
                        var outputType = regexOutputType.Groups["OutputType"].Value;

                        var regexAssemblyName = Regex.Match(fileContent, @"<AssemblyName>(?'AssemblyName'.*?)</AssemblyName>");
                        var assemblyName = regexAssemblyName.Groups["AssemblyName"].Value;

                        // MSBuild special chars
                        while (assemblyName.Contains("%"))
                        {
                            var data = assemblyName.Split('%');
                            var start = data[0];
                            var value = ConvertHexToString(data[1].Substring(0, 2));
                            var end = data[1].Substring(2);

                            for (var i = 2; i < data.Length; i++)
                            {
                                end += "%" + data[i];
                            }

                            assemblyName = string.Concat(start, value, end);
                        }

                        var old = server.FirstOrDefault(a => a.Developer == user && a.Repositroy == repo && a.Name == assemblyName);

                        var ass = new AssemblyEntity
                        {
                            Name = assemblyName,
                            Url = url,
                            Developer = user,
                            Repositroy = repo,
                            ProjectFile = file.Name,
                            AssemblyName = assemblyName,
                            OutputType = outputType,
                            Category = outputType == "Library" ? "Library" : ""
                        };

                        if (old != null)
                        {
                            ass.Category = old.Category;
                            ass.Description = old.Description;
                            ass.Points = old.Points;
                            ass.Votes = old.Votes;
                        }

                        if (ass.OutputType == "Exe" || ass.OutputType == "Library")
                        {
                            list.Add(ass);
                            Console.WriteLine(ass);
                        }
                    }
                });

                Console.WriteLine("Repo: " + repos.Length);
                Console.WriteLine("Assemblies: " + list.Count);
                Console.WriteLine("Type c for Champion");
                Console.WriteLine("Type u for Utility");

                foreach (var entity in list)
                {
                    if (string.IsNullOrEmpty(entity.Category))
                    {
                        Console.Write(entity.Name + " := ");
                        switch (Console.ReadKey().KeyChar)
                        {
                            case 'c':
                                entity.Category = "Champion";
                                break;
                            case 'u':
                                entity.Category = "Utility";
                                break;
                        }

                        Console.WriteLine(entity.Category);
                    }
                }

                File.WriteAllText("Repository.json", JsonConvert.SerializeObject(list, Formatting.Indented));

                Console.WriteLine("\nUpdate Complete");
                Console.WriteLine((list.Count - server.Count) + " Assemblies Changed");
            }

            Console.ReadLine();
        }

        public static string ConvertHexToString(string hexValue)
        {
            var strValue = "";
            while (hexValue.Length > 0)
            {
                strValue += Convert.ToChar(Convert.ToUInt32(hexValue.Substring(0, 2), 16)).ToString();
                hexValue = hexValue.Substring(2, hexValue.Length - 2);
            }
            return strValue;
        }

        private static IEnumerable<string> UpdateRepo(string url)
        {
            var match = Regex.Match(url, @"(?i:https://)(?<server>[^\s/]*)/(?<user>[^\s/]*)/(?<repo>[^\s/]*)");
            if (!match.Success && match.Groups["server"].Value == "github.com")
                throw new InvalidDataException("Invalid Github URL");

            var user = match.Groups["user"].Value;
            var repo = match.Groups["repo"].Value;
            var path = Path.Combine("Cache", user, repo);
            var trunkPath = Path.Combine("Cache", user, repo, "trunk");

            using (var svn = new SvnClient())
            {
                var cleanUp = false;
                svn.Conflict += (sender, args) => args.Choice = SvnAccept.TheirsFull;
                svn.Status(path, new SvnStatusArgs { ThrowOnError = false },
                    (sender, args) =>
                    {
                        if (args.Wedged)
                        {
                            cleanUp = true;
                        }
                    });

                if (cleanUp)
                {
                    svn.CleanUp(path);
                }
                svn.CheckOut(new Uri(url), path);
                svn.Update(path);
                //Console.WriteLine("Updated: " + url);
            }

            return Directory.GetFiles(trunkPath, "*.csproj", SearchOption.AllDirectories).ToList();
        }
    }

    public class AssemblyEntity
    {
        public string Name { get; set; }
        public string Repositroy { get; set; }
        public string Developer { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public int Votes { get; set; }
        public string ProjectFile { get; set; }
        public string AssemblyName { get; set; }
        public string OutputType { get; set; }

        public override string ToString()
        {
            return string.Format("{0,-30}{1,-20}{2,-25}{3,-15}{4}", Name, Developer, Repositroy, Category, ProjectFile);
        }
    }
}
