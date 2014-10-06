using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;

namespace Support.Plugins
{
    public class Taric : PluginBase
    {
        public Taric()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 750);
            W = new Spell(SpellSlot.W, 200);
            E = new Spell(SpellSlot.E, 625);
            R = new Spell(SpellSlot.R, 200);
            Protector.Init();
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                var ally = Utils.AllyBelowHp(GetValue<Slider>("ComboHealthQ").Value, Q.Range);
                if (Q.IsValidTarget(ally, "ComboQ", true, false))
                {
                    Q.Cast(ally, true);
                }

                if (W.IsValidTarget(Target, "ComboW"))
                {
                    W.Cast();
                }

                if (E.IsValidTarget(Target, "ComboE"))
                {
                    E.Cast(Target, true);
                }

                if (R.IsValidTarget(Target, "ComboR"))
                {
                    R.Cast();
                }
            }

            if (HarassMode)
            {
                var ally = Utils.AllyBelowHp(GetValue<Slider>("HarassHealthQ").Value, Q.Range);
                if (Q.IsValidTarget(ally, "HarassQ", true, false))
                {
                    Q.Cast(ally, true);
                }

                if (E.IsValidTarget(Target, "HarassE"))
                {
                    E.Cast(Target, true);
                }
            }
        }

        public override void OnTargetedProtection(Obj_AI_Base caster, Obj_AI_Hero target, SpellData spell)
        {
            if (!Q.IsValidTarget(target, true, false))
                return;

            if (caster.WillKill(target, spell))
                Q.Cast(target, true);
        }

        public override void OnSkillshotProtection(Obj_AI_Hero target, List<Evade.Skillshot> skillshots)
        {
            if (!Q.IsValidTarget(target, true, false))
                return;

            foreach (var skillshot in skillshots)
            {
                if (skillshot.Unit.WillKill(target, skillshot.SpellData))
                    Q.Cast(target, true);
            }
        }


        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (E.IsValidTarget(gapcloser.Sender, "GapcloserE"))
            {
                E.Cast(gapcloser.Sender, true);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High || unit.IsAlly)
                return;

            if (E.IsValidTarget(unit, "InterruptE"))
            {
                E.Cast(unit, true);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            config.AddBool("ComboE", "Use E", true);
            config.AddBool("ComboR", "Use R", true);
            config.AddSlider("ComboHealthQ", "Health to Heal", 20, 1, 100);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
            config.AddBool("HarassE", "Use E", true);
            config.AddSlider("HarassHealthQ", "Health to Heal", 20, 1, 100);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserE", "Use E to Interrupt Gapcloser", true);

            config.AddBool("InterruptE", "Use E to Interrupt Spells", true);
        }
    }
}