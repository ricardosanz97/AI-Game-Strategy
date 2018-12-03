using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMSO
{
	[System.Serializable]
	[CreateAssetMenu(menuName = "FSM/State")]
	public class State : ScriptableObject
	{
		public Action[] OnEnterActions;
		public Action[] OnExitActions;
		public Action[] actions;
		public Transition[] transitions;

		protected State(Action[] actions, Transition[] transitions)
		{
			this.actions = actions;
			this.transitions = transitions;
		}

		public void UpdateState(BrainController controller)
		{
			DoActions (controller);
			CheckTransitions (controller);
		}

		private void DoActions(BrainController controller)
		{
			for (int i = 0; i < actions.Length; i++) {
				actions [i].Act (controller);
			}
		}

		private void CheckTransitions(BrainController controller)
		{
			for (int i = 0; i < transitions.Length; i++) 
			{
				bool decisionSucceeded = transitions [i].decision.Decide (controller);

				if (decisionSucceeded) {
					controller.TransitionToState (transitions [i].trueState);
				} else 
				{
					controller.TransitionToState (transitions [i].falseState);
				}
			}
		}
	}
}
