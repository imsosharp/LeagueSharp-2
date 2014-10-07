using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using Support.Evade;
using SpellData = LeagueSharp.SpellData;

namespace Support.Plugins
{
    public class Lulu : PluginBase
    {
        public Lulu()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 925);
            W = new Spell(SpellSlot.W, 650);
            E = new Spell(SpellSlot.E, 650); //shield
            R = new Spell(SpellSlot.R, 900);

            Q.SetSkillshot(0.25f, 50, 1450, false, SkillshotType.SkillshotLine);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "ComboQ"))
                {
                    Q.Cast(Target, true);
                }

                if (W.IsValidTarget(Target, "ComboW"))
                {
                    W.Cast(Target, true);
                }
            }

            if (HarassMode)
            {
                if (Q.IsValidTarget(Target, "HarassQ"))
                {
                    Q.Cast(Target, true);
                }
            }
        }

        public override void OnSkillshotProtection(Obj_AI_Hero target, List<Skillshot> skillshots)
        {
            if (R.IsReady())
            {
                foreach (var skillshot in skillshots)
                {
                    if (skillshot.Unit.WillKill(target, skillshot.SpellData))
                        R.Cast(target, true);
                }
            }

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
            if (R.IsReady())
            {
                if (caster.WillKill(target, spell))
                    R.Cast(target, true);
            }

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

            if (W.IsValidTarget(gapcloser.Sender, "GapcloserW"))
            {
                W.Cast(gapcloser.Sender, true);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High || unit.IsAlly)
                return;

            if (W.IsValidTarget(unit, "InterruptW"))
            {
                W.Cast(unit, true);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserW", "Use W to Interrupt Gapcloser", true);

            config.AddBool("InterruptW", "Use W to Interrupt Spells", true);
        }
    }
}