using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagUISlot : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] Image flag;
    [SerializeField] GameObject check, checkLine, goLock;
    [SerializeField] TextMeshProUGUI tValue;
    [SerializeField] Button btn;
    bool isClicable;
    public void InitAndSet(int _index)    // 안바뀌는 내용
    {
        index = _index;
        flag.sprite = ShipFlagData.Instance.flagData[index].flagImage;
        tValue.text = ShipFlagData.Instance.flagData[index].moveSpeedExtra + "%";

        btn.onClick.AddListener(GetBtnUse);

        DataRefresh();
        newDot.SetActive(false);
    }

    public void DataRefresh()    //바뀌는 내용
    {
        isClicable = ShipFlagData.Instance.flagData[index].flagGetCount > 0;
        goLock.SetActive(!isClicable);
        btn.interactable = isClicable;

        check.SetActive(ShipFlagData.Instance.playerEquippedFlagIndex == index );
        checkLine.SetActive(ShipFlagData.Instance.playerEquippedFlagIndex == index );
    }
    [SerializeField] GameObject newDot;
    public void MakeNewDot()
    {
        newDot.SetActive(true);
    }

    public void GetBtnUse()
    {
        if ( isClicable == false ) return;

        ShipFlagData.Instance.PlayerFlagChange(index);
        if ( newDot.activeSelf == true ) newDot.SetActive(false);
    }



}
