#region LICENSE

// /*
// Copyright 2014 - 2014 Support
// Blitzcrank.cs is part of Support.
// Support is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// Support is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with Support. If not, see <http://www.gnu.org/licenses/>.
// */
// 

#endregion

#region

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Support.Plugins
{
    public class Blitzcrank : PluginBase
    {
        public Blitzcrank()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 950);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, AttackRange);
            R = new Spell(SpellSlot.R, 600);

            Q.SetSkillshot(0.25f, 70f, 1800f, true, SkillshotType.SkillshotLine);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "ComboQ"))
                {
                    Q.Cast(Target, UsePackets);
                }

                if (E.IsValidTarget(Target))
                {
                    if (E.Cast())
                    {
                        Orbwalking.ResetAutoAttackTimer();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                    }
                }

                if (E.IsReady() && Target.IsValidTarget() && Target.HasBuff("RocketGrab"))
                {
                    if (E.Cast())
                    {
                        Orbwalking.ResetAutoAttackTimer();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                    }
                }

                if (R.IsValidTarget(Target, "ComboR"))
                {
                    if (Helpers.EnemyInRange(ConfigValue<Slider>("ComboCountR").Value, R.Range))
                        R.Cast();
                }
            }

            if (HarassMode)
            {
                if (Q.IsValidTarget(Target, "HarassQ"))
                {
                    Q.Cast(Target, UsePackets);
                }

                if (E.IsValidTarget(Target))
                {
                    if (E.Cast())
                    {
                        Orbwalking.ResetAutoAttackTimer();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                    }
                }

                if (E.IsReady() && Target.IsValidTarget() && Target.HasBuff("RocketGrab"))
                {
                    if (E.Cast())
                    {
                        Orbwalking.ResetAutoAttackTimer();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                    }
                }
            }
        }

        public override void AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (!unit.IsMe)
                return;

            if (!(target is Obj_AI_Hero) && !target.Name.ToLower().Contains("ward"))
                return;

            if (!E.IsReady())
                return;

            if (E.Cast())
            {
                Orbwalking.ResetAutoAttackTimer();
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (E.IsValidTarget(gapcloser.Sender, "GapcloserE"))
            {
                if (E.Cast())
                {
                    Orbwalking.ResetAutoAttackTimer();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, gapcloser.Sender);
                }
            }

            if (R.IsValidTarget(gapcloser.Sender, "GapcloserR"))
            {
                R.Cast();
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High || unit.IsAlly)
                return;

            if (E.IsValidTarget(unit, "InterruptE"))
            {
                if (E.Cast())
                {
                    Orbwalking.ResetAutoAttackTimer();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, unit);
                }
            }
            if (Q.IsValidTarget(Target, "InterruptQ"))
            {
                Q.Cast(unit, UsePackets);
            }
            if (R.IsValidTarget(unit, "InterruptR"))
            {
                R.Cast();
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q/E", true);
            config.AddBool("ComboR", "Use R", true);
            config.AddSlider("ComboCountR", "Targets in range to Ult", 2, 1, 5);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q/E", true);
        }

        public override void InterruptMenu(Menu config)
        {
            config.AddBool("GapcloserE", "Use E to Interrupt Gapcloser", true);
            config.AddBool("GapcloserR", "Use R to Interrupt Gapcloser", true);
            config.AddBool("InterruptQ", "Use Q to Interrupt Spells", true);
            config.AddBool("InterruptE", "Use E to Interrupt Spells", true);
            config.AddBool("InterruptR", "Use R to Interrupt Spells", true);
        }
    }
}