using UnityEngine;
using System.Collections;

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
                chosenTarget.GetComponent<AttackOrder>().Attack = true;
                chosenTarget.StartCoroutine(PerformAttack());
            }
        }

        IEnumerator PerformAttack()
        {
            yield return new WaitForSeconds(0.5f);
            chosenTarget.GetComponent<Attack>().ObjectiveAssigned = true;
            chosenTarget.GetComponent<Attack>().targetEntity = chosenTarget;
        }
    }
}