using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace Support
{
    internal static class Utils
    {
        /// <summary>
        /// ReversePosition
        /// </summary>
        /// <param name="positionMe"></param>
        /// <param name="positionEnemy"></param>
        /// <remarks>Credit to LXMedia1</remarks>
        /// <returns>Vector3</returns>
        public static Vector3 ReversePosition(Vector3 positionMe, Vector3 positionEnemy)
        {
            var x = positionMe.X - positionEnemy.X;
            var y = positionMe.Y - positionEnemy.Y;
            return new Vector3(positionMe.X + x, positionMe.Y + y, positionMe.Z);
        }

        public static void PrintMessage(string message)
        {
            Game.PrintChat("<font color='#15C3AC'>Support:</font> <font color='#FFFFFF'>" + message + "</font>");
        }

        public static bool EnemyInRange(int numOfEnemy, float range)
        {
            return Utility.CountEnemysInRange((int)range) >= numOfEnemy;
        }

        public static List<Obj_AI_Hero> AllyInRange(float range)
        {
            return ObjectManager
                 .Get<Obj_AI_Hero>()
                 .Where(h => ObjectManager.Player.Distance(h.Position) < range && h.IsEnemy)
                 .OrderBy(h => ObjectManager.Player.Distance(h.Position))
                 .ToList();
        }

        public static Obj_AI_Hero AllyBelowHp(int percentHp, float range)
        {
            foreach (var ally in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (ally.IsMe)
                {
                    if (((ObjectManager.Player.Health / ObjectManager.Player.MaxHealth) * 100) < percentHp)
                    {
                        return ally;
                    }
                }
                else if (ally.IsAlly)
                {
                    if (Vector3.Distance(ObjectManager.Player.Position, ally.Position) < range && ((ally.Health / ally.MaxHealth) * 100) < percentHp)
                    {
                        return ally;
                    }
                }
            }

            return null;
        }
    }
}