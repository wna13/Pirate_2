using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFalseSelf : MonoBehaviour
{
    public void SelfActiveFalse()
    {
        this.gameObject.SetActive(false);
    }
}
