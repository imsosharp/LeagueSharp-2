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

namespace Support.Plugins
{
    public class Zilean : PluginBase
    {
        public Zilean()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 700);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, 700);
            R = new Spell(SpellSlot.R, 900);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "ComboQ"))
                {
                    Q.Cast(Target);
                }

                if (W.IsReady() && !Q.IsReady() && GetValue<bool>("ComboW"))
                {
                    W.Cast();
                }

                if (E.IsValidTarget(Target, "ComboE"))
                {
                    // TODO: speed adc/jungler/engage
                }
            }

            if (HarassMode)
            {
                if (Q.IsValidTarget(Target, "HarassQ"))
                {
                    Q.Cast(Target);
                }

                if (W.IsReady() && !Q.IsReady() && GetValue<bool>("HarassW"))
                {
                    W.Cast();
                }
            }
        }

        public override void OnTargetedProtection(Obj_AI_Base caster, Obj_AI_Hero target, SpellData spell)
        {
            if (!R.IsValidTarget(target, true, false))
                return;

            if (caster.WillKill(target, spell))
                R.Cast(target, true);
        }

        public override void OnSkillshotProtection(Obj_AI_Hero target, List<Evade.Skillshot> skillshots)
        {
            foreach (var skillshot in skillshots)
            {
                if (!R.IsValidTarget(target, true, false))
                    continue;

                if (skillshot.Unit.WillKill(target, skillshot.SpellData))
                    R.Cast(target, true);
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (E.IsValidTarget(gapcloser.Sender, "GapcloserE"))
            {
                E.Cast(gapcloser.Sender);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            //config.AddBool("ComboE", "Use E", true);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
            config.AddBool("HarassW", "Use W", true);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserE", "Use E to Interrupt Gapcloser", true);
        }
    }
}