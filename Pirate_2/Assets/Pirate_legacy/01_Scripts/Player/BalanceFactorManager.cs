using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class BalanceFactorManager : MonoBehaviour
{
    public static BalanceFactorManager Instance;
    public int balanceFactor;
    void Start()
    {
        Instance = this;
        DataLoad();
    }

    void DataLoad()
    {
        balanceFactor = ObscuredPrefs.GetInt("balanceFactor", -4);
    }

    public void BalanceFactorchange( int _value )
    {
        balanceFactor += _value;

        int playCount = GameDataManager.Instance.playCount;

        if ( playCount < 5 ) 
        {
            balanceFactor = Mathf.Clamp(balanceFactor, -4, 0);
        }
        if (playCount >= 5 && playCount < 10 ) 
        {
            balanceFactor = Mathf.Clamp(balanceFactor, -4, 1);
        }        
        if (playCount >= 10 && playCount < 14 ) 
        {
            balanceFactor = Mathf.Clamp(balanceFactor, -4, 2);
        }
        if ( playCount >= 14 ) 
        {
            balanceFactor = Mathf.Clamp(balanceFactor, -3, 3);
        }
        ObscuredPrefs.SetInt("balanceFactor", balanceFactor);
    }
    
    public int Damage()
    {
        List<int> listing = new List<int>();

        for  (int i = balanceFactor - 2; i < balanceFactor + 3; i ++ )
        {
            listing.Add(i);
        }
        int _result = 0;
        int rdm = Random.Range(0, 12);
        if ( rdm < 4 ) _result = listing[0];
        if ( rdm >= 4 && rdm < 7 ) _result = listing[1];
        if ( rdm >= 7 && rdm < 9 ) _result = listing[2];
        if ( rdm >= 9 && rdm < 11 ) _result = listing[3];
        if ( rdm >= 11 ) _result = listing[4];

        _result = Mathf.Clamp(_result, -6, 5);

        return _result;
    }
    public int ShipLevelInFirstTime()
    {
        int _result = 0;
        
        int _rdm = Random.Range(0, 10);

        if  (_rdm < 6 ) _result = 0;
        else 
        {
            _result = Random.Range(0, ShipModelData.Instance.shipData[ShipModelData.Instance.playerEquippedShipIndex].level );
        }
        return _result; 
    }

    public int ShipLevelInGameing()
    {
        List<int> listing = new List<int>();

        for  (int i = balanceFactor - 4; i < balanceFactor + 1; i ++ )
        {
            listing.Add(i);
        }
        int _result = 0;
        int rdm = Random.Range(0, 12);
        if ( rdm < 3 ) _result = listing[0];
        if ( rdm >= 3 && rdm < 6 ) _result = listing[1];
        if ( rdm >= 6 && rdm < 9 ) _result = listing[2];
        if ( rdm >= 9 && rdm < 11 ) _result = listing[3];
        if ( rdm >= 11  ) _result = listing[4];

        _result = GameFlowManager.Instance.inGamePlayerLevel + _result;

        _result = Mathf.Clamp(_result, 0, 14);

        return _result;
    }

    public int HP()
    {
        List<int> listing = new List<int>();

        for  (int i = balanceFactor - 3; i < balanceFactor + 3; i ++ )
        {
            listing.Add(i);
        }
        int _result = 0;
        int rdm = Random.Range(0, 13);
        if ( rdm < 4 ) _result = listing[0];
        if ( rdm >= 4 && rdm < 6 ) _result = listing[1];
        if ( rdm >= 6 && rdm < 9 ) _result = listing[2];
        if ( rdm >= 9 && rdm < 11 ) _result = listing[3];
        if ( rdm >= 11 && rdm < 13 ) _result = listing[4];
        if ( rdm >= 13 ) _result = listing[5];

        _result *= 10;
        _result = Mathf.Clamp(_result, -30, 30);

        //나중에 레벨에 맞게 퍼센트로 바꿔야 될 수도?
        return _result;
    }

}
