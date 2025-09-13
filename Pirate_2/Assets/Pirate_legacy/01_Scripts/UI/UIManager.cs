using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance ;
    public Timmer timmer;
    public DimUI dimUI;
    public CoinGoodsItem coinUI;
    public FlagUIManager flagUIManager;
    public StartUIManager startUIManager;
    public PopupManager popupManager;


    void Start()
    {
        Instance = this;
        ShipTryCheck();
        CheckInternetConnection();
    }

    public bool CheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            popupManager.NoInternetPopupOpen(true);
            return false;
        }
        else
        {
            return true;
        }
    }


    void ShipTryCheck()
    {
        if ( GameDataManager.Instance.isTutorialMode ) return;
        if ( GameDataManager.Instance.isJustStarted == true ) return;
        
        if ( GameDataManager.Instance.playCount % 4 == 0 && GameDataManager.Instance.playCount != 0 )
        {
            int rdm = Random.Range(1, ShipModelData.Instance.shipData.Count);
            if ( ShipModelData.Instance.shipData[rdm].level < 0 )
            {
                popupManager.shipTrialPopup.PopupSetting(rdm);
                popupManager.TryShipON(true);
                return;
            }
        }

        if ( GameDataManager.Instance.playCount == 5 )
        {
            popupManager.RateUsPopupON(true);
        }

    }

    public void HitUIEffect()
    {
        dimUI.DimStart();
    }
    [SerializeField] ToastMessageManager toastMessageManager;
    public void ToastShow( string _message )
    {
        toastMessageManager.gameObject.SetActive(true);
        toastMessageManager.Show(_message);
    }

    [SerializeField] GameObject loading;
    public void FakeloadingON( float _duration )
    {
        loading.SetActive(true);
        Invoke("InvokeLoadingOFF", _duration);
    }
    void InvokeLoadingOFF()
    {
        loading.SetActive(false);
    }
    public void FakeLoadingOFF()
    {
        CancelInvoke("InvokeLoadingOFF");
        InvokeLoadingOFF();
    }
}
