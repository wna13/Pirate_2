using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipColider : MonoBehaviour
{
    GameObject myShipParent;
    private void Start() 
    {
        myShipParent = this.gameObject.transform.parent.gameObject;
        isNearEnemy = false;
    }

    public bool isNearEnemy;
    private void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.tag == "Ship" && this.gameObject != myShipParent && isNearEnemy == false )
        {
            isNearEnemy = true;
            Debug.Log("TrigerENTER");
        }    
    }

    private void OnTriggerExit(Collider other) 
    {
        isNearEnemy = false;
    }
}
