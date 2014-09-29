using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace Support.Plugins
{
    public class Karma : PluginBase
    {
        private bool MantraIsActive
        {
            get { return Player.HasBuff("KarmaMantra"); }
        }

        public Karma()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 1050);
            W = new Spell(SpellSlot.W, 700);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 0);

            Q.SetSkillshot(0.25f, 60f, 1700f, true, SkillshotType.SkillshotLine);
            W.SetTargetted(0.25f, 2200f);
            E.SetTargetted(0.25f, float.MaxValue);
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

                var ally = Utils.AllyBelowHp(GetValue<Slider>("ComboHealthE").Value, E.Range);
                if (E.IsValidTarget(ally, "ComboE", true, false))
                {
                    E.Cast(ally, true);
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
                W.Cast(Target, true);
                E.Cast(Player, true);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            config.AddSlider("ComboHealthE", "Health to Shield", 20, 1, 100);
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
        }
    }
}