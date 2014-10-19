#region LICENSE

// /*
// Copyright 2014 - 2014 Support
// Nami.cs is part of Support.
// Support is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// Support is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with Support. If not, see <http://www.gnu.org/licenses/>.
// */
// 

#endregion

#region

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Support.Plugins
{
    public class Nami : PluginBase
    {
        public Nami()
        {
            Q = new Spell(SpellSlot.Q, 875);
            W = new Spell(SpellSlot.W, 725);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 2200);

            Q.SetSkillshot(1.0f, 200f, Int32.MaxValue, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.5f, 325f, 1200f, false, SkillshotType.SkillshotLine);
            GameObject.OnCreate += RangeAttackOnCreate;
        }

        private void RangeAttackOnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.IsValid<Obj_SpellMissile>())
                return;

            var missile = (Obj_SpellMissile) sender;

            // Caster ally hero / not me
            if (!missile.SpellCaster.IsValid<Obj_AI_Hero>() || !missile.SpellCaster.IsAlly ||
                missile.SpellCaster.IsMe || !missile.SpellCaster.IsHeroType(HeroType.Ad) ||
                missile.SpellCaster.IsMelee())
                return;

            // Target enemy hero
            if (!missile.Target.IsValid<Obj_AI_Hero>() || !missile.Target.IsEnemy)
                return;

            if (E.IsReady() && E.IsInRange(missile.SpellCaster) && ConfigValue<bool>("MiscE"))
            {
                E.CastOnUnit(missile.SpellCaster, UsePackets);
            }
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.CastCheck(Target, "ComboQ")) // TODO: add check for slowed targets by E or FrostQeen
                {
                    Q.Cast(Target, UsePackets);
                }

                var ally = Helpers.AllyBelowHp(ConfigValue<Slider>("ComboHealthW").Value, W.Range);
                if (W.CastCheck(ally, "ComboW", true, false))
                {
                    W.CastOnUnit(ally, UsePackets);
                }

                if (W.CastCheck(Target, "ComboW"))
                {
                    W.CastOnUnit(Target, UsePackets);
                }

                if (R.CastCheck(Target, "ComboR"))
                {
                    R.CastIfWillHit(Target, ConfigValue<Slider>("ComboCountR").Value, UsePackets);
                }
            }

            if (HarassMode)
            {
                if (Q.CastCheck(Target, "HarassQ"))
                {
                    Q.Cast(Target, UsePackets);
                }

                var ally = Helpers.AllyBelowHp(ConfigValue<Slider>("HarassHealthW").Value, W.Range);
                if (W.CastCheck(ally, "HarassW", true, false))
                {
                    W.CastOnUnit(ally, UsePackets);
                }

                if (W.CastCheck(Target, "HarassW"))
                {
                    W.CastOnUnit(Target, UsePackets);
                }

                Config.AddItem(new MenuItem("test", "test").SetValue(new Slider(0, 0, 100)));
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (R.CastCheck(gapcloser.Sender, "GapcloserR"))
            {
                R.Cast(gapcloser.Sender, UsePackets);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High || unit.IsAlly)
                return;

            if (Q.CastCheck(unit, "InterruptQ"))
            {
                Q.Cast(unit, UsePackets);
            }

            if (R.CastCheck(unit, "InterruptR"))
            {
                R.Cast(unit, UsePackets);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            config.AddBool("ComboR", "Use R", true);
            config.AddSlider("ComboCountR", "Targets in range to Ult", 2, 1, 5);
            config.AddSlider("ComboHealthW", "Health to Heal", 20, 1, 100);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
            config.AddBool("HarassW", "Use W", true);
            config.AddSlider("HarassHealthW", "Health to Heal", 20, 1, 100);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("MiscE", "Use E on ADC Attacks", true);
        }

        public override void InterruptMenu(Menu config)
        {
            config.AddBool("GapcloserR", "Use R to Interrupt Gapcloser", true);

            config.AddBool("InterruptQ", "Use Q to Interrupt Spells", true);
            config.AddBool("InterruptR", "Use R to Interrupt Spells", true);
        }
    }
}