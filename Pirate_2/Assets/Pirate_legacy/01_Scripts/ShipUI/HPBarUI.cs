using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class HPBarUI : MonoBehaviour
{
    [SerializeField] Image hpBarGuage;

    public void HPBarInit()
    {
        hpBarGuage.fillAmount = 1f;
        if ( UIDataManagerTable.Instance != null ) hpBarGuage.color = UIDataManagerTable.Instance.colorHPFull;
    }

    public void HPBarChange(float _shipHPRatio)
    {
        hpBarGuage.DOKill();
        _shipHPRatio = Mathf.Clamp(_shipHPRatio, 0f, 1f);
        hpBarGuage.DOFillAmount(_shipHPRatio, 0.2f);
        
        Color hpBarColor = Color.white;
        if ( _shipHPRatio >= 0.7f ) hpBarColor = UIDataManagerTable.Instance.colorHPFull;
        if ( _shipHPRatio < 0.7f && _shipHPRatio >- 0.3f ) hpBarColor = UIDataManagerTable.Instance.colorHPMid;
        if ( _shipHPRatio < 0.3f ) hpBarColor = UIDataManagerTable.Instance.colorHPLow;

        if ( hpBarGuage.color != hpBarColor )
        {
            hpBarGuage.DOColor(hpBarColor, 0.2f);
        }
    }


}
