using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultUImanager : MonoBehaviour
{
    [SerializeField] Button adReward, nothx;

    private void Start() 
    {
        adReward.onClick.AddListener(GetBtnAdsReward);
        nothx.onClick.AddListener(GetBtnNothanks);
    }
    [SerializeField] Transform coinPos;
    [SerializeField] TextMeshProUGUI tCoinRewardValue, tCoinRewardValueResult, tEXPValue;
    [SerializeField] StageMapUI stageMapUI;
    public void Init()
    {
        tCoinRewardValue.text = GameFlowManager.Instance.coinGotThisGame.ToString("N0");
        tCoinRewardValueResult.text = GameFlowManager.Instance.coinGotThisGame.ToString("N0");
        tEXPValue.text = GameFlowManager.Instance.expGotThisGame.ToString("N0");

        stageMapUI.Init();
    }
    [SerializeField] TextMeshProUGUI tRewardValueAtButton;
    int coinAdsValue;
    int CoinAdsValueFixed;
    public void MapAnimStart()
    {
        stageMapUI.FilledCountUPAnim();
    }
    [SerializeField] Animator valueAnimator;

    public void RewardValueChange(int _multiply)
    {
        // tRewardValueAtButton.DOKill();
        // tRewardValueAtButton.transform.localScale = Vector2.one * 1.1f;
        // tRewardValueAtButton.transform.DOScale(1f, 0.3f);
        coinAdsValue = GameFlowManager.Instance.coinGotThisGame * _multiply;
        tRewardValueAtButton.text = coinAdsValue.ToString("N0");
    }
    public void GetBtnAdsReward()
    {
        adReward.interactable = false;
        CoinAdsValueFixed = coinAdsValue;
        valueAnimator.speed = 0f;

        //AdsCall
        AdsTotalManager.Instance.resultUImanager = this;
        AdsTotalManager.Instance.RewardCall(AdsTotalManager.RewardType.ResultMoreCoin);
    }

    public void AdsCallBack()
    {
        GameDataManager.Instance.isJustWachedAd = true;
        Invoke("EndReset", 0.3f);
        UIManager.Instance.coinUI.GoodsValueUPwithAnimation(coinPos, CoinAdsValueFixed);
    }

    void EndReset()
    {
        adReward.DOKill();
        adReward.GetComponent<CanvasGroup>().alpha = 0.3f;
        nothx.gameObject.SetActive(false);

        Invoke("GetBtnNothanks", 1.5f);
    }

    public void GetBtnNothanks()
    {
        GameFlowManager.Instance.GameRestart();
    }

    void InvokeFalseThis()
    {
        this.gameObject.SetActive(false);
    }
}
