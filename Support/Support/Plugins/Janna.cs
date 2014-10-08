#region LICENSE

//  Copyright 2014 - 2014 Support
//  Janna.cs is part of Support.
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
    public class Janna : PluginBase
    {
        public Janna()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 600);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 700);

            Q.SetSkillshot(0.5f, 150f, 900f, false, SkillshotType.SkillshotLine);
            GameObject.OnCreate += GameObjectOnOnCreate;
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "ComboQ"))
                {
                    var pred = Q.GetPrediction(Target);
                    if (pred.Hitchance > HitChance.Medium)
                    {
                        Q.Cast(pred.CastPosition, UsePackets);
                        Q.Cast(pred.CastPosition, UsePackets);
                    }
                }

                if (W.IsValidTarget(Target, "ComboW"))
                {
                    W.Cast(Target, UsePackets);
                }

                var ally = Helpers.AllyBelowHp(GetValue<Slider>("ComboHealthR").Value, R.Range);
                if (R.IsValidTarget(ally, "ComboR", true, false))
                {
                    R.Cast();
                }
            }

            if (HarassMode)
            {
                if (W.IsValidTarget(Target, "HarassW"))
                {
                    W.Cast(Target, UsePackets);
                }
            }
        }

        private void GameObjectOnOnCreate(GameObject sender, EventArgs args)
        {
            if (sender is Obj_SpellMissile && sender.IsValid)
            {
                var missile = (Obj_SpellMissile) sender;

                // Ally Turret -> Enemy Hero
                if (missile.SpellCaster is Obj_AI_Turret && missile.SpellCaster.IsValid && missile.SpellCaster.IsAlly &&
                    missile.Target is Obj_AI_Hero && missile.Target.IsValid && missile.Target.IsEnemy)
                {
                    var turret = (Obj_AI_Turret) missile.SpellCaster;

                    if (ProtectionMana && E.IsReady())
                    {
                        if (E.IsInRange(turret))
                        {
                            E.Cast(turret, UsePackets);
                        }
                    }
                }

                //// Ally Hero special attack
                //if (missile.SpellCaster is Obj_AI_Hero && missile.SpellCaster.IsValid && missile.SpellCaster.IsAlly)
                //{
                //}
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (Q.IsValidTarget(gapcloser.Sender, "GapcloserQ"))
            {
                var pred = Q.GetPrediction(gapcloser.Sender);
                if (pred.Hitchance > HitChance.Medium)
                {
                    Q.Cast(pred.CastPosition, UsePackets);
                    Q.Cast(pred.CastPosition, UsePackets);
                }
            }

            if (W.IsValidTarget(gapcloser.Sender, "GapcloserW"))
            {
                W.Cast(gapcloser.Sender, UsePackets);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if ((spell.DangerLevel < InterruptableDangerLevel.High && !unit.BaseSkinName.Contains("Thresh")) ||
                unit.IsAlly)
                return;

            if (Q.IsValidTarget(unit, "InterruptQ"))
            {
                var pred = Q.GetPrediction(unit);
                if (pred.Hitchance > HitChance.Medium)
                {
                    Q.Cast(pred.CastPosition, UsePackets);
                    Q.Cast(pred.CastPosition, UsePackets);
                }
            }

            if (!Q.IsReady() && R.IsValidTarget(unit, "InterruptR"))
            {
                R.Cast();
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            config.AddBool("ComboR", "Use R", true);
            config.AddSlider("ComboHealthR", "Health to Ult", 15, 1, 100);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassW", "Use W", true);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserQ", "Use Q to Interrupt Gapcloser", true);
            config.AddBool("GapcloserW", "Use W to Interrupt Gapcloser", true);

            config.AddBool("InterruptQ", "Use Q to Interrupt Spells", true);
            config.AddBool("InterruptR", "Use R to Interrupt Spells", true);
        }

        //public override void OnDraw(EventArgs args)
        //{
        //    try
        //    {
        //        if (!Target.IsValidTarget())
        //            return;
        //        var pred = Q.GetPrediction(Target);
        //        Utility.DrawCircle(pred.CastPosition, 100, Color.Green);
        //        Utility.DrawCircle(pred.UnitPosition, 100, Color.Red);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}
    }
}