using System;
using System.IO;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using LSharpAssemblyProvider.Model;
using SharpSvn;

namespace LSharpAssemblyProvider.Helpers
{
    internal static class Github
    {
        public static void Update(string url)
        {
            var match = Regex.Match(url, @"(?i:https://)(?<server>[^\s/]*)/(?<user>[^\s/]*)/(?<repo>[^\s/]*)");
            if (!match.Success && match.Groups["server"].Value == "github.com")
                throw new InvalidDataException("Invalid Github URL");

            var user = match.Groups["user"].Value;
            var repo = match.Groups["repo"].Value;
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Repositories", user, repo);
            Directory.CreateDirectory(dir);

            using (var client = new SvnClient())
            {
                var cleanUp = false;

                client.Conflict += (sender, args) => args.Choice = SvnAccept.TheirsFull;
                client.Status(dir, new SvnStatusArgs { ThrowOnError = false },
                    (sender, args) =>
                    {
                        if (args.Wedged)
                        {
                            cleanUp = true;
                        }
                    });

                if (cleanUp)
                {
                    client.CleanUp(dir);
                }

                client.CheckOut(new Uri(String.Format("https://github.com/{0}/{1}", user, repo)), dir);
                client.Update(dir);
            }
        }

        public static long LocalVersion(AssemblyEntity entity)
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Repositories", entity.Developer, entity.Repositroy)))
                return 0;

            var workingCopyClient = new SvnWorkingCopyClient();
            SvnWorkingCopyVersion version;
            workingCopyClient.GetVersion(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Repositories", entity.Developer, entity.Repositroy), out version);
            return version.End;
        }

        public static long RepositorieVersion(AssemblyEntity entity)
        {
            var client = new SvnClient();
            SvnInfoEventArgs info;
            try
            {
                client.GetInfo(new Uri(String.Format("https://github.com/{0}/{1}", entity.Developer, entity.Repositroy)), out info);
            }
            catch (Exception e)
            {
                return 0;
            }
            return info.Revision;
        }

        public static string Compile(ProjectFile project)
        {
            project.Change();

            if (project.Project.Build())
            {
                Console.WriteLine("Compile: " + project.Project.FullPath + " - OK");
                return project.GetOutputFilePath();
            }
            else
            {
                Console.WriteLine("Compile: " + project.Project.FullPath + " - FAILED");
                return null;
            }
        }

        public static void UnzipStream(Stream zipStream, string outFolder)
        {
            var zipInputStream = new ZipInputStream(zipStream);
            var zipEntry = zipInputStream.GetNextEntry();

            while (zipEntry != null)
            {
                var entryFileName = zipEntry.Name;
                var buffer = new byte[4096];
                var fullZipToPath = Path.Combine(outFolder, entryFileName);
                var directoryName = Path.GetDirectoryName(fullZipToPath);

                if (!string.IsNullOrEmpty(directoryName))
                    Directory.CreateDirectory(directoryName);

                using (var streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                }

                zipEntry = zipInputStream.GetNextEntry();
            }
        }
    }
}
