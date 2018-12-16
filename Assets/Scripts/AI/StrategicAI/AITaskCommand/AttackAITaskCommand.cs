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
            if (chosenTarget != null) //filtramos la turret
            {
                _doer.GetComponent<AttackOrder>().Attack = true;
                //ahora mismo está en el estado Attack
                _doer.GetComponent<Attack>().AIAttack(chosenTarget);
            }
        }
    }
}