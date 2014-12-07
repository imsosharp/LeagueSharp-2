#region LICENSE

// Copyright 2014 - 2014 LoLSkill
// Program.cs is part of LoLSkill.
// LoLSkill is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// LoLSkill is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with LoLSkill. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace LoLSkill
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                var sprite = new Render.Sprite(Images.Loadscreen, new Vector2(1, 1)).Add(0);
                var player0 = new Player(ObjectManager.Player, 1, 10);
            };
        }
    }

    public class Player : IDisposable
    {
        private readonly Obj_AI_Hero _unit;
        private int _ping;
        private int _loaded;

        private string Name
        {
            get { return _unit.Name; }
        }

        private Bitmap Summoner1
        {
            get { return (Bitmap)Images.ResourceManager.GetObject(_unit.SummonerSpellbook.GetSpell(SpellSlot.Summoner1).Name); }
        }

        private Bitmap Summoner2
        {
            get { return (Bitmap)Images.ResourceManager.GetObject(_unit.SummonerSpellbook.GetSpell(SpellSlot.Summoner2).Name); }
        }

        public string SkillScore;
        public string Wins;
        public string Performance;
        public string League;
        public string Division;
        public string WinLose;
        public string Kills;
        public string Deaths;
        public string Assists;
        public string Cs;
        public string Gold;

        private readonly List<Render.RenderObject> _renderObjects = new List<Render.RenderObject>();

        public Player(Obj_AI_Hero unit, int pos, int max)
        {
            _unit = unit;
            InitRenderObjects(pos, max);

            Game.OnGameProcessPacket += args =>
            {
                // TODO: decode load screen packet
                if (args.PacketData[0] != 0x00)
                    return;

                _ping = 0;
                _loaded = 0;
            };
        }

        private void InitRenderObjects(int pos, int max)
        {
            var origin = new Vector2(260, 20);

            var namePanelPos = origin + new Vector2(15, 11);
            var performancePanelPos = origin + new Vector2(100, 100);
            var statsPanelPos = origin + new Vector2(100, 100);

            var namePos = namePanelPos + new Vector2(5, 2);
            var summoner1Pos = origin + new Vector2(230, 40);
            var summoner2Pos = origin + new Vector2(230, 75);
            var pingPos = origin + new Vector2(0, 0);
            var loadedPos = origin + new Vector2(0, 0);

            var skillScorePos = origin + new Vector2(0, 0);
            var winsPos = origin + new Vector2(0, 0);

            var performancePos = performancePanelPos + new Vector2(0, 0);
            var leaguePos = performancePanelPos + new Vector2(0, 0);
            var divisionPos = performancePanelPos + new Vector2(0, 0);

            var winLosePos = statsPanelPos + new Vector2(0, 0);
            var killsPos = statsPanelPos + new Vector2(0, 0);
            var deathsPos = statsPanelPos + new Vector2(0, 0);
            var assistsPos = statsPanelPos + new Vector2(0, 0);
            var csPos = statsPanelPos + new Vector2(0, 0);
            var goldPos = statsPanelPos + new Vector2(0, 0);

            // panels
            _renderObjects.Add(new Render.Rectangle(namePanelPos, 250, 20, "#121212".ToColorBgra()).Add());
            _renderObjects.Add(new Render.Rectangle(performancePanelPos, 250, 20, "#121212".ToColorBgra()).Add());
            _renderObjects.Add(new Render.Rectangle(statsPanelPos, 250, 20, "#202020".ToColorBgra()).Add());

            // name
            _renderObjects.Add(new Render.Text(namePos, Name, 20, new ColorBGRA()).Add());

            // summoner1
            var summoner1 = new Render.Sprite(Summoner1, summoner1Pos) { Scale = new Vector2(0.5f, 0.5f) };
            _renderObjects.Add(summoner1.Add());

            // summoner2
            var summoner2 = new Render.Sprite(Summoner2, summoner2Pos) { Scale = new Vector2(0.5f, 0.5f) };
            _renderObjects.Add(summoner2.Add());

            //// ping
            //var ping = new Render.Text(pingPos, "", 20, new ColorBGRA(255, 0, 0, 200));
            //ping.TextUpdate += () => string.Format("{0}", _ping);
            //_renderObjects.Add(ping.Add());

            //// loaded
            //var loaded = new Render.Text(loadedPos, "", 20, new ColorBGRA(255, 0, 0, 200));
            //loaded.TextUpdate += () => string.Format("{0}%", _loaded);
            //_renderObjects.Add(loaded.Add());

            //// skill score
            //var killscore = new Render.Text(skillScorePos, SkillScore, 20, new ColorBGRA(255, 0, 0, 200)) { Centered = true };
            //_renderObjects.Add(killscore.Add());

            //// ranked Wins + normal wins
            //var wins = new Render.Text(winsPos, Wins, 20, new ColorBGRA(255, 0, 0, 200));
            //_renderObjects.Add(wins.Add());

            //// performace
            //var performace = new Render.Text(performancePos, string.Format("PERFORMANCE: {0}", Performance), 20, new ColorBGRA(255, 0, 0, 200));
            //_renderObjects.Add(performace.Add());

            //// league
            //var league = new Render.Sprite(Helpers.Base64ToImage(League), leaguePos);
            //_renderObjects.Add(league.Add());

            //// division
            //var division = new Render.Text(divisionPos, Division, 20, new ColorBGRA(255, 0, 0, 200));
            //_renderObjects.Add(division.Add());

            //// winLose
            //var winLose = new Render.Text(winLosePos, WinLose, 20, new ColorBGRA(255, 0, 0, 200));
            //_renderObjects.Add(winLose.Add());

            //// kills
            //var kills = new Render.Text(killsPos, Kills, 20, new ColorBGRA(255, 0, 0, 200));
            //_renderObjects.Add(kills.Add());

            //// deaths
            //var deaths = new Render.Text(deathsPos, Deaths, 20, new ColorBGRA(255, 0, 0, 200));
            //_renderObjects.Add(deaths.Add());

            //// assists
            //var assists = new Render.Text(assistsPos, Assists, 20, new ColorBGRA(255, 0, 0, 200));
            //_renderObjects.Add(assists.Add());

            //// cs
            //var cs = new Render.Text(csPos, Cs, 20, new ColorBGRA(255, 0, 0, 200));
            //_renderObjects.Add(cs.Add());

            //// gold
            //var gold = new Render.Text(goldPos, Gold, 20, new ColorBGRA(255, 0, 0, 200));
            //_renderObjects.Add(gold.Add());
        }

        public void Dispose()
        {
            foreach (var renderObject in _renderObjects)
            {
                renderObject.Dispose();
            }
        }
    }

    public static class Helpers
    {
        public static Bitmap Base64ToImage(string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            var image = (Bitmap)Image.FromStream(ms, true);
            return image;
        }

        public static ColorBGRA ToColorBgra(this string color)
        {
            var c = ColorTranslator.FromHtml(color);
            return new ColorBGRA(c.R, c.G, c.B, c.A);
        }
    }
}