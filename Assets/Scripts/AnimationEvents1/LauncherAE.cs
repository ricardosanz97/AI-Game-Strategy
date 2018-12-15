using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherAE : MonoBehaviour {


	public void DoAnimationEvent()
    {
        FindObjectOfType<SoundManager>().PlaySingle(FindObjectOfType<SoundManager>().launcherSoundShot);
        GetComponentInChildren<ParticleSystem>().gameObject.SetActive(true);
        GetComponentInChildren<ParticleSystem>().Play();
    }
}
