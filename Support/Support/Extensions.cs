#region LICENSE

//  Copyright 2014 - 2014 Support
//  Extensions.cs is part of Support.
//  
//  Support is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  
//  Support is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with Support. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Support
{
    public static class Extensions
    {
        public static double HealthBuffer(this Obj_AI_Base hero, int buffer)
        {
            return ObjectManager.Player.Health*(1 - buffer/100);
        }

        public static bool IsValidTarget(this Items.Item item, Obj_AI_Base target)
        {
            return item != null && item.IsReady() && target.IsValidTarget(item.Range);
        }

        public static bool IsValidTarget(this Spell spell, Obj_AI_Base target, string menu, bool range = true,
            bool team = true)
        {
            return
                spell.IsReady() &&
                target.IsValidTarget(range ? spell.Range : float.MaxValue, team) &&
                PluginBase.Config.Item(menu + ObjectManager.Player.ChampionName).GetValue<bool>();
        }

        public static bool IsValidTarget(this Spell spell, Obj_AI_Base target, bool range = true, bool team = true)
        {
            return
                spell.IsReady() &&
                target.IsValidTarget(range ? spell.Range : float.MaxValue, team);
        }

        public static bool IsInRange(this Spell spell, Obj_AI_Base target)
        {
            return ObjectManager.Player.Distance(target) < spell.Range;
        }

        public static bool IsInRange(this Items.Item item, Obj_AI_Base target)
        {
            return ObjectManager.Player.Distance(target) < item.Range;
        }

        public static bool WillKill(this Obj_AI_Base caster, Obj_AI_Base target, string spell, int buffer = 20)
        {
            return caster.GetSpellDamage(target, spell) >= target.HealthBuffer(buffer);
        }

        public static bool WillKill(this Obj_AI_Base caster, Obj_AI_Base target, SpellData spell, int buffer = 20)
        {
            return caster.GetSpellDamage(target, spell.Name) >= target.HealthBuffer(buffer);
        }

        public static bool WillKill(this Obj_AI_Base caster, Obj_AI_Base target, Evade.SpellData spell, int buffer = 20)
        {
            return caster.GetSpellDamage(target, spell.SpellName) >= target.HealthBuffer(buffer);
        }

        public static void AddBool(this Menu menu, string name, string displayName, bool value)
        {
            menu.AddItem(new MenuItem(name + ObjectManager.Player.ChampionName, displayName).SetValue(value));
        }

        public static void AddSlider(this Menu menu, string name, string displayName, int value, int min, int max)
        {
            menu.AddItem(
                new MenuItem(name + ObjectManager.Player.ChampionName, displayName).SetValue(new Slider(value, min, max)));
        }
    }
}