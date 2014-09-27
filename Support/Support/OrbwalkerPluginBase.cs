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
using System.Drawing;
using System.Linq;
using LeagueSharp.Common;
using Utils = Support.Utils;

namespace LeagueSharp.OrbwalkerPlugins
{
    /// <summary>
    /// OrbwalkerPluginBase class
    /// </summary>
    public abstract class OrbwalkerPluginBase
    {
        /// <summary>
        /// Plugin display name
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Champion Description
        /// </summary>
        public string ChampionName { get; set; }

        /// <summary>
        /// Plugin Version
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Config
        /// </summary>
        public Menu Config { get; set; }
        public Menu ComboConfig { get; set; }
        public Menu HarassConfig { get; set; }
        public Menu ItemsConfig { get; set; }
        public Menu MiscConfig { get; set; }
        public Menu ManaConfig { get; set; }
        public Menu DrawingConfig { get; set; }

        /// <summary>
        /// Orbwalker
        /// </summary>
        public Orbwalking.Orbwalker Orbwalker { get; set; }

        /// <summary>
        /// ActiveMode
        /// </summary>
        public Orbwalking.OrbwalkingMode ActiveMode { get { return Orbwalker.ActiveMode; } }

        /// <summary>
        /// Player Object
        /// </summary>
        public Obj_AI_Hero Player { get { return ObjectManager.Player; } }

        /// <summary>
        /// TargetSelector
        /// </summary>
        public TargetSelector TargetSelector { get; set; }

        /// <summary>
        /// Target
        /// </summary>
        public Obj_AI_Hero Target { get { return TargetSelector.Target; } }

        /// <summary>
        /// OrbwalkerTarget
        /// </summary>
        public Obj_AI_Base OrbwalkerTarget { get { return Orbwalker.GetTarget(); } }

        /// <summary>
        /// Q
        /// </summary>
        public Spell Q { get; set; }

        /// <summary>
        /// W
        /// </summary>
        public Spell W { get; set; }

        /// <summary>
        /// E
        /// </summary>
        public Spell E { get; set; }

        /// <summary>
        /// R
        /// </summary>
        public Spell R { get; set; }

        public Items.Item Zhonyas { get; set; }
        public Items.Item FrostQueen { get; set; }
        public Items.Item TwinShadows { get; set; }
        public Items.Item Locket { get; set; }
        public Items.Item Talisman { get; set; }
        public Items.Item Mikael { get; set; }

        private readonly List<Spell> _spells = new List<Spell>();

        /// <summary>
        /// Init BaseClass
        /// </summary>
        protected OrbwalkerPluginBase(string description, Version version)
        {
            Description = description;
            ChampionName = ObjectManager.Player.ChampionName;
            Version = version;

            InitConfig();
            InitOrbwalker();
            InitTargetSelector();
            InitItems();
            InitPluginEvents();
            InitPrivateEvents();

            Utils.PrintMessage(string.Format("{0} {1} {2} loaded!", ChampionName, Description, Version));
        }

        #region Private Stuff

        private void InitTargetSelector()
        {
            TargetSelector = new TargetSelector(float.MaxValue, TargetSelector.TargetingMode.AutoPriority);
            TargetSelector.SetDrawCircleOfTarget(true);
            Utility.DelayAction.Add(300, () => TargetSelector.SetRange(_spells.Select(s => s.Range).Max()));
        }

        private void InitItems()
        {
            Zhonyas = new Items.Item(3157, float.MaxValue);
            FrostQueen = new Items.Item(3092, 850);
            TwinShadows = new Items.Item(3023, 1500);
            Locket = new Items.Item(3190, 600);
            Talisman = new Items.Item(3069, 600);
            Mikael = new Items.Item(3222, float.MaxValue);
        }

        /// <summary>
        /// PluginEvents Initialization
        /// </summary>
        private void InitPluginEvents()
        {
            Console.WriteLine("Init Events");
            Game.OnGameUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
            Orbwalking.BeforeAttack += BeforeAttack;
            Orbwalking.AfterAttack += AfterAttack;
            AntiGapcloser.OnEnemyGapcloser += OnEnemyGapcloser;
            Interrupter.OnPossibleToInterrupt += OnPossibleToInterrupt;
            OnLoad(new EventArgs());
        }

        private void InitPrivateEvents()
        {
            Utility.DelayAction.Add(250, () =>
            {
                _spells.Add(Q);
                _spells.Add(W);
                _spells.Add(E);
                _spells.Add(R);
            });

            Orbwalking.BeforeAttack += args =>
            {
                if (args.Target.IsMinion && !Config.Item("AttMin").GetValue<bool>())
                    args.Process = false;
            };

            Drawing.OnDraw += args =>
            {
                foreach (var spell in _spells.Where(s => s != null))
                {
                    var menuItem = Config.Item(spell.Slot + "Range").GetValue<Circle>();
                    if (menuItem.Active && spell.Level > 0 && spell.IsReady())
                        Utility.DrawCircle(Player.Position, spell.Range, menuItem.Color);
                }
            };
        }

