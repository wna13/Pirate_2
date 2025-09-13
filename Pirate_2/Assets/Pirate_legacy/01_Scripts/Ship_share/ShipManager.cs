using System.Collections;
using System.Collections.Generic;
using PirateConfig.Data;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;
    public List<Transform> ships = new List<Transform>(); // 모든 배의 Transform을 관리하는 리스트

    [SerializeField] Transform trSpawnParent, trPlayerSpawnPos;
    List<Transform> enemySpawnPos = new List<Transform>();
    [SerializeField] GameObject playerShip, enemyShip;
    public Transform allShipsParent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //스폰포스 불러오기
        for (int i = 0; i < trSpawnParent.childCount; i++)
        {
            enemySpawnPos.Add(trSpawnParent.GetChild(i).transform);
        }
    }

    public void ShipRemoveAllAndInit()
    {
        for (int i = ships.Count - 1; i >= 0; i--)
        {
            Destroy(ships[i].gameObject);
        }
        ships.Clear();
    }
    bool isFirstSpawn;
    // 초기 스폰
    public void SpawnManyEnemyShip(int _count, bool _isFirstStart)
    {
        isFirstSpawn = _isFirstStart;
        //플레이어 스폰
        PlayerSpawn(_isFirstStart);
        
        //적 스폰
        if (_count > enemySpawnPos.Count) _count = enemySpawnPos.Count;
        
        // 리스트를 복사하여 원본 리스트를 변경하지 않도록 함
        List<Transform> tempList = new List<Transform>(enemySpawnPos);

        // Fisher-Yates Shuffle 알고리즘으로 리스트 섞기
        for (int i = tempList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = tempList[i];
            tempList[i] = tempList[randomIndex];
            tempList[randomIndex] = temp;
        }


        for (int i = 0; i < _count; i++)
        {
            EnemyShipSpawn(tempList[i], true);
        }
        isFirstSpawn = false;
    }
    private Coroutine AutoSpawnCor;

    public void EnemyAutoSpawn()
    {
        if (AutoSpawnCor != null)
        {
            StopCoroutine(AutoSpawnCor);
        }
        AutoSpawnCor = StartCoroutine(AutoSpawnCorStart());
    }

    IEnumerator AutoSpawnCorStart()    //나중에 고도화. 지금은 일단 테스트. 6초에 하나씩 계속 보냄
    {
        yield return new WaitForSeconds(3f);

        int enemyMaxCount ;

        while (PlayerMover.Instance.shipMover.isLive)
        {
            enemyMaxCount = 3 + BalanceFactorManager.Instance.balanceFactor;
            enemyMaxCount = Mathf.Clamp(enemyMaxCount, 3, 5);    
            if ( GameDataManager.Instance.playCount < 4 ) enemyMaxCount = Mathf.Clamp(enemyMaxCount, 3, 4 ); 

            if (ships.Count < enemyMaxCount)
            {
                int _r = Random.Range(0, enemySpawnPos.Count);
                EnemyShipSpawn(enemySpawnPos[_r], false);

                yield return new WaitForSeconds(3f);
            }
            else
            {
                yield return new WaitForSeconds(3f);
            }
            if (!GameFlowManager.Instance.isGameStart) yield break;
            if (!PlayerMover.Instance.shipMover.isLive) yield break;
            yield return null;
        }
    }

    GameObject SpawnedPlayer;
    public void PlayerSpawn(bool _isFirstSpawn)
    {
        if (SpawnedPlayer != null)
        {
            ships.Remove(SpawnedPlayer.transform);
            Destroy(SpawnedPlayer);
            SpawnedPlayer = null;
        }

        GameObject _ship = Instantiate(playerShip, allShipsParent);
        _ship.transform.position = trPlayerSpawnPos.transform.position;
        _ship.transform.eulerAngles = new Vector3(0f, 145f, 0f);
        _ship.transform.localScale = Vector3.one;
        ShipMover _shipMover = _ship.GetComponent<ShipMover>();
        SpawnedPlayer = _ship;

        if (_shipMover)
        {
            if (_isFirstSpawn == false )
            {
                _shipMover.Init(GameFlowManager.Instance.inGamePlayerLevel, ShipModelData.Instance.playerEquippedShipIndex,  ShipFlagData.Instance.playerEquippedFlagIndex);
            }
            else
            {
                ShipModelData.ShipData data = ShipModelData.Instance.shipData[ShipModelData.Instance.playerEquippedShipIndex];
                _shipMover.Init(data.level, ShipModelData.Instance.playerEquippedShipIndex, ShipFlagData.Instance.playerEquippedFlagIndex);
                GameFlowManager.Instance.inGamePlayerLevel = data.level;
            }
            AddShip(_ship.transform);
        }
        else
        {
            Debug.LogError("PlayerShipMover is null!");
        }
    }

    //적 스폰
    void EnemyShipSpawn(Transform _spawnPos, bool _isFirstSpawn)
    {
        GameObject _ship = Instantiate(enemyShip, allShipsParent);
        _ship.transform.position = _spawnPos.transform.position;
        _ship.transform.eulerAngles = Vector3.zero;
        _ship.transform.localScale = Vector3.one;

        ShipMover _shipMover = _ship.GetComponent<ShipMover>();

        int _level = 0;
        if ( _isFirstSpawn ) _level = BalanceFactorManager.Instance.ShipLevelInFirstTime();
        else _level = BalanceFactorManager.Instance.ShipLevelInGameing();

        if (_shipMover)
        {
            int _getmodelIndex = GetEnemyModelIndex();
            if ( _getmodelIndex >= ShipModelData.Instance.shipData.Count) _getmodelIndex = 0;
            if ( _getmodelIndex > 0 ) _level = 2;

            _shipMover.Init(_level, _getmodelIndex, GetEnemyFlagIndex());
            AddShip(_ship.transform);
        }
    }

    int GetEnemyModelIndex()
    {
        int rdm = Random.Range (0, 100);
        
        if ( isFirstSpawn )
        {
            if ( ShipModelData.Instance.shipData[ShipModelData.Instance.playerEquippedShipIndex].level >= 1)
            {
                if ( rdm < 75 ) return 0;
                if ( rdm < 85 ) return 1;
                if ( rdm < 93 ) return 2;
                if ( rdm < 99 ) return 3;
            }
            else
            {
                if ( rdm < 95 ) return 0;
                if ( rdm < 97 ) return 1;
                if ( rdm < 98 ) return 2;
                if ( rdm < 99 ) return 3;
            }
        }
        else
        {
            if ( ShipModelData.Instance.shipData[ShipModelData.Instance.playerEquippedShipIndex].level >= 1)
            {
                if ( rdm < 75 ) return 0;
                if ( rdm < 85 ) return 1;
                if ( rdm < 93 ) return 2;
                if ( rdm < 99 ) return 3;
            }
            else
            {
                if ( rdm < 94 ) return 0;
                if ( rdm < 96 ) return 1;
                if ( rdm < 97 ) return 2;
                if ( rdm < 99 ) return 3;
            }
        }
        return 0;
    }
    int GetEnemyFlagIndex()
    {
        int result = 0;
        int _random = Random.Range(0, 100);
        if ( _random < 30 ) result = 0;
        if ( _random >= 30 && _random < 85 ) result = Random.Range(1, 20);
        if ( _random >= 85 ) result = Random.Range(20, ShipFlagData.Instance.flagData.Count);
        return result;
    }

    // 배 추가
    public void AddShip(Transform ship)
    {
        ships.Add(ship);
    }

    // 배 제거
    public void RemoveShip(Transform ship)
    {
        if (ship != null)
        {
            ships.Remove(ship);
            ship.gameObject.SetActive(false);
        }
    }
}
