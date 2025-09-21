using GermBox.Core;
using GermBox.Pathogens;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GermBox.Patches
{
    [HarmonyPatch(typeof(Actor), "checkDeath")]
    internal class ActorDeath
    {
        static void Postfix(Actor __instance) {
            //Debug.Log("Just died: " + __instance.name);
            Pathogen pathogen = PathogenManager.GetPathogenById(__instance.id);
            if (pathogen != null) {
                pathogen.Stats.infected--;
                if (pathogen.Stats.infected == 0)
                {
                    StatusAsset status = AssetManager.status.get(pathogen.Id());

                    for (int i = 0; i < AssetManager.status.list.Count; i++) {
                        if (AssetManager.status.list[i].id == status.id)
                        {
                           AssetManager.status.list.RemoveAt(i);
                            break;
                        }
                        AssetManager.status.dict.Remove(status.id);
                    }

                    PathogenManager.pathogens.Remove(pathogen);
                }
            }
        }
    }
}
