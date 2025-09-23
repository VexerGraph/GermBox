using GermBox.Core;
using GermBox.Pathogens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

namespace GermBox.Content
{
    public class WorldActions
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

            #region Special Effects
            foreach (string effectID in pathogen.symptoms.effects)
            {
                pathogen.symptoms.ApplyEffect(effectID, pTarget);
            }
            #endregion

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
                        pathogen.Stats.infected++;
                        actor.setStatsDirty();
                        actor.startShake();
                        actor.startColorEffect();
                    }
                }
                #region Mutation
                else if (actor.subspecies != null && actor.current_tile.Type.is_biome && infectChanceSuccess)
                {
                    if (pathogen.ShouldMutate() && !actor.hasStatus(pathogen.Id())) {
                        Pathogen newPathogen = null;

                        if (actor.subspecies.id == MapBox.instance.units.get(pTarget.id).subspecies.id && !pathogen.Biomes.Contains(actor.current_tile.getBiome().id)) //they share the same subspecies, but not the same biome
                        {
                            if (actor.current_tile.Type.is_biome) newPathogen = pathogen.Mutate(actor.current_tile.getBiome());
                        }
                        else if (pathogen.Biomes.Contains(actor.current_tile.getBiome().id)) //they share the same biome, but not the same subspecies
                        {
                            if (actor.hasSubspecies()) newPathogen = pathogen.Mutate(actor.subspecies);
                        }

                        if (newPathogen == null) return false;

                        if (actor.addStatusEffect(newPathogen.Id()))
                        {
                            //Debug.Log("Passing pathogen " + newPathogen.Name() + ", mutated from " + pathogen.Name() + ", to " + actor.name);
                            newPathogen.Stats.infected++;
                            actor.setStatsDirty();
                            actor.startShake();
                            actor.startColorEffect();
                        }
                    }
                }
                #endregion
            }
            return true;
        }

        internal static bool pathogenTimeout(BaseSimObject pTarget, WorldTile pTile)
        {
            Pathogen pathogen = PathogenManager.GetPathogenById(pTarget.id);
            if (pathogen == null) return false;

            pathogen.Stats.infected--;

            if (pathogen.Stats.infected == 0)
            {
                pathogen.Destroy();
                return true;
            }

            //handle immunity check here
            
            if (Randy.randomChance(80))
            {
                //we will need a way to display unit immunities, an extra window on the unit perhaps?
                HashSet<string> immunities = new() { pathogen.Id() };

                pTarget.a.data.get("immunities", out string immunityData);

                if (immunityData != null)
                {
                    var previousImmunities = JsonConvert.DeserializeObject<HashSet<string>>(immunityData);

                    immunities.UnionWith(previousImmunities);
                }

                pTarget.a.data.set("immunities", JsonConvert.SerializeObject(immunities));
            }

            return true;
        }


        //death actions
        public static bool Antimatter(BaseSimObject pTarget, WorldTile pTile = null)
        {
            DropsLibrary.action_antimatter_bomb(pTile);
            return true;
        }
        public static bool Coldone(BaseSimObject pTarget, WorldTile pTile = null)
        {
            ActionLibrary.turnIntoIceOne(pTarget);
            return true;
        }
    }
}
