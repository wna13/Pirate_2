using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartUpgradeButtons : MonoBehaviour
{
    [SerializeField] StartUpgradeButtonComp btnHP, btnDMG, btnReload;

    private void OnEnable() 
    {
        InitButtons();
    }

    public void InitButtons()
    {
        btnHP.InitButton(this, 0);
        btnDMG.InitButton(this, 1);
        btnReload.InitButton(this, 2);
    }

    public void InfoRefresh()
    {
        btnHP.InfoRefresh();
        btnDMG.InfoRefresh();
        btnReload.InfoRefresh();
    }




}
