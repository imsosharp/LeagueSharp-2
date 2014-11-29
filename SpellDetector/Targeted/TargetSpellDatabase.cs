#region LICENSE

// Copyright 2014 - 2014 SpellDetector
// TargetSpellDatabase.cs is part of SpellDetector.
// SpellDetector is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// SpellDetector is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with SpellDetector. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System.Collections.Generic;
using System.Linq;
using LeagueSharp;

#endregion

namespace SpellDetector.Targeted
{
    public static class TargetSpellDatabase
    {
        public static List<TargetSpellData> Spells;

        static TargetSpellDatabase()
        {
            Spells = new List<TargetSpellData>
            {
                #region Aatrox
                new TargetSpellData("aatrox", "aatroxq", SpellSlot.Q, Spelltype.Skillshot, CcType.Knockup, 650, 500, 20),
                new TargetSpellData("aatrox", "aatroxw", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("aatrox", "aatroxw2", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("aatrox", "aatroxe", SpellSlot.E, Spelltype.Skillshot, CcType.Stun, 1000, 500, 1200),
                new TargetSpellData("aatrox", "aatroxr", SpellSlot.R, Spelltype.Self, CcType.No, 550, 0, 0),

                #endregion Aatrox

                #region Ahri
                new TargetSpellData("ahri", "ahriorbofdeception", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 880, 500, 1100),
                new TargetSpellData("ahri", "ahrifoxfire", SpellSlot.W, Spelltype.Self, CcType.No, 800, 0, 1800),
                new TargetSpellData("ahri", "ahriseduce", SpellSlot.E, Spelltype.Skillshot, CcType.Charm, 975, 500, 1200),
                new TargetSpellData("ahri", "ahritumble", SpellSlot.R, Spelltype.Skillshot, CcType.No, 450, 500, 2200),

                #endregion Ahri

                #region Akali
                new TargetSpellData("akali", "akalimota", SpellSlot.Q, Spelltype.Targeted, CcType.No, 600, 650, 1000),
                new TargetSpellData("akali", "akalismokebomb", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 700, 500, 0),
                new TargetSpellData("akali", "akalishadowswipe", SpellSlot.E, Spelltype.Self, CcType.No, 325, 0, 0),
                new TargetSpellData("akali", "akalishadowdance", SpellSlot.R, Spelltype.Targeted, CcType.No, 800, 0, 2200),

                #endregion Akali

                #region Alistar
                new TargetSpellData("alistar", "pulverize", SpellSlot.Q, Spelltype.Self, CcType.Knockup, 365, 500, 20),
                new TargetSpellData("alistar", "headbutt", SpellSlot.W, Spelltype.Targeted, CcType.Knockback, 650, 500, 0),
                new TargetSpellData("alistar", "triumphantroar", SpellSlot.E, Spelltype.Self, CcType.No, 575, 0, 0),
                new TargetSpellData("alistar", "feroucioushowl", SpellSlot.R, Spelltype.Self, CcType.No, 0, 0, 828),

                #endregion Alistar

                #region Amumu
                new TargetSpellData("amumu", "bandagetoss", SpellSlot.Q, Spelltype.Skillshot, CcType.Stun, 1100, 500, 2000),
                new TargetSpellData("amumu", "auraofdespair", SpellSlot.W, Spelltype.Self, CcType.No, 300, 470, float.MaxValue),
                new TargetSpellData("amumu", "tantrum", SpellSlot.E, Spelltype.Self, CcType.No, 350, 500, float.MaxValue),
                new TargetSpellData("amumu", "curseofthesadmummy", SpellSlot.R, Spelltype.Self, CcType.Stun, 550, 500, float.MaxValue),

                #endregion Amumu

                #region Anivia
                new TargetSpellData("anivia", "flashfrost", SpellSlot.Q, Spelltype.Skillshot, CcType.Stun, 1200, 500, 850),
                new TargetSpellData("anivia", "crystalize", SpellSlot.W, Spelltype.Skillshot, CcType.No, 1000, 500, 1600),
                new TargetSpellData("anivia", "frostbite", SpellSlot.E, Spelltype.Targeted, CcType.No, 650, 500, 1200),
                new TargetSpellData("anivia", "glacialstorm", SpellSlot.R, Spelltype.Skillshot, CcType.Slow, 675, 300, float.MaxValue),

                #endregion Anivia

                #region Annie
                new TargetSpellData("annie", "disintegrate", SpellSlot.Q, Spelltype.Targeted, CcType.No, 623, 500, 1400),
                new TargetSpellData("annie", "incinerate", SpellSlot.W, Spelltype.Targeted, CcType.No, 623, 500, 0),
                new TargetSpellData("annie", "moltenshield", SpellSlot.E, Spelltype.Self, CcType.No, 100, 0, 20),
                new TargetSpellData("annie", "infernalguardian", SpellSlot.R, Spelltype.Skillshot, CcType.No, 600, 500, float.MaxValue),

                #endregion Annie

                #region Ashe
                new TargetSpellData("ashe", "frostshot", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("ashe", "frostarrow", SpellSlot.Q, Spelltype.Targeted, CcType.Slow, 0, 0, float.MaxValue),
                new TargetSpellData("ashe", "volley", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 1200, 500, 902),
                new TargetSpellData("ashe", "ashespiritofthehawk", SpellSlot.E, Spelltype.Skillshot, CcType.No, 2500, 500, 1400),
                new TargetSpellData("ashe", "enchantedcrystalarrow", SpellSlot.R, Spelltype.Skillshot, CcType.Stun, 50000, 500, 1600),

                #endregion Ashe

                #region Blitzcrank
                new TargetSpellData("blitzcrank", "rocketgrabmissile", SpellSlot.Q, Spelltype.Skillshot, CcType.Pull, 925, 220, 1800),
                new TargetSpellData("blitzcrank", "overdrive", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("blitzcrank", "powerfist", SpellSlot.E, Spelltype.Self, CcType.Knockup, 0, 0, 0),
                new TargetSpellData("blitzcrank", "staticfield", SpellSlot.R, Spelltype.Self, CcType.Silence, 600, 0, 0),

                #endregion Blitzcrank

                #region Brand
                new TargetSpellData("brand", "brandblaze", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1150, 500, 1200),
                new TargetSpellData("brand", "brandfissure", SpellSlot.W, Spelltype.Skillshot, CcType.No, 240, 500, 20),
                new TargetSpellData("brand", "brandconflagration", SpellSlot.E, Spelltype.Targeted, CcType.No, 0, 0, 1800),
                new TargetSpellData("brand", "brandwildfire", SpellSlot.R, Spelltype.Targeted, CcType.No, 0, 0, 1000),

                #endregion Brand

                #region Braum
                new TargetSpellData("braum", "braumq", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 1100, 500, 1200),
                new TargetSpellData("braum", "braumqmissle", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 1100, 500, 1200),
                new TargetSpellData("braum", "braumw", SpellSlot.W, Spelltype.Targeted, CcType.No, 650, 500, 1500),
                new TargetSpellData("braum", "braume", SpellSlot.E, Spelltype.Skillshot, CcType.No, 250, 0, float.MaxValue),
                new TargetSpellData("braum", "braumr", SpellSlot.R, Spelltype.Skillshot, CcType.Knockup, 1250, 0, 1200),

                #endregion Braum

                #region Caitlyn
                new TargetSpellData("caitlyn", "caitlynpiltoverpeacemaker", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 2000, 250, 2200),
                new TargetSpellData("caitlyn", "caitlynyordletrap", SpellSlot.W, Spelltype.Skillshot, CcType.Snare, 800, 0, 1400),
                new TargetSpellData("caitlyn", "caitlynentrapment", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 950, 250, 2000),
                new TargetSpellData("caitlyn", "caitlynaceinthehole", SpellSlot.R, Spelltype.Targeted, CcType.No, 2500, 0, 1500),

                #endregion Caitlyn

                #region Cassiopeia
                new TargetSpellData("cassiopeia", "cassiopeianoxiousblast", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 925, 250, float.MaxValue),
                new TargetSpellData("cassiopeia", "cassiopeiamiasma", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 925, 500, 2500),
                new TargetSpellData("cassiopeia", "cassiopeiatwinfang", SpellSlot.E, Spelltype.Targeted, CcType.No, 700, 0, 1900),
                new TargetSpellData("cassiopeia", "cassiopeiapetrifyinggaze", SpellSlot.R, Spelltype.Skillshot, CcType.Stun, 875, 500, float.MaxValue),

                #endregion Cassiopeia

                #region Chogath
                new TargetSpellData("chogath", "rupture", SpellSlot.Q, Spelltype.Skillshot, CcType.Knockup, 1000, 500, float.MaxValue),
                new TargetSpellData("chogath", "feralscream", SpellSlot.W, Spelltype.Skillshot, CcType.Silence, 675, 250, float.MaxValue),
                new TargetSpellData("chogath", "vorpalspikes", SpellSlot.E, Spelltype.Targeted, CcType.No, 0, 0, 347),
                new TargetSpellData("chogath", "feast", SpellSlot.R, Spelltype.Targeted, CcType.No, 230, 0, 500),

                #endregion Chogath

                #region Corki
                new TargetSpellData("corki", "phosphorusbomb", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 875, 0, float.MaxValue),
                new TargetSpellData("corki", "carpetbomb", SpellSlot.W, Spelltype.Skillshot, CcType.No, 875, 0, 700),
                new TargetSpellData("corki", "ggun", SpellSlot.E, Spelltype.Skillshot, CcType.No, 750, 0, 902),
                new TargetSpellData("corki", "missilebarrage", SpellSlot.R, Spelltype.Skillshot, CcType.No, 1225, 250, 828),

                #endregion Corki

                #region Darius
                new TargetSpellData("darius", "dariuscleave", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 425, 500, 0),
                new TargetSpellData("darius", "dariusnoxiantacticsonh", SpellSlot.W, Spelltype.Self, CcType.Slow, 210, 0, 0),
                new TargetSpellData("darius", "dariusaxegrabcone", SpellSlot.E, Spelltype.Skillshot, CcType.Pull, 540, 500, 1500),
                new TargetSpellData("darius", "dariusexecute", SpellSlot.R, Spelltype.Targeted, CcType.No, 460, 500, 20),

                #endregion Darius

                #region Diana
                new TargetSpellData("diana", "dianaarc", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 900, 500, 1500),
                new TargetSpellData("diana", "dianaorbs", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("diana", "dianavortex", SpellSlot.E, Spelltype.Self, CcType.Pull, 300, 500, 1500),
                new TargetSpellData("diana", "dianateleport", SpellSlot.R, Spelltype.Targeted, CcType.No, 800, 500, 1500),

                #endregion Diana

                #region Draven
                new TargetSpellData("draven", "dravenspinning", SpellSlot.Q, Spelltype.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("draven", "dravenfury", SpellSlot.W, Spelltype.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("draven", "dravendoubleshot", SpellSlot.E, Spelltype.Skillshot, CcType.Knockback, 1050, 500, 1600),
                new TargetSpellData("draven", "dravenrcast", SpellSlot.R, Spelltype.Skillshot, CcType.No, 20000, 500, 2000),

                #endregion Draven

                #region DrMundo
                new TargetSpellData("drmundo", "infectedcleavermissilecast", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 1000, 500, 1500),
                new TargetSpellData("drmundo", "burningagony", SpellSlot.W, Spelltype.Self, CcType.No, 225, float.MaxValue, float.MaxValue),
                new TargetSpellData("drmundo", "masochism", SpellSlot.E, Spelltype.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("drmundo", "sadism", SpellSlot.R, Spelltype.Self, CcType.No, 0, float.MaxValue, float.MaxValue),

                #endregion DrMundo

                #region Elise
                new TargetSpellData("elise", "elisehumanq", SpellSlot.Q, Spelltype.Targeted, CcType.No, 625, 750, 2200),
                new TargetSpellData("elise", "elisespiderqcast", SpellSlot.Q, Spelltype.Targeted, CcType.No, 475, 500, float.MaxValue),
                new TargetSpellData("elise", "elisehumanw", SpellSlot.W, Spelltype.Skillshot, CcType.No, 950, 750, 5000),
                new TargetSpellData("elise", "elisespiderw", SpellSlot.W, Spelltype.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("elise", "elisehumane", SpellSlot.E, Spelltype.Skillshot, CcType.Stun, 1075, 500, 1450),
                new TargetSpellData("elise", "elisespidereinitial", SpellSlot.E, Spelltype.Targeted, CcType.No, 975, float.MaxValue, float.MaxValue),
                new TargetSpellData("elise", "elisespideredescent", SpellSlot.E, Spelltype.Targeted, CcType.No, 975, float.MaxValue, float.MaxValue),
                new TargetSpellData("elise", "eliser", SpellSlot.R, Spelltype.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("elise", "elisespiderr", SpellSlot.R, Spelltype.Self, CcType.No, 0, float.MaxValue, float.MaxValue),

                #endregion Elise

                #region Evelynn
                new TargetSpellData("evelynn", "evelynnq", SpellSlot.Q, Spelltype.Self, CcType.No, 500, 500, float.MaxValue),
                new TargetSpellData("evelynn", "evelynnw", SpellSlot.W, Spelltype.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("evelynn", "evelynne", SpellSlot.E, Spelltype.Targeted, CcType.No, 290, 500, 900),
                new TargetSpellData("evelynn", "evelynnr", SpellSlot.R, Spelltype.Skillshot, CcType.Slow, 650, 500, 1300),

                #endregion Evelynn

                #region Ezreal
                new TargetSpellData("ezreal", "ezrealmysticshot", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1200, 250, 2000),
                new TargetSpellData("ezreal", "ezrealessenceflux", SpellSlot.W, Spelltype.Skillshot, CcType.No, 1050, 250, 1600),
                new TargetSpellData("ezreal", "ezrealessencemissle", SpellSlot.W, Spelltype.Skillshot, CcType.No, 1050, 250, 1600),
                new TargetSpellData("ezreal", "ezrealarcaneshift", SpellSlot.E, Spelltype.Targeted, CcType.No, 475, 500, float.MaxValue),
                new TargetSpellData("ezreal", "ezrealtruehotbarrage", SpellSlot.R, Spelltype.Skillshot, CcType.No, 20000, 1000, 2000),

                #endregion Ezreal

                #region FiddleSticks
                new TargetSpellData("fiddlesticks", "terrify", SpellSlot.Q, Spelltype.Targeted, CcType.Fear, 575, 500, float.MaxValue),
                new TargetSpellData("fiddlesticks", "drain", SpellSlot.W, Spelltype.Targeted, CcType.No, 575, 500, float.MaxValue),
                new TargetSpellData("fiddlesticks", "fiddlesticksdarkwind", SpellSlot.E, Spelltype.Skillshot, CcType.Silence, 750, 500, 1100),
                new TargetSpellData("fiddlesticks", "crowstorm", SpellSlot.R, Spelltype.Targeted, CcType.No, 800, 500, float.MaxValue),

                #endregion FiddleSticks

                #region Fiora
                new TargetSpellData("fiora", "fioraq", SpellSlot.Q, Spelltype.Targeted, CcType.No, 300, 500, 2200),
                new TargetSpellData("fiora", "fiorariposte", SpellSlot.W, Spelltype.Self, CcType.No, 100, 0, 0),
                new TargetSpellData("fiora", "fioraflurry", SpellSlot.E, Spelltype.Self, CcType.No, 210, 0, 0),
                new TargetSpellData("fiora", "fioradance", SpellSlot.R, Spelltype.Targeted, CcType.No, 210, 500, 0),

                #endregion Fiora

                #region Fizz
                new TargetSpellData("fizz", "fizzpiercingstrike", SpellSlot.Q, Spelltype.Targeted, CcType.No, 550, 500, float.MaxValue),
                new TargetSpellData("fizz", "fizzseastonepassive", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, 0),
                new TargetSpellData("fizz", "fizzjump", SpellSlot.E, Spelltype.Self, CcType.No, 400, 500, 1300),
                new TargetSpellData("fizz", "fizzjumptwo", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 400, 500, 1300),
                new TargetSpellData("fizz", "fizzmarinerdoom", SpellSlot.R, Spelltype.Skillshot, CcType.Knockup, 1275, 500, 1200),

                #endregion Fizz

                #region Galio
                new TargetSpellData("galio", "galioresolutesmite", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 940, 500, 1300),
                new TargetSpellData("galio", "galiobulwark", SpellSlot.W, Spelltype.Targeted, CcType.No, 800, 500, float.MaxValue),
                new TargetSpellData("galio", "galiorighteousgust", SpellSlot.E, Spelltype.Skillshot, CcType.No, 1180, 500, 1200),
                new TargetSpellData("galio", "galioidolofdurand", SpellSlot.R, Spelltype.Self, CcType.Taunt, 560, 500, float.MaxValue),

                #endregion Galio

                #region Gangplank
                new TargetSpellData("gangplank", "parley", SpellSlot.Q, Spelltype.Targeted, CcType.No, 625, 500, 2000),
                new TargetSpellData("gangplank", "removescurvy", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("gangplank", "raisemorale", SpellSlot.E, Spelltype.Self, CcType.No, 1300, 500, float.MaxValue),
                new TargetSpellData("gangplank", "cannonbarrage", SpellSlot.R, Spelltype.Skillshot, CcType.Slow, 20000, 500, 500),

                #endregion Gangplank

                #region Garen
                new TargetSpellData("garen", "garenq", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 200, float.MaxValue),
                new TargetSpellData("garen", "garenw", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("garen", "garene", SpellSlot.E, Spelltype.Self, CcType.No, 325, 0, 700),
                new TargetSpellData("garen", "garenr", SpellSlot.R, Spelltype.Targeted, CcType.No, 400, 120, float.MaxValue),

                #endregion Garen

                #region Gragas
                new TargetSpellData("gragas", "gragasq", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 1100, 300, 1000),
                new TargetSpellData("gragas", "gragasqtoggle", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1100, 300, 1000),
                new TargetSpellData("gragas", "gragasw", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("gragas", "gragase", SpellSlot.E, Spelltype.Skillshot, CcType.Knockback, 1100, 300, 1000),
                new TargetSpellData("gragas", "gragasr", SpellSlot.R, Spelltype.Skillshot, CcType.Knockback, 1100, 300, 1000),

                #endregion Gragas

                #region Graves
                new TargetSpellData("graves", "gravesclustershot", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1100, 300, 902),
                new TargetSpellData("graves", "gravessmokegrenade", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 1100, 300, 1650),
                new TargetSpellData("graves", "gravessmokegrenadeboom", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 1100, 300, 1650),
                new TargetSpellData("graves", "gravesmove", SpellSlot.E, Spelltype.Skillshot, CcType.No, 425, 300, 1000),
                new TargetSpellData("graves", "graveschargeshot", SpellSlot.R, Spelltype.Skillshot, CcType.No, 1000, 500, 1200),

                #endregion Graves

                #region Hecarim
                new TargetSpellData("hecarim", "hecarimrapidslash", SpellSlot.Q, Spelltype.Self, CcType.No, 350, 300, 1450),
                new TargetSpellData("hecarim", "hecarimw", SpellSlot.W, Spelltype.Self, CcType.No, 525, 120, 828),
                new TargetSpellData("hecarim", "hecarimramp", SpellSlot.E, Spelltype.Self, CcType.No, 0, float.MaxValue, float.MaxValue),
                new TargetSpellData("hecarim", "hecarimult", SpellSlot.R, Spelltype.Skillshot, CcType.Fear, 1350, 500, 1200),

                #endregion Hecarim

                #region Heimerdinger
                new TargetSpellData("heimerdinger", "heimerdingerq", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 350, 500, float.MaxValue),
                new TargetSpellData("heimerdinger", "heimerdingerw", SpellSlot.W, Spelltype.Skillshot, CcType.No, 1525, 500, 902),
                new TargetSpellData("heimerdinger", "heimerdingere", SpellSlot.E, Spelltype.Skillshot, CcType.Stun, 970, 500, 2500),
                new TargetSpellData("heimerdinger", "heimerdingerr", SpellSlot.R, Spelltype.Self, CcType.No, 0, 230, float.MaxValue),
                new TargetSpellData("heimerdinger", "heimerdingereult", SpellSlot.R, Spelltype.Skillshot, CcType.Stun, 970, 230, float.MaxValue),

                #endregion Heimerdinger

                #region Irelia
                new TargetSpellData("irelia", "ireliagatotsu", SpellSlot.Q, Spelltype.Targeted, CcType.No, 650, 0, 2200),
                new TargetSpellData("irelia", "ireliahitenstyle", SpellSlot.W, Spelltype.Self, CcType.No, 0, 230, 347),
                new TargetSpellData("irelia", "ireliaequilibriumstrike", SpellSlot.E, Spelltype.Targeted, CcType.Stun, 325, 500, float.MaxValue),
                new TargetSpellData("irelia", "ireliatranscendentblades", SpellSlot.R, Spelltype.Skillshot, CcType.No, 1200, 500, 779),

                #endregion Irelia

                #region Janna
                new TargetSpellData("janna", "howlinggale", SpellSlot.Q, Spelltype.Skillshot, CcType.Knockup, 1800, 0, float.MaxValue),
                new TargetSpellData("janna", "sowthewind", SpellSlot.W, Spelltype.Targeted, CcType.Slow, 600, 500, 1600),
                new TargetSpellData("janna", "eyeofthestorm", SpellSlot.E, Spelltype.Targeted, CcType.No, 800, 500, float.MaxValue),
                new TargetSpellData("janna", "reapthewhirlwind", SpellSlot.R, Spelltype.Self, CcType.Knockback, 725, 500, 828),

                #endregion Janna

                #region JarvanIV
                new TargetSpellData("jarvaniv", "jarvanivdragonstrike", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 700, 500, float.MaxValue),
                new TargetSpellData("jarvaniv", "jarvanivgoldenaegis", SpellSlot.W, Spelltype.Self, CcType.Slow, 300, 500, 0),
                new TargetSpellData("jarvaniv", "jarvanivdemacianstandard", SpellSlot.E, Spelltype.Skillshot, CcType.No, 830, 500, float.MaxValue),
                new TargetSpellData("jarvaniv", "jarvanivcataclysm", SpellSlot.R, Spelltype.Skillshot, CcType.No, 650, 500, 0),

                #endregion JarvanIV

                #region Jax
                new TargetSpellData("jax", "jaxleapstrike", SpellSlot.Q, Spelltype.Targeted, CcType.No, 210, 500, 0),
                new TargetSpellData("jax", "jaxempowertwo", SpellSlot.W, Spelltype.Targeted, CcType.No, 0, 500, 0),
                new TargetSpellData("jax", "jaxcounterstrike", SpellSlot.E, Spelltype.Self, CcType.Stun, 425, 500, 1450),
                new TargetSpellData("jax", "jaxrelentlessasssault", SpellSlot.R, Spelltype.Self, CcType.No, 0, 0, 0),

                #endregion Jax

                #region Jayce
                new TargetSpellData("jayce", "jaycetotheskies", SpellSlot.Q, Spelltype.Targeted, CcType.Slow, 600, 500, float.MaxValue),
                new TargetSpellData("jayce", "jayceshockblast", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1050, 500, 1200),
                new TargetSpellData("jayce", "jaycestaticfield", SpellSlot.W, Spelltype.Self, CcType.No, 285, 500, 1500),
                new TargetSpellData("jayce", "jaycehypercharge", SpellSlot.W, Spelltype.Self, CcType.No, 0, 750, float.MaxValue),
                new TargetSpellData("jayce", "jaycethunderingblow", SpellSlot.E, Spelltype.Targeted, CcType.Knockback, 300, 0, float.MaxValue),
                new TargetSpellData("jayce", "jayceaccelerationgate", SpellSlot.E, Spelltype.Skillshot, CcType.No, 685, 500, 1600),
                new TargetSpellData("jayce", "jaycestancehtg", SpellSlot.R, Spelltype.Self, CcType.No, 0, 750, float.MaxValue),
                new TargetSpellData("jayce", "jaycestancegth", SpellSlot.R, Spelltype.Self, CcType.No, 0, 750, float.MaxValue),

                #endregion Jayce

                #region Jinx
                new TargetSpellData("jinx", "jinxq", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("jinx", "jinxw", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 1550, 500, 1200),
                new TargetSpellData("jinx", "jinxwmissle", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 1550, 500, 1200),
                new TargetSpellData("jinx", "jinxe", SpellSlot.E, Spelltype.Skillshot, CcType.Snare, 900, 500, 1000),
                new TargetSpellData("jinx", "jinxr", SpellSlot.R, Spelltype.Skillshot, CcType.No, 25000, 0, float.MaxValue),
                new TargetSpellData("jinx", "jinxrwrapper", SpellSlot.R, Spelltype.Skillshot, CcType.No, 25000, 0, float.MaxValue),

                #endregion Jinx

                #region Karma
                new TargetSpellData("karma", "karmaq", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 950, 500, 902),
                new TargetSpellData("karma", "karmaspiritbind", SpellSlot.W, Spelltype.Targeted, CcType.Snare, 700, 500, 2000),
                new TargetSpellData("karma", "karmasolkimshield", SpellSlot.E, Spelltype.Targeted, CcType.No, 800, 500, float.MaxValue),
                new TargetSpellData("karma", "karmamantra", SpellSlot.R, Spelltype.Self, CcType.No, 0, 500, 1300),

                #endregion Karma

                #region Karthus
                new TargetSpellData("karthus", "laywaste", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 875, 500, float.MaxValue),
                new TargetSpellData("karthus", "wallofpain", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 1090, 500, 1600),
                new TargetSpellData("karthus", "defile", SpellSlot.E, Spelltype.Self, CcType.No, 550, 500, 1000),
                new TargetSpellData("karthus", "fallenone", SpellSlot.R, Spelltype.Self, CcType.No, 20000, 0, float.MaxValue),

                #endregion Karthus

                #region Kassadin
                new TargetSpellData("kassadin", "nulllance", SpellSlot.Q, Spelltype.Targeted, CcType.Silence, 650, 500, 1400),
                new TargetSpellData("kassadin", "netherblade", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("kassadin", "forcepulse", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 700, 500, float.MaxValue),
                new TargetSpellData("kassadin", "riftwalk", SpellSlot.R, Spelltype.Skillshot, CcType.No, 675, 500, float.MaxValue),

                #endregion Kassadin

                #region Katarina
                new TargetSpellData("katarina", "katarinaq", SpellSlot.Q, Spelltype.Targeted, CcType.No, 675, 500, 1800),
                new TargetSpellData("katarina", "katarinaw", SpellSlot.W, Spelltype.Self, CcType.No, 400, 500, 1800),
                new TargetSpellData("katarina", "katarinae", SpellSlot.E, Spelltype.Targeted, CcType.No, 700, 500, 0),
                new TargetSpellData("katarina", "katarinar", SpellSlot.R, Spelltype.Self, CcType.No, 550, 500, 1450),

                #endregion Katarina

                #region Kayle
                new TargetSpellData("kayle", "judicatorreckoning", SpellSlot.Q, Spelltype.Targeted, CcType.Slow, 650, 500, 1500),
                new TargetSpellData("kayle", "judicatordevineblessing", SpellSlot.W, Spelltype.Targeted, CcType.No, 900, 220, float.MaxValue),
                new TargetSpellData("kayle", "judicatorrighteousfury", SpellSlot.E, Spelltype.Self, CcType.No, 0, 500, 779),
                new TargetSpellData("kayle", "judicatorintervention", SpellSlot.R, Spelltype.Targeted, CcType.No, 900, 500, float.MaxValue),

                #endregion Kayle

                #region Kennen
                new TargetSpellData("kennen", "kennenshurikenhurlmissile1", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1000, 690, 1700),
                new TargetSpellData("kennen", "kennenbringthelight", SpellSlot.W, Spelltype.Self, CcType.No, 900, 500, float.MaxValue),
                new TargetSpellData("kennen", "kennenlightningrush", SpellSlot.E, Spelltype.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("kennen", "kennenshurikenstorm", SpellSlot.R, Spelltype.Self, CcType.No, 550, 500, 779),

                #endregion Kennen

                #region Khazix
                new TargetSpellData("khazix", "khazixq", SpellSlot.Q, Spelltype.Targeted, CcType.No, 325, 500, float.MaxValue),
                new TargetSpellData("khazix", "khazixqlong", SpellSlot.Q, Spelltype.Targeted, CcType.No, 375, 500, float.MaxValue),
                new TargetSpellData("khazix", "khazixw", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 1000, 500, 828),
                new TargetSpellData("khazix", "khazixwlong", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 1000, 500, 828),
                new TargetSpellData("khazix", "khazixe", SpellSlot.E, Spelltype.Skillshot, CcType.No, 600, 500, float.MaxValue),
                new TargetSpellData("khazix", "khazixelong", SpellSlot.E, Spelltype.Skillshot, CcType.No, 900, 500, float.MaxValue),
                new TargetSpellData("khazix", "khazixr", SpellSlot.R, Spelltype.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("khazix", "khazixrlong", SpellSlot.R, Spelltype.Self, CcType.No, 0, 0, float.MaxValue),

                #endregion Khazix

                #region KogMaw
                new TargetSpellData("kogmaw", "kogmawcausticspittle", SpellSlot.Q, Spelltype.Targeted, CcType.No, 625, 500, float.MaxValue),
                new TargetSpellData("kogmaw", "kogmawbioarcanbarrage", SpellSlot.W, Spelltype.Self, CcType.No, 130, 500, 2000),
                new TargetSpellData("kogmaw", "kogmawvoidooze", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 1000, 500, 1200),
                new TargetSpellData("kogmaw", "kogmawlivingartillery", SpellSlot.R, Spelltype.Skillshot, CcType.No, 1400, 600, 2000),

                #endregion KogMaw

                #region Leblanc
                new TargetSpellData("leblanc", "leblancchaosorb", SpellSlot.Q, Spelltype.Targeted, CcType.No, 700, 500, 2000),
                new TargetSpellData("leblanc", "leblancslide", SpellSlot.W, Spelltype.Skillshot, CcType.No, 600, 500, float.MaxValue),
                new TargetSpellData("leblanc", "leblacslidereturn", SpellSlot.W, Spelltype.Skillshot, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("leblanc", "leblancsoulshackle", SpellSlot.E, Spelltype.Skillshot, CcType.Snare, 925, 500, 1600),
                new TargetSpellData("leblanc", "leblancchaosorbm", SpellSlot.R, Spelltype.Targeted, CcType.No, 700, 500, 2000),
                new TargetSpellData("leblanc", "leblancslidem", SpellSlot.R, Spelltype.Skillshot, CcType.No, 600, 500, float.MaxValue),
                new TargetSpellData("leblanc", "leblancslidereturnm", SpellSlot.R, Spelltype.Skillshot, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("leblanc", "leblancsoulshacklem", SpellSlot.R, Spelltype.Skillshot, CcType.No, 925, 500, 1600),

                #endregion Leblanc

                #region LeeSin
                new TargetSpellData("leesin", "blindmonkqone", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1000, 500, 1800),
                new TargetSpellData("leesin", "blindmonkqtwo", SpellSlot.Q, Spelltype.Targeted, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("leesin", "blindmonkwone", SpellSlot.W, Spelltype.Targeted, CcType.No, 700, 0, 1500),
                new TargetSpellData("leesin", "blindmonkwtwo", SpellSlot.W, Spelltype.Self, CcType.No, 700, 0, float.MaxValue),
                new TargetSpellData("leesin", "blindmonkeone", SpellSlot.E, Spelltype.Self, CcType.No, 425, 500, float.MaxValue),
                new TargetSpellData("leesin", "blindmonketwo", SpellSlot.E, Spelltype.Self, CcType.Slow, 425, 500, float.MaxValue),
                new TargetSpellData("leesin", "blindmonkrkick", SpellSlot.R, Spelltype.Targeted, CcType.Knockback, 375, 500, 1500),

                #endregion LeeSin

                #region Leona
                new TargetSpellData("leona", "leonashieldofdaybreak", SpellSlot.Q, Spelltype.Self, CcType.Stun, 215, 0, 0),
                new TargetSpellData("leona", "leonasolarbarrier", SpellSlot.W, Spelltype.Self, CcType.No, 500, 3000, 0),
                new TargetSpellData("leona", "leonazenithblade", SpellSlot.E, Spelltype.Skillshot, CcType.Stun, 900, 0, 2000),
                new TargetSpellData("leona", "leonazenithblademissle", SpellSlot.E, Spelltype.Skillshot, CcType.Stun, 900, 0, 2000),
                new TargetSpellData("leona", "leonasolarflare", SpellSlot.R, Spelltype.Skillshot, CcType.Stun, 1200, 700, float.MaxValue),

                #endregion Leona

                #region Lissandra
                new TargetSpellData("lissandra", "lissandraq", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 725, 500, 1200),
                new TargetSpellData("lissandra", "lissandraw", SpellSlot.W, Spelltype.Self, CcType.Snare, 450, 500, float.MaxValue),
                new TargetSpellData("lissandra", "lissandrae", SpellSlot.E, Spelltype.Skillshot, CcType.No, 1050, 500, 850),
                new TargetSpellData("lissandra", "lissandrar", SpellSlot.R, Spelltype.Targeted, CcType.Stun, 550, 0, float.MaxValue),

                #endregion Lissandra

                #region Lucian
                new TargetSpellData("lucian", "lucianq", SpellSlot.Q, Spelltype.Targeted, CcType.No, 550, 500, 500),
                new TargetSpellData("lucian", "lucianw", SpellSlot.W, Spelltype.Skillshot, CcType.No, 1000, 500, 500),
                new TargetSpellData("lucian", "luciane", SpellSlot.E, Spelltype.Skillshot, CcType.No, 650, 500, float.MaxValue),
                new TargetSpellData("lucian", "lucianr", SpellSlot.R, Spelltype.Targeted, CcType.No, 1400, 500, float.MaxValue),

                #endregion Lucian

                #region Lulu
                new TargetSpellData("lulu", "luluq", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 925, 500, 1400),
                new TargetSpellData("lulu", "luluqmissle", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 925, 500, 1400),
                new TargetSpellData("lulu", "luluw", SpellSlot.W, Spelltype.Targeted, CcType.Polymorph, 650, 640, 2000),
                new TargetSpellData("lulu", "lulue", SpellSlot.E, Spelltype.Targeted, CcType.No, 650, 640, float.MaxValue),
                new TargetSpellData("lulu", "lulur", SpellSlot.R, Spelltype.Targeted, CcType.Knockup, 900, 500, float.MaxValue),

                #endregion Lulu

                #region Lux
                new TargetSpellData("lux", "luxlightbinding", SpellSlot.Q, Spelltype.Skillshot, CcType.Snare, 1300, 500, 1200),
                new TargetSpellData("lux", "luxprismaticwave", SpellSlot.W, Spelltype.Skillshot, CcType.No, 1075, 500, 1200),
                new TargetSpellData("lux", "luxlightstrikekugel", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 1100, 500, 1300),
                new TargetSpellData("lux", "luxlightstriketoggle", SpellSlot.E, Spelltype.Skillshot, CcType.No, 1100, 500, 1300),
                new TargetSpellData("lux", "luxmalicecannon", SpellSlot.R, Spelltype.Skillshot, CcType.No, 3340, 1750, 3000),
                new TargetSpellData("lux", "luxmalicecannonmis", SpellSlot.R, Spelltype.Skillshot, CcType.No, 3340, 1750, 3000),

                #endregion Lux

                #region Malphite
                new TargetSpellData("malphite", "seismicshard", SpellSlot.Q, Spelltype.Targeted, CcType.Slow, 625, 500, 1200),
                new TargetSpellData("malphite", "obduracy", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("malphite", "landslide", SpellSlot.E, Spelltype.Self, CcType.No, 400, 500, float.MaxValue),
                new TargetSpellData("malphite", "ufslash", SpellSlot.R, Spelltype.Skillshot, CcType.Knockup, 1000, 0, 700),

                #endregion Malphite

                #region Malzahar
                new TargetSpellData("malzahar", "alzaharcallofthevoid", SpellSlot.Q, Spelltype.Skillshot, CcType.Silence, 900, 500, float.MaxValue),
                new TargetSpellData("malzahar", "alzaharnullzone", SpellSlot.W, Spelltype.Skillshot, CcType.No, 800, 500, float.MaxValue),
                new TargetSpellData("malzahar", "alzaharmaleficvisions", SpellSlot.E, Spelltype.Targeted, CcType.No, 650, 500, float.MaxValue),
                new TargetSpellData("malzahar", "alzaharnethergrasp", SpellSlot.R, Spelltype.Targeted, CcType.Suppression, 700, 500, float.MaxValue),

                #endregion Malzahar

                #region Maokai
                new TargetSpellData("maokai", "maokaitrunkline", SpellSlot.Q, Spelltype.Skillshot, CcType.Knockback, 600, 500, 1200),
                new TargetSpellData("maokai", "maokaiunstablegrowth", SpellSlot.W, Spelltype.Targeted, CcType.Snare, 650, 500, float.MaxValue),
                new TargetSpellData("maokai", "maokaisapling2", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 1100, 500, 1750),
                new TargetSpellData("maokai", "maokaidrain3", SpellSlot.R, Spelltype.Targeted, CcType.No, 625, 500, float.MaxValue),

                #endregion Maokai

                #region MasterYi
                new TargetSpellData("masteryi", "alphastrike", SpellSlot.Q, Spelltype.Targeted, CcType.No, 600, 500, 4000),
                new TargetSpellData("masteryi", "meditate", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("masteryi", "wujustyle", SpellSlot.E, Spelltype.Self, CcType.No, 0, 230, float.MaxValue),
                new TargetSpellData("masteryi", "highlander", SpellSlot.R, Spelltype.Self, CcType.No, 0, 370, float.MaxValue),

                #endregion MasterYi

                #region MissFortune
                new TargetSpellData("missfortune", "missfortunericochetshot", SpellSlot.Q, Spelltype.Targeted, CcType.No, 650, 500, 1400),
                new TargetSpellData("missfortune", "missfortuneviciousstrikes", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("missfortune", "missfortunescattershot", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 1000, 500, 500),
                new TargetSpellData("missfortune", "missfortunebullettime", SpellSlot.R, Spelltype.Skillshot, CcType.No, 1400, 500, 775),

                #endregion MissFortune

                #region MonkeyKing
                new TargetSpellData("monkeyking", "monkeykingdoubleattack", SpellSlot.Q, Spelltype.Self, CcType.No, 300, 500, 20),
                new TargetSpellData("monkeyking", "monkeykingdecoy", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, 0),
                new TargetSpellData("monkeyking", "monkeykingdecoyswipe", SpellSlot.W, Spelltype.Self, CcType.No, 325, 500, 0),
                new TargetSpellData("monkeyking", "monkeykingnimbus", SpellSlot.E, Spelltype.Targeted, CcType.No, 625, 0, 2200),
                new TargetSpellData("monkeyking", "monkeykingspintowin", SpellSlot.R, Spelltype.Self, CcType.Knockup, 315, 0, 700),
                new TargetSpellData("monkeyking", "monkeykingspintowinleave", SpellSlot.R, Spelltype.Self, CcType.No, 0, 0, 700),

                #endregion MonkeyKing

                #region Mordekaiser
                new TargetSpellData("mordekaiser", "mordekaisermaceofspades", SpellSlot.Q, Spelltype.Self, CcType.No, 600, 500, 1500),
                new TargetSpellData("mordekaiser", "mordekaisercreepindeathcast", SpellSlot.W, Spelltype.Targeted, CcType.No, 750, 500, float.MaxValue),
                new TargetSpellData("mordekaiser", "mordekaisersyphoneofdestruction", SpellSlot.E, Spelltype.Skillshot, CcType.No, 700, 500, 1500),
                new TargetSpellData("mordekaiser", "mordekaiserchildrenofthegrave", SpellSlot.R, Spelltype.Targeted, CcType.No, 850, 500, 1500),

                #endregion Mordekaiser

                #region Morgana
                new TargetSpellData("morgana", "darkbindingmissile", SpellSlot.Q, Spelltype.Skillshot, CcType.Snare, 1300, 500, 1200),
                new TargetSpellData("morgana", "tormentedsoil", SpellSlot.W, Spelltype.Skillshot, CcType.No, 1075, 500, float.MaxValue),
                new TargetSpellData("morgana", "blackshield", SpellSlot.E, Spelltype.Targeted, CcType.No, 750, 500, float.MaxValue),
                new TargetSpellData("morgana", "soulshackles", SpellSlot.R, Spelltype.Self, CcType.Stun, 600, 500, float.MaxValue),

                #endregion Morgana

                #region Nami
                new TargetSpellData("nami", "namiq", SpellSlot.Q, Spelltype.Skillshot, CcType.Knockup, 875, 500, 1750),
                new TargetSpellData("nami", "namiw", SpellSlot.W, Spelltype.Targeted, CcType.No, 725, 500, 1100),
                new TargetSpellData("nami", "namie", SpellSlot.E, Spelltype.Targeted, CcType.Slow, 800, 500, float.MaxValue),
                new TargetSpellData("nami", "namir", SpellSlot.R, Spelltype.Skillshot, CcType.Knockup, 2550, 500, 1200),

                #endregion Nami

                #region Nasus
                new TargetSpellData("nasus", "nasusq", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("nasus", "nasusw", SpellSlot.W, Spelltype.Targeted, CcType.Slow, 600, 500, float.MaxValue),
                new TargetSpellData("nasus", "nasuse", SpellSlot.E, Spelltype.Skillshot, CcType.No, 850, 500, float.MaxValue),
                new TargetSpellData("nasus", "nasusr", SpellSlot.R, Spelltype.Skillshot, CcType.No, 1, 500, float.MaxValue),

                #endregion Nasus

                #region Nautilus
                new TargetSpellData("nautilus", "nautilusanchordrag", SpellSlot.Q, Spelltype.Skillshot, CcType.Pull, 950, 500, 1200),
                new TargetSpellData("nautilus", "nautiluspiercinggaze", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("nautilus", "nautilussplashzone", SpellSlot.E, Spelltype.Self, CcType.Slow, 600, 500, 1300),
                new TargetSpellData("nautilus", "nautilusgandline", SpellSlot.R, Spelltype.Targeted, CcType.Knockup, 1500, 500, 1400),

                #endregion Nautilus

                #region Nidalee
                new TargetSpellData("nidalee", "javelintoss", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1500, 500, 1300),
                new TargetSpellData("nidalee", "takedown", SpellSlot.Q, Spelltype.Self, CcType.No, 50, 0, 500),
                new TargetSpellData("nidalee", "bushwhack", SpellSlot.W, Spelltype.Skillshot, CcType.No, 900, 500, 1450),
                new TargetSpellData("nidalee", "pounce", SpellSlot.W, Spelltype.Skillshot, CcType.No, 375, 500, 1500),
                new TargetSpellData("nidalee", "primalsurge", SpellSlot.E, Spelltype.Targeted, CcType.No, 600, 0, float.MaxValue),
                new TargetSpellData("nidalee", "swipe", SpellSlot.E, Spelltype.Skillshot, CcType.No, 300, 500, float.MaxValue),
                new TargetSpellData("nidalee", "aspectofthecougar", SpellSlot.R, Spelltype.Self, CcType.No, 0, 0, float.MaxValue),

                #endregion Nidalee

                #region Nocturne
                new TargetSpellData("nocturne", "nocturneduskbringer", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1125, 500, 1600),
                new TargetSpellData("nocturne", "nocturneshroudofdarkness", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, 500),
                new TargetSpellData("nocturne", "nocturneunspeakablehorror", SpellSlot.E, Spelltype.Targeted, CcType.Fear, 500, 500, 0),
                new TargetSpellData("nocturne", "nocturneparanoia", SpellSlot.R, Spelltype.Targeted, CcType.No, 2000, 500, 500),

                #endregion Nocturne

                #region Nunu
                new TargetSpellData("nunu", "consume", SpellSlot.Q, Spelltype.Targeted, CcType.No, 125, 500, 1400),
                new TargetSpellData("nunu", "bloodboil", SpellSlot.W, Spelltype.Targeted, CcType.No, 700, 500, float.MaxValue),
                new TargetSpellData("nunu", "iceblast", SpellSlot.E, Spelltype.Targeted, CcType.Slow, 550, 500, 1000),
                new TargetSpellData("nunu", "absolutezero", SpellSlot.R, Spelltype.Self, CcType.Slow, 650, 500, float.MaxValue),

                #endregion Nunu

                #region Olaf
                new TargetSpellData("olaf", "olafaxethrowcast", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 1000, 500, 1600),
                new TargetSpellData("olaf", "olaffrenziedstrikes", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("olaf", "olafrecklessstrike", SpellSlot.E, Spelltype.Targeted, CcType.No, 325, 500, float.MaxValue),
                new TargetSpellData("olaf", "olafragnarok", SpellSlot.R, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Olaf

                #region Orianna
                new TargetSpellData("orianna", "orianaizunacommand", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1100, 500, 1200),
                new TargetSpellData("orianna", "orianadissonancecommand", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 0, 500, 1200),
                new TargetSpellData("orianna", "orianaredactcommand", SpellSlot.E, Spelltype.Targeted, CcType.No, 1095, 500, 1200),
                new TargetSpellData("orianna", "orianadetonatecommand", SpellSlot.R, Spelltype.Skillshot, CcType.Pull, 0, 500, 1200),

                #endregion Orianna

                #region Pantheon
                new TargetSpellData("pantheon", "pantheonq", SpellSlot.Q, Spelltype.Targeted, CcType.No, 600, 500, 1500),
                new TargetSpellData("pantheon", "pantheonw", SpellSlot.W, Spelltype.Targeted, CcType.Stun, 600, 500, float.MaxValue),
                new TargetSpellData("pantheon", "pantheone", SpellSlot.E, Spelltype.Skillshot, CcType.No, 600, 500, 775),
                new TargetSpellData("pantheon", "pantheonrjump", SpellSlot.R, Spelltype.Skillshot, CcType.No, 5500, 1000, 3000),
                new TargetSpellData("pantheon", "pantheonrfall", SpellSlot.R, Spelltype.Skillshot, CcType.Slow, 5500, 1000, 3000),

                #endregion Pantheon

                #region Poppy
                new TargetSpellData("poppy", "poppydevastatingblow", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("poppy", "poppyparagonofdemacia", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("poppy", "poppyheroiccharge", SpellSlot.E, Spelltype.Targeted, CcType.Stun, 525, 500, 1450),
                new TargetSpellData("poppy", "poppydiplomaticimmunity", SpellSlot.R, Spelltype.Targeted, CcType.No, 900, 500, float.MaxValue),

                #endregion Poppy

                #region Quinn
                new TargetSpellData("quinn", "quinnq", SpellSlot.Q, Spelltype.Skillshot, CcType.Blind, 1025, 500, 1200),
                new TargetSpellData("quinn", "quinnw", SpellSlot.W, Spelltype.Self, CcType.No, 2100, 0, 0),
                new TargetSpellData("quinn", "quinne", SpellSlot.E, Spelltype.Targeted, CcType.Knockback, 700, 500, 775),
                new TargetSpellData("quinn", "quinnr", SpellSlot.R, Spelltype.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("quinn", "quinnrfinale", SpellSlot.R, Spelltype.Self, CcType.No, 700, 0, 0),

                #endregion Quinn

                #region Rammus
                new TargetSpellData("rammus", "powerball", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, 775),
                new TargetSpellData("rammus", "defensiveballcurl", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("rammus", "puncturingtaunt", SpellSlot.E, Spelltype.Targeted, CcType.Taunt, 325, 500, float.MaxValue),
                new TargetSpellData("rammus", "tremors2", SpellSlot.R, Spelltype.Self, CcType.No, 300, 500, float.MaxValue),

                #endregion Rammus

                #region Renekton
                new TargetSpellData("renekton", "renektoncleave", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1, 500, float.MaxValue),
                new TargetSpellData("renekton", "renektonpreexecute", SpellSlot.W, Spelltype.Self, CcType.Stun, 0, 500, float.MaxValue),
                new TargetSpellData("renekton", "renektonsliceanddice", SpellSlot.E, Spelltype.Skillshot, CcType.No, 450, 500, 1400),
                new TargetSpellData("renekton", "renektonreignofthetyrant", SpellSlot.R, Spelltype.Skillshot, CcType.No, 1, 500, 775),

                #endregion Renekton

                #region Rengar
                new TargetSpellData("rengar", "rengarq", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("rengar", "rengarw", SpellSlot.W, Spelltype.Skillshot, CcType.No, 1, 500, float.MaxValue),
                new TargetSpellData("rengar", "rengare", SpellSlot.E, Spelltype.Targeted, CcType.Snare, 1000, 500, 1500),
                new TargetSpellData("rengar", "rengarr", SpellSlot.R, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Rengar

                #region Riven
                new TargetSpellData("riven", "riventricleave", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 250, 500, 0),
                new TargetSpellData("riven", "riventricleave_03", SpellSlot.Q, Spelltype.Skillshot, CcType.Knockup, 250, 500, 0),
                new TargetSpellData("riven", "rivenmartyr", SpellSlot.W, Spelltype.Self, CcType.Stun, 260, 250, 1500),
                new TargetSpellData("riven", "rivenfeint", SpellSlot.E, Spelltype.Skillshot, CcType.No, 325, 0, 1450),
                new TargetSpellData("riven", "rivenfengshuiengine", SpellSlot.R, Spelltype.Self, CcType.No, 0, 500, 1200),
                new TargetSpellData("riven", "rivenizunablade", SpellSlot.R, Spelltype.Skillshot, CcType.No, 900, 300, 1450),

                #endregion Riven

                #region Rumble
                new TargetSpellData("rumble", "rumbleflamethrower", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 600, 500, float.MaxValue),
                new TargetSpellData("rumble", "rumbleshield", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("rumble", "rumbegrenade", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 850, 500, 1200),
                new TargetSpellData("rumble", "rumblecarpetbomb", SpellSlot.R, Spelltype.Skillshot, CcType.Slow, 1700, 500, 1400),

                #endregion Rumble

                #region Ryze
                new TargetSpellData("ryze", "overload", SpellSlot.Q, Spelltype.Targeted, CcType.No, 625, 500, 1400),
                new TargetSpellData("ryze", "runeprison", SpellSlot.W, Spelltype.Targeted, CcType.Snare, 600, 500, float.MaxValue),
                new TargetSpellData("ryze", "spellflux", SpellSlot.E, Spelltype.Targeted, CcType.No, 600, 500, 1000),
                new TargetSpellData("ryze", "desperatepower", SpellSlot.R, Spelltype.Targeted, CcType.No, 625, 500, 1400),

                #endregion Ryze

                #region Sejuani
                new TargetSpellData("sejuani", "sejuaniarcticassault", SpellSlot.Q, Spelltype.Skillshot, CcType.Knockback, 650, 500, 1450),
                new TargetSpellData("sejuani", "sejuaninorthernwinds", SpellSlot.W, Spelltype.Skillshot, CcType.No, 1, 500, 1500),
                new TargetSpellData("sejuani", "sejuaniwintersclaw", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 1, 500, 1450),
                new TargetSpellData("sejuani", "sejuaniglacialprisonstart", SpellSlot.R, Spelltype.Skillshot, CcType.Stun, 1175, 500, 1400),

                #endregion Sejuani

                #region Shaco
                new TargetSpellData("shaco", "deceive", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 400, 500, float.MaxValue),
                new TargetSpellData("shaco", "jackinthebox", SpellSlot.W, Spelltype.Skillshot, CcType.Fear, 425, 500, 1450),
                new TargetSpellData("shaco", "twoshivpoisen", SpellSlot.E, Spelltype.Targeted, CcType.Slow, 625, 500, 1500),
                new TargetSpellData("shaco", "hallucinatefull", SpellSlot.R, Spelltype.Skillshot, CcType.No, 1125, 500, 395),

                #endregion Shaco

                #region Shen
                new TargetSpellData("shen", "shenvorpalstar", SpellSlot.Q, Spelltype.Targeted, CcType.No, 475, 500, 1500),
                new TargetSpellData("shen", "shenfeint", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("shen", "shenshadowdash", SpellSlot.E, Spelltype.Skillshot, CcType.Taunt, 600, 500, 1000),
                new TargetSpellData("shen", "shenstandunited", SpellSlot.R, Spelltype.Targeted, CcType.No, 25000, 500, float.MaxValue),

                #endregion Shen

                #region Shyvana
                new TargetSpellData("shyvana", "shyvanadoubleattack", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("shyvana", "shyvanadoubleattackdragon", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("shyvana", "shyvanaimmolationauraqw", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("shyvana", "shyvanaimmolateddragon", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("shyvana", "shyvanafireball", SpellSlot.E, Spelltype.Skillshot, CcType.No, 925, 500, 1200),
                new TargetSpellData("shyvana", "shyvanafireballdragon2", SpellSlot.E, Spelltype.Skillshot, CcType.No, 925, 500, 1200),
                new TargetSpellData("shyvana", "shyvanatransformcast", SpellSlot.R, Spelltype.Skillshot, CcType.No, 1000, 500, 700),
                new TargetSpellData("shyvana", "shyvanatransformleap", SpellSlot.R, Spelltype.Skillshot, CcType.Knockback, 1000, 500, 700),

                #endregion Shyvana

                #region Singed
                new TargetSpellData("singed", "poisentrail", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("singed", "megaadhesive", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 1175, 500, 700),
                new TargetSpellData("singed", "fling", SpellSlot.E, Spelltype.Targeted, CcType.Pull, 125, 500, float.MaxValue),
                new TargetSpellData("singed", "insanitypotion", SpellSlot.R, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Singed

                #region Sion
                new TargetSpellData("sion", "crypticgaze", SpellSlot.Q, Spelltype.Targeted, CcType.Stun, 550, 500, 1600),
                new TargetSpellData("sion", "deathscaressfull", SpellSlot.W, Spelltype.Self, CcType.No, 550, 500, float.MaxValue),
                new TargetSpellData("sion", "deathscaress", SpellSlot.W, Spelltype.Self, CcType.No, 550, 500, float.MaxValue),
                new TargetSpellData("sion", "enrage", SpellSlot.E, Spelltype.Self, CcType.Slow, 0, 500, float.MaxValue),
                new TargetSpellData("sion", "cannibalism", SpellSlot.R, Spelltype.Self, CcType.Stun, 0, 500, 500),

                #endregion Sion

                #region Sivir
                new TargetSpellData("sivir", "sivirq", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1165, 500, 1350),
                new TargetSpellData("sivir", "sivirw", SpellSlot.W, Spelltype.Targeted, CcType.No, 565, 500, float.MaxValue),
                new TargetSpellData("sivir", "sivire", SpellSlot.E, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("sivir", "sivirr", SpellSlot.R, Spelltype.Self, CcType.No, 1000, 500, float.MaxValue),

                #endregion Sivir

                #region Skarner
                new TargetSpellData("skarner", "skarnervirulentslash", SpellSlot.Q, Spelltype.Self, CcType.No, 350, 0, float.MaxValue),
                new TargetSpellData("skarner", "skarnerexoskeleton", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("skarner", "skarnerfracture", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 1000, 500, 1200),
                new TargetSpellData("skarner", "skarnerfracturemissilespell", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 1000, 500, 1200),
                new TargetSpellData("skarner", "skarnerimpale", SpellSlot.R, Spelltype.Targeted, CcType.Suppression, 350, 0, float.MaxValue),

                #endregion Skarner

                #region Sona
                new TargetSpellData("sona", "sonahymnofvalor", SpellSlot.Q, Spelltype.Self, CcType.No, 700, 500, 1500),
                new TargetSpellData("sona", "sonaariaofperseverance", SpellSlot.W, Spelltype.Self, CcType.No, 1000, 500, 1500),
                new TargetSpellData("sona", "sonasongofdiscord", SpellSlot.E, Spelltype.Self, CcType.No, 1000, 500, 1500),
                new TargetSpellData("sona", "sonacrescendo", SpellSlot.R, Spelltype.Skillshot, CcType.Stun, 900, 500, 2400),

                #endregion Sona

                #region Soraka
                new TargetSpellData("soraka", "starcall", SpellSlot.Q, Spelltype.Self, CcType.No, 675, 500, float.MaxValue),
                new TargetSpellData("soraka", "astralblessing", SpellSlot.W, Spelltype.Targeted, CcType.No, 750, 500, float.MaxValue),
                new TargetSpellData("soraka", "infusewrapper", SpellSlot.E, Spelltype.Targeted, CcType.No, 725, 500, float.MaxValue),
                new TargetSpellData("soraka", "wish", SpellSlot.R, Spelltype.Self, CcType.No, 25000, 500, float.MaxValue),

                #endregion Soraka

                #region Swain
                new TargetSpellData("swain", "swaindecrepify", SpellSlot.Q, Spelltype.Targeted, CcType.Slow, 625, 500, float.MaxValue),
                new TargetSpellData("swain", "swainshadowgrasp", SpellSlot.W, Spelltype.Skillshot, CcType.Snare, 1040, 500, 1250),
                new TargetSpellData("swain", "swaintorment", SpellSlot.E, Spelltype.Targeted, CcType.No, 625, 500, 1400),
                new TargetSpellData("swain", "swainmetamorphism", SpellSlot.R, Spelltype.Self, CcType.No, 700, 500, 950),

                #endregion Swain

                #region Syndra
                new TargetSpellData("syndra", "syndraq", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 800, 250, 1750),
                new TargetSpellData("syndra", "syndraw", SpellSlot.W, Spelltype.Targeted, CcType.No, 925, 500, 1450),
                new TargetSpellData("syndra", "syndrawcast", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 950, 500, 1450),
                new TargetSpellData("syndra", "syndrae", SpellSlot.E, Spelltype.Skillshot, CcType.Stun, 700, 500, 902),
                new TargetSpellData("syndra", "syndrar", SpellSlot.R, Spelltype.Targeted, CcType.No, 675, 500, 1100),

                #endregion Syndra

                #region Talon
                new TargetSpellData("talon", "talonnoxiandiplomacy", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("talon", "talonrake", SpellSlot.W, Spelltype.Skillshot, CcType.No, 750, 500, 1200),
                new TargetSpellData("talon", "taloncutthroat", SpellSlot.E, Spelltype.Targeted, CcType.Slow, 750, 0, 1200),
                new TargetSpellData("talon", "talonshadowassault", SpellSlot.R, Spelltype.Self, CcType.No, 750, 0, 0),

                #endregion Talon

                #region Taric
                new TargetSpellData("taric", "imbue", SpellSlot.Q, Spelltype.Targeted, CcType.No, 750, 500, 1200),
                new TargetSpellData("taric", "shatter", SpellSlot.W, Spelltype.Self, CcType.No, 400, 500, float.MaxValue),
                new TargetSpellData("taric", "dazzle", SpellSlot.E, Spelltype.Targeted, CcType.Stun, 625, 500, 1400),
                new TargetSpellData("taric", "tarichammersmash", SpellSlot.R, Spelltype.Self, CcType.No, 400, 500, float.MaxValue),

                #endregion Taric

                #region Teemo
                new TargetSpellData("teemo", "blindingdart", SpellSlot.Q, Spelltype.Targeted, CcType.Blind, 580, 500, 1500),
                new TargetSpellData("teemo", "movequick", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, 943),
                new TargetSpellData("teemo", "toxicshot", SpellSlot.E, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("teemo", "bantamtrap", SpellSlot.R, Spelltype.Skillshot, CcType.Slow, 230, 0, 1500),

                #endregion Teemo

                #region Thresh
                new TargetSpellData("thresh", "threshq", SpellSlot.Q, Spelltype.Skillshot, CcType.Pull, 1175, 500, 1200),
                new TargetSpellData("thresh", "threshw", SpellSlot.W, Spelltype.Skillshot, CcType.No, 950, 500, float.MaxValue),
                new TargetSpellData("thresh", "threshe", SpellSlot.E, Spelltype.Skillshot, CcType.Knockback, 515, 300, float.MaxValue),
                new TargetSpellData("thresh", "threshrpenta", SpellSlot.R, Spelltype.Skillshot, CcType.Slow, 420, 300, float.MaxValue),

                #endregion Thresh

                #region Tristana
                new TargetSpellData("tristana", "rapidfire", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("tristana", "rocketjump", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 900, 500, 1150),
                new TargetSpellData("tristana", "detonatingshot", SpellSlot.E, Spelltype.Targeted, CcType.No, 625, 500, 1400),
                new TargetSpellData("tristana", "bustershot", SpellSlot.R, Spelltype.Targeted, CcType.Knockback, 700, 500, 1600),

                #endregion Tristana

                #region Trundle
                new TargetSpellData("trundle", "trundletrollsmash", SpellSlot.Q, Spelltype.Targeted, CcType.Slow, 0, 500, float.MaxValue),
                new TargetSpellData("trundle", "trundledesecrate", SpellSlot.W, Spelltype.Skillshot, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("trundle", "trundlecircle", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 1100, 500, 1600),
                new TargetSpellData("trundle", "trundlepain", SpellSlot.R, Spelltype.Targeted, CcType.No, 700, 500, 1400),

                #endregion Trundle

                #region Tryndamere
                new TargetSpellData("tryndamere", "bloodlust", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("tryndamere", "mockingshout", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 400, 500, 500),
                new TargetSpellData("tryndamere", "slashcast", SpellSlot.E, Spelltype.Skillshot, CcType.No, 660, 500, 700),
                new TargetSpellData("tryndamere", "undyingrage", SpellSlot.R, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Tryndamere

                #region Twich
                new TargetSpellData("twich", "hideinshadows", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("twich", "twitchvenomcask", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 800, 500, 1750),
                new TargetSpellData("twich", "twitchvenomcaskmissle", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 800, 500, 1750),
                new TargetSpellData("twich", "expunge", SpellSlot.E, Spelltype.Targeted, CcType.No, 1200, 500, float.MaxValue),
                new TargetSpellData("twich", "fullautomatic", SpellSlot.R, Spelltype.Targeted, CcType.No, 850, 500, 500),

                #endregion Twich

                #region TwistedFate
                new TargetSpellData("twistedfate", "wildcards", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1450, 500, 1450),
                new TargetSpellData("twistedfate", "pickacard", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("twistedfate", "goldcardpreattack", SpellSlot.W, Spelltype.Targeted, CcType.Stun, 600, 500, float.MaxValue),
                new TargetSpellData("twistedfate", "redcardpreattack", SpellSlot.W, Spelltype.Targeted, CcType.Slow, 600, 500, float.MaxValue),
                new TargetSpellData("twistedfate", "bluecardpreattack", SpellSlot.W, Spelltype.Targeted, CcType.No, 600, 500, float.MaxValue),
                new TargetSpellData("twistedfate", "cardmasterstack", SpellSlot.E, Spelltype.Self, CcType.No, 525, 500, 1200),
                new TargetSpellData("twistedfate", "destiny", SpellSlot.R, Spelltype.Skillshot, CcType.No, 5500, 500, float.MaxValue),

                #endregion TwistedFate

                #region Udyr
                new TargetSpellData("udyr", "udyrtigerstance", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("udyr", "udyrturtlestance", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("udyr", "udyrbearstance", SpellSlot.E, Spelltype.Self, CcType.Stun, 0, 500, float.MaxValue),
                new TargetSpellData("udyr", "udyrphoenixstance", SpellSlot.R, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Udyr

                #region Urgot
                new TargetSpellData("urgot", "urgotheatseekinglineqqmissile", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1000, 500, 1600),
                new TargetSpellData("urgot", "urgotheatseekingmissile", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1000, 500, 1600),
                new TargetSpellData("urgot", "urgotterrorcapacitoractive2", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("urgot", "urgotplasmagrenade", SpellSlot.E, Spelltype.Skillshot, CcType.No, 950, 500, 1750),
                new TargetSpellData("urgot", "urgotplasmagrenadeboom", SpellSlot.E, Spelltype.Skillshot, CcType.No, 950, 500, 1750),
                new TargetSpellData("urgot", "urgotswap2", SpellSlot.R, Spelltype.Targeted, CcType.Suppression, 850, 500, 1800),

                #endregion Urgot

                #region Varus
                new TargetSpellData("varus", "varusq", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 1500, 500, 1500),
                new TargetSpellData("varus", "varusw", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, 0),
                new TargetSpellData("varus", "varuse", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 925, 500, 1500),
                new TargetSpellData("varus", "varusr", SpellSlot.R, Spelltype.Skillshot, CcType.Snare, 1300, 500, 1500),

                #endregion Varus

                #region Vayne
                new TargetSpellData("vayne", "vaynetumble", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 250, 500, float.MaxValue),
                new TargetSpellData("vayne", "vaynesilverbolts", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, float.MaxValue),
                new TargetSpellData("vayne", "vaynecondemm", SpellSlot.E, Spelltype.Targeted, CcType.Stun, 450, 500, 1200),
                new TargetSpellData("vayne", "vayneinquisition", SpellSlot.R, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),

                #endregion Vayne

                #region Veigar
                new TargetSpellData("veigar", "veigarbalefulstrike", SpellSlot.Q, Spelltype.Targeted, CcType.No, 650, 500, 1500),
                new TargetSpellData("veigar", "veigardarkmatter", SpellSlot.W, Spelltype.Skillshot, CcType.No, 900, 1200, 1500),
                new TargetSpellData("veigar", "veigareventhorizon", SpellSlot.E, Spelltype.Skillshot, CcType.Stun, 650, float.MaxValue, 1500),
                new TargetSpellData("veigar", "veigarprimordialburst", SpellSlot.R, Spelltype.Targeted, CcType.No, 650, 500, 1400),

                #endregion Veigar

                #region Velkoz
                new TargetSpellData("velkoz", "velkozq", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 1050, 300, 1200),
                new TargetSpellData("velkoz", "velkozqmissle", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 1050, 0, 1200),
                new TargetSpellData("velkoz", "velkozqplitactive", SpellSlot.Q, Spelltype.Skillshot, CcType.Slow, 1050, 800, 1200),
                new TargetSpellData("velkoz", "velkozw", SpellSlot.W, Spelltype.Skillshot, CcType.No, 1050, 0, 1200),
                new TargetSpellData("velkoz", "velkoze", SpellSlot.E, Spelltype.Targeted, CcType.Knockup, 850, 0, 500),
                new TargetSpellData("velkoz", "velkozr", SpellSlot.R, Spelltype.Skillshot, CcType.No, 1575, 0, 1500),

                #endregion Velkoz

                #region Vi
                new TargetSpellData("vi", "viq", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 800, 500, 1500),
                new TargetSpellData("vi", "viw", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, 0),
                new TargetSpellData("vi", "vie", SpellSlot.E, Spelltype.Skillshot, CcType.No, 600, 0, 0),
                new TargetSpellData("vi", "vir", SpellSlot.R, Spelltype.Targeted, CcType.Stun, 800, 500, 0),

                #endregion Vi

                #region Viktor
                new TargetSpellData("viktor", "viktorpowertransfer", SpellSlot.Q, Spelltype.Targeted, CcType.No, 600, 500, 1400),
                new TargetSpellData("viktor", "viktorgravitonfield", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 815, 500, 1750),
                new TargetSpellData("viktor", "viktordeathray", SpellSlot.E, Spelltype.Skillshot, CcType.No, 700, 500, 1210),
                new TargetSpellData("viktor", "viktorchaosstorm", SpellSlot.R, Spelltype.Skillshot, CcType.Silence, 700, 500, 1210),

                #endregion Viktor

                #region Vladimir
                new TargetSpellData("vladimir", "vladimirtransfusion", SpellSlot.Q, Spelltype.Targeted, CcType.No, 600, 500, 1400),
                new TargetSpellData("vladimir", "vladimirsanguinepool", SpellSlot.W, Spelltype.Self, CcType.Slow, 350, 500, 1600),
                new TargetSpellData("vladimir", "vladimirtidesofblood", SpellSlot.E, Spelltype.Self, CcType.No, 610, 500, 1100),
                new TargetSpellData("vladimir", "vladimirhemoplague", SpellSlot.R, Spelltype.Skillshot, CcType.No, 875, 500, 1200),

                #endregion Vladimir

                #region Volibear
                new TargetSpellData("volibear", "volibearq", SpellSlot.Q, Spelltype.Self, CcType.No, 300, 500, float.MaxValue),
                new TargetSpellData("volibear", "volibearw", SpellSlot.W, Spelltype.Targeted, CcType.No, 400, 500, 1450),
                new TargetSpellData("volibear", "volibeare", SpellSlot.E, Spelltype.Self, CcType.Slow, 425, 500, 825),
                new TargetSpellData("volibear", "volibearr", SpellSlot.R, Spelltype.Self, CcType.No, 425, 0, 825),

                #endregion Volibear

                #region Warwick
                new TargetSpellData("warwick", "hungeringstrike", SpellSlot.Q, Spelltype.Targeted, CcType.No, 400, 0, float.MaxValue),
                new TargetSpellData("warwick", "hunterscall", SpellSlot.W, Spelltype.Self, CcType.No, 1000, 0, float.MaxValue),
                new TargetSpellData("warwick", "bloodscent", SpellSlot.E, Spelltype.Self, CcType.No, 1500, 0, float.MaxValue),
                new TargetSpellData("warwick", "infiniteduress", SpellSlot.R, Spelltype.Targeted, CcType.Suppression, 700, 500, float.MaxValue),

                #endregion Warwick

                #region Xerath
                new TargetSpellData("xerath", "xeratharcanopulsechargeup", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 750, 750, 500),
                new TargetSpellData("xerath", "xeratharcanebarrage2", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 1100, 500, 20),
                new TargetSpellData("xerath", "xerathmagespear", SpellSlot.E, Spelltype.Skillshot, CcType.Stun, 1050, 500, 1600),
                new TargetSpellData("xerath", "xerathlocusofpower2", SpellSlot.R, Spelltype.Skillshot, CcType.No, 5600, 750, 500),

                #endregion Xerath

                #region Xin Zhao
                new TargetSpellData("xin zhao", "xenzhaocombotarget", SpellSlot.Q, Spelltype.Self, CcType.No, 200, 0, 2000),
                new TargetSpellData("xin zhao", "xenzhaobattlecry", SpellSlot.W, Spelltype.Self, CcType.No, 0, 0, 2000),
                new TargetSpellData("xin zhao", "xenzhaosweep", SpellSlot.E, Spelltype.Targeted, CcType.Slow, 600, 500, 1750),
                new TargetSpellData("xin zhao", "xenzhaoparry", SpellSlot.R, Spelltype.Self, CcType.Knockback, 375, 0, 1750),

                #endregion Xin Zhao

                #region Yasuo
                new TargetSpellData("yasuo", "yasuoqw", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 475, 750, 1500),
                new TargetSpellData("yasuo", "yasuoq2w", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 475, 750, 1500),
                new TargetSpellData("yasuo", "yasuoq3w", SpellSlot.Q, Spelltype.Skillshot, CcType.Knockup, 1000, 750, 1500),
                new TargetSpellData("yasuo", "yasuowmovingwall", SpellSlot.W, Spelltype.Skillshot, CcType.No, 400, 500, 500),
                new TargetSpellData("yasuo", "yasuodashwrapper", SpellSlot.E, Spelltype.Targeted, CcType.No, 475, 500, 20),
                new TargetSpellData("yasuo", "yasuorknockupcombow", SpellSlot.R, Spelltype.Self, CcType.No, 1200, 500, 20),

                #endregion Yasuo

                #region Yorick
                new TargetSpellData("yorick", "yorickspectral", SpellSlot.Q, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("yorick", "yorickdecayed", SpellSlot.W, Spelltype.Skillshot, CcType.Slow, 600, 500, float.MaxValue),
                new TargetSpellData("yorick", "yorickravenous", SpellSlot.E, Spelltype.Targeted, CcType.Slow, 550, 500, float.MaxValue),
                new TargetSpellData("yorick", "yorickreviveally", SpellSlot.R, Spelltype.Targeted, CcType.No, 900, 500, 1500),

                #endregion Yorick

                #region Zac
                new TargetSpellData("zac", "zacq", SpellSlot.Q, Spelltype.Skillshot, CcType.Knockup, 550, 500, 902),
                new TargetSpellData("zac", "zacw", SpellSlot.W, Spelltype.Self, CcType.No, 350, 500, 1600),
                new TargetSpellData("zac", "zace", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 1550, 500, 1500),
                new TargetSpellData("zac", "zacr", SpellSlot.R, Spelltype.Self, CcType.Knockback, 850, 500, 1800),

                #endregion Zac

                #region Zed
                new TargetSpellData("zed", "zedshuriken", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 900, 500, 902),
                new TargetSpellData("zed", "zedshdaowdash", SpellSlot.W, Spelltype.Skillshot, CcType.No, 550, 500, 1600),
                new TargetSpellData("zed", "zedpbaoedummy", SpellSlot.E, Spelltype.Self, CcType.Slow, 300, 0, 0),
                new TargetSpellData("zed", "zedult", SpellSlot.R, Spelltype.Targeted, CcType.No, 850, 500, 0),

                #endregion Zed

                #region Ziggs
                new TargetSpellData("ziggs", "ziggsq", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggsqspell", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggsw", SpellSlot.W, Spelltype.Skillshot, CcType.Knockup, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggswtoggle", SpellSlot.W, Spelltype.Self, CcType.Knockup, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggse", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggse2", SpellSlot.E, Spelltype.Skillshot, CcType.Slow, 850, 500, 1750),
                new TargetSpellData("ziggs", "ziggsr", SpellSlot.R, Spelltype.Skillshot, CcType.No, 850, 500, 1750),

                #endregion Ziggs

                #region Zilean
                new TargetSpellData("zilean", "timebomb", SpellSlot.Q, Spelltype.Targeted, CcType.No, 700, 0, 1100),
                new TargetSpellData("zilean", "rewind", SpellSlot.W, Spelltype.Self, CcType.No, 0, 500, float.MaxValue),
                new TargetSpellData("zilean", "timewarp", SpellSlot.E, Spelltype.Targeted, CcType.Slow, 700, 500, 1100),
                new TargetSpellData("zilean", "chronoshift", SpellSlot.R, Spelltype.Targeted, CcType.No, 780, 500, float.MaxValue),

                #endregion Zilean

                #region Zyra
                new TargetSpellData("zyra", "zyraqfissure", SpellSlot.Q, Spelltype.Skillshot, CcType.No, 800, 500, 1400),
                new TargetSpellData("zyra", "zyraseed", SpellSlot.W, Spelltype.Skillshot, CcType.No, 800, 500, 2200),
                new TargetSpellData("zyra", "zyragraspingroots", SpellSlot.E, Spelltype.Skillshot, CcType.Snare, 1100, 500, 1400),
                new TargetSpellData("zyra", "zyrabramblezone", SpellSlot.R, Spelltype.Skillshot, CcType.Knockup, 700, 500, 20),

                #endregion Zyra
            };
        }

        public static TargetSpellData GetByName(string spellName)
        {
            spellName = spellName.ToLower();
            return Spells.FirstOrDefault(spell => spell.Name == spellName);
        }
    }
}