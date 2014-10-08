#region LICENSE

//  Copyright 2014 - 2014 Support
//  Zilean.cs is part of Support.
//  
//  Support is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  
//  Support is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with Support. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Support.Plugins
{
    public class Zilean : PluginBase
    {
        public Zilean()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 700);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, 700);
            R = new Spell(SpellSlot.R, 900);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.IsValidTarget(Target, "ComboQ"))
                {
                    Q.Cast(Target, UsePackets);
                }

                if (W.IsReady() && !Q.IsReady() && GetValue<bool>("ComboW"))
                {
                    W.Cast();
                }

                if (E.IsValidTarget(Target, "ComboE"))
                {
                    // TODO: speed adc/jungler/engage
                }
            }

            if (HarassMode)
            {
                if (Q.IsValidTarget(Target, "HarassQ"))
                {
                    Q.Cast(Target, UsePackets);
                }

                if (W.IsReady() && !Q.IsReady() && GetValue<bool>("HarassW"))
                {
                    W.Cast();
                }
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (E.IsValidTarget(gapcloser.Sender, "GapcloserE"))
            {
                E.Cast(gapcloser.Sender);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            //config.AddBool("ComboE", "Use E", true);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
            config.AddBool("HarassW", "Use W", true);
        }

        public override void MiscMenu(Menu config)
        {
            config.AddBool("GapcloserE", "Use E to Interrupt Gapcloser", true);
        }
    }
}