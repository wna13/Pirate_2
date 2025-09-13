using System.Collections;
using PirateConfig.Data;
using UnityEngine;
using DG.Tweening;

public class ShipMover : MonoBehaviour
{
    [SerializeField] BoxCollider myBoxCol;
    public bool isPlayer;
    public bool isLive;
    bool isSpeedMode = false;
    public int level, shipModelIndex, damage;
    float moveSpeed = 0f;
    [SerializeField] float maxMoveSpeed = 8;
    [SerializeField] Rigidbody rb;
    public Transform tilt; // 기울기 조절
    public bool isMoving;

    [SerializeField] float maxTiltAngle = 30f; // 최대 기울어질 각도
    [SerializeField] float turnSpeed = 8f, tiltSpeed = 8f, tileMultiply = 100;
    HPPoint hpPoint;
    public ShipUI shipUI;
    GameObject shipModel;

    public int flagIndex;
    bool isJustSpawned;
    public void Init(int _startLevel, int _modelIndex, int _flagIndex)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (hpPoint == null) hpPoint = GetComponent<HPPoint>();
        if (myBoxCol == null) myBoxCol = GetComponent<BoxCollider>();

        Vector3 _colSize = new Vector3 ( myBoxCol.size.x, myBoxCol.size.y,  ShipData.DefaultStat.ColZsize[_startLevel]);
        myBoxCol.size = _colSize;
        
        flagIndex = _flagIndex;

        isPlayer = GetComponent<PlayerMover>();
        isLive = true;
        moveSpeed = 0;

        level = _startLevel;
        if ( GameFlowManager.Instance != null )
        {
            if ( GameFlowManager.Instance.isPlayerDeadOnce ) level ++;
        } 

        shipModelIndex = _modelIndex;
        
        if ( isPlayer )
        {
            damage = ShipModelData.Instance.shipData[shipModelIndex].defaultDMG + PlayerDataManager.Instance.shipCannonDMGAdded;
        }
        else
        {
            damage = ShipModelData.Instance.shipData[_modelIndex].defaultDMG + BalanceFactorManager.Instance.Damage();
            if ( GameDataManager.Instance.playCount < 6 ) damage = 6;
            if ( GameDataManager.Instance.playCount < 3 ) damage = 5;
        }
        if ( GameDataManager.Instance.isTutorialMode ) 
        {
            damage = isPlayer ? 8 : 1;
        }

        maxMoveSpeed = ShipData.DefaultStat.DefaultMaxSpeed;
        turnSpeed = ShipModelData.Instance.shipData[shipModelIndex].controllFactor;
        myBoxCol.enabled = true;

        ModelSpawn();

        PlayerHPUpdate();
        shipUI.UIInit(this);

