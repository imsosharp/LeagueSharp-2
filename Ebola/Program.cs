#region LICENSE

// Copyright 2014 - 2014 Ebola
// Program.cs is part of Ebola.
// Ebola is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// Ebola is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with Ebola. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Ebola
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (ObjectManager.Player.ChampionName == "Rengar")
                Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name.ToLower().Contains("bola"))
            {
                var targets = string.Join(" ",
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(h => h.IsEnemy && h.Distance(args.End) < 200)
                        .Select(h => h.Name));
                Game.Say("/all E-Bola " + targets);
            }
        }
    }
}