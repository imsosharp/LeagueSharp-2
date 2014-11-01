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
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace VayNivia
{
    public class Program
    {
        public static Spell Wall = new Spell(SpellSlot.W, 1000);
        public static Spell Condemn = new Spell(SpellSlot.E, 550);
        public static Menu Config = new Menu("VayNivia", "VayNivia", true);

        public static bool CondemnKey
        {
            get { return Config.Item("Condemn.Key").GetValue<KeyBind>().Active; }
        }

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                if (
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(h => h.IsAlly)
                        .Count(h => h.ChampionName == "Anivia" || h.ChampionName == "Vayne") == 2)
                {
                    if (ObjectManager.Player.ChampionName == "Anivia")
                    {
                        Obj_AI_Base.OnProcessSpellCast += AniviaIntegration;
                        Extensions.PrintMessage("Aniva by h3h3 loaded.");
                    }

                    if (ObjectManager.Player.ChampionName == "Vayne")
                    {
                        Config.AddItem(new MenuItem("Condemn.Key", "Condemn Key").SetValue(new KeyBind(32, KeyBindType.Press)));
                        Config.AddToMainMenu();

                        Game.OnGameUpdate += VayneIntegration;
                        Extensions.PrintMessage("Vayne by h3h3 loaded.");
                    }
                }
            };
        }

        private static void VayneIntegration(EventArgs args)
        {
            try
            {
                if (!Condemn.IsReady() || ObjectManager.Player.IsDead || !CondemnKey)
                    return;

                var anivia =
                    ObjectManager.Get<Obj_AI_Hero>()
                        .SingleOrDefault(h => h.IsAlly && !h.IsDead && h.ChampionName == "Anivia");
                var target = SimpleTs.GetTarget(Condemn.Range, SimpleTs.DamageType.Physical);

                if (target.IsValidTarget(Condemn.Range) && anivia != null &&
                    anivia.Spellbook.GetSpell(SpellSlot.W).State == SpellState.Ready)
                {
                    var condemEndPosMax =
                        target.ServerPosition.To2D()
                            .Extend(ObjectManager.Player.ServerPosition.To2D(), -450)
                            .To3D();

                    var condemEndPosMin =
                        target.ServerPosition.To2D()
                            .Extend(ObjectManager.Player.ServerPosition.To2D(), -100)
                            .To3D();

                    if (anivia.Distance(condemEndPosMax) < 990 || anivia.Distance(condemEndPosMin) < 990)
                    {
                        Condemn.CastOnUnit(target, true);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void AniviaIntegration(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (!Wall.IsReady() || ObjectManager.Player.IsDead)
                    return;

                if (!sender.IsValid<Obj_AI_Hero>() || !args.Target.IsValid<Obj_AI_Hero>() || sender.IsEnemy ||
                    args.SData.Name != "VayneCondemn")
                    return;

                var condemEndPos = args.End.To2D().Extend(sender.ServerPosition.To2D(), -450).To3D();
                var wallPosMin = args.End.To2D().Extend(sender.ServerPosition.To2D(), -100).To3D();
                var wallPosMax = args.End.To2D().Extend(sender.ServerPosition.To2D(), -450).To3D();

                // check if condem will hit wall
                var willhit =
                    NavMesh.GetCollisionFlags(condemEndPos).HasFlag(CollisionFlags.Wall | CollisionFlags.Building);

                if (!willhit && Wall.IsInRange(wallPosMin))
                {
                    Wall.Cast(wallPosMin, true);
                    return;
                }

                if (!willhit && Wall.IsInRange(wallPosMax))
                {
                    Wall.Cast(wallPosMax, true);
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