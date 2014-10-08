using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using Support.Evade;
using SpellData = LeagueSharp.SpellData;

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

        public override void OnTargetedProtection(Obj_AI_Base caster, Obj_AI_Hero target, SpellData spell)
        {
            if (!W.IsReady())
                return;

            if (W.IsReady() && W.IsInRange(target) && caster.WillKill(target, spell))
                W.Cast(target, UsePackets);
        }

        public override void OnSkillshotProtection(Obj_AI_Hero target, List<Skillshot> skillshots)
        {
            if (!W.IsValidTarget(target, true, false))
                return;

            foreach (var skillshot in skillshots)
            {
                if (skillshot.Unit.WillKill(target, skillshot.SpellData))
                    W.Cast(target, UsePackets);
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