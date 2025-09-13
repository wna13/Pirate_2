using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAutoGenerater : MonoBehaviour
{
    private void Start() 
    {
        StopAllCoroutines();
        StartCoroutine(AutoHealStart());
    }

    IEnumerator AutoHealStart()
    {
        yield return new WaitForSeconds( 2.5f );

        float _delay = 3.5f;
        
        while ( true )
        {
            if ( GameFlowManager.Instance != null )
            {
                if ( GameFlowManager.Instance.isGameStart)
                {
                    RandomHealSpawn();
                    yield return new WaitForSeconds(_delay );
                    _delay += 0.3f;
                }
            }
            if ( this.gameObject.activeSelf == false ) yield break;
            yield return null;
        }
    }
    private void Update() {
        if ( Input.GetKeyDown(KeyCode.H))
        {
            RandomHealSpawn();
        }
    }

    void RandomHealSpawn()
    {
        int _randomPos = Random.Range(0, this.transform.childCount);

        GameObject _go = null;
        if (ObjectPoolManager.Instance.healBox.TryGetNextObject(this.transform.position, Quaternion.identity, out _go))
        {
            HealObj _heal = _go.GetComponent<HealObj>();
            _go.transform.parent = this.transform;
            _go.transform.position = this.transform.GetChild(_randomPos).transform.position;
            _heal.AutoSpawned();
        }
    }
}
