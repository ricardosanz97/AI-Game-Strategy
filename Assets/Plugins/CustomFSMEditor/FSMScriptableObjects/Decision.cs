using UnityEngine;

namespace FSMSO
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide (BrainController controller);
    }
}