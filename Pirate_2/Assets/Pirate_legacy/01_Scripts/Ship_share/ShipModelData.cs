using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat;
using CodeStage.AntiCheat.ObscuredTypes;

public class ShipModelData : MonoBehaviour
{
    public static ShipModelData Instance;
    public int playerEquippedShipIndex;
    private void Start() 
    {
        Instance = this;    
        DataLoad();
    }

    void DataLoad()
    {
        //default
        string _defualt = "shipLv"+0;
        shipData[0].level = ObscuredPrefs.GetInt(_defualt, 0);

        for ( int i = 1; i < shipData.Count; i ++ )
        {
            string _count = "shipLv"+i;
            shipData[i].level = ObscuredPrefs.GetInt(_count, -1);
        }

        playerEquippedShipIndex = ObscuredPrefs.GetInt("playerEquippedShipIndex", 0);
    }
    public void PlayerEquipShipIndexReload()
    {
        for ( int i = 1; i < shipData.Count; i ++ )
        {
            string _count = "shipLv"+i;
            shipData[i].level = ObscuredPrefs.GetInt(_count, -1);
        }
        
        playerEquippedShipIndex = ObscuredPrefs.GetInt("playerEquippedShipIndex", 0);
    }
    public void ChangeEquippedShipIndex ( int _index )
    {
        playerEquippedShipIndex = _index;
        ObscuredPrefs.SetInt("playerEquippedShipIndex", playerEquippedShipIndex);
        
        if ( UIManager.Instance != null )
        {
            UIManager.Instance.startUIManager.shipUIManager.ButtonRefresh();
        }
        return;
    }

    public GameObject GetShipModel ( int _index, int _level )
    {
        if ( _index >= shipData.Count ) 
        {
            return null;
        }
        if ( _level >= shipData[_index].model.Length )
        {
            return null;
        }
        if ( _level >= shipData[_index].model.Length )
        {
            _level = shipData[_index].model.Length;
        }

        return shipData[_index].model[_level];
    }
    [System.Serializable]
    public class ShipData
    {
        public GameObject[] model;

        public enum BuyType
        {
            Default,
            Coin,
            IAP
        }

        [SerializeField]
        public BuyType buyType;
        public string iapID;
        public string iapPrice;
        public int coinPrice;
        public string name;
        public int level;
        public int levelMinimum;
        public int levelMAX;
        public int defaultHP;
        public int defaultDMG;
        public float reloadTime;
        public float controllFactor;
    }
    public List<ShipData> shipData = new List<ShipData>();
    public List<int> modelUpgradePrice = new List<int>();

    public void ShipUpgrade ( int _index )
    {
        shipData[_index].level ++;
        string _count = "shipLv"+_index;
        ObscuredPrefs.SetInt(_count, shipData[_index].level );

        if ( UIManager.Instance != null )
        {
            UIManager.Instance.startUIManager.shipUIManager.ButtonRefresh();
        }
        return;
    }

    public void BuyShipSuccess ( int _index )
    {
        shipData[_index].level = shipData[_index].levelMinimum;
        string _count = "shipLv"+_index;
        ObscuredPrefs.SetInt(_count, shipData[_index].level );

        if ( UIManager.Instance != null )
        {
            UIManager.Instance.startUIManager.shipUIManager.ButtonRefresh();
        }
        return;
    }

}
