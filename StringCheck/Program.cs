using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace StringCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                Game.PrintChat("ChampionName: " + ObjectManager.Player.ChampionName);
                Game.PrintChat("Lower: " + (ObjectManager.Player.ChampionName.ToLower() == "vayne"));
                Game.PrintChat("Invariant: " + (ObjectManager.Player.ChampionName.ToLowerInvariant() == "vayne"));
            };
        }
    }
}
