using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Numerics;

public class CoinGoodsItem : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI tmpTextValue;
    [SerializeField] Transform trIcon;
    [SerializeField] Transform trIconParent; //애니메이션 우르르 처리
    List<Transform> goodsList = new List<Transform>();

    void Start() 
    {
        if ( trIconParent != null )
        {
            if ( goodsList.Count < 1 )
            {
                for ( int i = 0 ; i < trIconParent.childCount; i ++ )
                {
                    goodsList.Add( trIconParent.GetChild(i).GetComponent<Transform>() );
                }
            }
        }
        canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;

        tmpTextValue.text = PlayerDataManager.Instance.coin.ToString("N0");
    }

    public void GoodsValueRefresh ()
    {

        if ( GameFlowManager.Instance.isGameStart == true) 
        {
            canvasGroup.alpha = 1f;
            canvasGroup.DOFade(0f, 0.5f).SetDelay(2f);
        }
        tmpTextValue.text = PlayerDataManager.Instance.coin.ToString("N0");
    }   

    public void CanvasGroupON(bool isOn)
    {
        if ( isOn )
        {
            canvasGroup.DOFade(1f, 0.3f);
        }
        else
        {
            canvasGroup.DOFade(0f, 0.3f);
        }
    }

    public void GoodsValueChange ( int _value )
    {
        PlayerDataManager.Instance.CoinvalueChange(_value);
        GoodsValueRefresh();
        
        tmpTextValue.DOKill();

        tmpTextValue.color = Color.yellow;
        tmpTextValue.DOColor(Color.white, 0.3f).SetDelay(0.1f);

        tmpTextValue.transform.localScale = UnityEngine.Vector2.one * 1.1f;
        tmpTextValue.transform.DOScale(1f, 0.3f);
    }

    public void GoodsValueUPwithAnimation( Transform _startPos, int _value )
    {
        trIconParent.transform.position = _startPos.position;
        StopCoroutine(GetStarEffectCor(_value));
        StartCoroutine(GetStarEffectCor(_value));
        
    }
    [SerializeField] float clusterRadius = 250f;
    IEnumerator GetStarEffectCor(int _value)
    {
        foreach( Transform t in goodsList )
        {
            t.gameObject.SetActive(true);
            t.localPosition = UnityEngine.Vector2.zero;

            // 극좌표를 사용하여 군집 내에서 랜덤한 위치 생성
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(0f, clusterRadius);

            // 극좌표를 직교좌표로 변환
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            UnityEngine.Vector2 _firstPos = new UnityEngine.Vector2 (x ,y );

            t.DOKill();
            t.DOLocalMove(_firstPos, 0.2f);
        
            t.localScale = UnityEngine.Vector2.zero;
            t.DOScale(1f, 0.2f);

            t.DOMove(trIcon.position, 0.4f).SetDelay(0.6f);
            t.DOScale(0f, 0.3f).SetDelay(0.7f).OnComplete(()=> t.gameObject.SetActive(false) );

            yield return new WaitForEndOfFrame();
        }
        SoundManager.Instance.PlayEffect("getcoin");

        yield return new WaitForSeconds ( 0.7f );

        GoodsValueChange( _value );
    
        yield break;
    }

    public void GoodsNotEnough()
    {
        tmpTextValue.DOKill();

        tmpTextValue.color = Color.red;
        tmpTextValue.DOColor(Color.white, 0.3f).SetDelay(0.2f);

        tmpTextValue.transform.localScale = UnityEngine.Vector2.one * 1.1f;
        tmpTextValue.transform.DOScale(1f, 0.3f).SetDelay(0.2f);

        UIManager.Instance.ToastShow("Not Enough Coin.");
    }

}
