using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    // public float fHeight;
    ShipMover myShip;

    int type; // 0 - EXP, 1 - HP , 2 - Coin

    private Coroutine moveCoroutine;

    public void GetByShip(ShipMover _ship, int _type )
    {
        myShip = _ship;
        type = _type;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToShip(_ship.transform));

        GameObject _go = null;
        Vector3 pos = this.transform.position;
        pos.y = 0f;
        if (ObjectPoolManager.Instance.hitWater.TryGetNextObject(pos, Quaternion.identity, out _go))
        {
            _go.transform.eulerAngles = Vector3.zero;
            _go.GetComponent<ParticleSystem>().Play();
        }

        if ( myShip.isPlayer )
        {
            SoundManager.Instance.PlayEffect("get_exp");
        }
    }
    IEnumerator MoveToShip(Transform _trP )
    {
        // this.transform.parent = _trP;
        float startTime = Time.time;

        float reduceHeight = 0.1f;
        float journeyTime = 0.3f;

        this.transform.DOKill();
        this.transform.localScale = Vector3.one * 1.1f;
        this.transform.DOScale(0f, 0.2f).SetDelay(0.1f);

        Vector3 _startPos = this.transform.localPosition;
        Vector3 _target = _trP.position;

        float _duration = 0.2f;

        while ( true )
        {
            _duration -= Time.deltaTime;
            if ( _duration < 0f ) 
            {
                Function();
                this.gameObject.SetActive(false);
                yield break;
            }

            Vector3 center = (_startPos + _target) * 0.5F; //Center 값만큼 위로 올라간다.
            center -= new Vector3(0, 1f * reduceHeight, 0); //y값을 높이면 높이가 낮아진다.
            Vector3 riseRelCenter = _startPos - center;
            Vector3 setRelCenter = _target - center;
            float fracComplete = (Time.time - startTime) / journeyTime;
            transform.localPosition = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.localPosition += center;

            yield return null;
        }
    }
    void Function()
    {
        if ( myShip.isPlayer ) 
        {
            VibrationManager.Instance.VivrateOnece();
        }
        if (type == 0) // EXP
        {
            myShip.GetEXP(1);
        }
        if ( type == 1) // heal
        {
            myShip.GetHP();
        }
        if ( type == 2 )
        {
            if ( myShip.isPlayer == false ) return;
            if ( GameDataManager.Instance.isTutorialMode ) return;
            
            int _coinValue = 10;
            UIManager.Instance.coinUI.GoodsValueChange(_coinValue);
            GameFlowManager.Instance.GetCoinThisGameValue(_coinValue);
        }
    }

    public void GoIntoTheWater( Transform _targetPos, int _type )   // 0  = exp
    {
        type = _type;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveStartsdCor( _targetPos ));
    }

    IEnumerator MoveStartsdCor(Transform _trP )
    {
        this.transform.SetParent( _trP);
        float startTime = Time.time;

        float reduceHeight = 0.1f;
        float journeyTime = 0.3f;

        this.transform.DOKill();
        this.transform.localScale = Vector3.one * 0.1f;
        this.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetDelay(0.1f);

        Vector3 _startPos = this.transform.localPosition;
        Vector3 _target = Vector3.zero;



        while ( true )
        {
            if ( this.transform.position.y < 0.1f ) 
            {
                //바다 안착.
                EXPobj expObj = this.gameObject.GetComponent<EXPobj>();
                expObj.OnTheWaterStart();

                GameObject _go = null;
                Vector3 pos = this.transform.position;
                pos.y = 0f;
                if (ObjectPoolManager.Instance.hitWater.TryGetNextObject(pos, Quaternion.identity, out _go))
                {
                    _go.transform.eulerAngles = Vector3.zero;
                    _go.GetComponent<ParticleSystem>().Play();
                }
                this.transform.SetParent(EXPTransManager.Instance.transform);
                yield break;
            }

            Vector3 center = (_startPos + _target) * 0.5F; //Center 값만큼 위로 올라간다.
            center -= new Vector3(0, 1f * reduceHeight, 0); //y값을 높이면 높이가 낮아진다.
            Vector3 riseRelCenter = _startPos - center;
            Vector3 setRelCenter = _target - center;
            float fracComplete = (Time.time - startTime) / journeyTime;
            transform.localPosition = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.localPosition += center;

            yield return null;
        }
    }
}
