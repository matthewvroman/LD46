using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager s_instance;
    public static AudioManager Instance
    {
        get
        {
            if(s_instance == null)
            {
                AudioManager prefab = Resources.Load<AudioManager>("AudioManager");
                s_instance = GameObject.Instantiate<AudioManager>(prefab);
            }
            return s_instance;
        }
    }

    [SerializeField] private AudioSource m_musicSource;
    [SerializeField] private AudioClip m_titleMusic;
    [SerializeField] private AudioClip m_gameMusic;
    [SerializeField] private AudioSource m_sfxChannel1;
    [SerializeField] private AudioSource m_sfxChannel2;
    [SerializeField] private AudioClip m_buttonSfx;
    [SerializeField] private AudioClip m_dialogueSfx;
    [SerializeField] private AudioClip m_playerHitSfx;
    [SerializeField] private AudioClip m_enemyHitSfx;
    [SerializeField] private AudioClip m_meteorSfx;
    [SerializeField] private AudioClip m_lightningSfx;
    [SerializeField] private AudioClip m_rockSfx;
    [SerializeField] private AudioClip m_portalSpawn;
    [SerializeField] private AudioClip m_portalGrabPlayer;
    [SerializeField] private AudioClip m_portalExpelPlayer;
    [SerializeField] private AudioClip m_portalDespawn;

    public void PlayTitleMusic()
    {
        m_musicSource.clip = m_titleMusic;
        m_musicSource.Play();
    }

    public void PlayGameMusic()
    {
        m_musicSource.clip = m_gameMusic;
        m_musicSource.Play();
    }

    public void PlayButtonSfx()
    {
        m_sfxChannel1.clip = m_buttonSfx;
        m_sfxChannel1.Play();
    }

    public void PlayDialogueSfx()
    {
        m_sfxChannel1.clip = m_dialogueSfx;
        m_sfxChannel1.Play();
    }

    public void PlayPlayerHitSfx()
    {
        m_sfxChannel1.clip = m_playerHitSfx;
        m_sfxChannel1.Play();
    }

    public void PlayEnemyHitSfx()
    {
        m_sfxChannel1.clip = m_enemyHitSfx;
        m_sfxChannel1.Play();
    }

    public void PlayMeteorSfx()
    {
        m_sfxChannel2.clip = m_meteorSfx;
        m_sfxChannel2.Play();
    }

    public void PlayLightningStrike()
    {
        m_sfxChannel2.clip = m_lightningSfx;
        m_sfxChannel2.Play();
    }

    public void PlayRockSfx()
    {
        m_sfxChannel2.clip = m_rockSfx;
        m_sfxChannel2.Play();
    }

    public void PlayPortalSpawn()
    {
        m_sfxChannel1.clip = m_portalSpawn;
        m_sfxChannel1.Play();
    }

    public void PlayPortalGrabPlayer()
    {
        m_sfxChannel2.clip = m_portalGrabPlayer;
        m_sfxChannel2.Play();
    }

    public void PlayPortalExpelPlayer()
    {
        m_sfxChannel2.clip = m_portalExpelPlayer;
        m_sfxChannel2.Play();
    }

    public void PlayPortalDespawn()
    {
        m_sfxChannel1.clip = m_portalDespawn;
        m_sfxChannel1.Play();
    }
}
