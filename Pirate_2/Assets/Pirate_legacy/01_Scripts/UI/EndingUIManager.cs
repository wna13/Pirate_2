using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingUIManager : MonoBehaviour
{
    public static EndingUIManager Instance;

    void Start()
    {
        Instance = this;
    }

    [SerializeField] ContinueUIManager continueUIManager;
    [SerializeField] ResultUImanager resultUImanager;

    public void ContinueUION()
    {
        continueUIManager.gameObject.SetActive(true);
        UIManager.Instance.coinUI.CanvasGroupON(true);
        SoundManager.Instance.PlayEffect("Result");
    }
    public void ResultUION()
    {
        resultUImanager.gameObject.SetActive(true);
        resultUImanager.Init();
        UIManager.Instance.coinUI.CanvasGroupON(true);
        SoundManager.Instance.PlayEffect("popupOpen");

    }


}
