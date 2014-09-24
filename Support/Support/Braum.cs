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


namespace Support
{
    internal class Braum : Champion
    {
        public Spell Q;
        public Spell W;
        public Spell E;
        public Spell R;

        public Braum()
        {
            Utils.PrintMessage("Braum Loaded");

            Q = new Spell(SpellSlot.Q, 1000);
            W = new Spell(SpellSlot.W, 650);
            E = new Spell(SpellSlot.E, 25000);
            R = new Spell(SpellSlot.R, 1250);

            Q.SetSkillshot(0.3333f, 70f, 1200f, true, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.5f, 80f, 1200f, false, SkillshotType.SkillshotLine);
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
            if (!GetValue<bool>("InterruptSpells")) return;

            if (spell.DangerLevel == InterruptableDangerLevel.High && unit.IsValidTarget(R.Range) && R.IsReady())
            {
                R.Cast(unit, true);
            }
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);

            if (ComboActive)
            {
                if (Q.IsReady() && target.IsValidTarget(Q.Range) && GetValue<bool>("UseQC"))
                {
                    Q.Cast(target, true);
                }

                if (R.IsReady() && target.IsValidTarget(R.Range) && R.GetPrediction(target, true).AoeTargetsHitCount > GetValue<Slider>("CountR").Value)
                {
                    R.Cast(target, true);
                }
            }

            if (HarassActive)
            {
                if (Q.IsReady() && target.IsValidTarget(Q.Range) && GetValue<bool>("UseQH"))
                {
                    Q.Cast(target, true);
                }
            }
        }


        public override void ComboMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQC" + Id, "Use Q").SetValue(true));
            config.AddItem(new MenuItem("CountR" + Id, "Num of Enemy in Range to Ult").SetValue(new Slider(2, 1, 5)));
        }

        public override void HarassMenu(Menu config)
        {
            config.AddItem(new MenuItem("UseQH" + Id, "Use Q").SetValue(false));
        }

        public override void DrawingMenu(Menu config)
        {
            config.AddItem(
                new MenuItem("DrawQ" + Id, "Q range").SetValue(new Circle(true,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawW" + Id, "W range").SetValue(new Circle(true,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawE" + Id, "E range").SetValue(new Circle(false,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
            config.AddItem(
                new MenuItem("DrawR" + Id, "R range").SetValue(new Circle(false,
                    System.Drawing.Color.FromArgb(100, 255, 0, 255))));
        }

        public override void MiscMenu(Menu config)
        {
            config.AddItem(new MenuItem("InterruptSpells" + Id, "Use R to Interrupt Spells").SetValue(true));
        }
    }
}
