using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using System;

public class AdsTotalManager : MonoBehaviour
{
    public static AdsTotalManager Instance;
    [SerializeField] MaxAdManager maxAdManager;
    private void OnEnable() 
    {
        Instance = this;
    }

    public void ShowBannerAD()
    {
        if (GameDataManager.Instance.isNoAdsMode ) return;
        maxAdManager.ShowBanner();
    }
    public void HideBannerAD()
    {
        maxAdManager.HideBanner();
    }

    public void ShowOpenAD()
    {
        if (GameDataManager.Instance.isNoAdsMode) return;
        //이번 버전에서는 제외.
    }

    public void InterstitialAdShow()
    {
        if (GameDataManager.Instance.isNoAdsMode ) return;

        maxAdManager.ShowIsInterstitial();
    }

//------------------------------이상 공통규격
//------------------------------이하 커스텀


    public enum RewardType
    {
        StatUpgrade_HP,
        StatUpgrade_cannonPower,
        StatUpgrade_cannonReloadSpeed,
        GetFlag,
        GameContinue,
        ResultMoreCoin,
        TryShip
    }
    RewardType rewardCallType;
    public bool RewardCall (RewardType rewardType)
    {
        rewardCallType = rewardType;

        // FirebaseLogSend();
        
        if ( maxAdManager.ShowRewardAd() )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public StartUpgradeButtonComp startUpgradeButtonComp;
    public FlagUIManager flagUIManager;
    public ContinueUIManager continueUIManager;
    public ResultUImanager resultUImanager;
    public ShipTrialPopup shipTrialPopup;
    void FirebaseLogSend()
    {
        switch (rewardCallType)
        {
            case 0 :  
            
                break;
            
            default :
                break;
        }
    }

    public void RewardAdsResultCallBack( bool _isSuccess )
    {
        StartCoroutine(RewardAdsResultCallBackCor(_isSuccess));
    }

    IEnumerator RewardAdsResultCallBackCor ( bool _isSuccess )
    {
        yield return new WaitForSeconds(0.5f);
        if ( _isSuccess )
        {
            RewardAdsResult(_isSuccess);
        }
        else
        {
            // PopupManager.Instance.AdClosedByUser(true);
        }
        yield break;
    }

    void RewardAdsResult( bool _isSuccess )
    {
        switch ( rewardCallType )
        {
            case RewardType.StatUpgrade_HP :  
                ShipUpgrade(_isSuccess);
                FirebaseAnalytics.LogEvent("Reward_Success_StatUpgrade_HP");
                break;
            case RewardType.StatUpgrade_cannonPower :  
                ShipUpgrade(_isSuccess);
                FirebaseAnalytics.LogEvent("Reward_Success_StatUpgrade_CannonPower");
                break;
            case RewardType.StatUpgrade_cannonReloadSpeed :  
                ShipUpgrade(_isSuccess);
                FirebaseAnalytics.LogEvent("Reward_Success_StatUpgrade_CannonReloadSpeed");
                break;
            case RewardType.GetFlag :  
                FlagGet(_isSuccess);
                FirebaseAnalytics.LogEvent("Reward_Success_GetFlag");
                break;
            case RewardType.GameContinue :  
                Continue(_isSuccess);
                FirebaseAnalytics.LogEvent("Reward_Success_GameContinue");
                break;
            case RewardType.ResultMoreCoin :  
                MoreCoin(_isSuccess);
                FirebaseAnalytics.LogEvent("Reward_Success_ResultMoreCoin");
                break;
            case RewardType.TryShip :  
                TryShip(_isSuccess);
                FirebaseAnalytics.LogEvent("Reward_Success_ResultMoreCoin");
                break;
            default : break;
        }
    }
    
//------------------------------이하 보상 커스텀

    void ShipUpgrade( bool _isSuccess )
    {
        if ( _isSuccess == false )
        {
            UIManager.Instance.ToastShow("Sorry, try again");
        }
        startUpgradeButtonComp.AdsCallBack();

        return;
    }
    
    void FlagGet( bool _isSuccess )
    {
        if ( _isSuccess == false )
        {
            UIManager.Instance.ToastShow("Sorry, try again");
        }
        flagUIManager.AdsCallBack();

        return;
    }
    
    void Continue( bool _isSuccess )
    {
        if ( _isSuccess == false )
        {
            UIManager.Instance.ToastShow("Sorry, try again");
        }
        continueUIManager.AdsCallBack();

        return;
    }
    void MoreCoin( bool _isSuccess )
    {
        if ( _isSuccess == false )
        {
            UIManager.Instance.ToastShow("Sorry, try again");
        }
        resultUImanager.AdsCallBack();

        return;
    }

    void TryShip ( bool _isSuccess)
    {
        if ( _isSuccess == false )
        {
            UIManager.Instance.ToastShow("Sorry, try again");
        }
        shipTrialPopup.AdsCallBack();

        return;
    }
}
