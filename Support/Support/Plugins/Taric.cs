#region LICENSE

//  Copyright 2014 - 2014 Support
//  Taric.cs is part of Support.
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

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Support.Plugins
{
    public class Taric : PluginBase
    {
        public Taric()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 750);
            W = new Spell(SpellSlot.W, 200);
            E = new Spell(SpellSlot.E, 625);
            R = new Spell(SpellSlot.R, 200);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                var ally = Helpers.AllyBelowHp(GetValue<Slider>("ComboHealthQ").Value, Q.Range);
                if (Q.IsValidTarget(ally, "ComboQ", true, false))
                {
                    Q.Cast(ally, UsePackets);
                }

                if (W.IsValidTarget(Target, "ComboW"))
                {
                    W.Cast();
                }

                if (E.IsValidTarget(Target, "ComboE"))
                {
                    E.Cast(Target, UsePackets);
                }

                if (R.IsValidTarget(Target, "ComboR"))
                {
                    R.Cast();
                }
            }

            if (HarassMode)
            {
                var ally = Helpers.AllyBelowHp(GetValue<Slider>("HarassHealthQ").Value, Q.Range);
                if (Q.IsValidTarget(ally, "HarassQ", true, false))
                {
                    Q.Cast(ally, UsePackets);
                }

                if (E.IsValidTarget(Target, "HarassE"))
                {
                    E.Cast(Target, UsePackets);
                }
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (E.IsValidTarget(gapcloser.Sender, "GapcloserE"))
            {
                E.Cast(gapcloser.Sender, UsePackets);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High || unit.IsAlly)
                return;

            if (E.IsValidTarget(unit, "InterruptE"))
            {
                E.Cast(unit, UsePackets);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            config.AddBool("ComboE", "Use E", true);
            config.AddBool("ComboR", "Use R", true);
            config.AddSlider("ComboHealthQ", "Health to Heal", 20, 1, 100);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
            config.AddBool("HarassE", "Use E", true);
            config.AddSlider("HarassHealthQ", "Health to Heal", 20, 1, 100);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserE", "Use E to Interrupt Gapcloser", true);

            config.AddBool("InterruptE", "Use E to Interrupt Spells", true);
        }
    }
}