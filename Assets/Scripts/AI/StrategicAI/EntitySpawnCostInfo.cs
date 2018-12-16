using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntitySpawnCostInfo {

    public static List<StrategicAI.EntityBloodCost> EntitySpawnCosts = 
        new List<StrategicAI.EntityBloodCost>
    {
        new StrategicAI.EntityBloodCost(ENTITY.Launcher, 3),
        new StrategicAI.EntityBloodCost(ENTITY.Prisioner, 2),
        new StrategicAI.EntityBloodCost(ENTITY.Tank, 4),
        new StrategicAI.EntityBloodCost(ENTITY.Turret, 4),
        new StrategicAI.EntityBloodCost(ENTITY.Wall, 1)
    };
}
