using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace AssemblyInstaller.Helpers
{
    internal class Injector
    {
        public static List<string> Assemblies = new List<string>();

        #region State
        private static InjectorState _state = InjectorState.Unknown;
        public static InjectorState State
        {
            get { return _state; }

            set
            {
                if (_state != value)
                {
                    _state = value;
                    RaiseEvent(value);
                }
            }
        }

        public delegate void StateEventHandler(object sender, InjectorEventArgs e);
        public static event StateEventHandler StateHandler;

        public class InjectorEventArgs : EventArgs
        {
            public readonly InjectorState State;

            public InjectorEventArgs(InjectorState state)
            {
                State = state;
            }
        }

        public enum InjectorState
        {
            Unknown,
            Idle,
            Injecting,
            Injected,
            Unloading
        }

        private static void RaiseEvent(InjectorState state)
        {
            if (StateHandler != null)
            {
                LogFile.Write("Injector", state.ToString());
                StateHandler(null, new InjectorEventArgs(state));
            }
        }
        #endregion

        public struct COPYDATASTRUCT
        {
            public int cbData;
            public int dwData;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpData;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public delegate bool InjectDllDelegate(int processId, string path);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(IntPtr zeroOnly, string lpWindowName);

        private static InjectDllDelegate _injectDll;
        private static bool _inject;

        static Injector()
        {
            ResolveInjectDll();
        }

        public static void Start()
        {
            _inject = true;

            var injectThread = new Thread(() =>
            {
                while (_inject)
                {
                    Thread.Sleep(1000);
                    State = Pulse();

                    switch (State)
                    {
                        case InjectorState.Injecting:
                            Thread.Sleep(1000);
                            SendConfig(Config.LSharpConfig.Afk, Config.LSharpConfig.Zoom, Config.LSharpConfig.Debug, 2);
                            break;

                        case InjectorState.Injected:
                            break;

                        case InjectorState.Unloading:
                            Assemblies.Clear();
                            break;
                    }
                }
            }) { IsBackground = true };

            injectThread.Start();
        }

        public static void Stop()
        {
            _inject = false;
        }

        private static void ResolveInjectDll()
        {
            var hModule = LoadLibrary(Path.Combine(Config.SystemDirectory, "LeagueSharp.Bootstrap.dll"));
            if (!(hModule != IntPtr.Zero))
            {
                return;
            }
            var procAddress = GetProcAddress(hModule, "_InjectDLL@8");
            if (!(procAddress != IntPtr.Zero))
            {
                return;
            }
            _injectDll = Marshal.GetDelegateForFunctionPointer(procAddress, typeof(InjectDllDelegate)) as InjectDllDelegate;
        }

        public static IntPtr GetLeagueWnd()
        {
            return FindWindow(IntPtr.Zero, "League of Legends (TM) Client");
        }

        public static Process GetLeagueProcess()
        {
            var processesByName = Process.GetProcessesByName("League of Legends");

            if (processesByName.Length > 0)
            {
                return processesByName[0];
            }

            return null;
        }

        public static InjectorState Pulse()
        {
            var leagueProcess = GetLeagueProcess();

            if (leagueProcess != null)
            {
                var flag = leagueProcess.Modules.Cast<ProcessModule>().Any(module => module.ModuleName == "LeagueSharp.Core.dll");

                if (!flag)
                {
                    LogFile.Write("Injector", "Injecting LeagueSharp.Core.dll");
                    _injectDll(leagueProcess.Id, Path.Combine(Config.SystemDirectory, "LeagueSharp.Core.dll"));
                    return InjectorState.Injecting;
                }

                return InjectorState.Injected;
            }

            if (State == InjectorState.Injected)
                return InjectorState.Unloading;

            return InjectorState.Idle;
        }

        public static void LoadAssembly(string name)
        {
            if (Assemblies.Contains(name))
                return;

            Assemblies.Add(name);
            SendCommand(1, string.Format("load \"{0}\"", name));
        }

        public static void UnloadAssembly(string name)
        {
            if (!Assemblies.Contains(name))
                return;

            Assemblies.Remove(name);
            SendCommand(1, string.Format("unload \"{0}\"", name));
        }

        public static void SendConfig(bool afk, bool zoom, bool debug, int tower)
        {
            var text = string.Format("{0}{1}{2}{3}",
                afk ? "1" : "0",
                zoom ? "1" : "0",
                debug ? "1" : "0",
                tower
                );

            SendCommand(2, text);
        }

        public static void SendCommand(int channel, string text)
        {
            if (State == InjectorState.Injected || State == InjectorState.Injecting)
            {
                var lParam = new COPYDATASTRUCT
                {
                    cbData = 1,
                    dwData = text.Length * 2 + 2,
                    lpData = text
                };

                LogFile.Write("Injector", text);
                SendMessage(GetLeagueWnd(), 74u, IntPtr.Zero, ref lParam);
            }
        }
    }
}
