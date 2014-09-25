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
using LeagueSharp.Common;


namespace LeagueSharp.OrbwalkerPlugins
{
    public class Leona : OrbwalkerPluginBase
    {
        public Leona()
            : base("by h3h3", new Version(4, 16, 14))
        {
            Q = new Spell(SpellSlot.Q, 0);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, 900);
            R = new Spell(SpellSlot.R, 1200);

            E.SetSkillshot(0f, 100, 2000, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.7f, 315, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }

        public override void OnLoad(EventArgs args)
        {
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                if (Q.IsReady() && E.IsReady() && Target.IsValidTarget(Player.AttackRange) && GetValue<bool>("UseQC"))
                {
                    Q.Cast();
                    Orbwalking.ResetAutoAttackTimer();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
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
            if(spell.DangerLevel < InterruptableDangerLevel.High)
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