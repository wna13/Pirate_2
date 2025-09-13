using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoShipManager : MonoBehaviour
{
    [SerializeField] GameObject playerShip, enemyShip;
    public Transform allShipsParent;
    [SerializeField] Transform trEnemySpawnPos, trPlayerSpawnPos;
    GameObject SpawnedPlayer;
    public List<Transform> ships = new List<Transform>(); // 모든 배의 Transform을 관리하는 리스트

    public void PlayerSpawn()
    {

        GameObject _ship = Instantiate(playerShip, allShipsParent);
        _ship.transform.position = trPlayerSpawnPos.transform.position;
        _ship.transform.eulerAngles = new Vector3(0f, 145f, 0f);
        _ship.transform.localScale = Vector3.one;
        ShipMover _shipMover = _ship.GetComponent<ShipMover>();
        SpawnedPlayer = _ship;

        if (_shipMover)
        {
            _shipMover.Init(2, 0,  0);
            AddShip(_ship.transform);
        }
        else
        {
            Debug.LogError("PlayerShipMover is null!");
        }
    }
    public void EnemyShipSpawn()
    {
        GameObject _ship = Instantiate(enemyShip, allShipsParent);
        _ship.transform.position = trEnemySpawnPos.position;
        _ship.transform.eulerAngles = Vector3.zero;
        _ship.transform.localScale = Vector3.one;

        ShipMover _shipMover = _ship.GetComponent<ShipMover>();

        int _level = 0;

        if (_shipMover)
        {
            _shipMover.Init(_level, 0, 1);
            AddShip(_ship.transform);
        }
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
