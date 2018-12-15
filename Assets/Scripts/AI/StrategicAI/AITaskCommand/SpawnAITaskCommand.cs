using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Zenject;

namespace StrategicAI
{
    public class SpawnAITaskCommand : AITaskCommand
    {
        private ENTITY troopToSpawn;
        private List<CellBehaviour> spawnableCells;
        [Inject] private SpawnablesManager _spawnablesManager;

        public SpawnAITaskCommand(ENTITY troopToSpawn, List<CellBehaviour> cells)
        {
            this.troopToSpawn = troopToSpawn;
            spawnableCells = cells;
        }

        public override void PerformCommand()
        {
            SpawnablesManager manager = Object.FindObjectOfType<SpawnablesManager>();
            Assert.IsNotNull(manager);
            Assert.IsNotNull(troopToSpawn);

            CellBehaviour spawnLocation = GetRandomFreeCell();
            switch (troopToSpawn)
            {
                    case ENTITY.Launcher:
                        manager.SpawnEntity(spawnLocation, troopToSpawn, Entity.Owner.AI);
                        break;
                        
                    case ENTITY.Prisioner:
                        manager.SpawnEntity(spawnLocation, troopToSpawn, Entity.Owner.AI);
                        break;
                        
                    case ENTITY.Tank:
                        manager.SpawnEntity(spawnLocation, troopToSpawn, Entity.Owner.AI);
                        break;
                    
                    case ENTITY.Turret:
                        manager.SpawnEntity(spawnLocation, troopToSpawn, Entity.Owner.AI);
                        break;
                    
                    case ENTITY.Wall:
                        manager.SpawnEntity(spawnLocation, troopToSpawn, Entity.Owner.AI);
                        break;
            }
        }

        private CellBehaviour GetRandomFreeCell()
        {
            int random = Random.Range(0, spawnableCells.Count);
            
            while(spawnableCells[random].entityIn != null)
            {
                random = Random.Range(0, spawnableCells.Count);
            }
            
            return spawnableCells[random];
        }
    }
}