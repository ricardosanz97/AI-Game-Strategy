using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Order : MonoBehaviour {
    public abstract bool Check();
}
