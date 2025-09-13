using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AppLovinMax;

public class MaxAdManager : MonoBehaviour
{
    private void OnEnable() 
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            // AppLovin SDK is initialized, start loading ads
        };
        MaxSdk.SetSdkKey("llJ9btzePXTSN8r71TTkx2yZT4NoD5MvfoMHsiu9Dhbnh5MAS7FO9CWcJPiNDLURqVIsm72KruKyxwuoupeVMw");
        // MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();

        InitializeBannerAds();

        StartCoroutine(InitAndLoadInterstitialCor());
        StartCoroutine(InitAndLoadRewardCor());
    }
    
    public void ShowBanner()
    {
        MaxSdk.ShowBanner(bannerAdUnitId);
    }

    public void HideBanner()
    {
        MaxSdk.HideBanner(bannerAdUnitId);
    }
    public void ShowIsInterstitial()
    {
        if ( MaxSdk.IsInterstitialReady(adUnitIdInitial) )
        {
            MaxSdk.ShowInterstitial(adUnitIdInitial);
        }
        else
        {
            LoadInterstitial();
        }
    }
    public void ShowRewardAdsDebugTest()
    {
        ShowRewardAd();
    }
    public bool ShowRewardAd()
    {
        if (MaxSdk.IsRewardedAdReady(adUnitId))
        {
            MaxSdk.ShowRewardedAd(adUnitId);
            return true;
        }
        else 
        {
            LoadRewardedAd();
            return false;
        }

    }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////             Banner        //////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#if UNITY_IOS
string bannerAdUnitId = "ed142db3e2e4572f"; // Retrieve the ID from your account
#else // UNITY_ANDROID
string bannerAdUnitId = "425a9ffc8a24c163"; // Retrieve the ID from your account
#endif

    public void InitializeBannerAds()
    {
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.white);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent      += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent  += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent     += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent    += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent   += OnBannerAdCollapsedEvent;
    }

    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {}

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)  {}

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////             Intistitial        /////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#if UNITY_IOS
string adUnitIdInitial = "b9365c8da363f6ff";
#else // UNITY_ANDROID
string adUnitIdInitial = "bb18b0716c93ff53";
#endif
int retryAttemptInitial;
    IEnumerator InitAndLoadInterstitialCor()
    {
        InitializeInterstitialAds();
        yield return new WaitForSeconds(30.0f);

        while ( true )
        {
            if (MaxSdk.IsInterstitialReady(adUnitIdInitial))
            {
                yield break;
            }
            else 
            {
                LoadInterstitial();
                yield return new WaitForSeconds(30.0f);
            }
        }
    }
    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        
        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(adUnitIdInitial);
    }

    private void OnInterstitialLoadedEvent(string adUnitIdInitial, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttemptInitial = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitIdInitial, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttemptInitial++;
        double retryDelay = Mathf.Pow(2, Mathf.Min(6, retryAttemptInitial));
        
        Invoke("LoadInterstitial", (float) retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitIdInitial, MaxSdkBase.AdInfo adInfo) {}

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitIdInitial, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitIdInitial, MaxSdkBase.AdInfo adInfo) {}

    private void OnInterstitialHiddenEvent(string adUnitIdInitial, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
    }





////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////             Reward        //////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#if UNITY_IOS
string adUnitId = "0ece45c84a7d3f20";
#else // UNITY_ANDROID
string adUnitId = "2fd92ff3d7f6e151";
#endif
int retryAttempt;
    IEnumerator InitAndLoadRewardCor()
    {
        InitializeRewardedAds();
        yield return new WaitForSeconds(30.0f);

        while ( true )
        {
            if (MaxSdk.IsRewardedAdReady(adUnitId))
            {
                yield break;
            }
            else 
            {
                LoadRewardedAd();
                yield return new WaitForSeconds(30.0f);
            }
        }
    }

    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
                
        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(adUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Mathf.Pow(2, Mathf.Min(6, retryAttempt));
        
        Invoke("LoadRewardedAd", (float) retryDelay);
        AdsTotalManager.Instance.RewardAdsResultCallBack(false);

    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        if ( AdsTotalManager.Instance != null )
        {
            AdsTotalManager.Instance.RewardAdsResultCallBack(true);
        }
        // The rewarded ad displayed and the user should receive the reward.
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }





}
