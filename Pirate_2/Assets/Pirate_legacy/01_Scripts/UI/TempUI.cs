using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempUI : MonoBehaviour
{
    public void GetBtnGameStart()
    {
        GameFlowManager.Instance.GameStart();
    }

    public void Continue()
    {
        GameFlowManager.Instance.GameContinue();
    }


    public void ContinueTEST()
    {
        GameFlowManager.Instance.GameRestart();
        // ShipManager.Instance.TESTSPAWN();
    }
    private void Update() {
        if ( Input.GetKeyDown(KeyCode.Q))
        {
            GetDistance();
        }
    }
    public Transform tA, tB;
    public void GetDistance()
    {
        float distanceToPlayer = Vector3.Distance(tA.position, tB.position);
        Debug.Log("Distance = " + distanceToPlayer);

    }
}
