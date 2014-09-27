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
using LeagueSharp.Common;

namespace LeagueSharp.OrbwalkerPlugins
{
    public class Braum : OrbwalkerPluginBase
    {
        public Braum()
            : base("by h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 1000);
            W = new Spell(SpellSlot.W, 650);
            E = new Spell(SpellSlot.E, 0);
            R = new Spell(SpellSlot.R, 1250);

            Q.SetSkillshot(0.3333f, 70f, 1200f, true, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.5f, 80f, 1200f, false, SkillshotType.SkillshotLine);
        }

        public override void OnLoad(EventArgs args)
        {
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                if (Q.IsReady() && Target.IsValidTarget(Q.Range) && GetValue<bool>("UseQC"))
                {
                    Q.Cast(Target, true);
                }

                if (R.IsReady() && Target.IsValidTarget(R.Range) && GetValue<bool>("UseRC"))
                {
                    R.CastIfWillHit(Target, GetValue<Slider>("CountR").Value, true);
                }
            }

            if (ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                if (Q.IsReady() && Target.IsValidTarget(Q.Range) && GetValue<bool>("UseQH"))
                {
                    Q.Cast(Target, true);
                }
            }
        }

        public override void BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
        }

        public override void AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsValidTarget(Q.Range) && Q.IsReady())
            {
                Q.Cast(gapcloser.Sender, true);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High)
                return;

            if (unit.IsValidTarget(R.Range) && R.IsReady() && GetValue<bool>("InterruptR"))
            {
                R.Cast(unit, true);
            }
        }

        public override void OnDraw(EventArgs args)
        {
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC", "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseRC", "Use R").SetValue(true));
            config.AddItem(new MenuItem("CountR", "Num of Enemy in Range to Ult").SetValue(new Slider(2, 0, 5)));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH", "Use Q").SetValue(true));
        }

        public override void ItemMenu(Menu config)
        {
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("InterruptR", "Use R to Interrupt Spells").SetValue(true));
        }

        public override void ManaMenu(Menu config)
        {
        }

        public override void DrawingMenu(Menu config)
        {
        }
    }
}
