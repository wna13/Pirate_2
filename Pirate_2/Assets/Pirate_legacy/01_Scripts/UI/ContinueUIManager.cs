using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContinueUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tCounter;
    [SerializeField] Button btnAdsContinue, btnNext;


    public void CountSetFive() => tCounter.text = "5";
    public void CountSetFour() => tCounter.text = "4";
    public void CountSetThree() => tCounter.text = "3";
    public void CountSetTwo() => tCounter.text = "2";
    public void CountSetOne() => tCounter.text = "1";
    public void CountSetZero() => tCounter.text = "0";

    private void Start() 
    {
        btnAdsContinue.onClick.AddListener(GetBtnContinueAdsCall);
        btnNext.onClick.AddListener(GetBtnNothanks);
    }

    public void GetBtnContinueAdsCall()
    {
        AdsTotalManager.Instance.continueUIManager = this;
        AdsTotalManager.Instance.RewardCall(AdsTotalManager.RewardType.GameContinue);
    }
    public void AdsCallBack()
    {
        AdsCallSuccess();
    }


    void AdsCallSuccess()
    {
        GameDataManager.Instance.isJustWachedAd = true;
        GameFlowManager.Instance.GameContinue();
        InvokeFalseThis();
    }



    public void CountEND()
    {
        Debug.Log("CountEND");

        GetBtnNothanks();
    }

    public void GetBtnNothanks()
    {
        Debug.Log("nothans");
        EndingUIManager.Instance.ResultUION();
        Invoke("InvokeFalseThis", 0.5f);
    }

    void InvokeFalseThis()
    {
        this.gameObject.SetActive(false);
    }
}
