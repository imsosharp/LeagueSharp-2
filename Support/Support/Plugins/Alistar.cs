using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using Support.Evade;
using SpellData = LeagueSharp.SpellData;

namespace Support.Plugins
{
    public class Alistar : PluginBase
    {
        public Alistar()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 365);
            W = new Spell(SpellSlot.W, 650);
            E = new Spell(SpellSlot.E, 575);
            R = new Spell(SpellSlot.R, 0);
            Protector.Init();
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "ComboQ"))
                {
                    Q.Cast();
                }

                if (Q.IsReady() && W.IsValidTarget(Target, "ComboW"))
                {
                    if (W.Cast(Target, true) == Spell.CastStates.SuccessfullyCasted)
                        Utility.DelayAction.Add(100, () => Q.Cast()); // TODO: calc timing
                }

                var ally = Utils.AllyBelowHp(GetValue<Slider>("ComboHealthE").Value, E.Range);
                if (E.IsValidTarget(ally, "ComboE", true, false))
                {
                    E.Cast();
                }
            }

            if (HarassMode)
            {
                if (Q.IsValidTarget(Target, "HarassQ"))
                {
                    Q.Cast();
                }

                var ally = Utils.AllyBelowHp(GetValue<Slider>("HarassHealthR").Value, E.Range);
                if (E.IsValidTarget(ally, "HarassE", true, false))
                {
                    E.Cast();
                }
            }
        }

        public override void OnTargetedProtection(Obj_AI_Base caster, Obj_AI_Hero target, SpellData spell)
        {
            if (!E.IsReady())
                return;

            if (E.IsReady() && E.IsInRange(target) && caster.WillKill(target, spell))
                E.Cast();
        }

        public override void OnSkillshotProtection(Obj_AI_Hero target, List<Skillshot> skillshots)
        {
            if (!E.IsValidTarget(target, true, false))
                return;

            foreach (var skillshot in skillshots)
            {
                if (skillshot.Unit.WillKill(target, skillshot.SpellData))
                    E.Cast();
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (Q.IsValidTarget(gapcloser.Sender, "GapcloserQ"))
            {
                Q.Cast();
            }

            if (W.IsValidTarget(gapcloser.Sender, "GapcloserW"))
            {
                W.Cast(gapcloser.Sender, true);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High || unit.IsAlly)
                return;

            if (Q.IsValidTarget(unit, "InterruptQ"))
            {
                Q.Cast();
            }

            if (W.IsValidTarget(unit, "InterruptW"))
            {
                W.Cast(unit, true);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use WQ", true);
            config.AddBool("ComboE", "Use E", true);
            config.AddSlider("ComboHealthR", "Health to Heal", 20, 1, 100);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
            config.AddBool("HarassE", "Use E", true);
            config.AddSlider("HarassHealthR", "Health to Heal", 20, 1, 100);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserQ", "Use Q to Interrupt Gapcloser", true);
            config.AddBool("GapcloserW", "Use W to Interrupt Gapcloser", true);

            config.AddBool("InterruptQ", "Use Q to Interrupt Spells", true);
            config.AddBool("InterruptW", "Use W to Interrupt Spells", true);
        }
    }
}