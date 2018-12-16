using UnityEngine;

namespace StrategicAI
{
    public class AttackAITaskCommand : AITaskCommand
    {
        private AbstractNPCBrain _doer;
        private readonly Entity chosenTarget;
        
        public AttackAITaskCommand(AbstractNPCBrain analyzedEntity, Entity chosenTarget) //si devuelve null chosenTarget, significa que es Turret
        {
            _doer = analyzedEntity;
            this.chosenTarget = chosenTarget;
        }

        public override void PerformCommand()
        {
            if (chosenTarget != null)
            {
                _doer.GetComponent<AttackOrder>().Attack = true;//ahora mismo está en el estado Attack
                _doer.GetComponent<Attack>().targetEntity = chosenTarget;
                _doer.GetComponent<Attack>().ObjectiveAssigned = true;
                _doer.GetComponent<Attack>().AIAttack(chosenTarget);
            }
        }
    }
}