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
using System.Windows.Data;
using AssemblyInstaller.Helpers;
using AssemblyInstaller.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls.Dialogs;

namespace AssemblyInstaller.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="StartPage" /> property's name.
        /// </summary>
        public const string StartPagePropertyName = "StartPage";

        private int _startPage = 0;

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
        /// The <see cref="Debug" /> property's name.
        /// </summary>
        public const string DebugPropertyName = "Debug";

        /// <summary>
        /// Sets and gets the Debug property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool Debug
        {
            get
            {
                return Config.LSharpConfig.Debug;
            }

            set
            {
                if (Config.LSharpConfig.Debug == value)
                {
                    return;
                }

                RaisePropertyChanging(() => Debug);
                Config.LSharpConfig.Debug = value;
                RaisePropertyChanged(() => Debug);
            }
        }

        /// <summary>
        /// The <see cref="Afk" /> property's name.
        /// </summary>
        public const string AfkPropertyName = "Afk";

        /// <summary>
        /// Sets and gets the Afk property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool Afk
        {
            get
            {
                return Config.LSharpConfig.Afk;
            }

            set
            {
                if (Config.LSharpConfig.Afk == value)
                {
                    return;
                }

                RaisePropertyChanging(() => Afk);
                Config.LSharpConfig.Afk = value;
                RaisePropertyChanged(() => Afk);
            }
        }

        /// <summary>
        /// The <see cref="Zoom" /> property's name.
        /// </summary>
        public const string ZoomPropertyName = "Zoom";

        /// <summary>
        /// Sets and gets the Zoom property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool Zoom
        {
            get
            {
                return Config.LSharpConfig.Zoom;
            }

            set
            {
                if (Config.LSharpConfig.Zoom == value)
                {
                    return;
                }

                RaisePropertyChanging(() => Zoom);
                Config.LSharpConfig.Zoom = value;
                RaisePropertyChanged(() => Zoom);
            }
        }

        /// <summary>
        /// The <see cref="Tower" /> property's name.
        /// </summary>
        public const string TowerPropertyName = "Tower";

        /// <summary>
        /// Sets and gets the Tower property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Tower
        {
            get
            {
                return Config.LSharpConfig.Tower;
            }

            set
            {
                if (Config.LSharpConfig.Tower == value)
                {
                    return;
                }

                RaisePropertyChanging(() => Tower);
                Config.LSharpConfig.Tower = value;
                RaisePropertyChanged(() => Tower);
            }
        }


        /// <summary>
        /// The <see cref="Champion" /> property's name.
        /// </summary>
        public const string ChampionPropertyName = "Champion";

        private ObservableCollection<AssemblyEntity> _champion;

        /// <summary>
        /// Sets and gets the Data property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<AssemblyEntity> Champion
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

        private ObservableCollection<AssemblyEntity> _utility;

        /// <summary>
        /// Sets and gets the Data property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<AssemblyEntity> Utility
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

        private ObservableCollection<AssemblyEntity> _library;

        /// <summary>
        /// Sets and gets the Data property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<AssemblyEntity> Library
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

        private ObservableCollection<AssemblyEntity> _update;

        /// <summary>
        /// Sets and gets the Data property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<AssemblyEntity> Update
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
        /// The <see cref="Injection" /> property's name.
        /// </summary>
        public const string InjectionPropertyName = "Injection";

        private ObservableCollection<AssemblyEntity> _injection;

        /// <summary>
        /// Sets and gets the Injection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<AssemblyEntity> Injection
        {
            get
            {
                return _injection;
            }
            set
            {
                Set(() => Injection, ref _injection, value);
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
        /// The <see cref="Search" /> property's name.
        /// </summary>
        public const string SearchPropertyName = "Search";

        private string _search = "";

        /// <summary>
        /// Sets and gets the Search property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Search
        {
            get
            {
                return _search;
            }
            set
            {
                if (_search == value)
                {
                    return;
                }

                Console.WriteLine(value);

                RaisePropertyChanging(() => Search);
                _search = value;
                RaisePropertyChanged(() => Search);

                if (value.Length > 1 || value.Length == 0)
                {
                    CollectionViewSource.GetDefaultView(Injection).Filter = a =>
                    {
                        var assembly = (a as AssemblyEntity);
                        return assembly == null || assembly.Name.Contains(Search, StringComparison.OrdinalIgnoreCase);
                    };
                    CollectionViewSource.GetDefaultView(Champion).Filter = a =>
                    {
                        var assembly = (a as AssemblyEntity);
                        return assembly == null || assembly.Name.Contains(Search, StringComparison.OrdinalIgnoreCase);
                    };
                    CollectionViewSource.GetDefaultView(Utility).Filter = a =>
                    {
                        var assembly = (a as AssemblyEntity);
                        return assembly == null || assembly.Name.Contains(Search, StringComparison.OrdinalIgnoreCase);
                    };
                    CollectionViewSource.GetDefaultView(Library).Filter = a =>
                    {
                        var assembly = (a as AssemblyEntity);
                        return assembly == null || assembly.Name.Contains(Search, StringComparison.OrdinalIgnoreCase);
                    };
                }
            }
        }

        /// <summary>
        /// The <see cref="Log" /> property's name.
        /// </summary>
        public const string LogPropertyName = "Log";

        private ObservableCollection<LogEntity> _log;

        /// <summary>
        /// Sets and gets the Log property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<LogEntity> Log
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
                                                  var collection = new ObservableCollection<AssemblyEntity>();
                                                  switch (StartPage)
                                                  {
                                                      case 1:
                                                          collection = Champion;
                                                          break;
                                                      case 2:
                                                          collection = Utility;
                                                          break;
                                                      case 3:
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
                                                  if (StartPage == 0)
                                                  {
                                                      p.IsInjected = true;
                                                      Injector.LoadAssembly(p.AssemblyName + ".exe");
                                                      return;
                                                  }

                                                  if (!_multiSelect.Contains(p))
                                                  {
                                                      p.IsChecked = true;
                                                      _multiSelect.Add(p);
                                                  }
                                              }

                                              _multiSelect = new ObservableCollection<AssemblyEntity>(_multiSelect.Distinct());
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
                                                  var collection = new ObservableCollection<AssemblyEntity>();
                                                  switch (StartPage)
                                                  {
                                                      case 0:
                                                          collection = Injection;
                                                          break;
                                                      case 1:
                                                          collection = Champion;
                                                          break;
                                                      case 2:
                                                          collection = Utility;
                                                          break;
                                                      case 3:
                                                          collection = Library;
                                                          break;
                                                  }

                                                  foreach (var c in collection)
                                                  {
                                                      if (StartPage == 0)
                                                          c.IsInjected = false;
                                                      else
                                                          c.IsChecked = false;
                                                      _multiSelect.Remove(c);
                                                  }
                                              }
                                              else
                                              {
                                                  if (StartPage == 0)
                                                  {
                                                      p.IsInjected = false;
                                                      Injector.UnloadAssembly(p.AssemblyName + ".exe");
                                                      return;
                                                  }

                                                  if (_multiSelect.Contains(p))
                                                      _multiSelect.Remove(p);

                                                  if (StartPage == 0)
                                                      p.IsInjected = false;
                                                  else
                                                      p.IsChecked = false;
                                              }

                                              _multiSelect = new ObservableCollection<AssemblyEntity>(_multiSelect.Distinct());
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
                                                  SelectedAssembly.Delete();
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
                       ?? (_updateCommand = new RelayCommand(UpdateAssemblies));

            }
        }

        private ObservableCollection<AssemblyEntity> _multiSelect = new ObservableCollection<AssemblyEntity>();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService service)
        {
            if (!IsInDesignMode)
                IsOverlay = true;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    OverlayText = "Loading";

                    while (!service.IsInitComplete())
                        Thread.Sleep(100);

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        Champion = service.GetChampionData();
                        Utility = service.GetUtilityData();
                        Library = service.GetLibraryData();
                        Log = service.GetLogData();
                        Update = new ObservableCollection<AssemblyEntity>();
                        Injection = new ObservableCollection<AssemblyEntity>();

                        Injection.Clear();
                        foreach (var guid in Config.InstallerConfig.Installed)
                        {
                            var champ = Champion.FirstOrDefault(a => a.Id == guid);
                            var util = Utility.FirstOrDefault(a => a.Id == guid);
                            var lib = Library.FirstOrDefault(a => a.Id == guid);

                            if (champ != null)
                            {
                                Update.Add(champ);
                                Injection.Add(champ);
                                champ.State = "Installed";
                            }

                            if (util != null)
                            {
                                Update.Add(util);
                                Injection.Add(util);
                                util.State = "Installed";
                            }

                            if (lib != null)
                            {
                                Update.Add(lib);
                                lib.State = "Installed";
                            }
                        }

                        foreach (var a in Update)
                        {
                            a.LocalVersion = Github.LocalVersion(a);
                        }

                        var sort = new SortDescription("Name", ListSortDirection.Ascending);

                        var v = CollectionViewSource.GetDefaultView(Injection);
                        v.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                        v.SortDescriptions.Add(sort);
                        v.ActiveLiveSorting(new[] { "Name" });
                        v.ActiveLiveGrouping(new[] { "Category" });
                        v.ActiveLiveFiltering(new[] { "Name" });

                        v = CollectionViewSource.GetDefaultView(Champion);
                        v.GroupDescriptions.Add(new PropertyGroupDescription("State"));
                        v.SortDescriptions.Add(sort);
                        v.ActiveLiveSorting(new[] { "Name" });
                        v.ActiveLiveGrouping(new[] { "State" });
                        v.ActiveLiveFiltering(new[] { "Name" });

                        v = CollectionViewSource.GetDefaultView(Utility);
                        v.GroupDescriptions.Add(new PropertyGroupDescription("State"));
                        v.SortDescriptions.Add(sort);
                        v.ActiveLiveSorting(new[] { "Name" });
                        v.ActiveLiveGrouping(new[] { "State" });
                        v.ActiveLiveFiltering(new[] { "Name" });

                        v = CollectionViewSource.GetDefaultView(Library);
                        v.GroupDescriptions.Add(new PropertyGroupDescription("State"));
                        v.SortDescriptions.Add(sort);
                        v.ActiveLiveSorting(new[] { "Name" });
                        v.ActiveLiveGrouping(new[] { "State" });
                        v.ActiveLiveFiltering(new[] { "Name" });

                        v = CollectionViewSource.GetDefaultView(Update);
                        v.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                        v.SortDescriptions.Add(sort);
                        v.ActiveLiveSorting(new[] { "Name" });
                        v.ActiveLiveGrouping(new[] { "Category" });
                        v.ActiveLiveFiltering(new[] { "Name" });
                    });

                    if (Injector.GetLeagueProcess() == null)
                        ApplicationUpdate();

                    Injector.StateHandler += (sender, args) =>
                    {
                        switch (args.State)
                        {
                            case Injector.InjectorState.Injected:
                                foreach (var assembly in Injection.Where(a => a.IsInjected))
                                {
                                    Thread.Sleep(100);
                                    Injector.LoadAssembly(assembly.AssemblyName + ".exe");
                                }
                                break;
                        }
                    };

                    Injector.Start();
                }
                catch (Exception e)
                {
                    DialogService.ShowMessage("Error", e.ToString(), MessageDialogStyle.Affirmative);
                }

                IsOverlay = false;

                LogFile.Write("App", "Init Complete");
            });
        }

        private async void ApplicationUpdate()
        {
            const string versionUrl = "https://dl.dropboxusercontent.com/u/54555251/InstallerVersion";
            const string lSharpVersionUrl = "https://dl.dropboxusercontent.com/u/54555251/LSharpVersion";

            try
            {
                Progress = 0;
                ProgressMax = 5;
                OverlayText = string.Format("{0}/{1} Update Check", Progress, ProgressMax);

                // Update cleanup
                if (Directory.Exists("Update"))
                    Directory.Delete("Update", true);

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
                        OverlayText = string.Format("{0}/{1} Update Check", Progress, ProgressMax);
                    });
                }
                catch
                {
                }

                using (var client = new WebClient())
                {
                    var installerVersion = result[versionUrl];
                    var lsharpVersion = result[lSharpVersionUrl];

                    if (string.IsNullOrEmpty(installerVersion))
                    {
                        LogFile.Write("Update", "Update Version not found!");
                        return;
                    }

                    if (string.IsNullOrEmpty(lsharpVersion))
                    {
                        LogFile.Write("Update", "LSharp Update Version not found!");
                        return;
                    }

                    if (Version.Parse(result[lSharpVersionUrl]) > Config.LSharpConfig.Version)
                    {
                        OverlayText = string.Format("{0}/{1} Downloading LeagueSharp " + lsharpVersion, Progress, ProgressMax);
                        Github.UnzipStream(client.OpenRead("https://dl.dropboxusercontent.com/u/54555251/LSharp.zip"), Config.BaseDirectory);
                        Config.LSharpConfig.Version = Version.Parse(result[lSharpVersionUrl]);
                        Progress++;
                    }

                    if (Version.Parse(installerVersion) > Assembly.GetExecutingAssembly().GetName().Version)
                    {
                        OverlayText = string.Format("{0}/{1} Downloading Update " + installerVersion, Progress, ProgressMax);
                        Github.UnzipStream(client.OpenRead("https://dl.dropboxusercontent.com/u/54555251/AssemblyInstaller.zip"), Config.UpdateDirectory);
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
                        OverlayText = string.Format("{0}/{1} Complete", Progress, ProgressMax);

                        await DialogService.ShowMessage("Auto Update", "Update Complete\n\nRestarting Application", MessageDialogStyle.Affirmative);

                        new Thread(() =>
                        {
                            Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                        }).Start();
                    }
                }
            }
            catch (Exception e)
            {
                DialogService.ShowMessage("Auto Update", e.ToString(), MessageDialogStyle.Affirmative);
            }
        }

        private void InstallAssembly(AssemblyEntity assembly)
        {
            Task.Factory.StartNew(() =>
            {
                IsOverlay = true;
                try
                {
                    Progress = 0;
                    ProgressMax = 3;
                    OverlayText = string.Format("{0}/{1} Downloading", Progress, ProgressMax);
                    Github.Download(new[] { assembly });
                    Progress++;
                    OverlayText = string.Format("{0}/{1} Compile", Progress, ProgressMax);
                    if (assembly.Compile())
                    {
                        Progress++;
                        OverlayText = string.Format("{0}/{1} Copy", Progress, ProgressMax);
                        assembly.Copy();
                        Progress++;
                        assembly.State = "Installed";
                        OverlayText = string.Format("{0}/{1} Complete", Progress, ProgressMax);
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            if (assembly.OutputType == "Exe")
                                Injection.Add(assembly);

                            if (!Config.InstallerConfig.Installed.Contains(assembly.Id))
                                Config.InstallerConfig.Installed.Add(assembly.Id);

                            Update.Add(assembly);
                        });
                        DialogService.ShowMessage("Install", "Install Complete", MessageDialogStyle.Affirmative);
                    }
                    else
                    {
                        assembly.State = "Broken";
                        DialogService.ShowMessage("Install", "Install Failed - Compiler Error", MessageDialogStyle.Affirmative);
                    }
                }
                catch (Exception e)
                {
                    LogFile.Write("Error", e.Message);
                    DialogService.ShowMessage("Error", e.ToString(), MessageDialogStyle.Affirmative);
                }

                //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                //{
                //    switch (StartPage)
                //    {
                //        case 1:
                //            CollectionViewSource.GetDefaultView(Champion).Refresh();
                //            break;
                //        case 2:
                //            CollectionViewSource.GetDefaultView(Utility).Refresh();
                //            break;
                //        case 3:
                //            CollectionViewSource.GetDefaultView(Library).Refresh();
                //            break;
                //    }
                //});

                IsOverlay = false;
            });
        }

        private void InstallSelectedAssembly()
        {
            Task.Factory.StartNew(() =>
            {
                IsOverlay = true;
                try
                {
                    Progress = 0;
                    ProgressMax = 1;

                    OverlayText = string.Format("{0}/{1} Download", Progress, ProgressMax);
                    Github.Download(_multiSelect);
                    Progress++;

                    var broken = 0;
                    var complete = 0;
                    foreach (var assembly in _multiSelect)
                    {
                        OverlayText = string.Format("{0}/{1} Compile", Progress, ProgressMax);
                        if (assembly.Compile())
                        {
                            OverlayText = string.Format("{0}/{1} Copy", Progress, ProgressMax);
                            assembly.Copy();

                            assembly.State = "Installed";
                            AssemblyEntity assembly1 = assembly;
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                if (assembly1.OutputType == "Exe")
                                    Injection.Add(assembly1);

                                if (!Config.InstallerConfig.Installed.Contains(assembly1.Id))
                                    Config.InstallerConfig.Installed.Add(assembly1.Id);

                                Update.Add(assembly1);
                            });
                            complete++;
                        }
                        else
                        {
                            assembly.State = "Broken";
                            broken++;
                        }
                    }
                    Progress++;
                    OverlayText = string.Format("{0}/{1} Complete", Progress, ProgressMax);

                    //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    //{
                    //    lock (_multiSelect)
                    //    {
                    //        foreach (var entity in _multiSelect)
                    //        {
                    //            entity.IsChecked = false;
                    //        }
                    //    }
                    //    _multiSelect.Clear();
                    //});

                    Progress = ProgressMax;
                    DialogService.ShowMessage("Install", "Install Complete\n\n" + complete + " Assemblies Installed.\n" + broken + " Assemblies Broken.", MessageDialogStyle.Affirmative);

                    //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    //{
                    //    CollectionViewSource.GetDefaultView(Champion).Refresh();
                    //    CollectionViewSource.GetDefaultView(Utility).Refresh();
                    //    CollectionViewSource.GetDefaultView(Library).Refresh();
                    //    CollectionViewSource.GetDefaultView(Update).Refresh();
                    //});
                }
                catch (Exception e)
                {
                    LogFile.Write("Error", e.Message);
                    DialogService.ShowMessage("Error", e.ToString(), MessageDialogStyle.Affirmative);
                }

                IsOverlay = false;
            });
        }

        private void UpdateAssemblies()
        {
            Task.Factory.StartNew(() =>
            {
                IsOverlay = true;
                try
                {
                    var updates = Github.VersionCheck(Update);
                    Github.Download(updates);

                    var complete = 0;
                    foreach (var assembly in updates)
                    {
                        if (assembly.Compile())
                        {
                            assembly.Copy();
                            assembly.State = "Installed";
                            AssemblyEntity assembly1 = assembly;
                            DispatcherHelper.CheckBeginInvokeOnUI(() => Update.Add(assembly1));
                            complete++;
                        }
                        else
                        {
                            assembly.State = "Broken";
                        }
                    }

                    OverlayText = "Complete";
                    DialogService.ShowMessage("Install", "Update Complete\n\n" + complete + " Assemblies Updated.", MessageDialogStyle.Affirmative);
                }
                catch (Exception e)
                {
                    LogFile.Write("Error", e.Message);
                    DialogService.ShowMessage("Error", e.ToString(), MessageDialogStyle.Affirmative);
                }

                //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                //{
                //    switch (StartPage)
                //    {
                //        case 1:
                //            CollectionViewSource.GetDefaultView(Champion).Refresh();
                //            break;
                //        case 2:
                //            CollectionViewSource.GetDefaultView(Utility).Refresh();
                //            break;
                //        case 3:
                //            CollectionViewSource.GetDefaultView(Library).Refresh();
                //            break;
                //        case 4:
                //            CollectionViewSource.GetDefaultView(Update).Refresh();
                //            break;
                //    }
                //});

                IsOverlay = false;
            });
        }
    }
}