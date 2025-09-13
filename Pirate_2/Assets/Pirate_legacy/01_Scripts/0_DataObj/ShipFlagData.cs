using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat;
using CodeStage.AntiCheat.ObscuredTypes;

public class ShipFlagData : MonoBehaviour
{
    public static ShipFlagData Instance;
    public int playerEquippedFlagIndex;
    private void Start() 
    {
        Instance = this;
        DataLoad();
    }

    void DataLoad()
    {
        //default
        string _defualt = "falgGetCount"+0;
        flagData[0].flagGetCount = ObscuredPrefs.GetInt(_defualt, 1);

        for ( int i = 1; i < flagData.Count; i ++ )
        {
            string _count = "falgGetCount"+i;
            flagData[i].flagGetCount = ObscuredPrefs.GetInt(_count, 0);
        }

        playerEquippedFlagIndex = ObscuredPrefs.GetInt("playerEquippedFlagIndex", 0);
    }
    public void PlayerFlagChange ( int _newIndex )
    {
        int _beforeIndex = playerEquippedFlagIndex;

        playerEquippedFlagIndex = _newIndex;
        ObscuredPrefs.SetInt("playerEquippedFlagIndex", playerEquippedFlagIndex);

        UIManager.Instance.flagUIManager.DataRefresh(_beforeIndex);
        UIManager.Instance.flagUIManager.DataRefresh(playerEquippedFlagIndex);

        if ( PlayerMover.Instance != null )
        {
            PlayerMover.Instance.shipMover.ChangeFlag();
        }
    }

    public void GetFalgDataSave( int _index )
    {
        flagData[_index].flagGetCount ++;
        string _count = "falgGetCount"+_index;
        ObscuredPrefs.SetInt(_count,flagData[_index].flagGetCount );
        UIManager.Instance.flagUIManager.DataRefresh(_index);
        UIManager.Instance.flagUIManager.MakeNewDot(_index);
    }

    public int GetFlagRandom()
    {
        List<int> rdm = new List<int>();

        for ( int i = 0; i < flagData.Count; i ++ )
        {
            if ( flagData[i].flagGetCount < 1 )
            {
                rdm.Add(i);
            }
            if ( rdm.Count > 15 )
            {
                break;
            }
        }
        if ( rdm.Count < 3)
        {
            for ( int i = flagData.Count; i > 0 ; i -- )
            {
                rdm.Add(i);
                if ( rdm.Count > 15 )
                {
                    break;
                }
            }
        }
        int result = rdm[ Random.Range(0, rdm.Count)];

        GetFalgDataSave(result);
        return result;
    }

    [System.Serializable]
    public class ShipFlag
    {
        public Sprite flagImage;
        public int grade;
        public float moveSpeedExtra;
        public int flagGetCount;
    }
    public List<ShipFlag> flagData = new List<ShipFlag>();
}
