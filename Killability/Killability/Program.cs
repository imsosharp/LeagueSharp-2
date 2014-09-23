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
using System.Collections.Generic;
using System.Linq;
using Killability.Properties;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Killability
{
    public class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += a => new KillDrawer();
        }
    }

    internal class KillDrawer
    {
        private readonly List<Spell> _spells;
        private readonly List<Items.Item> _items;
        private readonly SpellDataInst _ignite;
        private readonly Menu _config;

        public KillDrawer()
        {
            _ignite = ObjectManager.Player.SummonerSpellbook.Spells.FirstOrDefault(x => x.Name.ToLower() == "summonerdot");

            _items = new List<Items.Item>
            {
                new Items.Item(3128, 750), // Deathfire Grasp
                new Items.Item(3077, 400), // Tiamat
                new Items.Item(3074, 400), // Ravenous Hydra
                new Items.Item(3146, 700), // Hextech Gunblade
                new Items.Item(3144, 450), // Bilgewater Cutlass
                new Items.Item(3153, 450)  // Blade of the Ruined King
            };

            _spells = new List<Spell>
            {
                new Spell(SpellSlot.Q),
                new Spell(SpellSlot.W),
                new Spell(SpellSlot.E),
                new Spell(SpellSlot.R)
            };

            _config = new Menu("Killability", "Killability", true);
            _config.AddItem(new MenuItem("icon", "Show Icon").SetValue(true));
            _config.AddItem(new MenuItem("text", "Show Text").SetValue(true));
            _config.AddToMainMenu();

            InitDrawing();

            Game.PrintChat("Killability by h3h3 loaded.");
        }

        private void InitDrawing()
        {
            foreach (var h in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsEnemy))
            {
                var hero = h;
                var sprite = new Render.Sprite(Resources.Skull, hero.HPBarPosition);
                sprite.Scale = new Vector2(0.08f, 0.08f);
                sprite.Add(0);
                sprite.PositionUpdate += () => new Vector2(hero.HPBarPosition.X + 140, hero.HPBarPosition.Y + 10);
                sprite.VisibleCondition += s =>
                    Render.OnScreen(Drawing.WorldToScreen(hero.Position)) &&
                    GetComboResult(hero).IsKillable &&
                    _config.Item("icon").GetValue<bool>();

                var text = new Render.Text(hero.HPBarPosition, "-", 18, new ColorBGRA(255, 255, 255, 255));
                text.Add(1);
                text.OutLined = true;
                text.PositionUpdate += () => new Vector2(hero.HPBarPosition.X + 20, hero.HPBarPosition.Y + 50);
                text.VisibleCondition += s =>
                    Render.OnScreen(Drawing.WorldToScreen(hero.Position)) &&
                    _config.Item("text").GetValue<bool>();
                text.TextUpdate += () => GetComboResult(hero).Text;
            }

            //var test = new Render.Text(new Vector2(100, 100), string.Empty, 20, new ColorBGRA(255, 255, 255, 255));
            //test.Add(1);
            //test.OutLined = true;
            //test.TextUpdate += () =>
            //{
            //    var t = 0f;
            //    var nextAD = 0f;
            //    var nextQ = 0f;
            //    var nextW = 0f;
            //    var nextE = 0f;
            //    var nextR = 0f;
            //    var dmg = 0d;
            //    var combo = new List<DamageLib.SpellType>();
            //    var rota = new List<DamageLib.SpellType>();

            //    while (ObjectManager.Player.Health > dmg)
            //    {
            //        if (t > nextAD)
            //        {
            //            nextAD = t + ObjectManager.Player.AttackDelay * 1000;
            //            combo.Add(DamageLib.SpellType.AD);
            //            dmg += DamageLib.GetComboDamage(ObjectManager.Player, combo);
            //        }

            //        if (t > nextQ)
            //        {
            //            combo.Add(DamageLib.SpellType.Q);
            //            dmg += DamageLib.GetComboDamage(ObjectManager.Player, combo);
            //            nextQ = t + ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).CooldownExpires;
            //        }

            //        //if (t > nextW)
            //        //{
            //        //    combo.Add(DamageLib.SpellType.W);
            //        //    dmg += DamageLib.GetComboDamage(ObjectManager.Player, combo);
            //        //    nextW = t + ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Cooldown;
            //        //}

            //        if (t > nextE)
            //        {
            //            combo.Add(DamageLib.SpellType.E);
            //            dmg += DamageLib.GetComboDamage(ObjectManager.Player, combo);
            //            nextE = t + ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Cooldown;
            //        }

            //        //if (t > nextR)
            //        //{
            //        //    combo.Add(DamageLib.SpellType.R);
            //        //    dmg += DamageLib.GetComboDamage(ObjectManager.Player, combo);
            //        //    nextR = t + ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Cooldown;
            //        //}


            //        t += 100;
            //    }

            //    return "Attacks: " + combo.Count + "\nDamage: " + dmg + "\nTime: " + t + "\nNext: " + nextAD + "\nCombo:\n" + string.Join("\n", combo);
            //};
        }

        private class ComboResult
        {
            public List<DamageLib.SpellType> Spells { get; set; }
            public bool IsKillable { get; set; }
            public double ManaCost { get; set; }
            public string Text { get; set; }

            public ComboResult()
            {
                Spells = new List<DamageLib.SpellType>();
            }
        }

        private ComboResult GetComboResult(Obj_AI_Hero target)
        {
            if (!target.IsValidTarget())
                return new ComboResult();

            var combo = new List<Tuple<DamageLib.SpellType, DamageLib.StageType>>();
            var result = new ComboResult();
            var comboMana = 0f;

            foreach (var item in _items
                .Where(item => item.IsReady())
                .TakeWhile(item => !DamageLib.IsKillable(target, combo)))
            {
                switch (item.Id)
                {
                    case 3128: combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                        DamageLib.SpellType.DFG,
                        DamageLib.StageType.FirstDamage)); break;

                    case 3077: combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                        DamageLib.SpellType.TIAMAT,
                        DamageLib.StageType.Default)); break;

                    case 3074: combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                        DamageLib.SpellType.HYDRA,
                        DamageLib.StageType.Default)); break;

                    case 3146: combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                        DamageLib.SpellType.HEXGUN,
                        DamageLib.StageType.Default)); break;

                    case 3144: combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                        DamageLib.SpellType.BILGEWATER,
                        DamageLib.StageType.Default)); break;

                    case 3153: combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                        DamageLib.SpellType.BOTRK,
                        DamageLib.StageType.Default)); break;
                }
            }

            foreach (var spell in _spells
                .Where(spell => spell.Level > 0)
                .TakeWhile(item => !DamageLib.IsKillable(target, combo)))
            {
                switch (spell.Slot)
                {
                    case SpellSlot.Q: combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                        DamageLib.SpellType.Q,
                        DamageLib.StageType.Default)); break;

                    case SpellSlot.W: combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                        DamageLib.SpellType.W,
                        DamageLib.StageType.Default)); break;

                    case SpellSlot.E: combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                        DamageLib.SpellType.E,
                        DamageLib.StageType.Default)); break;

                    case SpellSlot.R: combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                        DamageLib.SpellType.R,
                        DamageLib.StageType.Default)); break;
                }

                comboMana += spell.Instance.ManaCost;
            }

            if (_ignite != null && _ignite.State == SpellState.Ready && !DamageLib.IsKillable(target, combo))
                combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                    DamageLib.SpellType.IGNITE,
                    DamageLib.StageType.Default));

            result.Spells.AddRange(combo.Select(c => c.Item1));
            result.IsKillable = DamageLib.IsKillable(target, combo) && ObjectManager.Player.Mana > comboMana;
            result.ManaCost = comboMana;

            if (result.IsKillable)
                result.Text = string.Join("/", result.Spells);

            if (ObjectManager.Player.Mana < comboMana)
                result.Text = "LOW MANA";

            return result;
        }
    }
}