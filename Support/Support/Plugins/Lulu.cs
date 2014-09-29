using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace Support.Plugins
{
    public class Lulu : PluginBase
    {
        public Lulu()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 925);
            W = new Spell(SpellSlot.W, 650);
            E = new Spell(SpellSlot.E, 650);
            R = new Spell(SpellSlot.R, 900);

            Q.SetSkillshot(0, 0, 0, false, SkillshotType.SkillshotLine);
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

                if (E.IsValidTarget(Target, "ComboE"))
                {
                    E.Cast(Target, true);
                }

                var ally = Utils.AllyBelowHp(GetValue<Slider>("ComboHealthR").Value, R.Range);
                if (R.IsValidTarget(ally, "ComboR", true, false))
                {
                    R.Cast(ally, true);
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
            config.AddBool("ComboE", "Use E", true);
            config.AddBool("ComboR", "Use R", true);
            config.AddSlider("ComboHealthR", "Health to Ult", 20, 1, 100);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
        }

        public override void ItemMenu(Menu config)
        {
            //config.AddBool("FrostQueen", "Use Frost Queen", true);
            //config.AddBool("Locket", "Use Locket", true);
            //config.AddBool("Talisman", "Use Talisman", true);
            //config.AddBool("Mikael", "Use Mikael", true);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserW", "Use W to Interrupt Gapcloser", true);
            config.AddBool("GapcloserE", "Use E to Interrupt Gapcloser", true);

            config.AddBool("InterruptW", "Use W to Interrupt Spells", true);
            config.AddBool("InterruptE", "Use E to Interrupt Spells", true);
        }
    }
}