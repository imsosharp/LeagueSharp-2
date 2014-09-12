using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AssemblyInstaller.Model;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using MahApps.Metro.Controls.Dialogs;
using SharpSvn;

namespace AssemblyInstaller.Helpers
{
    internal static class Github
    {
        public static List<AssemblyEntity> VersionCheck(IList<AssemblyEntity> list)
        {
            var updates = new List<AssemblyEntity>();

            Parallel.ForEach(list, assembly =>
            {
                try
                {
                    var old = assembly.State;
                    LogFile.Write(assembly.Name, "Version Check");
                    assembly.State = "Version Check";
                    assembly.LocalVersion = LocalVersion(assembly);
                    assembly.RepositroyVersion = RepositorieVersion(assembly);

                    if (assembly.LocalVersion == 0 || assembly.LocalVersion != assembly.RepositroyVersion)
                    {
                        updates.Add(assembly);
                    }

                    assembly.State = old;
                }
                catch (Exception e)
                {
                    DialogService.ShowMessage("VersionCheck - " + assembly.Name, e.Message, MessageDialogStyle.Affirmative);
                }
            });

            return updates;
        }

        public static void Download(IList<AssemblyEntity> list)
        {
            var complete = new List<String>();

            Parallel.ForEach(list, repository =>
            {
                try
                {
                    var download = false;

                    lock (complete)
                    {
                        if (!complete.Contains(repository.Url))
                        {
                            complete.Add(repository.Url);
                            download = true;
                        }
                    }

                    if (download)
                    {
                        var old = repository.State;
                        LogFile.Write(repository.Name, "Downloading");
                        repository.State = "Downloading";
                        Update(repository.Url);
                        repository.State = old;
                        repository.LocalVersion = LocalVersion(repository);
                    }
                }
                catch (Exception e)
                {
                    DialogService.ShowMessage("Download - " + repository.Name, e.Message, MessageDialogStyle.Affirmative);
                }
            });
        }

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
            try
            {
                if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Repositories", entity.Developer, entity.Repositroy)))
                    return 0;

                var workingCopyClient = new SvnWorkingCopyClient();
                SvnWorkingCopyVersion version;
                workingCopyClient.GetVersion(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Repositories", entity.Developer, entity.Repositroy), out version);
                return version.End;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static long RepositorieVersion(AssemblyEntity entity)
        {
            var client = new SvnClient();
            SvnInfoEventArgs info;
            try
            {
                client.GetInfo(new Uri(String.Format("https://github.com/{0}/{1}", entity.Developer, entity.Repositroy)), out info);
            }
            catch (Exception)
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

            try
            {
                while (zipEntry != null)
                {
                    var entryFileName = zipEntry.Name;
                    var buffer = new byte[4096];
                    var fullZipToPath = Path.Combine(outFolder, entryFileName);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);

                    if (!string.IsNullOrEmpty(directoryName))
                        Directory.CreateDirectory(directoryName);
                    if (zipEntry.IsFile)
                    {
                        using (var streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                        }
                    }

                    zipEntry = zipInputStream.GetNextEntry();
                }
            }
            catch (Exception e)
            {
                DialogService.ShowMessage("Unzip Stream", e.ToString(), MessageDialogStyle.Affirmative);
            }
        }
    }
}
