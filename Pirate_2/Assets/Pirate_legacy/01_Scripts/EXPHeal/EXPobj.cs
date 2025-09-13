using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class EXPobj : MonoBehaviour
{
    public ObjectMover objectMover;
    [SerializeField] int type;
    ShipMover ship;
    [SerializeField] SphereCollider col;

    void Init()
    {
        ship = null;
        if ( col == null ) col = this.gameObject.GetComponent<SphereCollider>();
    }

    public void BombAndGoInTotheWater( Transform _targetPos )
    {
        Init();
        objectMover.GoIntoTheWater(_targetPos, type);
    }
    public void AutoSpawned()
    {
        Init();
        OnTheWaterStart();
        this.transform.DOKill();
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);

        Animator _animator = this.gameObject.GetComponent<Animator>();  
        _animator.Play("AutoSpawned");

        GameObject _go = null;
        Vector3 pos = this.transform.position;
        pos.y = 0f;
        if (ObjectPoolManager.Instance.hitWater.TryGetNextObject(pos, Quaternion.identity, out _go))
        {
            _go.transform.eulerAngles = Vector3.zero;
            _go.GetComponent<ParticleSystem>().Play();
        }
    }
    private Coroutine KillCor;

    public void OnTheWaterStart()   // 바다에 빠진 후 부터 시작. 
    {
        if ( col == null ) col = this.gameObject.GetComponent<SphereCollider>();
        col.enabled = true;

        this.transform.parent = EXPTransManager.Instance.EXPParent;

        if (KillCor != null)
        {
            StopCoroutine(KillCor);
        }

        if ( GameDataManager.Instance.isTutorialMode == false ) 
        {
            KillCor = StartCoroutine(AutoKill());
        }
        else
        {
            Animator _animator = this.gameObject.GetComponent<Animator>();
            if ( _animator != null )
            {
                int _rdm = Random.Range(0, 3);
                string _name = "Idle_" + _rdm;
                _animator.Play(_name);
            }
        }
    }

    IEnumerator AutoKill()
    {
        Animator _animator = this.gameObject.GetComponent<Animator>();
        if ( _animator != null )
        {
            int _rdm = Random.Range(0, 3);
            string _name = "Idle_" + _rdm;
            _animator.Play(_name);
        }

        yield return new WaitForSeconds(7f);

        this.gameObject.transform.DOKill();
        this.gameObject.transform.DOLocalMoveY(-1.5f, 5f);

        yield return new WaitForSeconds(5f);
        this.gameObject.SetActive(false);
        yield break;
    }


    private void OnTriggerEnter(Collider other) 
    {
        if ( ship != null ) return;
        
        if (KillCor != null)
        {
            StopCoroutine(KillCor);
        }

        if ( objectMover == null ) objectMover = this.gameObject.AddComponent<ObjectMover>();
        if (other.gameObject.tag == "Ship")
        {
            ship = other.gameObject.gameObject.GetComponent<ShipMover>();
            objectMover.GetByShip(ship, type);
            col.enabled = false;
        }
    }
}
