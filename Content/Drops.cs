using GermBox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermBox.Content
{
    internal class Drops
    {
        public static void Init() {
            DropAsset pathogen = new DropAsset();
            pathogen.id = "pathogen_drop";
            pathogen.path_texture = "drops/drop_plague";
            pathogen.random_frame = true;
            pathogen.default_scale = 0.1f;
            pathogen.action_landed = new DropsAction(WorldActions.ActionInfect);
            pathogen.material = "mat_world_object_lit";
            pathogen.sound_drop = "event:/SFX/DROPS/DropPlague";
            pathogen.type = DropType.DropStatus;
            
            AssetManager.drops.add(pathogen);
        }
    }
}
