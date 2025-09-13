using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipTrialPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tShipName;
    [SerializeField] Image imgShipPreview;
    [SerializeField] Sprite[] shipSprite;
    public void PopupSetting( int _index )
    {
        tryShipIndex = _index;
        imgShipPreview.sprite = shipSprite[tryShipIndex];
        tShipName.text = ShipModelData.Instance.shipData[tryShipIndex].name;
    }
    public void GetBtnAdsCall()
    {
        tryShipIndex = 1;
        AdsTotalManager.Instance.shipTrialPopup = this;
        AdsTotalManager.Instance.RewardCall(AdsTotalManager.RewardType.TryShip);
    }
    int tryShipIndex;
    public void AdsCallBack()
    {
        UIManager.Instance.popupManager.TryShipON(false);
        ShipModelData.Instance.playerEquippedShipIndex = tryShipIndex;
        ShipModelData.Instance.shipData[tryShipIndex].level = 2;

        ShipManager.Instance.PlayerSpawn(true);
    }
}
