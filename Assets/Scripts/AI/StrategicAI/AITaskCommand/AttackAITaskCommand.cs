using UnityEngine;
using System.Collections;

namespace StrategicAI
{
    public class AttackAITaskCommand : AITaskCommand
    {
        private AbstractNPCBrain _doer;
        private Entity chosenTarget;
        
        public AttackAITaskCommand(AbstractNPCBrain analyzedEntity, Entity chosenTarget) //si devuelve null chosenTarget, significa que es Turret
        {
            _doer = analyzedEntity;
            this.chosenTarget = chosenTarget;
        }

        public override void PerformCommand()
        {
            if (_doer.GetComponent<Troop>() != null)
            {
                _doer.GetComponent<AttackOrder>().Attack = true;
                _doer.StartCoroutine(PerformAttack());

            }
        }

        IEnumerator PerformAttack()
        {
            yield return new WaitForSeconds(0.5f);
            _doer.GetComponent<Attack>().ObjectiveAssigned = true;
            _doer.GetComponent<Attack>().targetEntity = chosenTarget;
        }
    }
}