using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

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
                new Items.Item(3077, 400), // Tiamat
                new Items.Item(3074, 400), // Ravenous Hydra
                new Items.Item(3146, 700), // Hextech Gunblade
                new Items.Item(3144, 450), // Bilgewater Cutlass
                new Items.Item(3153, 450), // Blade of the Ruined King
                new Items.Item(3128, 750)  // Deathfire Grasp
            };

            _spells = new List<Spell>
            {
                new Spell(SpellSlot.Q),
                new Spell(SpellSlot.W),
                new Spell(SpellSlot.E),
                new Spell(SpellSlot.R)
            };

            _config = new Menu("Killability", "Killability", true);
            _config.AddItem(new MenuItem("color", "Color").SetValue(Color.Aqua));
            _config.AddItem(new MenuItem("combo", "Show Combo").SetValue(true));
            _config.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.PrintChat("Killability by h3h3 loaded.");
        }

        private Tuple<double, List<Tuple<DamageLib.SpellType, DamageLib.StageType>>> GetComboDamage(Obj_AI_Hero target)
        {
            var combo = new List<Tuple<DamageLib.SpellType, DamageLib.StageType>>();

            foreach (var item in _items.Where(item => item.IsReady()))
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

                if (DamageLib.IsKillable(target, combo))
                    break;
            }

            foreach (var spell in _spells.Where(spell => spell.IsReady()))
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

                if(DamageLib.IsKillable(target, combo))
                    break;
            }

            if (_ignite != null && _ignite.State == SpellState.Ready)
                combo.Add(new Tuple<DamageLib.SpellType, DamageLib.StageType>(
                    DamageLib.SpellType.IGNITE,
                    DamageLib.StageType.Default));

            return new Tuple<double, List<Tuple<DamageLib.SpellType, DamageLib.StageType>>>(
                DamageLib.GetComboDamage(target, combo), 
                combo);
        }

        private bool IsOnScreen(Vector3 v)
        {
            var pos = Drawing.WorldToScreen(v);
            return pos.X < Drawing.Width && pos.Y < Drawing.Width;
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            foreach (var target in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsValidTarget() && IsOnScreen(h.Position)))
            {
                var dmg = GetComboDamage(target);

                if (dmg.Item1 > target.Health * 0.9)
                {
                    var pos = Drawing.WorldToScreen(target.Position);
                    var text = "Killable";
                    var textOffset = Drawing.GetTextExtent(text).Width / 2;
                    var combo = string.Join("/", dmg.Item2.Select(d => d.Item1));
                    var comboOffset = Drawing.GetTextExtent(combo).Width / 2;

                    Drawing.DrawText(pos.X - textOffset, pos.Y, _config.Item("color").GetValue<Color>(), text);

                    if (_config.Item("combo").GetValue<bool>())
                        Drawing.DrawText(pos.X - comboOffset, pos.Y + 15, _config.Item("color").GetValue<Color>(), combo);
                }
            }
        }
    }
}
