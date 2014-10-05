using LeagueSharp;
using LeagueSharp.Common;

namespace Support
{
    public static class Extensions
    {
        public static bool IsValidTarget(this Items.Item item, Obj_AI_Base target)
        {
            return item != null && item.IsReady() && target.IsValidTarget(item.Range);
        }

        public static bool IsValidTarget(this Spell spell, Obj_AI_Base target, string menu, bool range = true, bool team = true)
        {
            return
                spell.IsReady() && 
                target.IsValidTarget(range ? spell.Range : float.MaxValue, team) && 
                PluginBase.Config.Item(menu + ObjectManager.Player.ChampionName).GetValue<bool>();
        }

        public static bool IsValidTarget(this Spell spell, Obj_AI_Base target, bool range = true, bool team = true)
        {
            return
                spell.IsReady() &&
                target.IsValidTarget(range ? spell.Range : float.MaxValue, team);
        }

        public static void AddBool(this Menu menu, string name, string displayName, bool value)
        {
            menu.AddItem(new MenuItem(name + ObjectManager.Player.ChampionName, displayName).SetValue(value));
        }

        public static void AddSlider(this Menu menu, string name, string displayName, int value, int min, int max)
        {
            menu.AddItem(new MenuItem(name + ObjectManager.Player.ChampionName, displayName).SetValue(new Slider(value, min, max)));
        }
    }
}
