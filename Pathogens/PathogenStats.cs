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
        public int infected = 0;

        internal PathogenStats(double creationTime)
        {
            this.creationTime = creationTime;
        }

        public double GetAge()
        {
            return creationTime;
        }
    }
}
