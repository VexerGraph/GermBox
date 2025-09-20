using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermBox.Content
{
    internal class GodPowers
    {
        public static void Init()
        {
            GodPower power = AssetManager.powers.clone("pathogen_power", "$template_drops$");
            power.name = "Pathogen Power";
            power.falling_chance = 0.01f;
            power.drop_id = "pathogen_drop"; //should be pathogen_drop
            power.sound_drawing = "event:/SFX/POWERS/ZombieInfection";

            power.cached_drop_asset = AssetManager.drops.get(power.drop_id);

        }
    }
}
