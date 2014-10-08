#region LICENSE

//  Copyright 2014 - 2014 Support
//  Nami.cs is part of Support.
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
    public class Nami : PluginBase
    {
        public Nami()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 875);
            W = new Spell(SpellSlot.W, 725);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 2200);

            Q.SetSkillshot(1.0f, 200f, Int32.MaxValue, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.5f, 325f, 1200f, false, SkillshotType.SkillshotLine);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "ComboQ"))
                {
                    Q.Cast(Target, UsePackets);
                }

                var ally = Helpers.AllyBelowHp(GetValue<Slider>("ComboHealthW").Value, W.Range);
                if (W.IsValidTarget(ally, "ComboR", true, false))
                {
                    W.Cast(ally, UsePackets);
                }

                if (W.IsValidTarget(Target, "ComboW"))
                {
                    W.Cast(Target, UsePackets);
                }

                //if (E.IsValidTarget(Target, "ComboE"))
                //{
                //    // TODO: Buff ally
                //}

                if (R.IsValidTarget(Target, "ComboR"))
                {
                    R.CastIfWillHit(Target, GetValue<Slider>("ComboCountR").Value, UsePackets);
                }
            }

            if (HarassMode)
            {
                if (Q.IsValidTarget(Target, "HarassQ"))
                {
                    Q.Cast(Target, UsePackets);
                }

                var ally = Helpers.AllyBelowHp(GetValue<Slider>("HarassHealthW").Value, W.Range);
                if (W.IsValidTarget(ally, "HarassW", true, false))
                {
                    W.Cast(ally, UsePackets);
                }

                if (W.IsValidTarget(Target, "HarassW"))
                {
                    W.Cast(Target, UsePackets);
                }
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (R.IsValidTarget(gapcloser.Sender, "GapcloserR"))
            {
                R.Cast(gapcloser.Sender, UsePackets);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High || unit.IsAlly)
                return;

            if (Q.IsValidTarget(unit, "InterruptQ"))
            {
                Q.Cast(unit, UsePackets);
            }

            if (R.IsValidTarget(unit, "InterruptR"))
            {
                R.Cast(unit, UsePackets);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            //config.AddBool("ComboE", "Use E", true);
            config.AddBool("ComboR", "Use R", true);
            config.AddSlider("ComboCountR", "Targets in range to Ult", 2, 1, 5);
            config.AddSlider("ComboHealthW", "Health to Heal", 20, 1, 100);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
            config.AddBool("HarassW", "Use W", true);
            config.AddSlider("HarassHealthW", "Health to Heal", 20, 1, 100);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserR", "Use R to Interrupt Gapcloser", true);

            config.AddBool("InterruptQ", "Use Q to Interrupt Spells", true);
            config.AddBool("InterruptR", "Use R to Interrupt Spells", true);
        }
    }
}