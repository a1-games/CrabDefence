using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager AskFor { get => instance; }
    private void Awake()
    {
        instance = this;
    }
    //--------------------------------------------------------
    //--------------------------------------------------------
    //--------------------------------------------------------

    public AudioSource seagullSource;
    public AudioSource uiClickSource;
    public AudioSource damageSource;

    private void Update()
    {
        if (savedSeagullTime + seagullCooldown < Time.time)
        {
            seagullSource.Play();
            seagullCooldown = Random.Range(120f, 330f);
            savedSeagullTime = Time.time;
        }
    }

    private float savedSeagullTime;
    private float seagullCooldown = 25f;

    public void ClickSound()
    {
        uiClickSource.pitch = Random.Range(0.96f, 1.04f);
        uiClickSource.Play();
    }
    public void PlayerDamage()
    {
        damageSource.pitch = Random.Range(0.96f, 1.04f);
        damageSource.Play();
    }
}
