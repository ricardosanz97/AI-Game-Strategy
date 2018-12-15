using ModestTree;
using UnityEngine;
using Zenject;

namespace StrategicAI
{
    public class SpawnAITaskCommand : AITaskCommand
    {
        private ENTITY troopToSpawn;
        private CellBehaviour[] spawnableCells;
        [Inject] private SpawnablesManager _spawnablesManager;

        public SpawnAITaskCommand(ENTITY troopToSpawn, CellBehaviour[] cells)
        {
            this.troopToSpawn = troopToSpawn;
            spawnableCells = cells;
        }

        public override void PerformCommand()
        {
            Assert.IsNotNull(troopToSpawn);

            switch (troopToSpawn)
            {
                //todo implement
                    case ENTITY.Launcher:
                    //todo get random cell
                        Object.FindObjectOfType<SpawnablesManager>().SpawnEntity(spawnableCells[0], troopToSpawn, Entity.Owner.AI);
                        break;
                        
                    case ENTITY.Prisioner:
                        break;
                        
                    case ENTITY.Tank:
                        break;
                    
                    case ENTITY.Turret:
                        break;
                    
                    case ENTITY.Wall:
                        break;
            }
        }
    }
}