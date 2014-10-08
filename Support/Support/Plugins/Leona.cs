using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace Support.Plugins
{
    public class Leona : PluginBase
    {
        public Leona()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, AttackRange);
            W = new Spell(SpellSlot.W, AttackRange);
            E = new Spell(SpellSlot.E, 875);
            R = new Spell(SpellSlot.R, 1200);

            E.SetSkillshot(0.25f, 85f, 2000f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.625f, 315f, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target))
                {
                    Orbwalking.ResetAutoAttackTimer();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                }

                if (W.IsValidTarget(Target, "ComboQWE"))
                {
                    W.Cast();
                }

                if (E.IsValidTarget(Target, "ComboQWE") && Q.IsReady())
                {
                    if (E.Cast(Target, UsePackets) == Spell.CastStates.SuccessfullyCasted)
                        W.Cast();
                }

                if (E.IsValidTarget(Target, "ComboE"))
                {
                    E.Cast(Target);
                }

                if (R.IsValidTarget(Target, "ComboR"))
                {
                    R.CastIfHitchanceEquals(Target, HitChance.Immobile, UsePackets);
                }
            }
        }

        public override void AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (!unit.IsMe)
                return;

            if (!(target is Obj_AI_Hero) && !target.Name.ToLower().Contains("ward"))
                return;

            if (!Q.IsReady())
                return;

            if (Q.Cast())
            {
                Orbwalking.ResetAutoAttackTimer();
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (Q.IsValidTarget(gapcloser.Sender, "GapcloserQ"))
            {
                if (Q.Cast())
                {
                    Orbwalking.ResetAutoAttackTimer();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, gapcloser.Sender);
                }
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High || unit.IsAlly)
                return;

            if (Q.IsValidTarget(unit, "InterruptQ"))
            {
                if (Q.Cast())
                {
                    Orbwalking.ResetAutoAttackTimer();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, unit);
                }

                return;
            }

            if (R.IsValidTarget(unit, "InterruptR"))
            {
                R.Cast(unit, UsePackets);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboE", "Use E without Q", false);
            config.AddBool("ComboQWE", "Use Q/W/E", true);
            config.AddBool("ComboR", "Use R", true);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserQ", "Use Q to Interrupt Gapcloser", true);

            config.AddBool("InterruptQ", "Use Q to Interrupt Spells", true);
            config.AddBool("InterruptR", "Use R to Interrupt Spells", true);
        }
    }
}