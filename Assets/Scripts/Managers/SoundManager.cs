using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource sfxSource;
    public float lowPitchRange = 0.95f;
    public float hightPitchRange = 1.05f;

    [HideInInspector]
    public AudioClip cageSoundAttack;
    [HideInInspector]
    public AudioClip cageSoundSpawn;
    [HideInInspector]
    public AudioClip cardsSound;
    [HideInInspector]
    public AudioClip incorrectMovement;
    [HideInInspector]
    public AudioClip launcherSoundShot;
    [HideInInspector]
    public AudioClip launcherSoundSpawn;
    [HideInInspector]
    public AudioClip tankSoundAttack;
    [HideInInspector]
    public AudioClip tankSoundSpawn;
    [HideInInspector]
    public AudioClip turretSoundShot;
    [HideInInspector]
    public AudioClip turretSoundSpawn;
    [HideInInspector]
    public AudioClip cancelActionSound;
    [HideInInspector]
    public AudioClip buttonPressedSound;

    void Start()
    {
        cageSoundAttack = Resources.Load<AudioClip>("SFX/CageSoundAttack");
        cageSoundSpawn = Resources.Load<AudioClip>("SFX/CageSoundSpawn");
        cardsSound = Resources.Load<AudioClip>("SFX/Cards");
        incorrectMovement = Resources.Load<AudioClip>("SFX/IncorrectMovement");
        launcherSoundShot = Resources.Load<AudioClip>("SFX/LauncherSoundShot");
        launcherSoundSpawn = Resources.Load<AudioClip>("SFX/LauncherSoundSpawn");
        tankSoundAttack = Resources.Load<AudioClip>("SFX/TankSoundAttack");
        tankSoundSpawn = Resources.Load<AudioClip>("SFX/TankSoundSpawn");
        turretSoundShot = Resources.Load<AudioClip>("SFX/TurretSoundShot");
        turretSoundSpawn = Resources.Load<AudioClip>("SFX/TurretSoundSpawn");
        cancelActionSound = Resources.Load<AudioClip>("SFX/CancelAction");
        buttonPressedSound = Resources.Load<AudioClip>("SFX/ButtonPressed");
    }

    public void PlaySingle (AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }
}
