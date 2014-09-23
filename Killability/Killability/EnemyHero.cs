using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Killability.Properties;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Killability
{
    internal class EnemyHero
    {
        public Obj_AI_Hero Hero { get; set; }
        public float Health { get { return Hero.Health; } }
        public float Mana { get { return Hero.Mana; } }
        public List<Spell> Spells { get; set; }
        public List<Items.Item> Items { get; set; }
        public SpellDataInst Ignite { get; set; }

        public List<DamageLib.SpellType> Rotation { get; set; }
        public bool IsKillable { get; set; }
        public double ManaCost { get; set; }
        public double Damage { get; set; }
        public float Time { get; set; }
        public string Text { get; set; }

        public EnemyHero(Obj_AI_Hero hero)
        {
            Hero = hero;
            Spells = new List<Spell>();
            Items = new List<Items.Item>();
            Rotation = new List<DamageLib.SpellType>();
            Ignite = ObjectManager.Player.SummonerSpellbook.Spells.FirstOrDefault(x => x.Name.ToLower() == "summonerdot");

            // Observe Items
            Game.OnGameNotifyEvent += Game_OnGameNotifyEvent;
            foreach (var item in Killability.Items.Where(item => LeagueSharp.Common.Items.HasItem(item.Id, Hero) && !Items.Contains(item)))
                Items.Add(item);

            InitDrawing();
            Task.Factory.StartNew(CalcTask);
        }

        private void CalcTask()
        {
            while (true)
            {
                var time = 0f;
                var mana = 0f;
                var target = ObjectManager.Player;
                var combo = new List<DamageLib.SpellType>();

                foreach (var item in Items.Where(item => item.IsReady()))
                {
                    switch (item.Id)
                    {
                        case 3128: combo.Add(DamageLib.SpellType.DFG); break;
                        case 3077: combo.Add(DamageLib.SpellType.TIAMAT); break;
                        case 3074: combo.Add(DamageLib.SpellType.HYDRA); break;
                        case 3146: combo.Add(DamageLib.SpellType.HEXGUN); break;
                        case 3144: combo.Add(DamageLib.SpellType.BILGEWATER); break;
                        case 3153: combo.Add(DamageLib.SpellType.BOTRK); break;
                    }

                    if (DamageLib.IsKillable(target, combo))
                        break;
                }

                while (!DamageLib.IsKillable(target, combo))
                {
                    var t = 0f;
                    var nextAA = 0f;


                    if (t > nextAA)
                    {
                        nextAA = t + Hero.AttackDelay*1000;
                    }

                    t += 10;
                }

                if (Ignite != null && Ignite.State == SpellState.Ready && !DamageLib.IsKillable(target, combo))
                    combo.Add(DamageLib.SpellType.IGNITE);

                Damage = DamageLib.GetComboDamage(target, combo);
                IsKillable = DamageLib.IsKillable(target, combo);
                ManaCost = mana;
                Time = time;

                Thread.Sleep(100);
            }
        }

        private void Game_OnGameNotifyEvent(GameNotifyEventArgs args)
        {
            if (args.NetworkId != Hero.NetworkId)
                return;

            switch (args.EventId)
            {
                case GameEventId.OnItemPurchased:
                    foreach (var item in Killability.Items.Where(item => LeagueSharp.Common.Items.HasItem(item.Id, Hero) && !Items.Contains(item)))
                        Items.Add(item);
                    break;

                case GameEventId.OnItemRemoved:
                    foreach (var item in Killability.Items.Where(item => LeagueSharp.Common.Items.HasItem(item.Id, Hero) && Items.Contains(item)))
                        Items.Remove(item);
                    break;
            }
        }

        private void InitDrawing()
        {
            var sprite = new Render.Sprite(Resources.Skull, Hero.HPBarPosition);
            sprite.PositionUpdate += () => new Vector2(Hero.HPBarPosition.X + 140, Hero.HPBarPosition.Y + 10);
            sprite.VisibleCondition += s => IsOnScreen() && IsKillable && Killability.Icon;
            sprite.Scale = new Vector2(0.08f, 0.08f);
            sprite.Add(0);

            var text = new Render.Text(Hero.HPBarPosition, string.Empty, 18, new ColorBGRA(255, 255, 255, 255));
            text.PositionUpdate += () => new Vector2(Hero.HPBarPosition.X + 20, Hero.HPBarPosition.Y + 50);
            text.VisibleCondition += s => IsOnScreen() && Killability.Text;
            text.TextUpdate += () => Text;
            text.OutLined = true;
            text.Add(0);
        }

        private bool IsOnScreen()
        {
            return Render.OnScreen(Drawing.WorldToScreen(Hero.Position));
        }
    }
}