        isJustSpawned = true;
        Invoke("Spawned", 0.2f);

    }
    void Spawned()
    {
        isJustSpawned = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ( isJustSpawned == false ) return;
        if (other.gameObject.CompareTag("Island") )
        {
            // 죽음 처리 로직
            myBoxCol.enabled = false;
            this.transform.DOKill();
            this.transform.localScale = Vector3.zero;
            this.gameObject.SetActive(false);

            if ( ShipManager.Instance != null )
            {
                ShipManager.Instance.RemoveShip(transform);
            }
        }
    }
    
    public void PlayerDMGUpdate()
    {
        damage = ShipModelData.Instance.shipData[shipModelIndex].defaultDMG + PlayerDataManager.Instance.shipCannonDMGAdded;
    }
    public void PlayerHPUpdate()
    {
        hpPoint.HPInitSetting(this);
    }

 
    void ModelSpawn( )
    {
        GameObject _go = Instantiate(ShipModelData.Instance.GetShipModel(shipModelIndex, level));

        if (_go != null)
        {
            if (shipModel != null) 
            {
                GameObject _destroy = shipModel;
                Destroy(_destroy);
                shipModel = null;
            }

            shipModel = _go;
            shipModel.transform.SetParent( tilt.transform );
            shipModel.transform.localPosition = Vector3.zero;
            shipModel.transform.localEulerAngles = Vector3.zero;
            shipModel.transform.localScale = Vector3.zero;
            shipModel.transform.DOKill();
            shipModel.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetAutoKill(true);

            ShipModelBase shipModelBase = shipModel.GetComponent<ShipModelBase>();
            if ( shipModelBase != null )
            {
                shipModelBase.CannonInit(this, damage);
                shipModelBase.SailInit(this);
                shipUI.transform.localPosition = new Vector3 (0f, shipModelBase.uiPos.localPosition.y , 0f);    
                shipModelBase.SpawnPtclPlay();
            }
        }
        if ( isPlayer )
        {
            Invoke("InvokeCamMove", 0.3f);
        }
    }
    public void ChangeFlag()
    {
        this.transform.DOKill();
        this.transform.localScale = Vector3.one * 1.1f;
        this.transform.DOScale(1f, 0.3f);
        flagIndex = ShipFlagData.Instance.playerEquippedFlagIndex;
        ShipModelBase shipModelBase = shipModel.GetComponent<ShipModelBase>();
        shipModelBase.SailChange();
    }

    void InvokeCamMove()
    {
        if ( CameraMover.Instance != null )
            CameraMover.Instance.FovChange();
    }

    public void GetDamage(int _damage)
    {
        if ( isLive == false ) return;
        SoundManager.Instance.PlayHit();

        if ( isPlayer ) 
        {
            UIManager.Instance.HitUIEffect();
            VibrationManager.Instance.VivrateOnece();
        }
        else
        {
        }

        if (!hpPoint.GetDamage(_damage))
        {
            // 죽음 처리 로직 추가
            isLive = false;
            Die();
        }
        else
        {
            shipUI.HPBarValueChange(hpPoint.fullHP, hpPoint.currentHP);
        }
    }

    void Die()
    {
        // 죽음 처리 로직
        myBoxCol.enabled = false;
        transform.DOScale(0f, 0.3f).OnComplete(() => gameObject.SetActive(false));

        GameObject _go = null;
        if (ObjectPoolManager.Instance.deadEXP.TryGetNextObject(transform.position, Quaternion.identity, out _go))
        {
            _go.transform.eulerAngles = transform.eulerAngles;
            _go.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            _go.GetComponent<EXPbomb>().EXPBombExplosionStart(level);
        }

        if ( ShipManager.Instance != null )
        {
            ShipManager.Instance.RemoveShip(transform);
        }
        if ( TutorialManager.Instance != null )
        {
            if ( isPlayer == false )
            {
                TutorialManager.Instance.EnemyKill();
            }
        }
        if (isPlayer)
        {
            if ( GameFlowManager.Instance != null )
            {
                GameFlowManager.Instance.PlayerDead();
            }
        } 
    }

    public void PlayerMove(Vector3 _dir)
    {
        if ( GameDataManager.Instance.isTutorialMode == false )
        {
            if (!isLive ) return;
            if ( GameFlowManager.Instance != null ) 
            {
                if ( GameFlowManager.Instance.isGameStart == false ) return;
            }
        }
        else
        {
            if ( TutorialManager.Instance != null )
            {
                if ( TutorialManager.Instance.tutorialStep == 0 )
                {
                    TutorialManager.Instance.FirstMove();
                }
            }
        }

        if (!isMoving) isMoving = true;

        // 입력 벡터가 유효한지 검사
        if (_dir == Vector3.zero) return;

        moveSpeed = Mathf.Clamp(moveSpeed + 0.1f, 0f, maxMoveSpeed);
        rb.linearVelocity = transform.forward * moveSpeed;

        Vector3 inputDir = new Vector3(_dir.x, 0f, _dir.y).normalized;

        if (inputDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        float tiltAngle = Mathf.Clamp(localVelocity.x * tileMultiply, -maxTiltAngle, maxTiltAngle);
        Quaternion targetTiltRotation = Quaternion.Euler(0f, 0f, tiltAngle);
        tilt.localRotation = Quaternion.Lerp(tilt.localRotation, targetTiltRotation, Time.deltaTime * tiltSpeed);
    }

    public void PlayerStop()
    {
        if (isMoving) isMoving = false;
        if ( isLive == false ) 
        {
            moveSpeed = 0f;
            rb.linearVelocity = Vector3.zero;
            return;
        }
        StopCoroutine(Break());
        StartCoroutine(Break());
    }

    private void FixedUpdate()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    IEnumerator Break()
    {
        while (!isMoving)
        {
            if (!isLive) yield break;

            moveSpeed = Mathf.Max(0f, moveSpeed - 0.03f);
            rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;

            tilt.localRotation = Quaternion.Lerp(tilt.localRotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * tiltSpeed);

            if (moveSpeed <= 0.01f && Quaternion.Angle(tilt.localRotation, Quaternion.Euler(0f, 0f, 0f)) < 0.01f)
            {
                moveSpeed = 0f;
                rb.linearVelocity = Vector3.zero;
                tilt.localRotation = Quaternion.Euler(0f, 0f, 0f);
                yield break;
            }

            yield return null;
        }
    }

    public void GetEXP(int _EXP)
    {
        shipUI.EXPValueChange(_EXP);
    }
    public void GetHP()
    {
        hpPoint.GetHealObj();
    }

    public void ShipLevelUP()
    {
        level++;
        if ( level < ShipModelData.Instance.shipData[shipModelIndex].model.Length )
        {
            ModelSpawn();
        }
        hpPoint.HPRefreshByLevelUP();
        if (isPlayer)
        {
            if ( GameFlowManager.Instance != null )
            {
                GameFlowManager.Instance.InGamePlayerLevelUP();
            }
            if ( GameDataManager.Instance.isTutorialMode )
            {
                if ( TutorialManager.Instance != null )
                {
                    TutorialManager.Instance.TutoLevelUP();
                }
            }
        }
    }
    public void SHipLevelUPMAX()
    {
        hpPoint.HPMaxUP();
        shipModel.GetComponent<ShipModelBase>().MAXLevelCannonDMGUP();

    }
}
