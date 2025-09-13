using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagUIManager : MonoBehaviour
{
    [SerializeField] Transform tFlagContentsParent;
    [SerializeField] List<FlagUISlot> flagSlot = new List<FlagUISlot>();
    [SerializeField] Button btnGetCoin, btnGetAds, btnGachaClose;
    [SerializeField] FlagUISlot sltObj;
    [SerializeField] int gachaPrice;
    [SerializeField] TextMeshProUGUI tGahcaPrice;


    private void Start() 
    {
        flagSlot.Clear();
        for  (int i = 0; i < ShipFlagData.Instance.flagData.Count; i ++ )
        {
            FlagUISlot _slot;

            if ( tFlagContentsParent.childCount <= i )
            {
                _slot = Instantiate(sltObj, tFlagContentsParent);    
            }
            else
            {
                _slot = tFlagContentsParent.GetChild(i).gameObject.GetComponent<FlagUISlot>();

            }
            if ( _slot )
            {
                flagSlot.Add(_slot);
                _slot.InitAndSet(i);
            }
        }
        tGahcaPrice.text = gachaPrice.ToString("N0");
        btnGetCoin.onClick.AddListener(GetBtnFalgGachaCoin);
        btnGetAds.onClick.AddListener(GetBtnFalgGachaAds);
        btnGachaClose.onClick.AddListener(GadchaClose);
        GachaButtonRefresh();
    }
    public void DataRefresh(int _index)
    {   
        flagSlot[_index].DataRefresh();
    }
    public void MakeNewDot(int _index)
    {
        flagSlot[_index].MakeNewDot();
    }

    [SerializeField] Animator gachaAnimator;
    [SerializeField] Image imgGachaFlag;
    void GachaButtonRefresh()
    {
        btnGetCoin.gameObject.SetActive(PlayerDataManager.Instance.coin >= gachaPrice);
        btnGetAds.gameObject.SetActive(!btnGetCoin.gameObject.activeSelf);
    }
    public void GetBtnFalgGachaCoin()
    {
        if (PlayerDataManager.Instance.coin >= gachaPrice ) 
        {
            UIManager.Instance.coinUI.GoodsValueChange(-gachaPrice);
            GachaStart();
        }
        else
        {
            UIManager.Instance.coinUI.GoodsNotEnough();
            return;
        }

    }

    void GetBtnFalgGachaAds()
    {
        AdsTotalManager.Instance.flagUIManager = this;
        AdsTotalManager.Instance.RewardCall(AdsTotalManager.RewardType.GetFlag);
    }

    public void AdsCallBack()
    {
        GachaStart();
    }
    [SerializeField] TextMeshProUGUI tGachaValue;
    void GachaStart()
    {
        int getGachaIndex = ShipFlagData.Instance.GetFlagRandom();
        imgGachaFlag.sprite = ShipFlagData.Instance.flagData[getGachaIndex].flagImage;
        tGachaValue.text = ShipFlagData.Instance.flagData[getGachaIndex].moveSpeedExtra + "%";
        gachaAnimator.gameObject.SetActive(true);
        gachaAnimator.Play("GachaStart");
    }

    public void GadchaClose()
    {
        gachaAnimator.Play("GachaHide");
    }
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                UIManager.Instance.startUIManager.GetBtnFlagToHome();
            }
        }
    }
}
