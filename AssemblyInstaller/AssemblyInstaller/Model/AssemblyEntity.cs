using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssemblyInstaller.Helpers;
using AssemblyInstaller.Properties;
using GalaSoft.MvvmLight;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;

namespace AssemblyInstaller.Model
{
    public class AssemblyEntity : ObservableObject
    {
        public Guid Id { get; set; }
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

        [JsonIgnore]
        public long RepositroyVersion
        {
            get
            {
                return _repositroyVersion;
            }
            set
            {
                Set(() => RepositroyVersion, ref _repositroyVersion, value);
            }
        }
        [JsonIgnore]
        public long LocalVersion
        {
            get
            {
                return _localVersion;
            }
            set
            {
                Set(() => LocalVersion, ref _localVersion, value);
            }
        }
        [JsonIgnore]
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                Set(() => State, ref _state, value);
            }
        }
        [JsonIgnore]
        public string Rating
        {
            get
            {
                return Votes > 0 ? "/Images/star" + (Points / Votes) + ".png" : "/Images/star0.png";
            }
        }
        [JsonIgnore]
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                Set(() => IsChecked, ref _isChecked, value);
            }
        }
        [JsonIgnore]
        public bool IsInjected
        {
            get
            {
                return _isInjected;
            }
            set
            {
                Set(() => IsInjected, ref _isInjected, value);
            }
        }

        private bool _isInjected;
        private bool _isChecked;
        private string _state;
        private long _repositroyVersion;
        private long _localVersion;

        public ProjectFile GetProjectFile()
        {
            var file = Directory.GetFiles(Path.Combine(Config.RepositoriesDirectory, Developer, Repositroy, "trunk"), ProjectFile, SearchOption.AllDirectories).FirstOrDefault();

            if (file == null)
                return null;

            return new ProjectFile(file)
            {
                Configuration = "Release",
                PlatformTarget = "x86",
                ReferencesPath = Config.SystemDirectory,
                UpdateReferences = true,
                PostbuildEvent = true,
                PrebuildEvent = true
            };
        }

        public void Delete()
        {
            var path = "";

            if (OutputType == "Exe")
                path = Path.Combine(Config.AssembliesDirectory, AssemblyName + "-" + Developer + ".exe");

            if (OutputType == "Library")
                path = Path.Combine(Config.SystemDirectory, AssemblyName + "-" + Developer + ".dll");

            if (File.Exists(path))
                File.Delete(path);

            State = "Uninstalled";
        }

        public bool Compile()
        {
            try
            {
                State = "Compile";
                var project = GetProjectFile();

                if (project != null)
                {
                    var result = Github.Compile(project);

                    if (result != null && File.Exists(result))
                    {
                        LogFile.Write(Name, "Compile Sucsesfull - " + result);
                        State = "Updated";
                        return true;
                    }

                    LogFile.Write(Name, "Compile Failed - Compiler Error");
                    State = "Broken";
                    return false;
                }

                LogFile.Write(Name, "Project File not Found - " + ProjectFile);
                State = "Broken";
                return false;
            }
            catch (Exception e)
            {
                DialogService.ShowMessage("Compile - " + Name, e.ToString(), MessageDialogStyle.Affirmative);
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}", Name, Developer, Repositroy, Category, ProjectFile);
        }

        public bool Copy()
        {
            try
            {
                var file =
                    Directory.GetFiles(Path.Combine(Config.RepositoriesDirectory, Developer, Repositroy, "trunk"),
                    AssemblyName + OutputType == "Exe" ? ".exe" : ".dll", SearchOption.AllDirectories).FirstOrDefault();


                if (file != null && File.Exists(file))
                {
                    if (OutputType == "Library")
                    {
                        var newFile = Path.Combine(Config.SystemDirectory, AssemblyName + ".dll");
                        if (File.Exists(newFile))
                            File.Delete(newFile);
                        File.Move(file, newFile);

                        LogFile.Write(Name, "Move - " + file + " -> " + newFile);
                        return true;
                    }

                    if (OutputType == "Exe")
                    {
                        var newFile = Path.Combine(Config.AssembliesDirectory, AssemblyName + ".exe");
                        if (File.Exists(newFile))
                            File.Delete(newFile);
                        File.Move(file, newFile);

                        LogFile.Write(Name, "Move - " + file + " -> " + newFile);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                DialogService.ShowMessage("Copy", e.Message, MessageDialogStyle.Affirmative);
            }

            return false;
        }
    }
}
