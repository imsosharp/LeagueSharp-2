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
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;


namespace Support
{
    internal class Janna : Champion
    {
        public Spell Q;
        public Spell W;
        public Spell E;
        public Spell R;

        public Janna()
        {
            Utils.PrintMessage("Janna Loaded");

            Q = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 600);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 725);

            Q.SetSkillshot(0.5f, 200f, 900f, false, SkillshotType.SkillshotLine);
            Q.SetCharged("", "", 1100, 1700, 1.5f);
            W.SetTargetted(0.5f, 1000f);
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            Spell[] spellList = { Q, W, E, R };
            foreach (var spell in spellList)
            {
                var menuItem = GetValue<Circle>("Draw" + spell.Slot);
                if (menuItem.Active)
                    Utility.DrawCircle(ObjectManager.Player.Position, spell.Range, menuItem.Color);
            }
        }

        public override void Interrupter_OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (GetValue<bool>("InterruptQ") && spell.DangerLevel == InterruptableDangerLevel.High && unit.IsValidTarget(Q.Range) && Q.IsReady())
            {
                Q.StartCharging();
                Utility.DelayAction.Add(100, () => Q.Cast(unit, true));
                return;
            }

            if (GetValue<bool>("InterruptR") && spell.DangerLevel == InterruptableDangerLevel.High && unit.IsValidTarget(R.Range) && R.IsReady() && !Q.IsReady())
            {
                R.Cast();
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            var qt = SimpleTs.GetTarget(Q.ChargedMaxRange, SimpleTs.DamageType.Magical);
            var wt = SimpleTs.GetTarget(W.Range, SimpleTs.DamageType.Magical);

            if (ComboActive)
            {
                if (Q.IsReady() && qt.IsValidTarget(Q.ChargedMaxRange) && GetValue<bool>("UseQC"))
                {
                    if (Q.IsCharging)
                    {
                        Q.Cast(qt, true);
                    }
                    else
                    {
                        Q.StartCharging();
                    }
                }

                if (W.IsReady() && wt.IsValidTarget(W.Range) && GetValue<bool>("UseWC"))
                {
                    W.CastOnUnit(wt, true);
                }

                if (E.IsReady() && GetValue<bool>("UseEC"))
                {
                    // TODO: shield ally
                }

                if (R.IsReady() && Utility.CountEnemysInRange(300) > GetValue<Slider>("CountR").Value && GetValue<bool>("UseRC"))
                {
                    if (ObjectManager.Player.Health < ObjectManager.Player.MaxHealth * GetValue<Slider>("HealthR").Value / 100)
                        R.Cast();
                }
            }

            if (HarassActive)
            {
                if (W.IsReady() && wt.IsValidTarget(W.Range) && GetValue<bool>("UseWH"))
                {
                    W.CastOnUnit(wt, true);
                }

                if (E.IsReady() && GetValue<bool>("UseEH"))
                {
                    // TODO: shield ally
                }
            }
        }


        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("UseWC" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEC" + Id, "Use E").SetValue(true));
            config.AddItem(new MenuItem("UseRC" + Id, "Use R").SetValue(true));
            config.AddItem(new MenuItem("CountR" + Id, "Emergency Ult, enemys in Range").SetValue(new Slider(2, 1, 5)));
            config.AddItem(new MenuItem("HealthR" + Id, "Emergency Ult, lower than % HP").SetValue(new Slider(30, 1, 100)));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseWH" + Id, "Use W").SetValue(true));
            config.AddItem(new MenuItem("UseEH" + Id, "Use E").SetValue(true));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false, Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(new MenuItem("DrawR" + Id, "R range").SetValue(new Circle(false, Color.FromArgb(100, 255, 0, 255))));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("InterruptQ" + Id, "Use Q to Interrupt Spells").SetValue(true));
            config.AddItem(new MenuItem("InterruptR" + Id, "Use R to Interrupt Spells").SetValue(true));
        }
    }
}
