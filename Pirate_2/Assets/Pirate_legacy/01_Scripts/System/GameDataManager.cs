using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat;
using CodeStage.AntiCheat.ObscuredTypes;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance ;
    public int playCount;
    public bool isJustWachedAd;
    public bool isTutorialMode;
    public bool isRated;
    public bool isJustStarted;
    void Start()
    {
        Instance = this;
        DataLoad();
    }

    void DataLoad()
    {
        isNoAdsMode = ObscuredPrefs.GetBool("isNoAdsMode", false);
        Invoke("InvokeBannerShow", 1f);

        playCount = ObscuredPrefs.GetInt("playCount", 0);

        isJustWachedAd = true;

        isTutorialMode = ObscuredPrefs.GetBool("isTutorialMode", true);
        isRated = ObscuredPrefs.GetBool("isRated", false);

        isJustStarted = true;
    }
    public void TutorialClear()
    {
        isTutorialMode = false;
        ObscuredPrefs.SetBool("isTutorialMode", false);
    }
    public void RateSuccess()
    {
        isRated = true;
        ObscuredPrefs.SetBool("isRated", true);
    }

    public void PlayCountUP()
    {
        playCount ++;
        ObscuredPrefs.SetInt("playCount", playCount);
        
        isJustStarted = false;
    }

    void InvokeBannerShow()
    {
        if ( isNoAdsMode == false )
        {
            AdsTotalManager.Instance.ShowBannerAD();
        }
    }

    public void BuyNoAdsMode()
    {
        isNoAdsMode = true;
        ObscuredPrefs.SetBool("isNoAdsMode", true);

        AdsTotalManager.Instance.HideBannerAD();
    }

    public bool isNoAdsMode;
}
