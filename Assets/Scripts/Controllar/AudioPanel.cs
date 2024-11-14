using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPanel : MonoBehaviour
{
    [SerializeField]
    private Slider m_BGMSlider;

    [SerializeField]
    private Slider m_SFXSlider;

    private void Start()
    {
        m_BGMSlider.value = SoundManager.Instance.bgmVolume;
        m_SFXSlider.value = SoundManager.Instance.sfxVolume;
    }

    public void OnBGMSliderValueChanger (float _volume)
    {
        SoundManager.Instance.SetBGMVolume(_volume);
    }

    public void OnSFXVolumeSliderValueChanger(float _volume)
    {
        SoundManager.Instance.SetSFXVolume(_volume);
    }
}
