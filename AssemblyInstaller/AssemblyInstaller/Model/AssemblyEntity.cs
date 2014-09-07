using System;
using System.IO;
using System.Linq;
using AssemblyInstaller.Helpers;
using AssemblyInstaller.Properties;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace AssemblyInstaller.Model
{
    public class AssemblyEntity : ObservableObject
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
                return Votes > 0 ? "/Images/star" + (Points/Votes) + ".png" : "/Images/star0.png";
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

        private bool _isChecked;
        private string _state;
        private long _repositroyVersion;
        private long _localVersion;

        public ProjectFile GetProjectFile()
        {
            var file = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Repositories", Developer, Repositroy, "trunk"), ProjectFile, SearchOption.AllDirectories).FirstOrDefault();
            
            if (file == null)
                return null;

            return new ProjectFile(file)
            {
                Configuration = "Release",
                PlatformTarget = "x86",
                ReferencesPath = Path.Combine(Settings.Default.LeagueSharpPath, "Assemblies", "System"),
                UpdateReferences = true,
                PostbuildEvent = true,
                PrebuildEvent = true
            };
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}", Name, Developer, Repositroy, Category, ProjectFile);
        }
    }
}
