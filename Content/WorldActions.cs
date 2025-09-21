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
            Pathogen pathogen = PathogenManager.GetPathogenById(pTarget.id);

            //Debug.Log(MapBox.instance.units.get(pTarget.id).name);

            if (pathogen == null) return false;

            float pDamage = Mathf.Max((float)pTarget.getHealth() * 0.08f, 1f);
            if (Randy.randomBool() && pTarget.getHealth() > 1)
                pTarget.getHit(pDamage, pAttackType: AttackType.Plague);
            else if (pTarget.getHealth() == 1) {
                pTarget.getHit(pDamage, pAttackType: AttackType.Plague);
                pathogen.Stats.kills++;
                //pathogen.Stats.infected--;
            }
            pTarget.a.spawnParticle(Toolbox.color_poisoned);
            pTarget.a.startShake(0.4f, 0.2f, pVertical: false);

            foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 6f))
            {
                bool infectChanceSuccess = pathogen.ShouldInfect();
                //Debug.Log(PathogenManager.pathogens.Last().Name());
                if (pathogen.CanInfect(actor) && infectChanceSuccess)
                {
                    if (actor.addStatusEffect(pathogen.Id()))
                    {
                        //PathogenManager.RegisterUnit(actor, pathogen);
                        pathogen.Stats.infected++;
                        actor.setStatsDirty();
                        //actor.removeTrait("blessed");
                        actor.startShake();
                        actor.startColorEffect();
                    }
                }
                else if (actor.subspecies != null && actor.current_tile.Type.is_biome && infectChanceSuccess)
                {
                   // Debug.Log("pathogen.ShouldMutate()=" + pathogen.ShouldMutate() + " actor.hasStatus(pathogen.Id())=" + actor.hasStatus(pathogen.Id()));
                    if (pathogen.ShouldMutate() && !actor.hasStatus(pathogen.Id())) {
                        Pathogen newPathogen = null;

                        if (actor.subspecies.id == MapBox.instance.units.get(pTarget.id).subspecies.id && !pathogen.Biomes.Contains(actor.current_tile.getBiome().id)) //they share the same subspecies, but not the same biome
                        {
                            if (actor.current_tile.Type.is_biome) newPathogen = pathogen.Mutate(actor.current_tile.getBiome());
                            //Debug.Log("Biome: " + actor.current_tile.Type.biome_id);
                        }
                        else if (pathogen.Biomes.Contains(actor.current_tile.getBiome().id)) //they share the same biome, but not the same subspecies
                        {
                            if (actor.hasSubspecies()) newPathogen = pathogen.Mutate(actor.subspecies);
                        }

                        if (newPathogen == null) return false;

                        if (actor.addStatusEffect(newPathogen.Id()))
                        {
                            Debug.Log("Passing pathogen " + newPathogen.Name() + ", mutated from " + pathogen.Name() + ", to " + actor.name);
                            //PathogenManager.RegisterUnit(actor, newPathogen);
                            newPathogen.Stats.infected++;
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

        internal static bool pathogenTimeout(BaseSimObject pTarget, WorldTile pTile)
        {
            Pathogen pathogen = PathogenManager.GetPathogenById(pTarget.id);
            if (pathogen == null) return false;

            pathogen.Stats.infected--;

            //handle immunity check here

            return true;
        }
    }
}
