using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AssemblyInstaller.Helpers;
using AssemblyInstaller.Model;
using AssemblyInstaller.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

namespace AssemblyInstaller.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="LeagueSharpPath" /> property's name.
        /// </summary>
        public const string LeagueSharpPathPropertyName = "LeagueSharpPath";

        /// <summary>
        /// Sets and gets the LeagueSharpPath property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LeagueSharpPath
        {
            get
            {
                return Settings.Default.LeagueSharpPath;
            }

            set
            {
                if (Settings.Default.LeagueSharpPath == value)
                {
                    return;
                }

                RaisePropertyChanging(() => LeagueSharpPath);
                Settings.Default.LeagueSharpPath = value;
                RaisePropertyChanged(() => LeagueSharpPath);
            }
        }

        /// <summary>
        /// The <see cref="StartPage" /> property's name.
        /// </summary>
        public const string StartPagePropertyName = "StartPage";

        private int _startPage = 3;

        /// <summary>
        /// Sets and gets the StartPage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int StartPage
        {
            get
            {
                return _startPage;
            }
            set
            {
                Set(() => StartPage, ref _startPage, value);
            }
        }


        /// <summary>
        /// The <see cref="Champion" /> property's name.
        /// </summary>
        public const string ChampionPropertyName = "Champion";

        private ObservableCollectionEx<AssemblyEntity> _champion;

        /// <summary>
        /// Sets and gets the Data property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollectionEx<AssemblyEntity> Champion
        {
            get
            {
                return _champion;
            }
            set
            {
                Set(() => Champion, ref _champion, value);
            }
        }


        /// <summary>
        /// The <see cref="Utility" /> property's name.
        /// </summary>
        public const string UtilityPropertyName = "Utility";

        private ObservableCollectionEx<AssemblyEntity> _utility;

        /// <summary>
        /// Sets and gets the Data property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollectionEx<AssemblyEntity> Utility
        {
            get
            {
                return _utility;
            }
            set
            {
                Set(() => Utility, ref _utility, value);
            }
        }


        /// <summary>
        /// The <see cref="Library" /> property's name.
        /// </summary>
        public const string LibraryPropertyName = "Library";

        private ObservableCollectionEx<AssemblyEntity> _library;

        /// <summary>
        /// Sets and gets the Data property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollectionEx<AssemblyEntity> Library
        {
            get
            {
                return _library;
            }
            set
            {
                Set(() => Library, ref _library, value);
            }
        }

        /// <summary>
        /// The <see cref="Progress" /> property's name.
        /// </summary>
        public const string ProgressPropertyName = "Progress";

        private long _progress;

        /// <summary>
        /// Sets and gets the Progress property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public long Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                Set(() => Progress, ref _progress, value);
            }
        }

        /// <summary>
        /// The <see cref="ProgressMax" /> property's name.
        /// </summary>
        public const string ProgressMaxPropertyName = "ProgressMax";

        private long _progressMax = 100;

        /// <summary>
        /// Sets and gets the Progress property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public long ProgressMax
        {
            get
            {
                return _progressMax;
            }
            set
            {
                Set(() => ProgressMax, ref _progressMax, value);
            }
        }

        /// <summary>
        /// The <see cref="Update" /> property's name.
        /// </summary>
        public const string UpdatePropertyName = "Update";

        private ObservableCollectionEx<AssemblyEntity> _update;

        /// <summary>
        /// Sets and gets the Data property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollectionEx<AssemblyEntity> Update
        {
            get
            {
                return _update;
            }
            set
            {
                Set(() => Update, ref _update, value);
            }
        }

        /// <summary>
        /// The <see cref="SelectedAssembly" /> property's name.
        /// </summary>
        public const string SelectedAssemblyPropertyName = "SelectedAssembly";

        private AssemblyEntity _selectedAssembly;

        /// <summary>
        /// Sets and gets the SelectedAssembly property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AssemblyEntity SelectedAssembly
        {
            get
            {
                return _selectedAssembly;
            }
            set
            {
                Set(() => SelectedAssembly, ref _selectedAssembly, value);
            }
        }

        /// <summary>
        /// The <see cref="Log" /> property's name.
        /// </summary>
        public const string LogPropertyName = "Log";

        private ObservableCollectionEx<LogEntity> _log;

        /// <summary>
        /// Sets and gets the Log property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollectionEx<LogEntity> Log
        {
            get
            {
                return _log;
            }
            set
            {
                Set(() => Log, ref _log, value);
            }
        }

        /// <summary>
        /// The <see cref="IsOverlay" /> property's name.
        /// </summary>
        public const string IsOverlayPropertyName = "IsOverlay";

        private bool _isOverlay = false;

        /// <summary>
        /// Sets and gets the LockInterface property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsOverlay
        {
            get
            {
                return _isOverlay;
            }
            set
            {
                Set(() => IsOverlay, ref _isOverlay, value);
            }
        }

        /// <summary>
        /// The <see cref="OverlayText" /> property's name.
        /// </summary>
        public const string OverlayTextPropertyName = "OverlayText";

        private string _overlayText = "";

        /// <summary>
        /// Sets and gets the OverlayText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string OverlayText
        {
            get
            {
                return _overlayText;
            }
            set
            {
                Set(() => OverlayText, ref _overlayText, value);
            }
        }

        private RelayCommand<AssemblyEntity> _checkAssemblyCommand;

        /// <summary>
        /// Gets the CheckAssemblyCommand.
        /// </summary>
        public RelayCommand<AssemblyEntity> CheckAssemblyCommand
        {
            get
            {
                return _checkAssemblyCommand
                    ?? (_checkAssemblyCommand = new RelayCommand<AssemblyEntity>(
                                          p =>
                                          {
                                              if (p == null)
                                              {
                                                  var collection = new ObservableCollectionEx<AssemblyEntity>();
                                                  switch (StartPage)
                                                  {
                                                      case 0:
                                                          collection = Champion;
                                                          break;
                                                      case 1:
                                                          collection = Utility;
                                                          break;
                                                      case 2:
                                                          collection = Library;
                                                          break;
                                                  }

                                                  foreach (var c in collection)
                                                  {
                                                      c.IsChecked = true;
                                                      _multiSelect.Add(c);
                                                  }
                                              }
                                              else
                                              {
                                                  if (!_multiSelect.Contains(p))
                                                  {
                                                      p.IsChecked = true;
                                                      _multiSelect.Add(p);
                                                  }
                                              }

                                              _multiSelect = new ObservableCollectionEx<AssemblyEntity>(_multiSelect.Distinct());
                                          }));
            }
        }

        private RelayCommand<AssemblyEntity> _uncheckAssemblyCommand;

        /// <summary>
        /// Gets the UncheckAssemblyCommand.
        /// </summary>
        public RelayCommand<AssemblyEntity> UncheckAssemblyCommand
        {
            get
            {
                return _uncheckAssemblyCommand
                    ?? (_uncheckAssemblyCommand = new RelayCommand<AssemblyEntity>(
                                          p =>
                                          {
                                              if (p == null)
                                              {
                                                  var collection = new ObservableCollectionEx<AssemblyEntity>();
                                                  switch (StartPage)
                                                  {
                                                      case 0:
                                                          collection = Champion;
                                                          break;
                                                      case 1:
                                                          collection = Utility;
                                                          break;
                                                      case 2:
                                                          collection = Library;
                                                          break;
                                                  }

                                                  foreach (var c in collection)
                                                  {
                                                      c.IsChecked = false;
                                                      _multiSelect.Remove(c);
                                                  }
                                              }
                                              else
                                              {
                                                  if (_multiSelect.Contains(p))
                                                      _multiSelect.Remove(p);
                                              }

                                              _multiSelect = new ObservableCollectionEx<AssemblyEntity>(_multiSelect.Distinct());
                                          }));
            }
        }

        private RelayCommand _installCommand;

        /// <summary>
        /// Gets the InstallCommand.
        /// </summary>
        public RelayCommand InstallCommand
        {
            get
            {
                return _installCommand
                    ?? (_installCommand = new RelayCommand(
                                          () =>
                                          {
                                              if (SelectedAssembly != null)
                                              {
                                                  InstallAssembly(SelectedAssembly);
                                              }
                                          }));
            }
        }

        private RelayCommand _installSelectedCommand;

        /// <summary>
        /// Gets the InstallCommand.
        /// </summary>
        public RelayCommand InstallSelectedCommand
        {
            get
            {
                return _installSelectedCommand
                    ?? (_installSelectedCommand = new RelayCommand(InstallSelectedAssembly));
            }
        }

        private RelayCommand _deleteCommand;

        /// <summary>
        /// Gets the DeleteCommand.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand
                    ?? (_deleteCommand = new RelayCommand(
                                          () =>
                                          {
                                              if (SelectedAssembly != null)
                                              {
                                                  var path = "";

                                                  if (SelectedAssembly.OutputType == "Exe")
                                                      path = Path.Combine(Settings.Default.LeagueSharpPath, "Assemblies", SelectedAssembly.Developer + "-" + SelectedAssembly.AssemblyName + ".exe");

                                                  if (SelectedAssembly.OutputType == "Library")
                                                      path = Path.Combine(Settings.Default.LeagueSharpPath, "Assemblies", "System", SelectedAssembly.AssemblyName + ".dll");

                                                  if (File.Exists(path))
                                                      File.Delete(path);

                                                  SelectedAssembly.State = "";
                                              }
                                          }));
            }
        }

        private RelayCommand _updateCommand;

        /// <summary>
        /// Gets the UpdateCommand.
        /// </summary>
        public RelayCommand UpdateCommand
        {
            get
            {
                return _updateCommand
                       ?? (_updateCommand = new RelayCommand(UpdateAssembly));

            }
        }

        private RelayCommand _saveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                    ?? (_saveCommand = new RelayCommand(() =>
                                          {
                                              var dia = new OpenFileDialog
                                              {
                                                  DefaultExt = ".exe",
                                                  Filter = "LeagueSharp.Loader.exe|*.exe"
                                              };
                                              var result = dia.ShowDialog();

                                              if (result == true)
                                              {
                                                  if (dia.FileName.EndsWith("LeagueSharp.Loader.exe"))
                                                  {
                                                      LeagueSharpPath = dia.FileName.Replace("LeagueSharp.Loader.exe", "");
                                                      Cleanup();
                                                      StartPage = 3;
                                                      UpdateLoaderPath();
                                                  }
                                                  else
                                                  {
                                                      DialogService.ShowMessage("Error", "LeagueSharp.Loader.exe not found @ " + dia.FileName, MessageDialogStyle.Affirmative);
                                                  }
                                              }
                                          }));
            }
        }

        private ObservableCollectionEx<AssemblyEntity> _multiSelect = new ObservableCollectionEx<AssemblyEntity>();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService service)
        {
            if (!IsInDesignMode)
                IsOverlay = true;

            if (string.IsNullOrEmpty(Settings.Default.LeagueSharpPath))
            {
                LeagueSharpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LSharp");
                Cleanup();
            }

            Task.Factory.StartNew(() =>
            {
                try
                {
                    OverlayText = "Loading";
                    ApplicationUpdate();

                    while (!service.IsInitComplete())
                        Thread.Sleep(100);

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        Champion = service.GetChampionData();
                        Utility = service.GetUtilityData();
                        Library = service.GetLibraryData();
                        Log = service.GetLogData();
                        Update = new ObservableCollectionEx<AssemblyEntity>();

                        var sort = new SortDescription("Name", ListSortDirection.Ascending);
                        var v = CollectionViewSource.GetDefaultView(Champion);
                        v.GroupDescriptions.Add(new PropertyGroupDescription("State"));
                        v.SortDescriptions.Add(sort);
                        v = CollectionViewSource.GetDefaultView(Utility);
                        v.GroupDescriptions.Add(new PropertyGroupDescription("State"));
                        v.SortDescriptions.Add(sort);
                        v = CollectionViewSource.GetDefaultView(Library);
                        v.GroupDescriptions.Add(new PropertyGroupDescription("State"));
                        v.SortDescriptions.Add(sort);
                        v = CollectionViewSource.GetDefaultView(Update);
                        v.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                        v.SortDescriptions.Add(sort);
                    });

                    UpdateLoaderPath();
                }
                catch (Exception e)
                {
                    DialogService.ShowMessage("Error", e.ToString(), MessageDialogStyle.Affirmative);
                }

                LogFile.Write("App", "Init Complete");
            });
        }

        private void UpdateLoaderPath()
        {
            // TODO: stays cancer for now
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                IsOverlay = true;
                Update.Clear();

                using (var update = Update.DelayNotifications())
                {
                    foreach (var lib in Library)
                    {
                        lib.LocalVersion = Github.LocalVersion(lib);

                        if (File.Exists(Path.Combine(Settings.Default.LeagueSharpPath, "Assemblies", "System", lib.Name + ".dll")))
                        {
                            lib.State = "Installed";
                            update.Add(lib);
                        }
                        else
                        {
                            lib.State = "Available";
                        }

                        Progress++;
                        OverlayText = string.Format("{0}/{1} Loading", Progress, ProgressMax);
                    }


                    foreach (var util in Utility)
                    {
                        util.LocalVersion = Github.LocalVersion(util);
                        var old = Path.Combine(Settings.Default.LeagueSharpPath, "Assemblies", util.Developer + "-" + util.AssemblyName + ".exe");
                        var newName = Path.Combine(Settings.Default.LeagueSharpPath, "Assemblies", util.AssemblyName + " - " + util.Developer + ".exe");

                        if (File.Exists(old) && !File.Exists(newName))
                            File.Move(old, newName);

                        if (File.Exists(Path.Combine(Settings.Default.LeagueSharpPath, "Assemblies", util.AssemblyName + " - " + util.Developer + ".exe")))
                        {
                            util.State = "Installed";
                            update.Add(util);
                        }
                        else
                        {
                            util.State = "Available";
                        }

                        Progress++;
                        OverlayText = string.Format("{0}/{1} Loading", Progress, ProgressMax);
                    }

                    foreach (var champ in Champion)
                    {
                        champ.LocalVersion = Github.LocalVersion(champ);
                        var old = Path.Combine(Settings.Default.LeagueSharpPath, "Assemblies", champ.Developer + "-" + champ.AssemblyName + ".exe");
                        var newName = Path.Combine(Settings.Default.LeagueSharpPath, "Assemblies", champ.AssemblyName + " - " + champ.Developer + ".exe");

                        if (File.Exists(old) && !File.Exists(newName))
                            File.Move(old, newName);

                        if (File.Exists(newName))
                        {
                            champ.State = "Installed";
                            update.Add(champ);
                        }
                        else
                        {
                            champ.State = "Available";
                        }

                        Progress++;
                        OverlayText = string.Format("{0}/{1} Loading", Progress, ProgressMax);
                    }
                }

                Update = new ObservableCollectionEx<AssemblyEntity>(Update.OrderBy(a => a.Name));
                CollectionViewSource.GetDefaultView(Update).Refresh();
                CollectionViewSource.GetDefaultView(Champion).Refresh();
                CollectionViewSource.GetDefaultView(Utility).Refresh();
                CollectionViewSource.GetDefaultView(Library).Refresh();
                IsOverlay = false;
                LogFile.Write("App", Update.Count + " Installed Assemblies found");
            });
        }

        public override void Cleanup()
        {
            Console.WriteLine("Save: " + LeagueSharpPath);
            Settings.Default.LeagueSharpPath = LeagueSharpPath;
            Settings.Default.Save();
        }

        private async void ApplicationUpdate()
        {
            const string versionUrl = "https://dl.dropboxusercontent.com/u/54555251/Version.txt";
            const string lSharpVersionUrl = "https://dl.dropboxusercontent.com/u/54555251/LSharpVersion.txt";

            try
            {
                Progress = 0;
                ProgressMax = 6;
                OverlayText = string.Format("{0}/{1} Update Check", Progress, ProgressMax);

                // Update cleanup
                if (Directory.Exists("Update"))
                {
                    Directory.Delete("Update", true);

                    if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LSharpAssemblyProvider.exe")))
                        File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LSharpAssemblyProvider.exe"));

                    if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LSharpAssemblyProvider.exe.config")))
                        File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LSharpAssemblyProvider.exe.config"));
                }


                var result = new Dictionary<string, string> { { versionUrl, "" }, { lSharpVersionUrl, "" } };

                try
                {
                    Parallel.ForEach(result, url =>
                    {

                        using (var client = new WebClient())
                        {
                            lock (result)
                            {
                                result[url.Key] = client.DownloadString(url.Key);
                            }
                        }

                        Progress++;
                    });
                }
                catch
                {
                }

                using (var client = new WebClient())
                {
                    var localInstallerVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    var installerVersion = result[versionUrl];
                    var localLsharpVersion = "";
                    var lsharpVersion = result[lSharpVersionUrl];

                    if (File.Exists(Path.Combine("LSharp", "Assemblies", "Version.txt")))
                        localLsharpVersion = File.ReadAllText(Path.Combine("LSharp", "Assemblies", "Version.txt"));

                    Progress++;

                    if (string.IsNullOrEmpty(installerVersion))
                    {
                        Console.WriteLine("Update Version not found!");
                        return;
                    }

                    if (string.IsNullOrEmpty(lsharpVersion))
                    {
                        Console.WriteLine("LSharp Update Version not found!");
                        return;
                    }

                    if (!Directory.Exists("LSharp") || lsharpVersion != localLsharpVersion)
                    {
                        OverlayText = string.Format("{0}/{1} Downloading LeagueSharp " + lsharpVersion, Progress, ProgressMax);
                        Github.UnzipStream(client.OpenRead("https://dl.dropboxusercontent.com/u/54555251/LSharp.zip"), "LSharp");
                        Progress++;
                    }

                    if (Version.Parse(installerVersion) > localInstallerVersion)
                    {
                        OverlayText = string.Format("{0}/{1} Downloading Update " + installerVersion, Progress, ProgressMax);
                        Github.UnzipStream(client.OpenRead("https://dl.dropboxusercontent.com/u/54555251/AssemblyInstaller.zip"), "Update");
                        Progress++;

                        OverlayText = string.Format("{0}/{1} Installing Update " + installerVersion, Progress, ProgressMax);

                        foreach (var f in Directory.EnumerateFiles("Update"))
                        {
                            var newFile = new FileInfo(f);
                            var oldFile = new FileInfo(newFile.Name);

                            if (oldFile.Exists)
                            {
                                oldFile.MoveTo(Path.Combine("Update", oldFile.Name + "_update"));
                            }

                            newFile.MoveTo(newFile.Name);
                        }
                        Progress++;

                        await DialogService.ShowMessage("Auto Update", "Update Complete\n\nRestarting Application", MessageDialogStyle.Affirmative);

                        Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AssemblyInstaller.exe"));
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception e)
            {
                DialogService.ShowMessage("Auto Update", e.ToString(), MessageDialogStyle.Affirmative);
            }
        }

        private List<AssemblyEntity> VersionCheck(IList<AssemblyEntity> list)
        {
            Progress = 0;
            ProgressMax = list.Count;
            OverlayText = string.Format("{0}/{1} Version Check", Progress, ProgressMax);
            var updates = new List<AssemblyEntity>();

            Parallel.ForEach(list, assembly =>
            {
                try
                {
                    var old = assembly.State;
                    LogFile.Write(assembly.Name, "Version Check");
                    assembly.State = "Version Check";
                    assembly.LocalVersion = Github.LocalVersion(assembly);
                    assembly.RepositroyVersion = Github.RepositorieVersion(assembly);

                    if (assembly.LocalVersion == 0 || assembly.LocalVersion != assembly.RepositroyVersion)
                    {
                        updates.Add(assembly);
                    }

                    assembly.State = old;
                    Progress++;
                    OverlayText = string.Format("{0}/{1} Version Check", Progress, ProgressMax);
                }
                catch (Exception e)
                {
                    DialogService.ShowMessage("VersionCheck - " + assembly.Name, e.Message, MessageDialogStyle.Affirmative);
                }
            });

            return updates;
        }

        private void Download(IList<AssemblyEntity> list)
        {
            Progress = 0;
            ProgressMax = list.Count;
            OverlayText = string.Format("{0}/{1} Download", Progress, ProgressMax);
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
                        Github.Update(repository.Url);
                        repository.State = old;
                        repository.LocalVersion = Github.LocalVersion(repository);
                    }

                    Progress++;
                    OverlayText = string.Format("{0}/{1} Download", Progress, ProgressMax);
                }
                catch (Exception e)
                {
                    DialogService.ShowMessage("Download - " + repository.Name, e.Message, MessageDialogStyle.Affirmative);
                }
            });

            Progress = 0;
            ProgressMax = 1;
        }

        private Dictionary<AssemblyEntity, string> Compile(IList<AssemblyEntity> list)
        {
            Progress = 0;
            ProgressMax = list.Count;
            OverlayText = string.Format("{0}/{1} Compile", Progress, ProgressMax);
            var complete = new Dictionary<AssemblyEntity, string>();

            foreach (var assembly in list)
            {
                try
                {
                    assembly.State = "Compile";
                    var project = assembly.GetProjectFile();

                    if (project != null)
                    {
                        var result = Github.Compile(project);

                        if (result != null && File.Exists(result))
                        {
                            LogFile.Write(assembly.Name, "Compile Sucsesfull - " + result);
                            assembly.State = "Updated";
                            complete.Add(assembly, result);
                        }
                        else
                        {
                            LogFile.Write(assembly.Name, "Compile Failed - Compiler Error");
                            assembly.State = "Broken";
                        }
                    }
                    else
                    {
                        LogFile.Write(assembly.Name, "Project File not Found - " + assembly.ProjectFile);
                        assembly.State = "Broken";
                    }

                    Progress++;
                    OverlayText = string.Format("{0}/{1} Compile", Progress, ProgressMax);
                }
                catch (Exception e)
                {
                    DialogService.ShowMessage("Compile - " + assembly.Name, e.ToString(), MessageDialogStyle.Affirmative);
                }
            }

            return complete;
        }

        private void Copy(IDictionary<AssemblyEntity, string> list)
        {
            Progress = 0;
            ProgressMax = list.Count;
            OverlayText = string.Format("{0}/{1} Copy", Progress, ProgressMax);

            //Parallel.ForEach(list, file =>
            foreach (var file in list)
            {
                try
                {
                    var info = new FileInfo(file.Value);

                    if (File.Exists(info.FullName))
                    {
                        if (file.Key.OutputType == "Library")
                        {
                            var dll = Path.Combine(Settings.Default.LeagueSharpPath, "Assemblies", "System", file.Key.AssemblyName + " - " + file.Key.Developer + ".dll");
                            if (File.Exists(dll))
                                File.Delete(dll);
                            File.Move(file.Value, dll);

                            LogFile.Write(file.Key.Name, "Move - " + info.FullName + " -> " + dll);
                        }

                        if (file.Key.OutputType == "Exe")
                        {
                            var exe = Path.Combine(Settings.Default.LeagueSharpPath, "Assemblies", file.Key.AssemblyName + " - " + file.Key.Developer + ".exe");
                            if (File.Exists(exe))
                                File.Delete(exe);
                            File.Move(file.Value, exe);

                            LogFile.Write(file.Key.Name, "Move - " + info.FullName + " -> " + exe);
                        }
                    }

                    Progress++;
                    OverlayText = string.Format("{0}/{1} Copy", Progress, ProgressMax);
                }
                catch (Exception e)
                {
                    // TODO: Fix duplicate assembly names
                    DialogService.ShowMessage("Copy - " + file, e.Message, MessageDialogStyle.Affirmative);
                }
            }//);
        }

        private void InstallAssembly(AssemblyEntity assembly)
        {
            Task.Factory.StartNew(() =>
            {
                IsOverlay = true;
                try
                {
                    Download(new[] { assembly });
                    var complete = Compile(new[] { assembly });

                    if (complete.Count > 0)
                    {
                        Copy(complete);
                        assembly.State = "Installed";

                        DispatcherHelper.CheckBeginInvokeOnUI(() => Update.Add(assembly));

                        Progress = ProgressMax;
                        OverlayText = "Complete";
                        DialogService.ShowMessage("Install", "Install Complete\n\n" + complete.Count + " Assemblies Installed.", MessageDialogStyle.Affirmative);
                    }
                    else
                    {
                        DialogService.ShowMessage("Install", "Install Failed - Compiler Error", MessageDialogStyle.Affirmative);
                    }
                }
                catch (Exception e)
                {
                    LogFile.Write("Error", e.Message);
                    DialogService.ShowMessage("Error", e.ToString(), MessageDialogStyle.Affirmative);
                }

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    switch (StartPage)
                    {
                        case 0:
                            CollectionViewSource.GetDefaultView(Champion).Refresh();
                            break;
                        case 1:
                            CollectionViewSource.GetDefaultView(Utility).Refresh();
                            break;
                        case 2:
                            CollectionViewSource.GetDefaultView(Library).Refresh();
                            break;
                        case 3:
                            CollectionViewSource.GetDefaultView(Update).Refresh();
                            break;
                    }
                });

                IsOverlay = false;
            });
        }

        private void InstallSelectedAssembly()
        {
            foreach (var entity in _multiSelect)
            {
                Console.WriteLine("Install: " + entity);
            }

            Task.Factory.StartNew(() =>
            {
                IsOverlay = true;
                try
                {
                    var updates = VersionCheck(_multiSelect);
                    Download(updates);
                    var complete = Compile(_multiSelect);

                    if (complete.Count > 0)
                    {
                        Copy(complete);
                        lock (_multiSelect)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                foreach (var entity in _multiSelect)
                                {
                                    if (!Update.Contains(entity))
                                        Update.Add(entity);

                                    if (complete.ContainsKey(entity))
                                        entity.State = "Installed";
                                    else
                                        entity.State = "Broken";

                                    entity.IsChecked = false;
                                }
                            });

                            _multiSelect.Clear();
                        }

                        Progress = ProgressMax;
                        OverlayText = "Complete";
                        DialogService.ShowMessage("Install", "Install Complete\n\n" + complete.Count + " Assemblies Installed.", MessageDialogStyle.Affirmative);
                    }
                    else
                    {
                        DialogService.ShowMessage("Install", "Install Failed - Compiler Error", MessageDialogStyle.Affirmative);
                    }

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        CollectionViewSource.GetDefaultView(Champion).Refresh();
                        CollectionViewSource.GetDefaultView(Utility).Refresh();
                        CollectionViewSource.GetDefaultView(Library).Refresh();
                        CollectionViewSource.GetDefaultView(Update).Refresh();
                    });
                }
                catch (Exception e)
                {
                    LogFile.Write("Error", e.Message);
                    DialogService.ShowMessage("Error", e.ToString(), MessageDialogStyle.Affirmative);
                }

                IsOverlay = false;
            });
        }

        private void UpdateAssembly()
        {
            Task.Factory.StartNew(async () =>
            {
                IsOverlay = true;
                try
                {
                    var updates = VersionCheck(Update);
                    Download(updates);
                    var complete = Compile(updates);
                    Copy(complete);

                    Progress = ProgressMax;
                    OverlayText = "Complete";
                    var result = await DialogService.ShowMessage("Update", "Update Complete\n" + updates.Count + " Assemblies Updated.\n\nClose Application and Start LeagueSharp?", MessageDialogStyle.AffirmativeAndNegative);

                    if (result == MessageDialogResult.Affirmative)
                    {
                        StartLeagueSharpLoader();
                    }

                    OverlayText = "";
                }
                catch (Exception e)
                {
                    LogFile.Write("Error", e.Message);
                    MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    switch (StartPage)
                    {
                        case 0:
                            CollectionViewSource.GetDefaultView(Champion).Refresh();
                            break;
                        case 1:
                            CollectionViewSource.GetDefaultView(Utility).Refresh();
                            break;
                        case 2:
                            CollectionViewSource.GetDefaultView(Library).Refresh();
                            break;
                        case 3:
                            CollectionViewSource.GetDefaultView(Update).Refresh();
                            break;
                    }
                });

                IsOverlay = false;
            });
        }

        private void StartLeagueSharpLoader()
        {
            var p = new ProcessStartInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LSharp", "LeagueSharp.Loader.exe"))
            {
                WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LSharp")
            };

            Process.Start(p);
            Environment.Exit(0);
        }
    }
}