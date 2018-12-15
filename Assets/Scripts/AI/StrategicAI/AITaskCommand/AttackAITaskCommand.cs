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
            _doer.GetComponent<AttackOrder>().Attack = true;
            _doer.GetComponent<Attack>().StartAttack(chosenTarget);
        }
    }
}