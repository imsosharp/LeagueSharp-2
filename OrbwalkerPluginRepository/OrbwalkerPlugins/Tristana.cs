using System;
using LeagueSharp.Common;

namespace LeagueSharp.OrbwalkerPlugins
{
    public class Tristana : OrbwalkerPluginBase
    {
        /* OrbwalkerPluginBase
         * 
         * [ Class Properties ]
         * 
         * public string                    Name
         * public Version                   Version
         * 
         * public Obj_AI_Hero               Player
         * public Obj_AI_Hero               Target
         * public Obj_AI_Base               OrbwalkerTarget
         * 
         * public Orbwalking.Orbwalker      Orbwalker
         * public Orbwalking.OrbwalkingMode ActiveMode
         * 
         * public Menu                      Config
         * 
         * public Spell                     Q
         * public Spell                     W
         * public Spell                     E
         * public Spell                     R
         * 
         * public double                    ComboDamage
         * 
         * 
         * 
         * [ Class Methods ]
         * 
         * public virtual void OnLoad(EventArgs args)
         * public virtual void OnLoadConfig(Menu root)
         * public virtual void OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
         * public virtual void OnEnemyGapcloser(ActiveGapcloser gapcloser)
         * public virtual void OnUpdate(EventArgs args)
         * public virtual void OnDraw(EventArgs args)
         * public virtual void OnUnload(EventArgs args)
         * 
         */

        public Tristana()
            : base("Tristana by h3h3", "Tristana", new Version(4, 15, 14))
        {
            // Set Spells + Range
            Q = new Spell(SpellSlot.Q, 703f);
            E = new Spell(SpellSlot.E, 703f);
            R = new Spell(SpellSlot.R, 703f);
        }

        public override void OnLoadConfig(Menu root)
        {
        }

        public override void OnLoad(EventArgs args)
        {
            Game.PrintChat("{0} loaded!", Name);
        }

        public override void OnUpdate(EventArgs args)
        {
            // SBTW logic
            if (ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                if (Target.IsValidTarget())
                {
                    if (Q.Cast())
                    {
                        Orbwalking.ResetAutoAttackTimer();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                    }

                    if (E.Cast(Target) == Spell.CastStates.SuccessfullyCasted)
                    {
                        Orbwalking.ResetAutoAttackTimer();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                    }

                    if (R.Cast())
                    {
                        Orbwalking.ResetAutoAttackTimer();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                    }
                }
            }

            // Mixed logic
            if (ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                if (E.Cast(Target) == Spell.CastStates.SuccessfullyCasted)
                {
                    Orbwalking.ResetAutoAttackTimer();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                }
            }

            // LaneClear logic
            if (ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                if (Q.Cast())
                {
                    Orbwalking.ResetAutoAttackTimer();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                }

                if (E.Cast(Target) == Spell.CastStates.SuccessfullyCasted)
                {
                    Orbwalking.ResetAutoAttackTimer();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                }
            }
        }

        public override void OnDraw(EventArgs args)
        {
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
        }

        public override void OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
        }

        public override void OnUnload(EventArgs args)
        {
            base.OnUnload(args);
        }
    }
}
