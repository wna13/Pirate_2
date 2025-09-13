using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CodeStage.AntiCheat.ObscuredTypes;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public List<AudioSource> effectChannels;
    public List<AudioSource> effect_fire;
    public List<AudioSource> effect_hit;
    public AudioSource bgmChannel;

    private Dictionary<string, AudioClip> effectDic;

    public bool isEffectOn, isBGMOn;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        effectDic = new Dictionary<string, AudioClip>();

        var effectSounds = Resources.LoadAll<AudioClip>("Sounds/SFX/");

        foreach (var effectSound in effectSounds)
        {
            effectDic[effectSound.name] = effectSound;
        }

        isEffectOn = ObscuredPrefs.GetBool("EffectOn", true);
        isBGMOn = ObscuredPrefs.GetBool("isBGMOn", true);
    }

    public void DataSaveSFX(bool onOff)
    {
        isEffectOn = onOff;
        ObscuredPrefs.SetBool("EffectOn", isEffectOn);
        OnIsEffectChange(isEffectOn);
    }

    public void DataSaveBGM(bool onOff)
    {
        isBGMOn = onOff;
        ObscuredPrefs.SetBool("isBGMOn", isBGMOn);
        if (isBGMOn) PlayBGM();
        else StopBGM();
    }

    public void PlayBGM()
    {
        if (!isBGMOn || bgmChannel == null) return;

        bgmChannel.volume = 0.5f;
        bgmChannel.Play();
    }

    public void StopBGM()
    {
        if (bgmChannel == null) return;
        bgmChannel.Stop();
    }

    public void PauseSound()
    {
        isBGMOn = false;
        isEffectOn = false;
    }

    public void ResumeSound()
    {
        isBGMOn = ObscuredPrefs.GetBool("isBGMOn", true);
        isEffectOn = ObscuredPrefs.GetBool("EffectOn", true);
    }

    public bool PlayEffect(string keyName)
    {
        if (!isEffectOn || !effectDic.ContainsKey(keyName)) return false;

        foreach (var channel in effectChannels)
        {
            if (channel != null && !channel.isPlaying)
            {
                channel.clip = effectDic[keyName];
                channel.Play();
                return true;
            }
        }
        return false;
    }

    public bool PlayFire()
    {
        if (!isEffectOn ) return false;

        foreach (var channel in effect_fire)
        {
            if (channel != null && !channel.isPlaying)
            {
                channel.Play();
                return true;
            }
        }
        return false;
    }

    public bool PlayHit()
    {
        if (!isEffectOn ) return false;

        foreach (var channel in effect_hit)
        {
            if (channel != null && !channel.isPlaying)
            {
                channel.Play();
                return true;
            }
        }
        return false;
    }

    public void OnIsEffectChange(bool value)
    {
        if (!value)
        {
            foreach (var channel in effectChannels)
            {
                if (channel != null)
                {
                    channel.Stop();
                    channel.clip = null;
                }
            }
        }
    }
}
