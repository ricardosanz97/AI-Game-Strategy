using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Action : MonoBehaviour{

    public int bloodCost;
    public abstract void Act(); 

}
