using System.Collections;
using System.Collections.Generic;
using PirateConfig.Data;
using UnityEngine;

public class CannonLauncher : MonoBehaviour
{
    float cooltimeFull;
    float cooltime = 0f;
    int damage;
    [SerializeField] bool isNearEnemy = false;
    [SerializeField] ShipMover myShip;
    [SerializeField] Transform cannonBodyPos, firePos;
    [SerializeField] GameObject targetShipCol;
    [SerializeField] private List<GameObject> enemiesInRange = new List<GameObject>();
    [SerializeField] ParticleSystem firePtcl;
    [SerializeField] BoxCollider myBoxCol;

    [SerializeField] private float maxDistanceFromPlayer = 100f; // 플레이어로부터의 최대 거리
    float firePower, fireY;
    public void CannonInit(ShipMover _parentShip, int _damage)
    {
        myShip = _parentShip;

        if ( myBoxCol == null ) myBoxCol = this.gameObject.GetComponent<BoxCollider>();
        float _sizeX = ((float) myShip.level * 0.6f) + 8.5f;
        myBoxCol.size = new Vector3 ( _sizeX, 0.6f, 3.7f);
        myBoxCol.center = new Vector3 ( (_sizeX * 0.5f ) * -1f, 0f, 1f);

        cooltimeFull = ShipModelData.Instance.shipData[myShip.shipModelIndex].reloadTime;
        if ( myShip.isPlayer ) cooltimeFull -= PlayerDataManager.Instance.shipResloadAdded;
        cooltime = cooltimeFull;

        isNearEnemy = false;
        damage = _damage;
        if ( myShip.isPlayer ) 
        {
            damage += PlayerDataManager.Instance.shipCannonDMGAdded;
            if ( GameDataManager.Instance.playCount < 3 ) damage ++;
            if ( GameDataManager.Instance.playCount < 6 ) damage ++;
        }

        firePower = ShipData.DefaultStat.CannonFirePower + ((float) myShip.level * 2f);
        fireY = (float) myShip.level * 0.03f;
        fireY = Mathf.Clamp(fireY, 0f, 0.2f);
    }

    public void CannonMAXLevelDamageUP()
    {
        float dmgAdded = (float) damage * 5/100;
        int dmgAdd = Mathf.RoundToInt(dmgAdded);

        damage += dmgAdd;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ship" && other.gameObject != myShip && !enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Add(other.gameObject);

            if (!isNearEnemy)
            {
                targetShipCol = other.gameObject;
                isNearEnemy = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ship" && other.gameObject != myShip)
        {
            enemiesInRange.Remove(other.gameObject);

            if (other.gameObject == targetShipCol)
            {
                targetShipCol = null;
                isNearEnemy = false;

                if (enemiesInRange.Count > 0)
                {
                    targetShipCol = GetClosestEnemy();
                    isNearEnemy = true;
                }
            }
        }
    }

    private GameObject GetClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy.GetComponent<ShipMover>().isLive == false) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    void CannonFire()
    {
        if (targetShipCol == null || targetShipCol.GetComponent<ShipMover>().isLive == false)
        {
            enemiesInRange.Remove(targetShipCol);
            isNearEnemy = false;

            targetShipCol = GetClosestEnemy();

            return;
        }

        GameObject _go = null;
        if (ObjectPoolManager.Instance.canonProjectile.TryGetNextObject(firePos.position, Quaternion.identity, out _go))
        {
            _go.transform.eulerAngles = firePos.eulerAngles;

            Vector3 _targetPos = targetShipCol.transform.position;
            _targetPos.y += 2f;

            // 방향을 약간 위쪽으로 조정
            Vector3 _dir = (_targetPos - cannonBodyPos.position).normalized;
            _dir.y += fireY; // 약간 위쪽으로 조정

            _go.GetComponent<CannonProjectile>().Fire(_dir, firePower, damage, myShip.gameObject);

            firePtcl.Play();
        }
        SoundManager.Instance.PlayFire();
        if ( myShip.isPlayer )
        {
            VibrationManager.Instance.VivrateOnece();
        }
    }
    public T FindComponentInParent<T>() where T : Component
    {
        // 현재 객체의 부모부터 루트까지 탐색
        Transform currentTransform = transform;

        while (currentTransform != null)
        {
            T component = currentTransform.GetComponent<T>();

            if (component != null)
            {
                return component; // 원하는 컴포넌트를 찾으면 반환
            }

            // 부모로 올라가기
            currentTransform = currentTransform.parent;
        }

        // 부모 체인에 해당 컴포넌트가 없으면 null 반환
        return null;
    }

    private void Update()
    {
        if ( GameFlowManager.Instance != null )
        {
            if (GameFlowManager.Instance.isGameStart == false) return;
        } 
        if ( myShip != null )
        {
            if (myShip.isLive == false) return;
        }
        else
        {
            myShip = FindComponentInParent<ShipMover>();
        }

               if (cooltime > 0f)
        {
            cooltime -= Time.deltaTime;
        }
        else if (isNearEnemy)
        {
            CannonFire();
            cooltime = cooltimeFull;
        }
    }
}
