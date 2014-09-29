using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;


namespace Support
{
    public class SupportTargetSelector
    {
        private static readonly string[] Ap =
        {
            "Ahri", "Akali", "Anivia", "Annie", "Brand", "Cassiopeia", "Diana", "FiddleSticks", "Fizz", "Gragas", 
            "Heimerdinger", "Karthus", "Kassadin", "Katarina", "Kayle", "Kennen", "Leblanc", "Lissandra", "Lux", 
            "Malzahar", "Mordekaiser", "Morgana", "Nidalee", "Orianna", "Ryze", "Sion", "Swain", "Syndra", "Teemo", 
            "TwistedFate", "Veigar", "Viktor", "Vladimir", "Xerath", "Ziggs", "Zyra", "Velkoz"
        };

        private static readonly string[] Ad =
        {
            "Ashe", "Caitlyn", "Corki", "Draven", "Ezreal", "Graves", "KogMaw", "MissFortune", "Quinn", "Sivir", 
            "Talon", "Tristana", "Twitch", "Urgot", "Varus", "Vayne", "Zed", "Jinx", "Yasuo", "Lucian"
        };

        private static readonly string[] Bruiser =
        {
            "Darius", "Elise", "Evelynn", "Fiora", "Gangplank", "Gnar", "Jayce", "Pantheon", "Irelia", "JarvanIV", 
            "Jax", "Khazix", "LeeSin", "Nocturne", "Olaf", "Poppy", "Renekton", "Rengar", "Riven", "Shyvana", "Trundle", 
            "Tryndamere", "Udyr", "Vi", "MonkeyKing", "XinZhao", "Aatrox", "Rumble", "Shaco", "MasterYi"
        };

        private static readonly string[] Tank =
        {
            "Amumu", "Chogath", "DrMundo", "Galio", "Hecarim", "Malphite", "Maokai", "Nasus", "Rammus", "Sejuani", 
            "Shen", "Singed", "Skarner", "Volibear", "Warwick", "Yorick", "Zac", "Nunu", "Taric", "Alistar", "Garen", 
            "Nautilus", "Braum"
        };

        private static readonly string[] Support =
        {
            "Blitzcrank", "Janna", "Karma", "Leona", "Lulu", "Nami", "Sona", "Soraka", "Thresh", "Zilean"
        };

        public Obj_AI_Hero Target { get; set; }
        public bool DrawSelectedTarget { get; set; }
        public float Range { get; set; }

        private Obj_AI_Hero _maintarget;
        private double _lasttick;

        public SupportTargetSelector(float range)
        {
            Range = range;

            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnWndProc += Game_OnWndProc;
        }

        private void Game_OnWndProc(WndEventArgs args)
        {
            if (MenuGUI.IsChatOpen || ObjectManager.Player.Spellbook.SelectedSpellSlot != SpellSlot.Unknown)
                return;

            if (args.WParam == 1) // LMouse
            {
                switch (args.Msg)
                {
                    case 513:
                        foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget() && Vector2.Distance(Game.CursorPos.To2D(), hero.Position.To2D()) < 300))
                        {
                            Target = hero;
                            _maintarget = hero;
                            Game.PrintChat("SupportTargetSelector: New main target: " + _maintarget.ChampionName);
                        }
                        break;
                }
            }
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            if (!ObjectManager.Player.IsDead && DrawSelectedTarget && _maintarget != null && _maintarget.IsVisible && !_maintarget.IsDead)
            {
                Utility.DrawCircle(_maintarget.Position, 125, Color.Red);
            }
        }

        private void Game_OnGameUpdate(EventArgs args)
        {
            if (!(Environment.TickCount > _lasttick + 100)) 
                return;

            _lasttick = Environment.TickCount;

            if (_maintarget == null) // No MainTarget selected
            {
                Target = AutoPriority();
            }
            else
            {
                if (_maintarget.IsValidTarget(Range)) // MainTarget valid
                {
                    Target = _maintarget;
                }
                else // MainTarget not valid or Out of range
                {
                    Target = AutoPriority();
                }
            }
        }

        private Obj_AI_Hero AutoPriority()
        {
            Obj_AI_Hero autopriority = null;

            var prio = 5;
            foreach (var target in ObjectManager.Get<Obj_AI_Hero>().Where(target => target.IsValidTarget(Range)))
            {
                var priority = FindPrioForTarget(target.ChampionName);

                if (autopriority == null)
                {
                    autopriority = target;
                    prio = priority;
                }
                else
                {
                    if (priority < prio)
                    {
                        autopriority = target;
                        prio = FindPrioForTarget(target.ChampionName);
                    }
                    else if (priority == prio)
                    {
                        if (!(target.Health < autopriority.Health))
                        {
                            continue;
                        }
                        autopriority = target;
                        prio = priority;
                    }
                }
            }
            return autopriority;
        }

        private static int FindPrioForTarget(string championName)
        {
            if (Ap.Contains(championName))
            {
                return 2;
            }
            if (Ad.Contains(championName))
            {
                return 1;
            }
            if (Support.Contains(championName))
            {
                return 5;
            }
            if (Bruiser.Contains(championName))
            {
                return 3;
            }
            if (Tank.Contains(championName))
            {
                return 4;
            }
            return 5;
        }

        public override string ToString()
        {
            return "Target: " + Target.ChampionName + " Range: " + Range;
        }
    }
}
