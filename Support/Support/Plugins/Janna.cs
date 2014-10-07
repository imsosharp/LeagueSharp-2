using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using Support.Evade;
using SpellData = LeagueSharp.SpellData;

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
                    if (Q.Cast(Target) == Spell.CastStates.SuccessfullyCasted)
                        Q.Cast();
                }

                if (W.IsValidTarget(Target, "ComboW"))
                {
                    W.Cast(Target, true);
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
                    W.Cast(Target, true);
                }
            }
        }

        private void GameObjectOnOnCreate(GameObject sender, EventArgs args)
        {
            if (sender is Obj_SpellMissile && sender.IsValid)
            {
                var missile = (Obj_SpellMissile)sender;

                // Ally Turret -> Enemy Hero
                if (missile.SpellCaster is Obj_AI_Turret && missile.SpellCaster.IsValid && missile.SpellCaster.IsAlly &&
                    missile.Target is Obj_AI_Hero && missile.Target.IsValid && missile.Target.IsEnemy)
                {
                    var turret = (Obj_AI_Turret)missile.SpellCaster;

                    if (ProtectionMana && E.IsReady())
                    {
                        if (E.IsInRange(turret))
                        {
                            E.Cast(turret, true);
                        }
                    }
                }

                //// Ally Hero special attack
                //if (missile.SpellCaster is Obj_AI_Hero && missile.SpellCaster.IsValid && missile.SpellCaster.IsAlly)
                //{
                //}
            }
        }

        public override void OnSkillshotProtection(Obj_AI_Hero target, List<Skillshot> skillshots)
        {
            if (ProtectionMana && E.IsReady())
            {
                if (E.IsInRange(target))
                {
                    E.Cast(target, true);
                }
            }
        }

        public override void OnTargetedProtection(Obj_AI_Base caster, Obj_AI_Hero target, SpellData spell)
        {
            if (ProtectionMana && E.IsReady())
            {
                if (E.IsInRange(target))
                {
                    E.Cast(target, true);
                }
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (Q.IsValidTarget(gapcloser.Sender, "GapcloserQ"))
            {
                Q.Cast(gapcloser.Sender);
                Q.Cast();
            }

            if (W.IsValidTarget(gapcloser.Sender, "GapcloserW"))
            {
                W.Cast(gapcloser.Sender, true);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if ((spell.DangerLevel < InterruptableDangerLevel.High && !unit.BaseSkinName.Contains("Thresh")) || unit.IsAlly)
                return;

            if (Q.IsValidTarget(unit, "InterruptQ"))
            {
                Q.Cast(Target);
                Q.Cast();
                return;
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
    }
}