        private void InitConfig()
        {
            Config = new Menu(Player.ChampionName, Player.ChampionName, true);
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            ComboConfig = Config.AddSubMenu(new Menu("Combo", "Combo"));
            HarassConfig = Config.AddSubMenu(new Menu("Harass", "Harass"));
            ItemsConfig = Config.AddSubMenu(new Menu("Items", "Items"));

            ManaConfig = Config.AddSubMenu(new Menu("Mana Limiter", "Mana Limiter"));
            ManaConfig.AddItem(new MenuItem("ComboMana", "Combo Mana %").SetValue(new Slider(1, 100, 0)));
            ManaConfig.AddItem(new MenuItem("HarassMana", "Harass Mana %").SetValue(new Slider(30, 100, 0)));

            MiscConfig = Config.AddSubMenu(new Menu("Misc", "Misc"));
            MiscConfig.AddItem(new MenuItem("AttMin", "Attack Minions?").SetValue(true));

            DrawingConfig = Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            DrawingConfig.AddItem(new MenuItem("QRange", "Q range").SetValue(new Circle(true, Color.FromArgb(150, Color.DodgerBlue))));
            DrawingConfig.AddItem(new MenuItem("WRange", "W range").SetValue(new Circle(true, Color.FromArgb(150, Color.DodgerBlue))));
            DrawingConfig.AddItem(new MenuItem("ERange", "E range").SetValue(new Circle(false, Color.FromArgb(150, Color.DodgerBlue))));
            DrawingConfig.AddItem(new MenuItem("RRange", "R range").SetValue(new Circle(false, Color.FromArgb(150, Color.DodgerBlue))));

            ComboMenu(ComboConfig);
            HarassMenu(HarassConfig);
            ItemMenu(ItemsConfig);
            ManaMenu(ManaConfig);
            MiscMenu(MiscConfig);
            DrawingMenu(DrawingConfig);

            Config.AddToMainMenu();
        }

        /// <summary>
        /// Orbwalker Initialization
        /// </summary>
        private void InitOrbwalker()
        {
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));
        }

        #endregion

        /// <summary>
        /// GetValue
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="item">Item Name</param>
        /// <remarks>
        /// Helper for 
        /// </remarks>
        /// <returns></returns>
        public T GetValue<T>(string item)
        {
            return Config.Item(item).GetValue<T>();
        }

        /// <summary>
        /// OnPossibleToInterrupt
        /// </summary>
        /// <remarks>
        /// override to Implement SpellsInterrupt logic
        /// </remarks>
        /// <param name="unit">unit</param>
        /// <param name="spell">spell</param>
        public virtual void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
        }

        /// <summary>
        /// OnEnemyGapcloser
        /// </summary>
        /// <remarks>
        /// override to Implement AntiGapcloser logic
        /// </remarks>
        /// <param name="gapcloser">args</param>
        public virtual void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
        }

        /// <summary>
        /// OnUpdate
        /// </summary>
        /// <remarks>
        /// override to Implement Update logic
        /// </remarks>
        /// <param name="args">args</param>
        public virtual void OnUpdate(EventArgs args)
        {
        }

        /// <summary>
        /// BeforeAttack
        /// </summary>
        /// <remarks>
        /// override to Implement BeforeAttack logic
        /// </remarks>
        /// <param name="args">args</param>
        public virtual void BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
        }

        /// <summary>
        /// AfterAttack
        /// </summary>
        /// <remarks>
        /// override to Implement AfterAttack logic
        /// </remarks>
        /// <param name="unit">unit</param>
        /// <param name="target">target</param>
        public virtual void AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
        }

        /// <summary>
        /// OnLoad
        /// </summary>
        /// <remarks>
        /// override to Implement class Initialization
        /// </remarks>
        /// <param name="args">args</param>
        public virtual void OnLoad(EventArgs args)
        {
        }

        /// <summary>
        /// OnDraw
        /// </summary>
        /// <remarks>
        /// override to Implement Drawing
        /// </remarks>
        /// <param name="args">args</param>
        public virtual void OnDraw(EventArgs args)
        {
        }

        public virtual void ComboMenu(Menu config)
        {
        }

        public virtual void HarassMenu(Menu config)
        {
        }

        public virtual void ItemMenu(Menu config)
        {
        }

        public virtual void ManaMenu(Menu config)
        {
        }

        public virtual void MiscMenu(Menu config)
        {
        }

        public virtual void DrawingMenu(Menu config)
        {
        }
    }
}
