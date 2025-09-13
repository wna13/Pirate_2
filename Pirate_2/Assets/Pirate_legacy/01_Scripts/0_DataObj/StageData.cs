using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat;
using CodeStage.AntiCheat.ObscuredTypes;

public class StageData : MonoBehaviour
{
    public static StageData Instance;
    public int currentStage;
    public bool isLastStage;
    private void Start() 
    {
        Instance = this;
        DataLoad();
    }
    void DataLoad()
    {
        currentStage = ObscuredPrefs.GetInt("currentStage", 0);
        for ( int i = 0; i < data.Count; i ++ )
        {
            string fill ="filledCount" + i;
            data[i].filledCount = ObscuredPrefs.GetInt(fill, 0);            
        }

        isLastStage = currentStage >= data.Count-1;
    }

    public void FillCountUP( int _count )
    {
        data[currentStage].filledCount += _count;
        string fill ="filledCount" + currentStage;
        ObscuredPrefs.SetInt(fill, data[currentStage].filledCount );   
    }

    public void stageChange()
    {
        currentStage ++;
        ObscuredPrefs.SetInt("currentStage", currentStage);

        isLastStage = currentStage >= data.Count-1;
    }

    [System.Serializable]
    public class Data
    {
        public Sprite stageImg;
        public int filledCount;
        public int needCountperDot;
        public GameObject stageprefab;
    }
    public List<Data> data = new List<Data>();
}
