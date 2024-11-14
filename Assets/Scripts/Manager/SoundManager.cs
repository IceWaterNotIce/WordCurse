using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public float bgmVolume {  get; private set; }

    public float sfxVolume { get; private set; }
    private AudioSource m_sfxSource;

    private AudioSource m_bgmSource;

    private void Awake()
    {
        m_bgmSource = GameObject.FindGameObjectWithTag("BGMSource").GetComponent<AudioSource>();
        m_sfxSource = GameObject.FindGameObjectWithTag("SFXSource").GetComponent<AudioSource>();
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

    }

    public void SetBGMVolume(float _volume)
    {
        bgmVolume = _volume;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);

        m_bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float _volume) 
    { 
        sfxVolume = _volume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void PlayBGM()
    {
        if(!m_bgmSource)
        {
            m_bgmSource = GameObject.FindGameObjectWithTag("BGMSource").GetComponent<AudioSource>();
        }

        m_bgmSource.Play();
    }

    public void StopBGM()
    {
        if (!m_bgmSource)
        {
            m_bgmSource = GameObject.FindGameObjectWithTag("BGMSource").GetComponent<AudioSource>();
        }
        m_bgmSource.Stop();
    }

    public void PlaySFX(AudioClip _clip)
    {
        if (!m_sfxSource)
        {
            m_sfxSource = GameObject.FindGameObjectWithTag("SFXSource").GetComponent<AudioSource>();
        }

        m_sfxSource.PlayOneShot(_clip, sfxVolume);
    }

}
