using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrategicAI
{
    [System.Serializable]
    public class AISpawnStrategy
    {
        [SerializeField]
        public List<EntityBloodCost> entitiesBloodCost;
        public AISpawnStrategy (List<EntityBloodCost> list)
        {
            this.entitiesBloodCost = list;
        }
    }

    [System.Serializable]
    public class EntityBloodCost
    {
        public ENTITY entity;
        public int bloodCost;
        public EntityBloodCost (ENTITY _entity, int _bloodCost)
        {
            this.entity = _entity;
            this.bloodCost = _bloodCost;
        }
    }
}

