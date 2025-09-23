using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GermBox.Pathogens
{
    //Symptoms is for the class that applies to pathogens, this class is used by symptoms for special effects
    internal class Effect
    {
        internal static void Damage(BaseSimObject target)
        {
            float pDamage = Mathf.Max((float)target.getHealth() * 0.08f, 1f);
            if (Randy.randomBool() && target.getHealth() > 1)
                target.getHit(pDamage, pAttackType: AttackType.Plague);
            else if (target.getHealth() == 1)
            {
                target.getHit(pDamage, pAttackType: AttackType.Plague);
            }
        }
        internal static void Anger(BaseSimObject target)
        {
            if (Randy.randomChance(10))
            {
                if (target.a.addStatusEffect("angry")) target.a.setStatsDirty();
            }
        }
        internal static void Depression(BaseSimObject target)
        {
            target.a.setHappiness(target.a.getHappiness() - 2);
        }
        internal static void Explode(BaseSimObject target)
        {
            ActionLibrary.deathBomb(target);
        }
        internal static void Madness(BaseSimObject target)
        {
            //changeHappiness
            if (Randy.randomChance(10) && !target.a.hasTrait("strong_minded")) {
                target.a.addTrait("madness");
            }
        }
        internal static void Age(BaseSimObject target) {
            ActionLibrary.timeParadox(null, target.a);
        }
        internal static void Teleport(BaseSimObject target) {
            ActionLibrary.teleportRandom(null,target);
        }
        internal static void Alien(BaseSimObject target) { 
            ActionLibrary.spawnAliens(target);
        }
        internal static void ColdOne(BaseSimObject target) { 
            ActionLibrary.turnIntoIceOne(target);
        }
    }
}
