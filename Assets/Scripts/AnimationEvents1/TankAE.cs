using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAE : MonoBehaviour
{

    public void DoAnimationEvent()
    {
        FindObjectOfType<SoundManager>().PlaySingle(FindObjectOfType<SoundManager>().tankSoundAttack);
    }
}
