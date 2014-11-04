#region

using System.IO;
using System.Timers;
using LeagueSharp;

#endregion

namespace Toaster2
{
    internal class Program
    {
        private const int WmKeydown = 0x100;
        private const int VkEscape = 0x1b;

        private static bool _escaped;

        private static readonly MemoryStream Packet = new MemoryStream();

        private static Timer _timer;


        private static void Main(string[] args)
        {
            Game.OnGameSendPacket += Game_OnGameSendPacket;
            Game.OnWndProc += Game_OnWndProc;
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != WmKeydown || args.WParam != VkEscape || _escaped)
                return;

            _escaped = true;
            Game.SendPacket(Packet.ToArray(), PacketChannel.C2S, PacketProtocolFlags.Reliable);

            Packet.Close();
        }

        private static void Game_OnGameSendPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] != 0xbd || _escaped)
                return;

            args.Process = false;

            Packet.WriteByte(args.PacketData[0]);
            Packet.Write(args.PacketData, 0, args.PacketData.Length);

            _timer = new Timer(260000.0);
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (!_escaped)
            {
                _escaped = true;
                Game.SendPacket(Packet.ToArray(), PacketChannel.C2S, PacketProtocolFlags.Reliable);

                Packet.Close();
            }
            _timer.Close();
        }
    }
}