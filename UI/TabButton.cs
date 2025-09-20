using GermBox.Core;
using GermBox.Pathogens;
using NeoModLoader.General;
using NeoModLoader.General.UI.Tab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace GermBox.UI
{
    internal class TabButton
    {
        public const string STATS = "stats";

        public const string POWER = "power";

        public static PowersTab tab;

        public static void Init() {
            tab = TabManager.CreateTab("GermBox", "germbox_tab_name", "germbox_tab_description", SpriteTextureLoader.getSprite("ui/icons/achievements/achievements_plagueworld"));

            tab.SetLayout(new List<string> { STATS, POWER });

            AddButtons();

            tab.UpdateLayout();
        }

        private static void AddButtons() {
            //the stats
            tab.AddPowerButton(STATS, PowerButtonCreator.CreateSimpleButton("germbox_stats", new UnityAction(() =>
            {
                foreach (Pathogen pathogen in PathogenManager.pathogens)
                {
                    Debug.Log("PATHOGEN: " + pathogen.Name() + ", KILLS: " + pathogen.Stats.kills + ", INFECTED: " + pathogen.Stats.infected);
                    var hosts = new List<string>();
                    foreach (long id in pathogen.Hosts)
                    {
                        //hosts.Add(MapBox.instance.subspecies.get(id).name); //this line is a problem if a host subspecies goes extinct
                        var host = MapBox.instance.subspecies.get(id);

                        if (host != null) hosts.Add(host.name);
                    }
                    Debug.Log("|        BIOMES: " + string.Join(", ", pathogen.Biomes) + "| HOSTS: " + string.Join(", ", hosts));
                }
            }
            ), SpriteTextureLoader.getSprite("ui/icons/iconStatistics"))); //will eventually be changed to a window button.

            //the god power
            tab.AddPowerButton(POWER, PowerButtonCreator.CreateGodPowerButton("pathogen_power", SpriteTextureLoader.getSprite("ui/icons/achievements/achievements_plagueworld")));

            //later on, a button to create your own pathogens
        }
    }
}
