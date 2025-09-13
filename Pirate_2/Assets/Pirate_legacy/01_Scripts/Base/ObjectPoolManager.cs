using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public EZObjectPools.EZObjectPool canonProjectile;
    public EZObjectPools.EZObjectPool hitWater;
    public EZObjectPools.EZObjectPool hit;
    public EZObjectPools.EZObjectPool expBox;
    public EZObjectPools.EZObjectPool healBox;
    public EZObjectPools.EZObjectPool healPtcl;
    public EZObjectPools.EZObjectPool deadEXP;
    public EZObjectPools.EZObjectPool coinBox;

    private void OnEnable() {
        Instance = this;
    }
    

}
