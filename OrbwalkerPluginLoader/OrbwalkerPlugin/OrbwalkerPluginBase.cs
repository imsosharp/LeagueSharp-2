using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp.Common;

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
        public string Name { get; set; }

        /// <summary>
        /// Champion Name
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
        /// Target
        /// </summary>
        public Obj_AI_Hero Target { get; set; }

        /// <summary>
        /// OrbwalkerTarget
        /// </summary>
        public Obj_AI_Base OrbwalkerTarget { get { return Orbwalker.GetTarget(); } }

        /// <summary>
        /// ComboDamage
        /// </summary>
        public double ComboDamage
        {
            get
            {
                if (!Target.IsValidTarget())
                    return 0;

                var spellCombo = new List<DamageLib.SpellType>();

                foreach (var spell in _spells.Where(spell => spell != null && spell.IsReady()))
                {
                    switch (spell.Slot)
                    {
                        case SpellSlot.Q: spellCombo.Add(DamageLib.SpellType.Q); break;
                        case SpellSlot.W: spellCombo.Add(DamageLib.SpellType.W); break;
                        case SpellSlot.E: spellCombo.Add(DamageLib.SpellType.E); break;
                        case SpellSlot.R: spellCombo.Add(DamageLib.SpellType.R); break;
                    }
                }

                return DamageLib.GetComboDamage(Target, spellCombo);
            }
        }

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

        private readonly List<Spell> _spells = new List<Spell>();

        #region Champion Range
        // TODO: adjust range
        private readonly Dictionary<string, float> _championMaxRange = new Dictionary<string, float>()
        {
            { "Aatrox",         float.MaxValue },
            { "Ahri",           float.MaxValue },
            { "Akali",          float.MaxValue },
            { "Alistar",        float.MaxValue },
            { "Amumu",          float.MaxValue },
            { "Anivia",         float.MaxValue },
            { "Annie",          float.MaxValue },
            { "Ashe",           float.MaxValue },
            { "Blitzcrank",     float.MaxValue },
            { "Brand",          float.MaxValue },
            { "Braum",          float.MaxValue },
            { "Caitlyn",        float.MaxValue },
            { "Cassiopeia",     float.MaxValue },
            { "ChoGath",        float.MaxValue },
            { "Corki",          float.MaxValue },
            { "Darius",         float.MaxValue },
            { "Diana",          float.MaxValue },
            { "DrMundo",        float.MaxValue },
            { "Draven",         float.MaxValue },
            { "Elise",          float.MaxValue },
            { "Evelynn",        float.MaxValue },
            { "Ezreal",         float.MaxValue },
            { "Fiddlesticks",   float.MaxValue },
            { "Fiora",          float.MaxValue },
            { "Fizz",           float.MaxValue },
            { "Galio",          float.MaxValue },
            { "Gangplank",      float.MaxValue },
            { "Garen",          float.MaxValue },
            { "Gnar",           float.MaxValue },
            { "Gragas",         float.MaxValue },
            { "Graves",         float.MaxValue },
            { "Hecarim",        float.MaxValue },
            { "Heimerdinger",   float.MaxValue },
            { "Irelia",         float.MaxValue },
            { "Janna",          float.MaxValue },
            { "JarvanIV",       float.MaxValue },
            { "Jax",            float.MaxValue },
            { "Jayce",          float.MaxValue },
            { "Jinx",           float.MaxValue },
            { "Karma",          float.MaxValue },
            { "Karthus",        float.MaxValue },
            { "Kassadin",       float.MaxValue },
            { "Katarina",       float.MaxValue },
            { "Kayle",          float.MaxValue },
            { "Kennen",         float.MaxValue },
            { "KhaZix",         float.MaxValue },
            { "KogMaw",         float.MaxValue },
            { "LeBlanc",        float.MaxValue },
            { "LeeSin",         float.MaxValue },
            { "Leona",          float.MaxValue },
            { "Lissandra",      float.MaxValue },
            { "Lucian",         float.MaxValue },
            { "Lulu",           float.MaxValue },
            { "Lux",            float.MaxValue },
            { "Malphite",       float.MaxValue },
            { "Malzahar",       float.MaxValue },
            { "Maokai",         float.MaxValue },
            { "MasterYi",       float.MaxValue },
            { "MissFortune",    float.MaxValue },
            { "Mordekaiser",    float.MaxValue },
            { "Morgana",        float.MaxValue },
            { "Nami",           float.MaxValue },
            { "Nasus",          float.MaxValue },
            { "Nautilus",       float.MaxValue },
            { "Nidalee",        float.MaxValue },
            { "Nocturne",       float.MaxValue },
            { "Nunu",           float.MaxValue },
            { "Olaf",           float.MaxValue },
            { "Orianna",        float.MaxValue },
            { "Pantheon",       float.MaxValue },
            { "Poppy",          float.MaxValue },
            { "Quinn",          float.MaxValue },
            { "Rammus",         float.MaxValue },
            { "Renekton",       float.MaxValue },
            { "Rengar",         float.MaxValue },
            { "Riven",          float.MaxValue },
            { "Rumble",         float.MaxValue },
            { "Ryze",           float.MaxValue },
            { "Sejuani",        float.MaxValue },
            { "Shaco",          float.MaxValue },
            { "Shen",           float.MaxValue },
            { "Shyvana",        float.MaxValue },
            { "Singed",         float.MaxValue },
            { "Sion",           float.MaxValue },
            { "Sivir",          float.MaxValue },
            { "Skarner",        float.MaxValue },
            { "Sona",           float.MaxValue },
            { "Soraka",         float.MaxValue },
            { "Swain",          float.MaxValue },
            { "Syndra",         float.MaxValue },
            { "Talon",          float.MaxValue },
            { "Taric",          float.MaxValue },
            { "Teemo",          float.MaxValue },
            { "Thresh",         float.MaxValue },
            { "Tristana",       float.MaxValue },
            { "Trundle",        float.MaxValue },
            { "Tryndamere",     float.MaxValue },
            { "TwistedFate",    float.MaxValue },
            { "Twitch",         float.MaxValue },
            { "Udyr",           float.MaxValue },
            { "Urgot",          float.MaxValue },
            { "Varus",          float.MaxValue },
            { "Vayne",          float.MaxValue },
            { "Veigar",         float.MaxValue },
            { "VelKoz",         float.MaxValue },
            { "Vi",             float.MaxValue },
            { "Viktor",         float.MaxValue },
            { "Vladimir",       float.MaxValue },
            { "Volibear",       float.MaxValue },
            { "Warwick",        float.MaxValue },
            { "Wukong",         float.MaxValue },
            { "Xerath",         float.MaxValue },
            { "XinZhao",        float.MaxValue },
            { "Yasuo",          float.MaxValue },
            { "Yorick",         float.MaxValue },
            { "Zac",            float.MaxValue },
            { "Zed",            float.MaxValue },
            { "Ziggs",          float.MaxValue },
            { "Zilean",         float.MaxValue },
            { "Zyra",           float.MaxValue }
        };
        #endregion

        /// <summary>
        /// Init BaseClass
        /// </summary>
        protected OrbwalkerPluginBase(string name, string championName, Version version)
        {
            Name = name;
            ChampionName = championName;
            Version = version;

            InitConfig();
            InitOrbwalker();
            InitTargetSelector();
            InitDrawing();
            InitSpells();
            InitPluginEvents();
        }

        /// <summary>
        /// TargetSelector Initialization
        /// </summary>
        private void InitPluginEvents()
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
            CustomEvents.Game.OnGameEnd += OnUnload;
            Game.OnGameUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
            AntiGapcloser.OnEnemyGapcloser += OnEnemyGapcloser;
            Interrupter.OnPosibleToInterrupt += OnPosibleToInterrupt;
        }

        /// <summary>
        /// TargetSelector Initialization
        /// </summary>
        private void InitTargetSelector()
        {
            Game.OnGameUpdate += args =>
            {
                var range = float.MaxValue;

                if (_championMaxRange.ContainsKey(Player.ChampionName))
                {
                    range = _championMaxRange[Player.ChampionName];
                }

                Target = SimpleTs.GetTarget(range, SimpleTs.DamageType.Physical);
            };
        }

        private void InitConfig()
        {
            Config = new Menu(Player.ChampionName, Player.ChampionName, true);
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("QRange", "Q range").SetValue(new Circle(true, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("WRange", "W range").SetValue(new Circle(true, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("ERange", "E range").SetValue(new Circle(false, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("RRange", "R range").SetValue(new Circle(false, Color.FromArgb(150, Color.DodgerBlue))));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("KillIndicator", "Kill Indicator").SetValue(new Circle(false, Color.FromArgb(150, Color.Firebrick))));
            Config.AddToMainMenu();
        }

        /// <summary>
        /// Orbwalker Initialization
        /// </summary>
        private void InitOrbwalker()
        {
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));
        }

        private void InitDrawing()
        {
            Drawing.OnDraw += args =>
            {
                foreach (var spell in _spells.Where(s => s != null))
                {
                    var menuItem = Config.Item(spell.Slot + "Range").GetValue<Circle>();
                    if (menuItem.Active && spell.Level > 0 && spell.IsReady())
                        Utility.DrawCircle(Player.Position, spell.Range, menuItem.Color);
                }

                if (Config.Item("KillIndicator").GetValue<Circle>().Active && Target.IsValidTarget() && ComboDamage > Target.Health)
                {
                    Drawing.DrawText(Target.Position.X, Target.Position.Y, Config.Item("KillIndicator").GetValue<Circle>().Color, "KILL HIM!");
                }
            };
        }

        private void InitSpells()
        {
            CustomEvents.Game.OnGameLoad += args =>
            {
                _spells.Add(Q);
                _spells.Add(W);
                _spells.Add(E);
                _spells.Add(R);
            };
        }

        /// <summary>
        /// OnPosibleToInterrupt
        /// </summary>
        /// <remarks>
        /// override to Implement SpellsInterrupt logic
        /// </remarks>
        /// <param name="unit">unit</param>
        /// <param name="spell">spell</param>
        public virtual void OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
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
        /// OnUnload
        /// </summary>
        /// <remarks>
        /// override to Implement Cleanup
        /// </remarks>
        /// <param name="args">args</param>
        public virtual void OnUnload(EventArgs args)
        {
            CustomEvents.Game.OnGameLoad -= OnLoad;
            CustomEvents.Game.OnGameEnd -= OnUnload;
            Game.OnGameUpdate -= OnUpdate;
            Drawing.OnDraw -= OnDraw;
            AntiGapcloser.OnEnemyGapcloser -= OnEnemyGapcloser;
            Interrupter.OnPosibleToInterrupt -= OnPosibleToInterrupt;
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

        /// <summary>
        /// OnLoadConfig
        /// </summary>
        /// <remarks>
        /// override to Implement Custom Config
        /// </remarks>
        /// <param name="root">root</param>
        public virtual void OnLoadConfig(Menu root)
        {
        }
    }
}
