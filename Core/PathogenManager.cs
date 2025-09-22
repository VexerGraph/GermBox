using GermBox.Content;
using GermBox.Pathogens;
using GermBox.UI;
using NeoModLoader.General.Game.extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

namespace GermBox.Core
{
    internal class PathogenManager
    {
        public static List<Pathogen> pathogens = new();
        //private static Dictionary<long, Pathogen> UnitToPathogen = new();

        public static void CreatePathogen(Actor unit)
        {
            Pathogen pathogen = new Pathogen();
            pathogen.Stats.infected++;
            pathogen.genus = Pathogens.NameGenerator.GenerateGenus();
            if (unit.current_tile.Type.is_biome)
            {
                pathogen.species = unit.current_tile.Type.biome_asset.subspecies_name_suffix.GetRandom<string>();
                pathogen.Biomes.Add(unit.current_tile.Type.biome_id);
            }
            else { 
                pathogen.species = Pathogens.NameGenerator.GenerateSpecies();
            }
            
            pathogen.Hosts.Add(unit.subspecies.id);

            pathogen.numeral = "I";

            //if (!UnitToPathogen.ContainsKey(unit.id)) UnitToPathogen.Add(unit.id, pathogen);

            CreateStatus(pathogen);
            pathogens.Add(pathogen);
        }

        public static Pathogen Clone(Pathogen parent)
        {
            Pathogen pathogen = new Pathogen();
            //pathogen.Stats.infected++;
            pathogen.genus = parent.genus;
            pathogen.Biomes = new HashSet<string>(parent.Biomes);
            pathogen.species = parent.species; //AssetManager.biome_library.get(pathogen.Biomes.GetRandom()).subspecies_name_suffix.GetRandom<string>();
            pathogen.Hosts = new HashSet<long>(parent.Hosts);

            pathogen.numeral = Pathogens.NameGenerator.IntToRoman(PathogenManager.pathogens.FindAll(match => match.genus == parent.genus && match.species == parent.species).Count + 1);

            //CreateStatus(pathogen);
            //pathogens.Add(pathogen);
            //handle these methods outside

            return pathogen;
        }

        public static void CreateStatus(Pathogen pathogen)
        {
            StatusAsset status = new StatusAsset();
            status.id = pathogen.Id();
            status.locale_id = "status_title_"+pathogen.Id();
            status.locale_description = "status_description_default";//"status_description_" + pathogen.Id();
            status.duration = 120f; //default 120f
            status.allow_timer_reset = false;
            status.action = new WorldAction(WorldActions.pathogenEffect);
            status.action_finish = new WorldAction(WorldActions.pathogenTimeout);
            status.action_interval = 1f;
            status.path_icon = Icons.Random();

            AssetManager.status.add(status);

            status.base_stats["multiplier_damage"] = -0.3f; //these will be controlled by symptoms eventually, and will be completely randomized
            status.base_stats["multiplier_health"] = -0.3f;

            LocalizedTextManager.add(status.locale_id, pathogen.Name());
            //LocalizedTextManager.add(status.locale_description, "Suffering from a contagious pathogen.");
        }

        public static Pathogen GetPathogenById(long id)
        {
            Actor unit = MapBox.instance.units.get(id);
            foreach (Pathogen pathogen in pathogens)
            {
                if (unit.hasStatus(pathogen.Id()))
                {
                    return pathogen;
                }
            }
            return null;
        }
    }
}
