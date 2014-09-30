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
            Q = new Spell(SpellSlot.Q, Player.AttackRange);
            W = new Spell(SpellSlot.W, Player.AttackRange);
            E = new Spell(SpellSlot.E, 875);
            R = new Spell(SpellSlot.R, 1200);

            E.SetSkillshot(0f, 100, 2000, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.7f, 315, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "ComboQ"))
                {
                    if (Q.Cast())
                    {
                        Orbwalking.ResetAutoAttackTimer();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                    }
                }

                if (W.IsValidTarget(Target, "ComboW"))
                {
                    W.Cast();
                }

                if (E.IsValidTarget(Target, "ComboE") && Q.IsReady())
                {
                    if (E.Cast(Target, true) == Spell.CastStates.SuccessfullyCasted)
                        Utility.DelayAction.Add(100, () => W.Cast());
                }

                //if (R.IsValidTarget(Target, "ComboR"))
                //{
                //    R.CastIfWillHit(Target, GetValue<Slider>("ComboCountR").Value, true);
                //}
            }
        }

        public override void AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (!unit.IsMe)
                return;

            if (!(target is Obj_AI_Hero) && !target.Name.Contains("Ward")) 
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
                    Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
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
                R.Cast(unit, true);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            config.AddBool("ComboE", "Use E", true);
            config.AddBool("ComboR", "Use R", true);
            config.AddSlider("ComboCountR", "Targets in range to Ult", 2, 1, 5);
        }

        public override void ItemMenu(Menu config)
        {
            //config.AddBool("Locket", "Use Locket", true);
            //config.AddBool("Talisman", "Use Talisman", true);
            //config.AddBool("Mikael", "Use Mikael", true);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserQ", "Use Q to Interrupt Gapcloser", true);

            config.AddBool("InterruptQ", "Use Q to Interrupt Spells", true);
            config.AddBool("InterruptR", "Use R to Interrupt Spells", true);
        }
    }
}