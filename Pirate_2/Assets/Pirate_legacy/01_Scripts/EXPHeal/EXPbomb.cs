using System;
using System.Collections;
using System.Collections.Generic;
using PirateConfig.Data;
using UnityEngine;

public class EXPbomb : MonoBehaviour
{
    [SerializeField] Transform posParent;

    // private void OnEnable() 
    // {
    //     EXPBombExplosionStart(1);
    // }
    [SerializeField] ParticleSystem ptcl;
    public void EXPBombExplosionStart( int _shipLevel )
    { 
        ptcl.Play();
        int _coinBoxCount = Mathf.FloorToInt(_shipLevel / 3f);
        if ( _coinBoxCount < 1) _coinBoxCount = 1;

        for ( int i = 0 ; i < ShipData.DefaultStat.EXPRewardCountByLevel[_shipLevel] + _coinBoxCount; i ++ )
        {
            GameObject _go = null;

            if ( i < _coinBoxCount )
            {
                ObjectPoolManager.Instance.coinBox.TryGetNextObject(this.transform.position, Quaternion.identity, out _go);
            }
            else
            {
                ObjectPoolManager.Instance.expBox.TryGetNextObject(this.transform.position, Quaternion.identity, out _go);
            }
            if ( _go )
            {
                EXPobj _exp = _go.GetComponent<EXPobj>();
                _go.transform.parent = this.transform;
                _go.transform.localPosition = new Vector3 ( 0f, 0.3f, 0f);
                _exp.BombAndGoInTotheWater(posParent.GetChild(i).transform);
            }
        }
        Invoke("AutoKill", 1.5f);
    }

    void AutoKill()
    {
        this.gameObject.SetActive(false);
    }
}
