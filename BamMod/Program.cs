using System;
using System.Media;
using LeagueSharp;
using LeagueSharp.Common;

namespace BamMod
{
    class Program
    {
        private static readonly SoundPlayer Player = new SoundPlayer(Sounds.bam);
        private static readonly Menu Config = new Menu("BamMod", "BamMod", true);

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                Config.AddItem(new MenuItem("Print", "BAM!").SetValue(true));
                Config.AddItem(new MenuItem("PlaySound", "Play Sound").SetValue(true));
                Config.AddToMainMenu();
            };

            Game.OnGameProcessPacket += packet =>
            {
                if (packet.PacketData[0] != Packet.S2C.Damage.Header)
                    return;

                var dmgPacket = Packet.S2C.Damage.Decoded(packet.PacketData);

                if (dmgPacket.SourceNetworkId != ObjectManager.Player.NetworkId || dmgPacket.Type != Packet.DamageTypePacket.CriticalAttack)
                    return;

                if (Config.Item("Print").GetValue<bool>())
                    Packet.S2C.FloatText.Encoded(new Packet.S2C.FloatText.Struct("BAM " + (int)dmgPacket.DamageAmount, Packet.FloatTextPacket.Critical,
                        dmgPacket.TargetNetworkId)).Process();

                if (Config.Item("PlaySound").GetValue<bool>())
                    Player.Play();
            };
        }
    }
}
