using GermBox.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GermBox.Pathogens
{
    internal class Pathogen
    {
        public string genus;
        public string species;
        public string numeral;
        //public readonly string type;
        private float infectiousness = 10f;
        public List<string> symptoms;
        public HashSet<long> Hosts; //list of the subspecies this pathogen can infect
        public HashSet<string> Biomes; //list of biomes the pathogen can spread in
        public PathogenStats Stats;

        public float mutation_chance = 10f;

        public Pathogen()
        {
            this.Stats = new PathogenStats(MapBox.instance.getCurWorldTime());
            this.Hosts = new HashSet<long>();
            this.Biomes = new HashSet<string>() {"biome_sand", "biome_hill"};
        }

        public bool ShouldMutate()
        {
            float random = Randy.randomFloat(0f, 100f);
            return (random < mutation_chance);
        }

        public bool ShouldInfect()
        {
            float random = Randy.randomFloat(0f, 100f);
            return (random < infectiousness);
        }
        public bool CanInfect(Actor unit)
        {
            if (Hosts.Contains(unit.subspecies.id) && unit.current_tile.Type.is_biome && !unit.hasStatus(this.Id()))
            {
                if (Biomes.Contains(unit.current_tile.getBiome().id))
                {
                    unit.data.get("immunities", out string immunityData);

                    HashSet<string> immunities = new();

                    if (immunityData != null)
                    {
                        immunities = JsonConvert.DeserializeObject<HashSet<string>>(immunityData);
                    }  

                    return !immunities.Contains(this.Id());
                }
            }
            return false;
        }

        public Pathogen Mutate<T>(T mutation) //handles subspecies, biome, and later on unit types
        {
            //check that the mutation is not already found somewhere else

            //clone new pathogen from previous pathogen data and fluctuate infectiosness/ mutation rate
            Pathogen newPathogen = PathogenManager.Clone(this);
            switch (Randy.randomBool())
            {
                case true:
                    newPathogen.mutation_chance = Math.Min(100f, newPathogen.mutation_chance + 2f);
                    break;
                case false:
                    newPathogen.mutation_chance = Math.Max(0f, newPathogen.mutation_chance - 2f);
                    break;
            }
            switch (Randy.randomBool())
            {
                case true:
                    newPathogen.infectiousness = Math.Min(100f, newPathogen.infectiousness + 2f);
                    break;
                case false:
                    newPathogen.infectiousness = Math.Max(0f, newPathogen.infectiousness - 2f);
                    break;
            }

            //handle mutation type

            if (mutation is BiomeAsset biome)
            {
                newPathogen.Biomes.Add(biome.id);
            }
            else if (mutation is Subspecies subspecies) {
                newPathogen.Hosts.Add(subspecies.id);
            }
            else
            {
                Debug.LogWarning("Unknown mutation type: " + mutation.GetType());
            }

            var ExactMutations = PathogenManager.pathogens.FindAll(match => match.Biomes.SetEquals(newPathogen.Biomes) && match.Hosts.SetEquals(newPathogen.Hosts));
            if (ExactMutations.Any()) { 
                return ExactMutations.First();
            }

            PathogenManager.CreateStatus(newPathogen);
            PathogenManager.pathogens.Add(newPathogen);
            return newPathogen;
        }

        public string Name()
        {
            return genus + " " + species + " " + numeral;
        }
        public string Id() { 
            return genus.ToLowerInvariant() + "_" + species.ToLowerInvariant() + "_" + numeral.ToLowerInvariant();
        }

        public void Destroy()
        {
            StatusAsset status = AssetManager.status.get(Id());

            for (int i = 0; i < AssetManager.status.list.Count; i++)
            {
                if (AssetManager.status.list[i].id == status.id)
                {
                    AssetManager.status.list.RemoveAt(i);
                    break;
                }
                AssetManager.status.dict.Remove(status.id);
            }

            PathogenManager.pathogens.Remove(this);
        }
    }
}
