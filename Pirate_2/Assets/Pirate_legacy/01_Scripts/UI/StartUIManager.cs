using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
    Animator animator;
    [SerializeField] Button btnStart, btnFlag, btnFlagClose, btnShip, btnShipClose;
    public ShipUIManager shipUIManager;
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        btnStart.onClick.AddListener(GetBtnStart);
        btnFlag.onClick.AddListener(GetBtnFlag);
        btnFlagClose.onClick.AddListener(GetBtnFlagToHome);
        btnShip.onClick.AddListener(GetBtnShip);
        btnShipClose.onClick.AddListener(GetBtnShipToHome);

    }

    public void GetBtnStart()
    {
        if ( UIManager.Instance.CheckInternetConnection() == true )
        {
            animator.Play("Hide");
            CameraMover.Instance.CamInGameView();
            GameFlowManager.Instance.GameStart();
        }
        else
        {
            UIManager.Instance.popupManager.NoInternetPopupOpen(true);
        }
    }
    public void GetBtnFlag()
    {
        animator.Play("IdleToFlag");
    }
    public void GetBtnFlagToHome()
    {
        animator.Play("FlagToIdle");
    }

    public void GetBtnShip()
    {
        shipUIManager.OpenInit();
        animator.Play("IdleToShip");

    }
    public void GetBtnShipToHome()
    {
        shipUIManager.Close();
        animator.Play("ShipToIdle");
    }


}
