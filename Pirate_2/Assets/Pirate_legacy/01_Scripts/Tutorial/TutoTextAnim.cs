using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Burst.Intrinsics;

public class TutoTextAnim : MonoBehaviour
{
    [SerializeField] Transform tText;
    private void OnEnable() 
    {
        if ( tText != null )
        {
            tText.localScale = Vector2.zero;
            tText.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }    
    }
}
