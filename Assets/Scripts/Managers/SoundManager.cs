using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public float defaultVolume = 0.3f;
    public float pauseVolume = 0.05f;

    public AudioSource sfxSource;
    public AudioSource backgroundSource;

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
    [HideInInspector]
    public AudioClip levelUpSound;
    [HideInInspector]
    public AudioClip menuBackgroundSound;
    [HideInInspector]
    public AudioClip gameBackgroundSound;
    [HideInInspector]
    public AudioClip quitSound;
    [HideInInspector]
    public AudioClip readySound;

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
        levelUpSound = Resources.Load<AudioClip>("SFX/LevelUpSound");
        menuBackgroundSound = Resources.Load<AudioClip>("SFX/MenuBackgroundSound");
        gameBackgroundSound = Resources.Load<AudioClip>("SFX/GameBackgroundSound");
        quitSound = Resources.Load<AudioClip>("SFX/QuitSound");
        readySound = Resources.Load<AudioClip>("SFX/ReadySound");
    }

    public void PlaySingle (AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void PlayBackground (AudioClip clip, float volume)
    {
        backgroundSource.volume = volume;
        
        if (clip != backgroundSource.clip)
        {
            backgroundSource.loop = true;
            backgroundSource.clip = clip;
            backgroundSource.Play();
        }
        //backgroundSource.Play();
    }

    public void PauseBackground()
    {
        backgroundSource.Pause();
    }
}
