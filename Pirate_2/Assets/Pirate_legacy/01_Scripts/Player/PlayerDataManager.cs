using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat;
using CodeStage.AntiCheat.ObscuredTypes;
using JetBrains.Annotations;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    public int playerShipSelectedIndex = 0;
    public int shipHPLevel, shipCannonDMGLevel, shipCannonReloadLevel;
    public int shipHPAdded, shipCannonDMGAdded;
    public float shipResloadAdded;
    public int coin;
    public int[] shipUpgradePrice;
    void Start()
    {
        Instance = this;
        DataLoad();
    }

    void DataLoad()
    {
        playerShipSelectedIndex = ObscuredPrefs.GetInt("playerShipSelectedIndex", 0);
        coin = ObscuredPrefs.GetInt("coin", 10);

        shipHPLevel = ObscuredPrefs.GetInt("shipHPLevel", 0);
        shipCannonDMGLevel = ObscuredPrefs.GetInt("shipCannonDMGLevel", 0);
        shipCannonReloadLevel = ObscuredPrefs.GetInt("shipCannonReloadLevel", 0);

        CannomDmgData();
        shipResloadAddedData();
        ShipHPData();

    }
    public void HPLevelUpgrade()
    {
        shipHPLevel++;
        ObscuredPrefs.SetInt("shipHPLevel", shipHPLevel);
        ShipHPData();
    }
    void ShipHPData()
    {
        shipHPAdded = shipHPLevel * 5;
        if ( PlayerMover.Instance != null) PlayerMover.Instance.shipMover.PlayerHPUpdate();
    }
    public void CannonDMGUpgrade()
    {
        shipCannonDMGLevel++;
        ObscuredPrefs.SetInt("shipCannonDMGLevel", shipCannonDMGLevel);
        CannomDmgData();
    }
    void CannomDmgData()
    {
        shipCannonDMGAdded = shipCannonDMGLevel;
        if ( PlayerMover.Instance != null) PlayerMover.Instance.shipMover.PlayerDMGUpdate();
    }
    public void CannonReloadUpgrade()
    {
        shipCannonReloadLevel++;
        ObscuredPrefs.SetInt("shipCannonReloadLevel", shipCannonReloadLevel);
        shipResloadAddedData();
    }
    void shipResloadAddedData()
    {
        shipResloadAdded = (float) shipCannonReloadLevel * 0.1f;
    }

    public void CoinvalueChange ( int _value )
    {
        coin += _value;
        ObscuredPrefs.SetInt("coin", coin);
    }

}
