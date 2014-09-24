#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using System.IO;

#endregion

// Based on the popular marksman framework :D
namespace Support
{
    internal class Program
    {

        public static Menu Config;
        public static Champion ChampClass = null;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            Config = new Menu("Support", "Support", true);

            ChampClass = new Champion();

            switch (ObjectManager.Player.ChampionName)
            {
                case "Thresh":
                    ChampClass = new Thresh();
                    break;
                case "Morgana":
                    ChampClass = new Morgana();
                    break;
                case "Blitzcrank":
                    ChampClass = new Blitzcrank();
                    break;
                case "Sona":
                    ChampClass = new Sona();
                    break;
                case "Leona":
                    ChampClass = new Leona();
                    break;
                case "Braum":
                    ChampClass = new Braum();
                    break;
                case "Janna":
                    ChampClass = new Janna();
                    break;
            }

            ChampClass.Id = ObjectManager.Player.BaseSkinName;
            ChampClass.Config = Config;

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            var orbwalking = Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            ChampClass.Orbwalker = new Orbwalking.Orbwalker(orbwalking);

            var items = Config.AddSubMenu(new Menu("Items", "Items"));
            ChampClass.ItemMenu(items);

            var combo = Config.AddSubMenu(new Menu("Combo", "Combo"));
            ChampClass.ComboMenu(combo);

            var harass = Config.AddSubMenu(new Menu("Harass", "Harass"));
            ChampClass.HarassMenu(harass);

            // Mana sliders :D
            var mana = Config.AddSubMenu(new Menu("Mana Limiter", "Mana Limiter"));
            mana.AddItem(new MenuItem("comboMana", "Combo Mana %").SetValue(new Slider(1, 100, 0)));
            mana.AddItem(new MenuItem("harassMana", "Harass Mana %").SetValue(new Slider(30, 100, 0)));
            ChampClass.ManaMenu(mana);

            var misc = Config.AddSubMenu(new Menu("Misc", "Misc"));
            misc.AddItem(new MenuItem("AttMin", "Attack Minions?").SetValue(false));
            ChampClass.MiscMenu(misc);

            var drawing = Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            ChampClass.DrawingMenu(drawing);

            ChampClass.MainMenu(Config);

            Config.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;
            Interrupter.OnPossibleToInterrupt += Interrupter_OnPosibleToInterrupt;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Game.OnGameSendPacket += Game_OnGameSendPacket;

        }

        static void Game_OnGameSendPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == Packet.C2S.Move.Header)
            {
                var decodedPacket = Packet.C2S.Move.Decoded(args.PacketData);
                if (decodedPacket.MoveType == 3)
                {
                    if (ChampClass.Orbwalker.GetTarget().IsMinion && !Config.Item("AttMin").GetValue<bool>())
                    {
                        args.Process = false;
                    }
                }
            }
        }

        static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            ChampClass.AntiGapcloser_OnEnemyGapCloser(gapcloser);
        }

        private static void Interrupter_OnPosibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            ChampClass.Interrupter_OnPossibleToInterrupt(unit, spell);
        }

        private static void Orbwalking_AfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            ChampClass.Orbwalking_AfterAttack(unit, target);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            ChampClass.Drawing_OnDraw(args);
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            // Update the combo and harass values.
            ChampClass.ComboActive = ChampClass.Config.Item("Orbwalk").GetValue<KeyBind>().Active && (((ObjectManager.Player.Mana / ObjectManager.Player.MaxMana) * 100) > Config.Item("comboMana").GetValue<Slider>().Value);
            ChampClass.HarassActive = ChampClass.Config.Item("Farm").GetValue<KeyBind>().Active && (((ObjectManager.Player.Mana / ObjectManager.Player.MaxMana) * 100) > Config.Item("harassMana").GetValue<Slider>().Value);
            ChampClass.Game_OnGameUpdate(args);
        }
    }
}
