using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace Killability
{
    internal class Killability
    {
        #region Static
        private readonly static Menu Config;

        static Killability()
        {
            Config = new Menu("Killability", "Killability", true);
            Config.AddItem(new MenuItem("icon", "Show Icon").SetValue(true));
            Config.AddItem(new MenuItem("text", "Show Text").SetValue(true));
            Config.AddToMainMenu();
        }

        public static List<Items.Item> Items = new List<Items.Item>
        {
            new Items.Item(3128, 750), // Deathfire Grasp
            new Items.Item(3077, 400), // Tiamat
            new Items.Item(3074, 400), // Ravenous Hydra
            new Items.Item(3146, 700), // Hextech Gunblade
            new Items.Item(3153, 450)  // Blade of the Ruined King
        };
        public static bool Icon { get { return Config.Item("icon").GetValue<bool>(); } }
        public static bool Text { get { return Config.Item("text").GetValue<bool>(); } }
        #endregion

        private readonly List<EnemyHero> _enemys = new List<EnemyHero>(); 

        public Killability()
        {
            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsEnemy))
            {
                _enemys.Add(new EnemyHero(hero));
            }
        }
    }
}
