using GermBox.Content;
using NeoModLoader.General.Game.extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GermBox.Pathogens
{
    internal class Symptoms
    {
        private readonly Dictionary<string, float> statModifiers = new();

        private static readonly Dictionary<string, Action<BaseSimObject>> _specialEffects = new()
        {
            { "damage", Effect.Damage},
            { "teleport", Effect.Teleport },
            { "madness", Effect.Madness },
            { "alien", Effect.Alien },
            { "age", Effect.Age },
            { "anger", Effect.Anger },
            { "depression", Effect.Depression },
        };
        //public static readonly Dictionary<string, Action<BaseSimObject>> Deaths = new()
        //{
        //    { "demon", Effect.Demon},
        //    { "explode", Effect.Explode },
        //    { "antimatter", Effect.Antimatter },
        //    { "coldone", Effect.ColdOne }
        //};
        private static readonly List<WorldAction> _deaths = new()
        {
            new WorldAction(ActionLibrary.turnIntoDemon),
            new WorldAction(ActionLibrary.deathBomb),
            new WorldAction(WorldActions.Antimatter),
            new WorldAction(ActionLibrary.spawnAliens),
            new WorldAction(WorldActions.Coldone),

        };

        public List<string> effects = new(); //corresponds to special effects

        public WorldAction death_effect = null;

        public Symptoms(Symptoms parent = null)
        {
            if (parent != null)
            {
                AssetManager.base_stats_library.ForEach<BaseStatAsset, BaseStatsLibrary>(stat =>
                {
                    statModifiers.Add(stat.id, parent[stat.id]);
                });
                return;
            }
            AssetManager.base_stats_library.ForEach<BaseStatAsset, BaseStatsLibrary>(stat =>
            {
                if (stat.multiplier && !stat.hidden)
                {
                    statModifiers.Add(stat.id, 0.0f);
                }
            });
            effects.Add("damage");
            effects.Add(GetRandomEffect());
            if (Randy.randomBool()) death_effect = GetRandomDeath();
            Randomize();
        }

        public void ApplyEffect(string ID, BaseSimObject target)
        {
            _specialEffects[ID](target);
        }

        public void AddNew()
        {
            //the main problem with this is if it gets a stat that the pathogen already modifies, it won't be adding a new stat
            var stat = AssetManager.base_stats_library.get(GetRandomStat());
            if (stat.show_as_percents)
            {
                this[stat.id] = -Randy.randomFloat(0.01f, 1f);
            }
            else
            {
                this[stat.id] = -Randy.randomFloat(1f, 100f);
            }
        }

        public void SetSymptoms(BaseStats stats)
        {
            foreach (KeyValuePair<string, float> symptom in statModifiers)
            {
                stats[symptom.Key] = symptom.Value;
            }
        }

        private void Randomize()
        {
            int numSymptoms = Randy.randomInt(1, 6);

            for (int i = 0; i < numSymptoms; i++)
            {
                var stat = AssetManager.base_stats_library.get(GetRandomStat());
                if (stat.show_as_percents)
                {
                    this[stat.id] = -Randy.randomFloat(0.01f, 1f);
                }
                else
                {
                    this[stat.id] = -Randy.randomFloat(1f, 100f);
                }
            }
        }
        private string GetRandomEffect()
        {
            var keyList = _specialEffects.Keys.ToList();

            return keyList[Randy.randomInt(0, keyList.Count)];
        }
        private WorldAction GetRandomDeath()
        {
            return _deaths[Randy.randomInt(0, _deaths.Count)];
        }
        private string GetRandomStat()
        {
            var keyList = AssetManager.base_stats_library.getList();

            return keyList.GetRandom().id;
        }
        public float this[string pKey]
        {
            get
            {
                return get(pKey);
            }
            set
            {
                set(pKey, value);
            }
        }

        private float get(string pID)
        {
            if (statModifiers.TryGetValue(pID, out var value))
            {
                return value;
            }

            return 0f;
        }

        private void set(string pID, float pAmount)
        {
            try
            {
                statModifiers[pID] = pAmount;
            }
            catch (Exception exception) {
                Debug.LogError("Couldn't set value: " + exception.Message);
            }
        }
    }
}
