using UnityEngine;

namespace StrategicAI
{
    public class AttackAITaskCommand : AITaskCommand
    {
        private AbstractNPCBrain _doer;
        private readonly Entity chosenTarget;
        
        public AttackAITaskCommand(AbstractNPCBrain analyzedEntity, Entity chosenTarget)
        {
            _doer = analyzedEntity;
            this.chosenTarget = this.chosenTarget;
        }

        public override void PerformCommand()
        {
            throw new System.NotImplementedException();
        }
    }
}