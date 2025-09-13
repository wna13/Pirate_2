using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ToastMessageManager : MonoBehaviour
{
    [SerializeField] Image fade;
    [SerializeField] TextMeshProUGUI tMessage;

    public void Show( string _message )
    {
        fade.gameObject.SetActive(true);
        fade.DOKill();

        tMessage.gameObject.SetActive(true);
        tMessage.DOKill();

        fade.DOFade(0.95f, 0.1f);
        fade.DOFade(0f, 1f).SetDelay(1f).OnComplete(()=>this.gameObject.SetActive(false));

        tMessage.text = _message;
        tMessage.DOFade(1f, 0.1f);
        tMessage.DOFade(0f, 1f).SetDelay(1f);
        
        SoundManager.Instance.PlayEffect("error");
    }


}
