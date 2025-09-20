using GermBox.Core;
using GermBox.Pathogens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace GermBox.Content
{
    internal class WorldActions
    {
        internal static void ActionInfect(WorldTile pTile = null, string pDropID = null)
        {
            //obviously I'll change this to match the status effect
            foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
            {
                PathogenManager.CreatePathogen(actor);
                //Debug.Log(PathogenManager.pathogens.Last().Name());
                if (actor.addStatusEffect(PathogenManager.pathogens.Last().Id()))
                {
                    actor.setStatsDirty();
                    //actor.removeTrait("blessed");
                    actor.startShake();
                    actor.startColorEffect();
                }
            }
        }

        internal static bool pathogenEffect(BaseSimObject pTarget, WorldTile pTile = null)
        {
            Pathogen pathogen;
            PathogenManager.UnitToPathogen.TryGetValue(pTarget.id, out pathogen);

            //Debug.Log(MapBox.instance.units.get(pTarget.id).name);

            if (pathogen == null) return false;

            float pDamage = Mathf.Max((float)pTarget.getHealth() * 0.08f, 1f);
            if (Randy.randomBool() && pTarget.getHealth() > 1)
                pTarget.getHit(pDamage, pAttackType: AttackType.Plague);
            else if (pTarget.getHealth() == 1) {
                pTarget.getHit(pDamage, pAttackType: AttackType.Plague);
                pathogen.Stats.kills++;
                pathogen.Stats.infected--;
            }
            pTarget.a.spawnParticle(Toolbox.color_infected);
            pTarget.a.startShake(0.4f, 0.2f, pVertical: false);

            foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 6f))
            {
                //Debug.Log(PathogenManager.pathogens.Last().Name());
                if (pathogen.Hosts.Contains(actor.subspecies.id) && actor.current_tile.Type.is_biome && pTarget.id != actor.id && !PathogenManager.UnitToPathogen.ContainsKey(actor.id))
                {
                    if (pathogen.Biomes.Contains(actor.current_tile.getBiome().id))
                    {
                        if (actor.addStatusEffect(pathogen.Id())) //we need to nerf this so it's a chance rather than just always
                        {
                            //Debug.Log("Passed the infection to " + actor.name);
                            PathogenManager.UnitToPathogen.Add(actor.id, pathogen);
                            pathogen.Stats.infected++;
                            actor.setStatsDirty();
                            //actor.removeTrait("blessed");
                            actor.startShake();
                            actor.startColorEffect();
                        }
                    }

                }
            }

            return true;
        }
    }
}
