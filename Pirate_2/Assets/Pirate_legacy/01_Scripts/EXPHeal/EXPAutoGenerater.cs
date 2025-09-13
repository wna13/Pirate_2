using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPAutoGenerater : MonoBehaviour
{
    private void Start() 
    {
        StopAllCoroutines();
        StartCoroutine(AutoEXPStart());
    }

    IEnumerator AutoEXPStart()
    {
        yield return new WaitForSeconds( 2f );

        while ( true )
        {
            if ( GameFlowManager.Instance != null )
            {
                if ( GameFlowManager.Instance.isGameStart)
                {
                    RandomEXPSpawn();
                    float _delay = Random.Range ( 1.5f, 4f);
                    yield return new WaitForSeconds(_delay );
                }
            }
            if ( this.gameObject.activeSelf == false ) yield break;
            yield return null;
        }
    }


    void RandomEXPSpawn()
    {
        int _randomPos = Random.Range(0, this.transform.childCount);

        GameObject _go = null;
        if (ObjectPoolManager.Instance.expBox.TryGetNextObject(this.transform.position, Quaternion.identity, out _go))
        {
            EXPobj _exp = _go.GetComponent<EXPobj>();
            _go.transform.parent = this.transform;
            _go.transform.position = this.transform.GetChild(_randomPos).transform.position;
            _exp.AutoSpawned();
        }
    }
}
