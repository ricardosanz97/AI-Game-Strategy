using ModestTree;
using UnityEngine;
using Zenject;

namespace StrategicAI
{
    public class SpawnAITaskCommand : AITaskCommand
    {
        private TROOP troopToSpawn;
        private CellBehaviour[] spawnableCells;
        [Inject] private SpawnablesManager _spawnablesManager;

        public SpawnAITaskCommand(TROOP troopToSpawn, CellBehaviour[] cells)
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
                    case TROOP.Launcher:
                        //todo get random cell
                        Object.FindObjectOfType<SpawnablesManager>().SpawnEntityAI(troopToSpawn,spawnableCells[0]);
                        break;
                        
                    case TROOP.Prisioner:
                        break;
                        
                    case TROOP.Tank:
                        break;
                    
                    case TROOP.Turret:
                        break;
                    
                    case TROOP.Wall:
                        break;
            }
        }
    }
}