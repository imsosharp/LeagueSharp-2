/*
    Copyright (C) 2014 h3h3

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/


using System;
using System.Linq;
using LeagueSharp.Common;


namespace LeagueSharp.OrbwalkerPlugins
{
    public class SonaDisabled : OrbwalkerPluginBase
    {
        public SonaDisabled()
            : base("by h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 650);
            W = new Spell(SpellSlot.W, 1000);
            E = new Spell(SpellSlot.E, 350);
            R = new Spell(SpellSlot.R, 1000);

            R.SetSkillshot(0.5f, 125, float.MaxValue, false, SkillshotType.SkillshotLine);
        }

        public override void OnLoad(EventArgs args)
        {
        }

        public override void OnUpdate(EventArgs args)
        {


            if (ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                if (Q.IsReady() && Target.IsValidTarget(Q.Range) && GetValue<bool>("UseQC"))
                {
                    var check = ObjectManager.Get<Obj_AI_Base>().Where(h => h.IsValidTarget() && (h is Obj_AI_Hero || h is Obj_AI_Minion)).OrderBy(h => Player.Distance(h)).ToList();

                    if (check.Count >= 2 && !check[0].IsMinion && !check[1].IsMinion)
                        Q.Cast();
                }

                if (W.IsReady() && Target.IsValidTarget(Player.AttackRange) && GetValue<bool>("UseWC"))
                {
                    W.Cast();
                }

                if (E.IsReady() && Target.IsValidTarget(E.Range) && GetValue<bool>("UseEC"))
                {
                    E.Cast(Target, true);
                }

                if (R.IsReady() && Target.IsValidTarget(R.Range) && GetValue<bool>("UseRC"))
                {
                    R.CastIfWillHit(Target, GetValue<Slider>("CountR").Value, true);
                }

                if (FrostQueen.IsReady() && Target.IsValidTarget(FrostQueen.Range) && GetValue<bool>("UseFrostQueen"))
                {
                    FrostQueen.Cast(Target);
                }
            }

            if (ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                if (Q.IsReady() && Target.IsValidTarget(Q.Range) && GetValue<bool>("UseQH"))
                {
                    var check = ObjectManager.Get<Obj_AI_Base>().Where(h => h.IsValidTarget() && (h is Obj_AI_Hero || h is Obj_AI_Minion)).OrderBy(h => Player.Distance(h)).ToList();

                    if (check.Count >= 2 && !check[0].IsMinion && !check[1].IsMinion)
                        Q.Cast();
                }
            }
        }

        public override void BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
        }

        public override void AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (Q.IsReady() && Target.IsValidTarget(Player.AttackRange) && GetValue<bool>("UseQA"))
            {
                Q.Cast();
                Orbwalking.ResetAutoAttackTimer();
                Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (Q.IsReady() && gapcloser.Sender.IsValidTarget(Player.AttackRange) && GetValue<bool>("UseQG"))
            {
                Q.Cast();
                Orbwalking.ResetAutoAttackTimer();
                Player.IssueOrder(GameObjectOrder.AttackUnit, gapcloser.Sender);
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High)
                return;

            if (Q.IsReady() && unit.IsValidTarget(Player.AttackRange) && GetValue<bool>("UseQI"))
            {
                Q.Cast();
                Orbwalking.ResetAutoAttackTimer();
                Player.IssueOrder(GameObjectOrder.AttackUnit, unit);
            }

            if (R.IsReady() && !Q.IsReady() && Target.IsValidTarget(R.Range) && GetValue<bool>("UseRI"))
            {
                R.Cast(Target, true);
            }
        }

        public override void OnDraw(EventArgs args)
        {
        }

        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC", "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC", "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC", "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC", "Use R").SetValue(true));
            config.AddItem(new MenuItem("CountR", "Num of Enemy in Range to Ult").SetValue(new Slider(2, 1, 5)));
        }

        public override void HarassMenu(Menu config)
        {
        }

        public override void ItemMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseFrostQueen", "Use FrostQueen").SetValue(true));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQA", "Use Q after Attack").SetValue(true));
            config.AddItem(new MenuItem("UseQG", "Use Q to Interrupt Gapcloser").SetValue(true));
            config.AddItem(new MenuItem("UseQI", "Use Q to Interrupt Spells").SetValue(true));
            config.AddItem(new MenuItem("UseRI", "Use R to Interrupt Spells").SetValue(true));
        }

        public override void ManaMenu(Menu config)
        {
        }

        public override void DrawingMenu(Menu config)
        {
        }
    }
}

//using System;
//using System.Linq;
//using LeagueSharp;
//using LeagueSharp.Common;
//using SharpDX;

//namespace Support
//{
//    internal class SonaDisabled : Champion
//    {

//        public Spell Q;
//        public Spell W;
//        public Spell E;
//        public Spell R;
//        public bool Debug = false;
//        PredictionOutput pred;

//        // Items Consants.
//        public const int FrostQueen = 3092;

//        public SonaDisabled()
//        {
//            Utils.PrintMessage("SonaDisabled loaded.");

//            Q = new Spell(SpellSlot.Q, 700f);
//            W = new Spell(SpellSlot.W, 1000f);
//            E = new Spell(SpellSlot.E, 1000f);
//            R = new Spell(SpellSlot.R, 1000f);


//            // Just setting R skillshot values.
//            R.SetSkillshot(0.5f, 125f, float.MaxValue, false, SkillshotType.SkillshotLine);

//        }

//        public override void OnGameUpdate(EventArgs args)
//        {

//            var autoMana = ((ObjectManager.Player.Mana / ObjectManager.Player.MaxMana) * 100) > GetValue<Slider>("autoMana").Value;

//            if (GetValue<bool>("AutoQ") && autoMana)
//            {
//                if (Q.IsReady())
//                {
//                    // Cast Q when target is in range
//                    var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
//                    if (Vector3.Distance(ObjectManager.Player.Position, t.Position) < Q.Range)
//                    {
//                        ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Q);
//                    }
//                }
//            }

//            if (GetValue<bool>("AutoW") && autoMana)
//            {
//                if (W.IsReady())
//                {
//                    // Casts when an Ally is below the set HP threshold.
//                    if (Utils.AllyBelowHP(GetValue<Slider>("AutoHeal").Value, W.Range))
//                    {
//                        ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W);
//                    }
//                }
//            }

//            if ((!ComboActive && !HarassActive) || (!Orbwalking.CanMove(100)))
//                return;

//            // Vars that grab values based on the menu
//            var useQ = GetValue<bool>("UseQ" + (ComboActive ? "C" : "H"));
//            var useW = GetValue<bool>("UseW" + (ComboActive ? "C" : "H"));
//            var useE = GetValue<bool>("UseEC");
//            var useR = GetValue<bool>("UseRC");

//            if (useQ && Q.IsReady())
//            {
//                // Cast Q when target is in range
//                var t = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
//                if (Vector3.Distance(ObjectManager.Player.Position, t.Position) < Q.Range)
//                {
//                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Q);
//                }
//            }

//            if (useW && W.IsReady())
//            {
//                // Casts when an Ally is below the set HP threshold.
//                if (Utils.AllyBelowHP(GetValue<Slider>("AutoHeal").Value, W.Range))
//                {
//                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W);
//                }
//            }

//            if (useE && E.IsReady() && ComboActive)
//            {
//                // This uses E just to proc passive :D
//                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E);
//            }

//            if (useR && R.IsReady() && ComboActive)
//            {
//                var t = SimpleTs.GetTarget(R.Range, SimpleTs.DamageType.Physical);
//                var ultTar = Utils.GetEnemyHitByR(R, GetValue<Slider>("CountR").Value);
//                if (ultTar != null)
//                {
//                    R.Cast(ultTar, true);
//                    // Attempt at faster cast.
//                    //R.CastOnUnit(ultTar, true);
//                }
//            }

//            // Item menu
//            if (ComboActive)
//            {
//                // Gets the target within frostQueens range (850).
//                var target = SimpleTs.GetTarget(840, SimpleTs.DamageType.Physical);
//                if (Items.HasItem(FrostQueen) && Items.CanUseItem(FrostQueen))
//                {
//                    // Grab the prediction based on arbitrary values ^.^
//                    pred = Prediction.GetPrediction(target, 0.5f, 50f, 1200f);
//                    foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)FrostQueen))
//                    {
//                        if (pred.Hitchance >= HitChance.Medium)
//                        {
//                            slot.UseItem(pred.CastPosition);
//                        }
//                    }
//                }
//            }

//        }

//        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
//        {
//            if (spell.DangerLevel == InterruptableDangerLevel.High)
//            {
//                R.Cast(unit);
//            }
//        }

//        public override void OnDraw(EventArgs args)
//        {
//            Spell[] spellList = { Q, W, R };
//            foreach (var spell in spellList)
//            {
//                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
//                if (menuItem.Active)
//                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
//            }
//            if (Debug)
//            {
//                //Utils.rect.ToPolygon().Draw(System.Drawing.Color.White, 2);
//                Utility.DrawCircle(pred.CastPosition, 40, System.Drawing.Color.White);
//            }

//        }

//        public override void ManaMenu(Menu config)
//        {
//            config.AddItem(new MenuItem("autoMana" + Id, "Auto Mana %").SetValue(new Slider(30, 100, 0)));
//        }

//        public override void ItemMenu(Menu config)
//        {
//            config.AddItem(new MenuItem("frostQueen" + Id, "Use Frost Queen on Combo").SetValue(true));
//        }

//        public override void ComboMenu(Menu config)
//        {
//            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
//            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
//            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
//            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
//            config.AddItem(new MenuItem("spacer", "--- Options ---"));
//            config.AddItem(new MenuItem("CountR" + Id, "R If Can Hit X").SetValue(new Slider(2, 5, 0)));
//        }

//        public override void HarassMenu(Menu config)
//        {
//            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
//            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(false));
//        }

//        public override void DrawingMenu(Menu config)
//        {
//            config.AddItem(
//                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true,
//                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
//            config.AddItem(
//                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(true,
//                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
//            config.AddItem(
//                new MenuItem("DrawR" + Id, "R range").SetValue(new Circle(false,
//                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
//        }

//        public override void MiscMenu(Menu config)
//        {
//            config.AddItem(new MenuItem("AutoQ" + Id, "Auto cast Q").SetValue(true));
//            config.AddItem(new MenuItem("AutoW" + Id, "Auto cast W").SetValue(true));
//            config.AddItem(new MenuItem("AutoHeal" + Id, "Auto W when below % hp").SetValue(new Slider(60, 100, 0)));
//        }


//    }
//}
