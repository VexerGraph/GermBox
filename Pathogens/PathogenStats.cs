using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermBox.Pathogens
{
    internal class PathogenStats
    {
        private readonly double creationTime;
        public int kills = 0;
        public int infected = 0; //change this to a method that counts all units in the instance with the status

        internal PathogenStats(double creationTime)
        {
            this.creationTime = creationTime;
        }

        public string GetAgo()
        {
            return Date.getAgoString(creationTime);
        }
    }
}
