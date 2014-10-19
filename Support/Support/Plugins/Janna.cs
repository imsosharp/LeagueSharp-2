#region LICENSE

// /*
// Copyright 2014 - 2014 Support
// Janna.cs is part of Support.
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
    public class Janna : PluginBase
    {
        public Janna()
        {
            Q = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 600);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 700);

            Q.SetSkillshot(0.5f, 150f, 900f, false, SkillshotType.SkillshotLine);
            GameObject.OnCreate += GameObjectOnCreate;
            GameObject.OnCreate += RangeAttackOnCreate;
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.CastCheck(Target, "ComboQ"))
                {
                    var pred = Q.GetPrediction(Target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        Q.Cast(pred.CastPosition, UsePackets);
                        Q.Cast(pred.CastPosition, UsePackets);
                    }
                }

                if (W.CastCheck(Target, "ComboW"))
                {
                    W.CastOnUnit(Target, UsePackets);
                }

                var ally = Helpers.AllyBelowHp(ConfigValue<Slider>("ComboHealthR").Value, R.Range);
                if (R.CastCheck(ally, "ComboR", true, false))
                {
                    R.Cast();
                }
            }

            if (HarassMode)
            {
                if (W.CastCheck(Target, "HarassW"))
                {
                    W.CastOnUnit(Target, UsePackets);
                }
            }
        }

        private void RangeAttackOnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.IsValid<Obj_SpellMissile>())
                return;

            var missile = (Obj_SpellMissile) sender;

            // Caster ally hero / not me
            if (!missile.SpellCaster.IsValid<Obj_AI_Hero>() || !missile.SpellCaster.IsAlly ||
                missile.SpellCaster.IsMe || !missile.SpellCaster.IsHeroType(HeroType.Ad) ||
                missile.SpellCaster.IsMelee())
                return;

            // Target enemy hero
            if (!missile.Target.IsValid<Obj_AI_Hero>() || !missile.Target.IsEnemy)
                return;

            // only in SBTW mode
            if (E.IsReady() && E.IsInRange(missile.SpellCaster) && (ComboMode || HarassMode) &&
                ConfigValue<bool>("MiscE"))
            {
                E.CastOnUnit(missile.SpellCaster, UsePackets);
            }
        }

        private void GameObjectOnCreate(GameObject sender, EventArgs args)
        {
            if (sender is Obj_SpellMissile && sender.IsValid)
            {
                var missile = (Obj_SpellMissile) sender;

                // Ally Turret -> Enemy Hero
                if (missile.SpellCaster.IsValid<Obj_AI_Turret>() && missile.SpellCaster.IsAlly &&
                    missile.Target.IsValid<Obj_AI_Hero>() && missile.Target.IsEnemy)
                {
                    var turret = (Obj_AI_Turret) missile.SpellCaster;

                    if (E.IsReady())
                    {
                        if (E.IsInRange(turret))
                        {
                            E.CastOnUnit(turret, UsePackets);
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

            if (Q.CastCheck(gapcloser.Sender, "GapcloserQ"))
            {
                var pred = Q.GetPrediction(gapcloser.Sender);
                if (pred.Hitchance >= HitChance.High)
                {
                    Q.Cast(pred.CastPosition, UsePackets);
                    Q.Cast(pred.CastPosition, UsePackets);
                }
            }

            if (W.CastCheck(gapcloser.Sender, "GapcloserW"))
            {
                W.CastOnUnit(gapcloser.Sender, UsePackets);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if ((spell.DangerLevel < InterruptableDangerLevel.High && !unit.BaseSkinName.Contains("Thresh")) ||
                unit.IsAlly)
                return;

            if (Q.CastCheck(unit, "InterruptQ"))
            {
                var pred = Q.GetPrediction(unit);
                if (pred.Hitchance >= HitChance.High)
                {
                    Q.Cast(pred.CastPosition, UsePackets);
                    Q.Cast(pred.CastPosition, UsePackets);
                }
            }

            if (!Q.IsReady() && R.CastCheck(unit, "InterruptR"))
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
            config.AddBool("MiscE", "Use E on ADC Attacks", false);
        }

        public override void InterruptMenu(Menu config)
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