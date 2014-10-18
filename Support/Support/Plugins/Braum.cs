#region LICENSE

// /*
// Copyright 2014 - 2014 Support
// Braum.cs is part of Support.
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
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Support.Evade;
using SpellData = LeagueSharp.SpellData;

#endregion

namespace Support.Plugins
{
    public class Braum : PluginBase
    {
        public Braum()
            : base("h3h3", new Version(4, 18, 14))
        {
            Q = new Spell(SpellSlot.Q, 1000);
            W = new Spell(SpellSlot.W, 650);
            E = new Spell(SpellSlot.E, 0);
            R = new Spell(SpellSlot.R, 1250);

            Q.SetSkillshot(0.3333f, 70f, 1200f, true, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.5f, 80f, 1200f, false, SkillshotType.SkillshotLine);
            Protector.OnSkillshotProtection += ProtectorOnSkillshotProtection;
            Protector.OnTargetedProtection += ProtectorOnTargetedProtection;
        }

        private bool IsShieldActive { get; set; }

        private void ProtectorOnTargetedProtection(Obj_AI_Base caster, Obj_AI_Hero target, SpellData spell)
        {
            try
            {
                if (!ConfigValue<bool>("Misc.Shield.Target"))
                    return;

                if (Orbwalking.IsAutoAttack(spell.Name) &&
                    target.HealthPercent() > ConfigValue<Slider>("Misc.Shield.Health").Value)
                    return;

                if (spell.MissileSpeed > 2000 || spell.MissileSpeed == 0)
                    return;

                if (target.IsMe && E.IsReady())
                {
                    E.Cast(caster.Position, UsePackets);
                    IsShieldActive = true;
                    Utility.DelayAction.Add(4000, () => IsShieldActive = false);
                }

                if (!target.IsMe && W.IsReady() && W.IsInRange(target) && (IsShieldActive || E.IsReady()))
                {
                    var jumpTime = (Player.Distance(target) * 1000 / W.Instance.SData.MissileSpeed) +
                                   W.Instance.SData.SpellCastTime + Game.Ping / 2;
                    var missileTime = (caster.Distance(target) * 1000 / spell.MissileSpeed) + Game.Ping / 2;

                    if (jumpTime > missileTime)
                    {
                        Console.WriteLine("Abort Jump - Missile too Fast: {0} {1}", jumpTime, missileTime);
                        return;
                    }

                    W.CastOnUnit(target, UsePackets);

                    Utility.DelayAction.Add((int)jumpTime, () =>
                    {
                        E.Cast(caster.Position, UsePackets);
                        IsShieldActive = true;
                        Utility.DelayAction.Add(4000, () => IsShieldActive = false);
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void ProtectorOnSkillshotProtection(Obj_AI_Hero target, List<Skillshot> skillshots)
        {
            try
            {
                if (!ConfigValue<bool>("Misc.Shield.Skill"))
                    return;

                var max = skillshots.First();
                foreach (var spell in skillshots)
                {
                    if (spell.Unit.GetSpellDamage(target, spell.SpellData.SpellName) >
                        max.Unit.GetSpellDamage(target, max.SpellData.SpellName))
                    {
                        max = spell;
                    }
                }

                if (max.SpellData.MissileSpeed > 2000 || max.SpellData.MissileSpeed == 0)
                    return;

                if (target.IsMe && E.IsReady())
                {
                    E.Cast(max.Start, UsePackets);

                    IsShieldActive = true;
                    Utility.DelayAction.Add(4000, () => IsShieldActive = false);
                }

                if (!target.IsMe && W.IsReady() && W.IsInRange(target) && (IsShieldActive || E.IsReady()))
                {
                    var jumpTime = (Player.Distance(target) * 1000 / W.Instance.SData.MissileSpeed) +
                                   W.Instance.SData.SpellCastTime + Game.Ping / 2;
                    var missileTime = (target.Distance(max.MissilePosition) * 1000 / max.SpellData.MissileSpeed) + Game.Ping / 2;

                    if (jumpTime > missileTime)
                    {
                        Console.WriteLine("Abort Jump - Missile too Fast: {0} {1}", jumpTime, missileTime);
                        return;
                    }

                    W.CastOnUnit(target, UsePackets);

                    Utility.DelayAction.Add((int)jumpTime, () =>
                    {
                        E.Cast(max.Start, UsePackets);
                        IsShieldActive = true;
                        Utility.DelayAction.Add(4000, () => IsShieldActive = false);
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "Combo.Q"))
                {
                    Q.Cast(Target, UsePackets);
                }

                if (R.IsValidTarget(Target, "Combo.R"))
                {
                    R.CastIfWillHit(Target, ConfigValue<Slider>("Combo.R.Count").Value, true);
                }
            }

            if (HarassMode)
            {
                if (Q.IsValidTarget(Target, "Harass.Q"))
                {
                    Q.Cast(Target, UsePackets);
                }
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (Q.IsValidTarget(gapcloser.Sender, "Gapcloser.Q"))
            {
                Q.Cast(gapcloser.Sender, UsePackets);
            }

            if (R.IsValidTarget(gapcloser.Sender, "Gapcloser.R"))
            {
                R.Cast(gapcloser.Sender, UsePackets);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High || unit.IsAlly)
                return;

            if (R.IsValidTarget(unit, "Interrupt.R"))
            {
                R.Cast(unit, UsePackets);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("Combo.Q", "Use Q", true);
            config.AddBool("Combo.R", "Use R", true);
            config.AddSlider("Combo.R.Count", "Targets hit by R", 2, 1, 5);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("Harass.Q", "Use Q", true);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("Misc.Shield.Skill", "Shield Skillshots", true);
            config.AddBool("Misc.Shield.Target", "Shield Targeted", true);
            config.AddSlider("Misc.Shield.Health", "Shield AA below HP", 30, 1, 100);
        }

        public override void InterruptMenu(Menu config)
        {
            config.AddBool("Gapcloser.Q", "Use Q to Interrupt Gapcloser", true);
            config.AddBool("Gapcloser.R", "Use R to Interrupt Gapcloser", false);

            config.AddBool("Interrupt.R", "Use R to Interrupt Spells", true);
        }
    }
}