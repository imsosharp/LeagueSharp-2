using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using Support.Evade;
using SpellData = LeagueSharp.SpellData;

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
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "ComboQ"))
                {
                    Q.Cast(Target, UsePackets);
                }

                if (W.IsValidTarget(Target, "ComboW"))
                {
                    W.Cast(Target, UsePackets);
                }
            }

            if (HarassMode)
            {
                if (Q.IsValidTarget(Target, "HarassQ"))
                {
                    Q.Cast(Target, UsePackets);
                }
            }
        }

        public override void OnSkillshotProtection(Obj_AI_Hero target, List<Skillshot> skillshots)
        {
            if (ProtectionMana && E.IsReady())
            {
                if (E.IsInRange(target))
                {
                    E.Cast(target, UsePackets);
                }
            }
        }

        public override void OnTargetedProtection(Obj_AI_Base caster, Obj_AI_Hero target, SpellData spell)
        {
            if (ProtectionMana && E.IsReady())
            {
                if (E.IsInRange(target))
                {
                    E.Cast(target, UsePackets);
                }
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (W.IsValidTarget(gapcloser.Sender, "GapcloserW"))
            {
                W.Cast(Target, UsePackets);
                E.Cast(Player, UsePackets);
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

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserW", "Use W to Interrupt Gapcloser", true);
        }
    }
}