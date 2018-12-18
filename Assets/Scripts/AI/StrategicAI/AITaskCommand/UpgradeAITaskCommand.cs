using UnityEngine;

namespace StrategicAI
{
    public class UpgradeAITaskCommand : AITaskCommand
    {
        private Entity entityToUpgrade;
        
        public UpgradeAITaskCommand(Entity chosenTarget)
        {
            entityToUpgrade = chosenTarget;
        }

        public override void PerformCommand()
        {
            entityToUpgrade.GetComponent<AbstractNPCBrain>().UpgradeNPC();
        }
    }
}