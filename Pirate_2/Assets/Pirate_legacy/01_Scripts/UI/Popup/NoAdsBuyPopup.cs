using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoAdsBuyPopup : MonoBehaviour
{
    [SerializeField] Button btnBuy;

    public void ButtonRefresh()
    {
        btnBuy.interactable = ! GameDataManager.Instance.isNoAdsMode;
    }

    public void NoadsBuySuccess()
    {
        GameDataManager.Instance.BuyNoAdsMode();
        UIManager.Instance.ToastShow("Purchase Complete.");
        UIManager.Instance.FakeLoadingOFF();

        Invoke("CloseThis", 0.5f);
    }
    
    void CloseThis()
    {
        UIManager.Instance.popupManager.NoAdsPopupON(false);
    }

    public void NoAdsBuyFail()
    {
        UIManager.Instance.ToastShow("Sorry. Try Again.");
    }
    public void FakeLoadingON()
    {
        UIManager.Instance.FakeloadingON(5f);
    }
}
