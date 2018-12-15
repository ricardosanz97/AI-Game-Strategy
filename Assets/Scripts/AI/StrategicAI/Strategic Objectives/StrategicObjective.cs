using System.Collections.Generic;
using InfluenceMap;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace StrategicAI
{        
    [System.Serializable]
    public abstract class StrategicObjective : MonoBehaviour
    {
        [SerializeField] private int _sampleRadius;
        public int SampleRadius => _sampleRadius;

        public abstract Entity DecideBasedOnInfluenceData(AbstractNPCBrain analyzedNpc, List<Node> influenceData,
            Entity[] playerControlledEntites, LevelController levelController);

        protected Entity GetClosestEntityInCollection(Entity e, Entity[] collection)
        {
            float minDistance = int.MaxValue;
            Entity closestEntity = null;

            for (int i = 0; i < collection.Length; i++)
            {
                float tempDistance = Vector3.Distance(e.transform.position, collection[i].transform.position);

                if (tempDistance < minDistance)
                {
                    minDistance = tempDistance;
                    closestEntity = collection[i];
                } 
            }

            return closestEntity;
        }
        
        protected Entity GetClosestEntityInCollection(Entity e, Entity[] collection, ENTITY entityFilter)
        {
            float minDistance = int.MaxValue;
            Entity closestEntity = null;

            for (int i = 0; i < collection.Length; i++)
            {
                if (collection[i].entityType == entityFilter)
                {
                    float tempDistance = Vector3.Distance(e.transform.position, collection[i].transform.position);

                    if (tempDistance < minDistance)
                    {
                        minDistance = tempDistance;
                        closestEntity = collection[i];
                    } 
                }
            }

            return closestEntity;
        }
    }
}