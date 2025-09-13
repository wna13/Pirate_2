using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonScaller : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    Button button;
    CanvasGroup canvasGroup;
    bool isButtonDowned;
    private void Start() {
        button = this.GetComponent<Button>();
        if (this.gameObject.GetComponent<CanvasGroup>() == null ) this.gameObject.AddComponent<CanvasGroup>();
        canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        isButtonDowned = false;

        canvasGroup.alpha = button.interactable ? 1f : 0.8f;
    }
    public void OnPointerDown (PointerEventData eventData)
    {
        BtnDown();
    }
    public void OnPointerUp (PointerEventData eventData)
    {
        BtnUp();
    }
    public void BtnDown()
    {
        if ( button.interactable == false ) return;

        isButtonDowned = true;
        this.gameObject.transform.DOKill();
        this.gameObject.transform.DOScale(0.8f, 0.1f);

        canvasGroup.DOKill();
        canvasGroup.alpha = 0.5f;

        SoundManager.Instance.PlayEffect("btn");
    }
    public void BtnUp()
    {
        if ( isButtonDowned == false ) return;
        this.gameObject.transform.DOKill();
        this.gameObject.transform.DOScale(1f, 0.1f);
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, 0.3f);
    }
    
}
