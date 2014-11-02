#region LICENSE

// Copyright 2014 - 2014 ProFlash
// Program.cs is part of ProFlash.
// ProFlash is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// ProFlash is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with ProFlash. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace ProFlash
{
    internal class Program
    {
        private static readonly List<byte> SummonerByte = new List<byte> { 0xE9, 0xEF, 0x8B, 0xED, 0x63 };

        private static void Main(string[] args)
        {
            Game.OnGameSendPacket += p =>
            {
                if (Packet.C2S.Cast.Header != p.PacketData[0])
                    return;

                var packet = new GamePacket(p.PacketData);
                var summoner = SummonerByte.Contains(packet.ReadByte(5));
                var slot = (SpellSlot)packet.ReadByte();
                var flash = ObjectManager.Player.SummonerSpellbook.Spells.SingleOrDefault(s => s.Name == "summonerflash");

                if (flash != null)
                {
                    var flashSlot = flash.Slot;
                    var flashPacket = Packet.C2S.Cast.Decoded(p.PacketData);

                    if (summoner && slot == flashSlot && flashPacket.SourceNetworkId == ObjectManager.Player.NetworkId)
                    {
                        var to = new Vector2(flashPacket.ToX, flashPacket.ToY);
                        if (ObjectManager.Player.ServerPosition.To2D().Distance(to) < 390)
                        {
                            p.Process = false;

                            var maxRange = ObjectManager.Player.ServerPosition.To2D().Extend(to, 400);
                            flashPacket.FromX = maxRange.X;
                            flashPacket.FromY = maxRange.Y;
                            flashPacket.ToX = maxRange.X;
                            flashPacket.ToY = maxRange.Y;

                            Game.PrintChat("- ProFlash -");
                            Packet.C2S.Cast.Encoded(flashPacket).Send(p.Channel, p.ProtocolFlag);
                        }
                    }
                }
            };
        }
    }
}