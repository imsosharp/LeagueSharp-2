using System;
using System.Media;
using LeagueSharp;
using LeagueSharp.Common;

namespace BamMod
{
    class Program
    {
        private static float _last;
        private static readonly SoundPlayer Player = new SoundPlayer(Sounds.bam);
        private static readonly Menu Config = new Menu("BamMod", "BamMod", true);

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                Config.AddItem(new MenuItem("OnlyNew", "Only Largest Critical Strike").SetValue(true));
                Config.AddItem(new MenuItem("PrintChat", "Print Chat").SetValue(true));
                Config.AddItem(new MenuItem("PlaySound", "Play Sound").SetValue(true));
                Config.AddToMainMenu();
            };

            Game.OnGameUpdate += eventArgs =>
            {
                try
                {
                    if (ObjectManager.Player.LargestCriticalStrike == 0)
                        return;

                    if (ObjectManager.Player.LargestCriticalStrike != _last || !Config.Item("OnlyNew").GetValue<bool>())
                    {
                        if (Config.Item("PrintChat").GetValue<bool>())
                            Game.PrintChat("<font color='#FF0000'>BAM</font> <font color='#FFFFFF'>" + ObjectManager.Player.LargestCriticalStrike + "</font>");

                        if (Config.Item("PlaySound").GetValue<bool>())
                            Player.Play();
                        
                        _last = ObjectManager.Player.LargestCriticalStrike;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            };
        }
    }
}
