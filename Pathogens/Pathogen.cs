using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermBox.Pathogens
{
    internal class Pathogen
    {
        public string genus;
        public string species;
        public string numeral;
        //public readonly string type;
        //public readonly float infectiousness;
        //public List<string> symptoms;
        public List<string> Hosts; //list of the subspecies this pathogen can infect
        public List<string> Biomes; //list of biomes the pathogen can spread in
        public PathogenStats Stats;

        public Pathogen()
        {
            this.Stats = new PathogenStats(MapBox.instance.getCurWorldTime());
        }

        public Pathogen Mutate<T>(T mutation) //handles subspecies, biome, and unit types
        {
            return null;
        }

        public string Name()
        {
            return genus + " " + species + " " + numeral;
        }
        public string Id() { 
            return genus.ToLowerInvariant() + "_" + species.ToLowerInvariant() + "_" + numeral.ToLowerInvariant();
        }
    }
}
