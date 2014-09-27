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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using LeagueSharp.Common;

namespace LeagueSharp.OrbwalkerPlugins
{
    public class JannaDisabled : OrbwalkerPluginBase
    {
        public JannaDisabled()
            : base("by h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 1700);
            W = new Spell(SpellSlot.W, 600);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 725);

            Q.SetSkillshot(0.5f, 200f, 900f, false, SkillshotType.SkillshotLine);
            Q.SetCharged("HowlingGale", "HowlingGale", 1100, 1700, 1.5f);
            W.SetTargetted(0.5f, 1000f);
        }

        public override void OnLoad(EventArgs args)
        {
        }

        public override void OnUpdate(EventArgs args)
        {
            try
            {
                if (ActiveMode == Orbwalking.OrbwalkingMode.Combo)
                {
                    if (Q.IsReady() && Target.IsValidTarget(Q.ChargedMaxRange))
                    {
                        Console.WriteLine("Cast: Q " + Target.Name);
                        Q.Cast(Target, true);
                        Utility.DelayAction.Add(100, () => Q.Cast(Target, true));
                    }

                    if (W.IsReady() && Target.IsValidTarget(W.Range))
                    {
                        W.Cast(Target, true);
                        Console.WriteLine("Cast: W " + Target.Name);
                    }

                    //if (R.IsReady() && Utility.CountEnemysInRange((int)Player.AttackRange) > GetValue<Slider>("CountR").Value)
                    //{
                    //    if (ObjectManager.Player.Health < ObjectManager.Player.MaxHealth*GetValue<Slider>("HealthR").Value/100)
                    //    {
                    //        R.Cast();
                    //        Console.WriteLine("Cast: R " + Target.Name);
                    //    }
                    //}
                }

                if (ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
                {
                    if (W.IsReady() && Target.IsValidTarget(W.Range))
                    {
                        W.Cast(Target, true);
                        Console.WriteLine("Cast: W " + Target.Name);
                    }

                    if (E.IsReady() && GetValue<bool>("UseEH"))
                    {
                        // TODO: shield ally
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High)
                return;

            if (GetValue<bool>("InterruptQ") && unit.IsValidTarget(Q.Range) && Q.IsReady())
            {
                Q.StartCharging();
                Utility.DelayAction.Add(100, () => Q.Cast(unit, true));
                return;
            }

            if (GetValue<bool>("InterruptR") && unit.IsValidTarget(R.Range) && R.IsReady() && !Q.IsReady())
            {
                R.Cast();
            }
        }

        public override void OnDraw(EventArgs args)
        {
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC", "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC", "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC", "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC", "Use R").SetValue(true));
            config.AddItem(new MenuItem("CountR", "Ult enemys in Range").SetValue(new Slider(2, 1, 5)));
            config.AddItem(new MenuItem("HealthR", "Ult lower than % HP").SetValue(new Slider(30, 1, 100)));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseWH", "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEH", "Use E").SetValue(true));
        }

        public override void ItemMenu(Menu config)
        {
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("InterruptQ", "Use Q to Interrupt Spells").SetValue(true));
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
