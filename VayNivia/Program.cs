#region LICENSE

// Copyright 2014 - 2014 VayNivia
// Program.cs is part of VayNivia.
// VayNivia is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// VayNivia is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with VayNivia. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace VayNivia
{
    public class Program
    {
        public static Spell Wall = new Spell(SpellSlot.W, 990);
        public static Menu Config = new Menu("VayNivia", "VayNivia", true);

        public static int WallOffset { get { return Config.Item("Wall.Offset").GetValue<Slider>().Value; } }
        public static int CondemnDistance { get { return Config.Item("Condemn.Distance").GetValue<Slider>().Value; } }

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                if (ObjectManager.Player.ChampionName != "Anivia")
                    return;

                Config.AddItem(new MenuItem("Condemn.Distance", "Condemn Distance").SetValue(new Slider(425, 0, 500)));
                Config.AddItem(new MenuItem("Wall.Offset", "Wall Offset").SetValue(new Slider(5, 5, 50)));
                Config.AddToMainMenu();

                Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
                Extensions.PrintMessage("by h3h3 loaded.");
            };
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (!Wall.IsReady())
                    return;

                if (!sender.IsValid<Obj_AI_Hero>() || !args.Target.IsValid<Obj_AI_Hero>() || sender.IsEnemy ||
                    args.SData.Name != "VayneCondemn")
                    return;

                var condemEndPos = args.End.To2D().Extend(sender.ServerPosition.To2D(), -CondemnDistance).To3D();
                var wallPos = args.End.To2D().Extend(sender.ServerPosition.To2D(), -(CondemnDistance - WallOffset)).To3D();

                // check if condem will hit wall
                var willhit = NavMesh.GetCollisionFlags(condemEndPos).HasFlag(CollisionFlags.Wall) ||
                              NavMesh.GetCollisionFlags(condemEndPos).HasFlag(CollisionFlags.Building);

                if (!willhit && Wall.IsInRange(wallPos))
                {
                    Wall.Cast(wallPos, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    internal static class Extensions
    {
        public static bool IsValid<T>(this GameObject obj)
        {
            return obj.IsValid && obj is T;
        }

        public static bool IsInRange(this Spell spell, Vector3 pos)
        {
            return ObjectManager.Player.Distance(pos) < spell.Range;
        }

        public static void PrintMessage(string message)
        {
            Game.PrintChat("<font color='#15C3AC'>VayNivia:</font> <font color='#FFFFFF'>" + message + "</font>");
        }
    }
}
