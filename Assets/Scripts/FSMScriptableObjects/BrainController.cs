using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FSMSO
{
    public class BrainController : MonoBehaviour
    {
        public BrainConfiguration BrainConfiguration;
        public FSMSO.State currentState;
        private bool aiActive;
    
        public void SetupAI(params object[] parameters)
        {
            currentState = BrainConfiguration.initialState;
        }
    
        void Update()
        {
            if (!aiActive)
                return;
            currentState.UpdateState(this);
        }
    
        public void TransitionToState(FSMSO.State nextState)
        {
            if (nextState != BrainConfiguration.remainState)
            {
                foreach (var onExitAction in currentState.OnExitActions)
                {
                    onExitAction.Act(this);
                }
                
                currentState = nextState;
            }
        }
    }
}
