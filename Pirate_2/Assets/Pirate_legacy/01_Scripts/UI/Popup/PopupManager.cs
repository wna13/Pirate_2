using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupManager : MonoBehaviour
{
    [SerializeField] Image imgFade;

    void Fade ( bool _on )
    {
        imgFade.DOKill();

        if ( _on )
        {
            imgFade.gameObject.SetActive(true);
            imgFade.color = new Color (0f,0f,0f,0f);
            imgFade.DOFade(0.9f, 0.2f);
        }
        else
        {
            imgFade.DOFade(0f, 0.2f).OnComplete(()=> imgFade.gameObject.SetActive(false));
        }
    }
    [SerializeField] NoAdsBuyPopup noAdsBuyPopup;

    public void NoAdsPopupON(bool _on)
    {
        Fade(_on);
        
        if ( _on )
        {
            noAdsBuyPopup.gameObject.SetActive(true);
            noAdsBuyPopup.ButtonRefresh();
            noAdsBuyPopup.transform.localScale = Vector2.zero;
            noAdsBuyPopup.transform.DOKill();
            noAdsBuyPopup.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }
        else
        {
            noAdsBuyPopup.transform.DOScale(0f, 0.3f).OnComplete(()=> noAdsBuyPopup.gameObject.SetActive(false));
        }
        SoundManager.Instance.PlayEffect("popupOpen");
    }
    public ShipTrialPopup shipTrialPopup;
    public void TryShipON ( bool _on )
    {
        Fade(_on);
        
        if ( _on )
        {
            shipTrialPopup.gameObject.SetActive(true);
            shipTrialPopup.transform.localScale = Vector2.zero;
            shipTrialPopup.transform.DOKill();
            shipTrialPopup.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }
        else
        {
            shipTrialPopup.transform.DOScale(0f, 0.3f).OnComplete(()=> shipTrialPopup.gameObject.SetActive(false));
        }
        SoundManager.Instance.PlayEffect("popupOpen");
    }

    [SerializeField] SettingManager settingManager;
    public void SettingON ( bool _on )
    {
        Fade(_on);
        
        if ( _on )
        {
            settingManager.gameObject.SetActive(true);
            settingManager.transform.localScale = Vector2.zero;
            settingManager.transform.DOKill();
            settingManager.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }
        else
        {
            settingManager.transform.DOScale(0f, 0.3f).OnComplete(()=> settingManager.gameObject.SetActive(false));
        }
    }

    [SerializeField] RateUsPopup rateUspopup;
    public void RateUsPopupON( bool _on )
    {
        Fade(_on);
        
        if ( _on )
        {
            rateUspopup.gameObject.SetActive(true);
            rateUspopup.transform.localScale = Vector2.zero;
            rateUspopup.transform.DOKill();
            rateUspopup.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }
        else
        {
            rateUspopup.transform.DOScale(0f, 0.3f).OnComplete(()=> rateUspopup.gameObject.SetActive(false));
        }

        SoundManager.Instance.PlayEffect("popupOpen");
    }

    [SerializeField] GameObject noInternet;

    public void NoInternetPopupOpen( bool _on )
    {
        Fade(_on);
        
        if ( _on )
        {
            noInternet.gameObject.SetActive(true);
            noInternet.transform.localScale = Vector2.zero;
            noInternet.transform.DOKill();
            noInternet.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }
        else
        {
            noInternet.transform.DOScale(0f, 0.3f).OnComplete(()=> noInternet.gameObject.SetActive(false));
        }

        SoundManager.Instance.PlayEffect("popupOpen");
    }

}
