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
            throw new System.NotImplementedException();
        }
    }
}