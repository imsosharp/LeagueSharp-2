using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace Support.Disabled
{
    public class Thresh : PluginBase
    {
        public Thresh()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 950);
            E = new Spell(SpellSlot.E, 400);
            R = new Spell(SpellSlot.R, 400);

            Q.SetSkillshot(0.5f, 50f, 1900, true, SkillshotType.SkillshotCircle);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "ComboQ"))
                {

                }

                if (W.IsValidTarget(Target, "ComboW"))
                {

                }

                if (E.IsValidTarget(Target, "ComboE"))
                {
                    E.Cast(Utils.ReversePosition(ObjectManager.Player.Position, Target.Position), true);
                }

                if (R.IsValidTarget(Target, "ComboR"))
                {

                }
            }

            if (HarassMode)
            {
                if (Q.IsValidTarget(Target, "HarassQ"))
                {

                }

                if (W.IsValidTarget(Target, "HarassW"))
                {

                }

                if (E.IsValidTarget(Target, "HarassE"))
                {
                    E.Cast(Utils.ReversePosition(ObjectManager.Player.Position, Target.Position), true);
                }
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
            config.AddSlider("ComboCountR", "Targets in range to Ult", 2, 0, 5);
            config.AddSlider("ComboHealthR", "Health to Ult", 20, 1, 100);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
            config.AddBool("HarassW", "Use W", true);
            config.AddBool("HarassE", "Use E", true);
        }

        public override void ItemMenu(Menu config)
        {
            config.AddBool("Zhonyas", "Use Zhonyas", true);
            config.AddBool("FrostQueen", "Use Frost Queen", true);
            config.AddBool("TwinShadows", "Use Twin Shadows", true);
            config.AddBool("Locket", "Use Locket", true);
            config.AddBool("Talisman", "Use Talisman", true);
            config.AddBool("Mikael", "Use Mikael", true);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserQ", "Use Q to Interrupt Gapcloser", true);
            config.AddBool("GapcloserW", "Use W to Interrupt Gapcloser", true);
            config.AddBool("GapcloserE", "Use E to Interrupt Gapcloser", true);
            config.AddBool("GapcloserR", "Use R to Interrupt Gapcloser", true);

            config.AddBool("InterruptQ", "Use Q to Interrupt Spells", true);
            config.AddBool("InterruptW", "Use W to Interrupt Spells", true);
            config.AddBool("InterruptE", "Use E to Interrupt Spells", true);
            config.AddBool("InterruptR", "Use R to Interrupt Spells", true);
        }
    }
}