using System;
using LeagueSharp.Common;

namespace LeagueSharp.OrbwalkerPlugins
{
    public class Template : OrbwalkerPluginBase
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

        public Template()
            : base("Template by h3h3", "Template", new Version(4, 15, 14))
        {
            // Set Spells + Range
            Q = new Spell(SpellSlot.Q, float.MaxValue);
            W = new Spell(SpellSlot.W, float.MaxValue);
            E = new Spell(SpellSlot.E, float.MaxValue);
            R = new Spell(SpellSlot.R, float.MaxValue);
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
                // Attack Enemy Champion with Q Spell
                if (Target.IsValidTarget())
                {
                    if (Q.Cast(Target) == Spell.CastStates.SuccessfullyCasted)
                    {
                        Orbwalking.ResetAutoAttackTimer();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
                    }
                }

                // Attack Enemy Champion with Q Spell
                if (R.IsReady() && Target.IsValidTarget(R.Range))
                {
                    R.Cast(Target);
                }
            }

            // Mixed logic
            if (ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {

            }

            // LaneClear logic
            if (ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                // Use E Spell to farm Minions
                if (OrbwalkerTarget is Obj_AI_Minion && E.IsReady() && OrbwalkerTarget.IsValidTarget(E.Range))
                {
                    E.Cast(OrbwalkerTarget);
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
        }
    }
}
