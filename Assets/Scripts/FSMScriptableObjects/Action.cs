using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMSO
{
    public abstract class Action : ScriptableObject 
    {
        public abstract void Act (BrainController controller);
    }
}
