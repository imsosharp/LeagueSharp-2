/*
    Copyright (C) 2014 h3h3

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/


using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;

namespace Support
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += a =>
            {
                try
                {
                    var type = Type.GetType("Support.Plugins." + ObjectManager.Player.ChampionName);

                    if (type != null)
                    {
                        Activator.CreateInstance(type);
                        return;
                    }

                    Utils.PrintMessage(ObjectManager.Player.ChampionName + " not supported");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            };

            Protector.OnSkillshotProtection += Protector_OnSkillshotProtection;
            Protector.OnTargetedProtection += Protector_OnTargetedProtection;
        }

        static void Protector_OnTargetedProtection(Obj_AI_Base caster, Obj_AI_Hero target, SpellData spell)
        {
            Console.WriteLine("{0} -> {1} - {2} {3}",
                caster.Name,
                target.Name,
                spell.Name,
                Math.Round(caster.GetSpellDamage(target, spell.Name)));
        }

        static void Protector_OnSkillshotProtection(Obj_AI_Hero target, List<Evade.Skillshot> skillshots)
        {
            foreach (var skillshot in skillshots)
            {
                Console.WriteLine("{0} -> {1} - {2} {3}",
                    skillshot.Unit.Name,
                    target.Name,
                    skillshot.SpellData.SpellName,
                    Math.Round(skillshot.Unit.GetSpellDamage(target, skillshot.SpellData.SpellName)));
            }
        }
    }
}