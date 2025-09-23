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
                if (__instance._last_attack_type == AttackType.Plague) //really I should be using my own attack type
                {
                    pathogen.Stats.kills++;
                }
                if (pathogen.Stats.infected == 0)
                {
                    pathogen.Destroy();
                }
            }
        }
    }
}
