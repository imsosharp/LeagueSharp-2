#region LICENSE

//  Copyright 2014 - 2014 Support
//  Protector.cs is part of Support.
//  
//  Support is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  
//  Support is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with Support. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Support.Evade;
using Collision = Support.Evade.Collision;
using SpellData = LeagueSharp.SpellData;

#endregion

namespace Support
{
    internal class ProtectorSpell
    {
        public string Name { get; set; }
        public string ChampionName { get; set; }
        public Spell Spell { get; set; }
        public int HpBuffer { get; set; }
        public bool Harass { get; set; }
    }

    internal class ProtectorItem
    {
        public string Name { get; set; }
        public Items.Item Item { get; set; }
        public int HpBuffer { get; set; }
    }

    internal class Protector
    {
        public delegate void OnSkillshotProtectionH(Obj_AI_Hero target, List<Skillshot> skillshots);

        public delegate void OnTargetedProtectionH(Obj_AI_Base caster, Obj_AI_Hero target, SpellData spell);

        public static List<ProtectorSpell> ProtectorSpells = new List<ProtectorSpell>();
        public static List<ProtectorItem> ProtectorItems = new List<ProtectorItem>();
        public static SpellList<Skillshot> DetectedSkillshots = new SpellList<Skillshot>();
        public static Menu Menu;
        private static bool _isInitComplete;

        public static event OnSkillshotProtectionH OnSkillshotProtection;

        public static event OnTargetedProtectionH OnTargetedProtection;

        public static void Init()
        {
            if (!_isInitComplete)
            {
                CustomEvents.Game.OnGameLoad += a =>
                {
                    // Init stuff
                    InitSpells();
                    CreateMenu();
                    Collision.Init();

                    // Internal events
                    Game.OnGameUpdate += OnGameUpdate;
                    SkillshotDetector.OnDetectSkillshot += OnDetectSkillshot;
                    Obj_AI_Base.OnProcessSpellCast += HeroOnProcessSpellCast;
                    Obj_AI_Base.OnProcessSpellCast += TurretOnProcessSpellCast;
                    GameObject.OnCreate += SpellMissile_OnCreate;

                    // Actives
                    Game.OnGameUpdate += CcCheck;
                    OnSkillshotProtection += ProtectorOnOnSkillshotProtection;
                    OnTargetedProtection += ProtectorOnOnTargetedProtection;

                    // Debug
                    OnSkillshotProtection += Protector_OnSkillshotProtection;
                    OnTargetedProtection += Protector_OnTargetedProtection;

                    Helpers.PrintMessage(string.Format("Protector by h3h3 loaded!"));
                    _isInitComplete = true;
                };
            }
        }

        private static void CcCheck(EventArgs args)
        {
            var mikael = new Items.Item(3222, 750);

            if (!mikael.IsReady())
                return;

            foreach (
                var hero in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(h => h.IsAlly && !h.IsDead)
                        .OrderByDescending(h => h.FlatPhysicalDamageMod)
                        .ThenBy(h => h.Health)
                        .Where(mikael.IsInRange)
                        .Where(
                            hero =>
                                hero.HasBuffOfType(BuffType.Charm) || hero.HasBuffOfType(BuffType.Fear) ||
                                hero.HasBuffOfType(BuffType.Polymorph) || hero.HasBuffOfType(BuffType.Snare) ||
                                hero.HasBuffOfType(BuffType.Stun) || hero.HasBuffOfType(BuffType.Taunt)))
            {
                mikael.Cast(hero);
            }
        }

        private static void ProtectorOnOnTargetedProtection(Obj_AI_Base caster, Obj_AI_Hero target, SpellData spell)
        {
            ProtectionIntegration(caster, target, spell.Name);
        }

        private static void ProtectorOnOnSkillshotProtection(Obj_AI_Hero target, IEnumerable<Skillshot> skillshots)
        {
            foreach (var skillshot in skillshots)
            {
                ProtectionIntegration(skillshot.Unit, target, skillshot.SpellData.SpellName);
            }
        }

