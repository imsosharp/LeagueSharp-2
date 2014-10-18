#region LICENSE

// /*
// Copyright 2014 - 2014 Support
// Program.cs is part of Support.
// Support is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// Support is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with Support. If not, see <http://www.gnu.org/licenses/>.
// */
// 

#endregion

#region

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Support
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += a =>
            {
                try
                {
                    var type = Type.GetType("Support.Plugins." + ObjectManager.Player.ChampionName);

                    if (type != null)
                    {
                        Helpers.UpdateCheck();
                        Protector.Init();
                        Activator.CreateInstance(type);
                        return;
                    }

                    Helpers.PrintMessage(ObjectManager.Player.ChampionName + " not supported");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            };
        }
    }
}