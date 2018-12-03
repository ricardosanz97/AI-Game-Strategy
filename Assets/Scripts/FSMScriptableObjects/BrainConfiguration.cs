using Boo.Lang;
using UnityEngine;

namespace FSMSO
{
    [CreateAssetMenu(menuName = "FSM/Brain Configuration")]
    public class BrainConfiguration : ScriptableObject
    {
        public FSMSO.State initialState;
        public FSMSO.State remainState;

        [SerializeField]public FSMSO.State[] states;
        public List<Transition> transitions;
        public List<Decision> decisions;

        private void OnEnable()
        {
            states = new State[]{};
        }
    }
}