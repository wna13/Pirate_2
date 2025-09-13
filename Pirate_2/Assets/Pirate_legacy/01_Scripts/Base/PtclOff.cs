using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PtclOff : MonoBehaviour
{
    ParticleSystem particle;
    [SerializeField] float duration;
    private void OnEnable() 
    {
        CancelInvoke("End");
        Invoke("End", duration);
    }

    void End()
    {
        this.gameObject.SetActive(false);
    }
}
