using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace Support.Disabled
{
    public class Blitzcrank : PluginBase
    {
        public Blitzcrank()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 925);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, Player.AttackRange);
            R = new Spell(SpellSlot.R, 600);

            Q.SetSkillshot(0.25f, 70f, 1800f, true, SkillshotType.SkillshotLine);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "ComboQ"))
                {
                    Q.Cast(Target, true);
                }

                if (E.IsValidTarget(Target, "ComboE"))
                {
                    if (E.Cast())
                    {
                        Orbwalking.ResetAutoAttackTimer();
                    }
                }

                if (R.IsValidTarget(Target, "ComboR"))
                {
                    R.CastIfWillHit(Target, GetValue<Slider>("ComboCountR").Value, true);
                }
            }

            if (HarassMode)
            {
                if (Q.IsValidTarget(Target, "HarassQ"))
                {
                    Q.Cast(Target, true);
                }

                if (E.IsValidTarget(Target, "HarassE"))
                {
                    if (E.Cast())
                    {
                        Orbwalking.ResetAutoAttackTimer();
                    }
                }
            }
        }

        public override void AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (!unit.IsMe && !(target is Obj_AI_Hero))
                return;

            if (E.IsValidTarget(target, "AfterAttackE"))
            {
                if (E.Cast())
                {
                    Orbwalking.ResetAutoAttackTimer();
                }
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
                }
            }

            if (R.IsValidTarget(unit, "InterruptR"))
            {
                R.Cast();
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboE", "Use E", true);
            config.AddBool("ComboR", "Use R", true);
            config.AddSlider("ComboCountR", "Targets in range to Ult", 2, 1, 5);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
            config.AddBool("HarassE", "Use E", true);
        }

        public override void ItemMenu(Menu config)
        {
            //config.AddBool("Locket", "Use Locket", true);
            //config.AddBool("Talisman", "Use Talisman", true);
            //config.AddBool("Mikael", "Use Mikael", true);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserE", "Use E to Interrupt Gapcloser", true);
            config.AddBool("GapcloserR", "Use R to Interrupt Gapcloser", true);

            config.AddBool("InterruptE", "Use E to Interrupt Spells", true);
            config.AddBool("InterruptR", "Use R to Interrupt Spells", true);

            config.AddBool("AfterAttackE", "Use E After Attack", true);
        }
    }
}