        private static void ProtectionIntegration(Obj_AI_Base caster, Obj_AI_Hero target, string spell)
        {
            try
            {
                foreach (
                    var ps in
                        ProtectorSpells.Where(
                            s => s.ChampionName == ObjectManager.Player.ChampionName && s.Spell.IsReady()))
                {
                    if (Menu.Item("spell" + ps.Name) == null || !Menu.Item("spell" + ps.Name).GetValue<bool>())
                        continue;

                    if (ps.Spell.Instance.ManaCost > ObjectManager.Player.Mana)
                        continue;

                    if (!ps.Spell.IsInRange(target))
                        continue;

                    if (ps.Harass || caster.WillKill(target, spell, ps.HpBuffer))
                    {
                        ps.Spell.Cast(target);
                    }
                }

                foreach (var pi in ProtectorItems.Where(i => i.Item.IsReady()))
                {
                    if (Menu.Item("item" + pi.Name) == null || !Menu.Item("item" + pi.Name).GetValue<bool>())
                        continue;

                    if (!pi.Item.IsInRange(target))
                        continue;

                    if (caster.WillKill(target, spell, pi.HpBuffer))
                    {
                        pi.Item.Cast(target);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void InitSpells()
        {
            ProtectorSpells.Add(new ProtectorSpell
            {
                Name = "Triumphant Roar",
                ChampionName = "Alistar",
                Spell = new Spell(SpellSlot.E, 575),
                HpBuffer = 0,
                Harass = true
            });

            ProtectorSpells.Add(new ProtectorSpell
            {
                Name = "Eye of the Storm",
                ChampionName = "Janna",
                Spell = new Spell(SpellSlot.E, 800),
                HpBuffer = 0,
                Harass = true
            });

            ProtectorSpells.Add(new ProtectorSpell
            {
                Name = "Inspire",
                ChampionName = "Karma",
                Spell = new Spell(SpellSlot.E, 800),
                HpBuffer = 0,
                Harass = true
            });

            ProtectorSpells.Add(new ProtectorSpell
            {
                Name = "Wild Growth",
                ChampionName = "Lulu",
                Spell = new Spell(SpellSlot.R, 900),
                HpBuffer = 20,
                Harass = false
            });

            ProtectorSpells.Add(new ProtectorSpell
            {
                Name = "Help, Pix!",
                ChampionName = "Lulu",
                Spell = new Spell(SpellSlot.E, 650),
                HpBuffer = 0,
                Harass = true
            });

            ProtectorSpells.Add(new ProtectorSpell
            {
                Name = "Ebb and Flow",
                ChampionName = "Nami",
                Spell = new Spell(SpellSlot.W, 725),
                HpBuffer = 0,
                Harass = true
            });

            ProtectorSpells.Add(new ProtectorSpell
            {
                Name = "Imbue",
                ChampionName = "Taric",
                Spell = new Spell(SpellSlot.Q, 750),
                HpBuffer = 0,
                Harass = true
            });

            ProtectorSpells.Add(new ProtectorSpell
            {
                Name = "Intervention",
                ChampionName = "Kayle",
                Spell = new Spell(SpellSlot.R, 900),
                HpBuffer = 20,
                Harass = false
            });

            ProtectorSpells.Add(new ProtectorSpell
            {
                Name = "Divine Blessing",
                ChampionName = "Kayle",
                Spell = new Spell(SpellSlot.W, 900),
                HpBuffer = 0,
                Harass = true
            });

            ProtectorSpells.Add(new ProtectorSpell
            {
                Name = "Chrono Shift",
                ChampionName = "Zilean",
                Spell = new Spell(SpellSlot.R, 900),
                HpBuffer = 20,
                Harass = false
            });

            ProtectorItems.Add(new ProtectorItem
            {
                Name = "Zhonya's Hourglass",
                Item = new Items.Item(3157, float.MaxValue),
                HpBuffer = 20
            });

            ProtectorItems.Add(new ProtectorItem
            {
                Name = "Seraph's Embrace",
                Item = new Items.Item(3040, float.MaxValue),
                HpBuffer = 20
            });

            ProtectorItems.Add(new ProtectorItem
            {
                Name = "Locket of the Iron Solari",
                Item = new Items.Item(3190, 600),
                HpBuffer = 20
            });

            ProtectorItems.Add(new ProtectorItem
            {
                Name = "Mikael's Crucible",
                Item = new Items.Item(3222, 750),
                HpBuffer = 20
            });
        }

        private static void CreateMenu()
        {
            Menu = new Menu("Protector", "Protector", true);

            var items = Menu.AddSubMenu(new Menu("Items", "Items"));
            foreach (var i in ProtectorItems)
            {
                items.AddItem(new MenuItem("item" + i.Name, "Use " + i.Name).SetValue(true));
            }

            var spells = Menu.AddSubMenu(new Menu("Spells", "Spells"));
            foreach (var i in ProtectorSpells.Where(s => s.ChampionName == ObjectManager.Player.ChampionName))
            {
                spells.AddItem(new MenuItem("spell" + i.Name, "Use " + i.Name).SetValue(true));
            }

            var targeted = Menu.AddSubMenu(new Menu("Targeted", "ProtectTargeted"));
            var skillshots = Menu.AddSubMenu(new Menu("Skillshot", "ProtectSkillshot"));
            targeted.AddItem(new MenuItem("TargetedActive", "Active").SetValue(true));
            skillshots.AddItem(new MenuItem("SkillshotActive", "Active").SetValue(true));

            foreach (var ally in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsAlly))
            {
                targeted.AddItem(
                    new MenuItem("targeted" + ally.ChampionName, "Protect " + ally.ChampionName).SetValue(true));
                skillshots.AddItem(
                    new MenuItem("skillshot" + ally.ChampionName, "Protect " + ally.ChampionName).SetValue(true));
            }

            Menu.AddToMainMenu();
        }

        private static void TurretOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (!(sender is Obj_AI_Turret))
                    return;

                if (ObjectManager.Player.IsDead)
                    return;

                if (!Menu.Item("TargetedActive").GetValue<bool>())
                    return;

                if (!sender.IsValid || sender.IsAlly)
                    return;

                if (!args.Target.IsValid || !(args.Target is Obj_AI_Hero))
                    return;

                var caster = (Obj_AI_Turret) sender;
                var target = (Obj_AI_Hero) args.Target;

                var protectAlly = Menu.Item("targeted" + target.ChampionName);
                if (protectAlly != null && protectAlly.GetValue<bool>())
                {
                    if (OnTargetedProtection != null)
                    {
                        OnTargetedProtection(caster, target, args.SData);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void HeroOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            try
            {
                if (!(sender is Obj_AI_Hero))
                    return;

                if (ObjectManager.Player.IsDead)
                    return;

                if (!Menu.Item("TargetedActive").GetValue<bool>())
                    return;

                if (!sender.IsValid || sender.IsAlly || !sender.IsMelee())
                    return;

                if (!args.Target.IsValid || !(args.Target is Obj_AI_Hero))
                    return;

                var caster = (Obj_AI_Hero) sender;
                var target = (Obj_AI_Hero) args.Target;

                var protectAlly = Menu.Item("targeted" + target.ChampionName);
                if (protectAlly != null && protectAlly.GetValue<bool>())
                {
                    if (OnTargetedProtection != null)
                    {
                        OnTargetedProtection(caster, target, args.SData);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void SpellMissile_OnCreate(GameObject sender, EventArgs args)
        {
            try
            {
                if (!(sender is Obj_SpellMissile))
                    return;

                if (ObjectManager.Player.IsDead)
                    return;

                if (!Menu.Item("TargetedActive").GetValue<bool>())
                    return;

                var missile = (Obj_SpellMissile) sender;

                if (!(missile.SpellCaster is Obj_AI_Hero) || !missile.SpellCaster.IsValid || missile.SpellCaster.IsAlly)
                    return;

                if (!(missile.Target is Obj_AI_Hero) || !missile.Target.IsValid || !missile.Target.IsAlly)
                    return;

                var caster = (Obj_AI_Hero) missile.SpellCaster;
                var target = (Obj_AI_Hero) missile.Target;

                var protectAlly = Menu.Item("targeted" + target.ChampionName);
                if (protectAlly != null && protectAlly.GetValue<bool>())
                {
                    if (OnTargetedProtection != null)
                    {
                        OnTargetedProtection(caster, target, missile.SData);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (!Menu.Item("SkillshotActive").GetValue<bool>())
                {
                    return;
                }

                //Remove the detected skillshots that have expired.
                DetectedSkillshots.RemoveAll(skillshot => !skillshot.IsActive());

                //Trigger OnGameUpdate on each skillshot.
                foreach (var skillshot in DetectedSkillshots)
                {
                    skillshot.Game_OnGameUpdate();
                }

                //Avoid sending move/cast packets while dead.
                if (ObjectManager.Player.IsDead)
                {
                    return;
                }

                // Protect
                foreach (var ally in ObjectManager.Get<Obj_AI_Hero>()
                    .Where(h => h.IsAlly && h.IsValidTarget(2000, false))
                    .OrderByDescending(h => h.Health))
                {
                    var protectAlly = Menu.Item("skillshot" + ally.ChampionName);
                    if (protectAlly != null && protectAlly.GetValue<bool>())
                    {
                        var allySafeResult = IsSafe(ally.ServerPosition.To2D());

                        if (!allySafeResult.IsSafe && IsAboutToHit(ally, 100))
                        {
                            if (OnSkillshotProtection != null)
                            {
                                OnSkillshotProtection(ally, allySafeResult.SkillshotList);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void OnDetectSkillshot(Skillshot skillshot)
        {
            try
            {
                //Check if the skillshot is already added.
                var alreadyAdded = false;

                // Integration disabled
                if (!Menu.Item("SkillshotActive").GetValue<bool>())
                {
                    return;
                }

                foreach (var item in DetectedSkillshots)
                {
                    if (item.SpellData.SpellName == skillshot.SpellData.SpellName &&
                        (item.Unit.NetworkId == skillshot.Unit.NetworkId &&
                         (skillshot.Direction).AngleBetween(item.Direction) < 5 &&
                         (skillshot.Start.Distance(item.Start) < 100 || skillshot.SpellData.FromObjects.Length == 0)))
                    {
                        alreadyAdded = true;
                    }
                }

                //Check if the skillshot is from an ally.
                if (skillshot.Unit.Team == ObjectManager.Player.Team)
                {
                    return;
                }

                //Check if the skillshot is too far away.
                if (skillshot.Start.Distance(ObjectManager.Player.ServerPosition.To2D()) >
                    (skillshot.SpellData.Range + skillshot.SpellData.Radius + 1000)*1.5)
                {
                    return;
                }


                //Add the skillshot to the detected skillshot list.
                if (!alreadyAdded)
                {
                    //Multiple skillshots like twisted fate Q.
                    if (skillshot.DetectionType == DetectionType.ProcessSpell)
                    {
                        if (skillshot.SpellData.MultipleNumber != -1)
                        {
                            var originalDirection = skillshot.Direction;

                            for (var i = -(skillshot.SpellData.MultipleNumber - 1)/2;
                                i <= (skillshot.SpellData.MultipleNumber - 1)/2;
                                i++)
                            {
                                var end = skillshot.Start +
                                          skillshot.SpellData.Range*
                                          originalDirection.Rotated(skillshot.SpellData.MultipleAngle*i);
                                var skillshotToAdd = new Skillshot(
                                    skillshot.DetectionType, skillshot.SpellData, skillshot.StartTick, skillshot.Start,
                                    end,
                                    skillshot.Unit);

                                DetectedSkillshots.Add(skillshotToAdd);
                            }
                            return;
                        }

                        if (skillshot.SpellData.SpellName == "UFSlash")
                        {
                            skillshot.SpellData.MissileSpeed = 1600 + (int) skillshot.Unit.MoveSpeed;
                        }

                        if (skillshot.SpellData.Invert)
                        {
                            var newDirection = -(skillshot.End - skillshot.Start).Normalized();
                            var end = skillshot.Start + newDirection*skillshot.Start.Distance(skillshot.End);
                            var skillshotToAdd = new Skillshot(
                                skillshot.DetectionType, skillshot.SpellData, skillshot.StartTick, skillshot.Start, end,
                                skillshot.Unit);
                            DetectedSkillshots.Add(skillshotToAdd);
                            return;
                        }

                        if (skillshot.SpellData.Centered)
                        {
                            var start = skillshot.Start - skillshot.Direction*skillshot.SpellData.Range;
                            var end = skillshot.Start + skillshot.Direction*skillshot.SpellData.Range;
                            var skillshotToAdd = new Skillshot(
                                skillshot.DetectionType, skillshot.SpellData, skillshot.StartTick, start, end,
                                skillshot.Unit);
                            DetectedSkillshots.Add(skillshotToAdd);
                            return;
                        }

                        if (skillshot.SpellData.SpellName == "SyndraE" || skillshot.SpellData.SpellName == "syndrae5")
                        {
                            var angle = 60;
                            var edge1 =
                                (skillshot.End - skillshot.Unit.ServerPosition.To2D()).Rotated(
                                    -angle/2*(float) Math.PI/180);
                            var edge2 = edge1.Rotated(angle*(float) Math.PI/180);

                            foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
                            {
                                var v = minion.ServerPosition.To2D() - skillshot.Unit.ServerPosition.To2D();
                                if (minion.Name == "Seed" && edge1.CrossProduct(v) > 0 && v.CrossProduct(edge2) > 0 &&
                                    minion.Distance(skillshot.Unit) < 800 &&
                                    (minion.Team != ObjectManager.Player.Team))
                                {
                                    var start = minion.ServerPosition.To2D();
                                    var end = skillshot.Unit.ServerPosition.To2D()
                                        .Extend(
                                            minion.ServerPosition.To2D(),
                                            skillshot.Unit.Distance(minion) > 200 ? 1300 : 1000);

                                    var skillshotToAdd = new Skillshot(
                                        skillshot.DetectionType, skillshot.SpellData, skillshot.StartTick, start, end,
                                        skillshot.Unit);
                                    DetectedSkillshots.Add(skillshotToAdd);
                                }
                            }
                            return;
                        }

                        if (skillshot.SpellData.SpellName == "AlZaharCalloftheVoid")
                        {
                            var start = skillshot.End - skillshot.Direction.Perpendicular()*400;
                            var end = skillshot.End + skillshot.Direction.Perpendicular()*400;
                            var skillshotToAdd = new Skillshot(
                                skillshot.DetectionType, skillshot.SpellData, skillshot.StartTick, start, end,
                                skillshot.Unit);
                            DetectedSkillshots.Add(skillshotToAdd);
                            return;
                        }

                        if (skillshot.SpellData.SpellName == "ZiggsQ")
                        {
                            var d1 = skillshot.Start.Distance(skillshot.End);
                            var d2 = d1*0.4f;
                            var d3 = d2*0.69f;


                            var bounce1SpellData = SpellDatabase.GetByName("ZiggsQBounce1");
                            var bounce2SpellData = SpellDatabase.GetByName("ZiggsQBounce2");

                            var bounce1Pos = skillshot.End + skillshot.Direction*d2;
                            var bounce2Pos = bounce1Pos + skillshot.Direction*d3;

                            bounce1SpellData.Delay =
                                (int) (skillshot.SpellData.Delay + d1*1000f/skillshot.SpellData.MissileSpeed + 500);
                            bounce2SpellData.Delay =
                                (int) (bounce1SpellData.Delay + d2*1000f/bounce1SpellData.MissileSpeed + 500);

                            var bounce1 = new Skillshot(
                                skillshot.DetectionType, bounce1SpellData, skillshot.StartTick, skillshot.End,
                                bounce1Pos,
                                skillshot.Unit);
                            var bounce2 = new Skillshot(
                                skillshot.DetectionType, bounce2SpellData, skillshot.StartTick, bounce1Pos, bounce2Pos,
                                skillshot.Unit);

                            DetectedSkillshots.Add(bounce1);
                            DetectedSkillshots.Add(bounce2);
                        }

                        if (skillshot.SpellData.SpellName == "ZiggsR")
                        {
                            skillshot.SpellData.Delay =
                                (int) (1500 + 1500*skillshot.End.Distance(skillshot.Start)/skillshot.SpellData.Range);
                        }

                        if (skillshot.SpellData.SpellName == "JarvanIVDragonStrike")
                        {
                            var endPos = new Vector2();

                            foreach (var s in DetectedSkillshots)
                            {
                                if (s.Unit.NetworkId == skillshot.Unit.NetworkId && s.SpellData.Slot == SpellSlot.E)
                                {
                                    endPos = s.End;
                                }
                            }

                            foreach (var m in ObjectManager.Get<Obj_AI_Minion>())
                            {
                                if (m.BaseSkinName == "jarvanivstandard" && m.Team == skillshot.Unit.Team &&
                                    skillshot.IsDanger(m.Position.To2D()))
                                {
                                    endPos = m.Position.To2D();
                                }
                            }

                            if (!endPos.IsValid())
                            {
                                return;
                            }

                            skillshot.End = endPos + 200*(endPos - skillshot.Start).Normalized();
                            skillshot.Direction = (skillshot.End - skillshot.Start).Normalized();
                        }
                    }

                    if (skillshot.SpellData.SpellName == "OriannasQ")
                    {
                        var endCSpellData = SpellDatabase.GetByName("OriannaQend");

                        var skillshotToAdd = new Skillshot(
                            skillshot.DetectionType, endCSpellData, skillshot.StartTick, skillshot.Start, skillshot.End,
                            skillshot.Unit);

                        DetectedSkillshots.Add(skillshotToAdd);
                    }


                    //Dont allow fow detection.
                    if (skillshot.SpellData.DisableFowDetection && skillshot.DetectionType == DetectionType.RecvPacket)
                    {
                        return;
                    }
#if DEBUG
                    Console.WriteLine(Environment.TickCount + "Adding new skillshot: " + skillshot.SpellData.SpellName);
#endif

                    DetectedSkillshots.Add(skillshot);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        /// <summary>
        ///     Returns true if the point is not inside the detected skillshots.
        /// </summary>
        private static IsSafeResult IsSafe(Vector2 point)
        {
            var result = new IsSafeResult {SkillshotList = new List<Skillshot>()};

            foreach (var skillshot in DetectedSkillshots)
            {
                result.SkillshotList.Add(skillshot);
            }

            result.IsSafe = (result.SkillshotList.Count == 0);

            return result;
        }

        /// <summary>
        ///     Returns true if some detected skillshot is about to hit the unit.
        /// </summary>
        private static bool IsAboutToHit(Obj_AI_Base unit, int time)
        {
            time += 150;
            return DetectedSkillshots
                .Any(skillshot => skillshot.IsAboutToHit(time, unit));
        }

        private static void Protector_OnTargetedProtection(Obj_AI_Base caster, Obj_AI_Hero target, SpellData spell)
        {
            try
            {
                var text = string.Format("{0,-10} -> {1,-10} - {2} {3}",
                    caster.BaseSkinName,
                    target.BaseSkinName,
                    spell.Name,
                    Math.Round(caster.GetSpellDamage(target, spell.Name)));

                Console.WriteLine(text);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void Protector_OnSkillshotProtection(Obj_AI_Hero target, IEnumerable<Skillshot> skillshots)
        {
            try
            {
                foreach (var skillshot in skillshots)
                {
                    var text = string.Format("{0,-10} -> {1,-10} - {2} {3}",
                        skillshot.Unit.BaseSkinName,
                        target.BaseSkinName,
                        skillshot.SpellData.SpellName,
                        Math.Round(skillshot.Unit.GetSpellDamage(target, skillshot.SpellData.SpellName)));

                    Console.WriteLine(text);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private struct IsSafeResult
        {
            public bool IsSafe;
            public List<Skillshot> SkillshotList;
        }
    }
}