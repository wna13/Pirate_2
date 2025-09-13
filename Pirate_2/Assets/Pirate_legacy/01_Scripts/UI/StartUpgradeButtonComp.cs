using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PirateConfig.Data;
using System.Globalization;
using DG.Tweening;
using System.Threading.Tasks;
public class StartUpgradeButtonComp : MonoBehaviour
{
    [SerializeField] Image icon, textIcon;
    [SerializeField] int type, price;
    [SerializeField] TextMeshProUGUI tValue, tPrice;
    [SerializeField] Button btnUpgradeCoin, btnUpgradeAds;
    [SerializeField] GameObject goMAX;
    [SerializeField] StartUpgradeButtons startUpgradeButtons;

    public void InitButton( StartUpgradeButtons parent, int _type )
    {
        startUpgradeButtons = parent;
        type = _type;

         btnUpgradeCoin.onClick.AddListener( GetBtnCoinUpgrade );
         btnUpgradeAds.onClick.AddListener( GetBtnAdsUpgrade );

        InfoRefresh();
    }
    public void InfoRefresh()
    {
        bool isMaxLv = false;
        bool isFirstLevel = false;
        ShipModelData.ShipData data = ShipModelData.Instance.shipData[ShipModelData.Instance.playerEquippedShipIndex];
        if ( type == 0 ) //hp
        {
            isFirstLevel = PlayerDataManager.Instance.shipHPLevel < 1 ? true : false;
            tValue.text = PlayerDataManager.Instance.shipHPLevel * 5 + "%";
            price = PlayerDataManager.Instance.shipUpgradePrice[PlayerDataManager.Instance.shipHPLevel];
            if ( PlayerDataManager.Instance.shipHPLevel >= 20 ) isMaxLv = true;
        }
        if ( type == 1 ) // dmg
        {
            isFirstLevel = PlayerDataManager.Instance.shipCannonDMGLevel < 1 ? true : false;
            int dmg = data.defaultDMG + PlayerDataManager.Instance.shipCannonDMGAdded;
            tValue.text =  dmg.ToString("N0");
            price = PlayerDataManager.Instance.shipUpgradePrice[PlayerDataManager.Instance.shipCannonDMGLevel];
            if ( PlayerDataManager.Instance.shipCannonDMGLevel >= 20 ) isMaxLv = true;
        }
        if ( type == 2 ) // reload
        {
            isFirstLevel = PlayerDataManager.Instance.shipCannonReloadLevel < 1 ? true : false;
            float reload = data.reloadTime - PlayerDataManager.Instance.shipResloadAdded;
            tValue.text = reload + "sec";
            price = PlayerDataManager.Instance.shipUpgradePrice[PlayerDataManager.Instance.shipCannonReloadLevel] * 2;
            if ( PlayerDataManager.Instance.shipCannonReloadLevel >= 4 ) isMaxLv = true;
        }

        int playerHasCoin = PlayerDataManager.Instance.coin;
        if ( isMaxLv )
        {
            goMAX.SetActive(true);
            btnUpgradeCoin.gameObject.SetActive(false);
            btnUpgradeAds.gameObject.SetActive(false);
        }
        else
        {
            goMAX.SetActive(false);
            tPrice.text = price.ToString("N0");

            if ( isFirstLevel )
            {
                btnUpgradeCoin.gameObject.SetActive(true);
                btnUpgradeCoin.interactable = playerHasCoin >= price;

                btnUpgradeAds.gameObject.SetActive(false);
            }
            else
            {
                btnUpgradeCoin.gameObject.SetActive(playerHasCoin >= price);
                btnUpgradeAds.gameObject.SetActive(playerHasCoin < price);
            }

        }


    }
    public void GetBtnCoinUpgrade()
    {
        if ( PlayerDataManager.Instance.coin >= price )
        {
            UIManager.Instance.coinUI.GoodsValueChange(-price);
            UpgradeFunction();

        }
        else
        {
            UIManager.Instance.coinUI.GoodsNotEnough();
        }
    }
    public void GetBtnAdsUpgrade()
    {
        AdsTotalManager.Instance.startUpgradeButtonComp = this;
        if ( type == 0 ) AdsTotalManager.Instance.RewardCall(AdsTotalManager.RewardType.StatUpgrade_HP);
        if ( type == 1 ) AdsTotalManager.Instance.RewardCall(AdsTotalManager.RewardType.StatUpgrade_cannonPower);
        if ( type == 2 ) AdsTotalManager.Instance.RewardCall(AdsTotalManager.RewardType.StatUpgrade_cannonReloadSpeed);
    }

    public void AdsCallBack()
    {
        UpgradeFunction();
    }
    void UpgradeFunction()
    {
        if ( type == 0 )
        {
            PlayerDataManager.Instance.HPLevelUpgrade();
        }
        if ( type == 1 ) 
        {
            PlayerDataManager.Instance.CannonDMGUpgrade();
        }
        if ( type == 2 )
        {
            PlayerDataManager.Instance.CannonReloadUpgrade();
        }
        startUpgradeButtons.InfoRefresh();

        icon.DOKill();
        textIcon.DOKill();
        tValue.DOKill();

        icon.color = Color.yellow;
        textIcon.color = Color.yellow;
        tValue.color = Color.yellow;

        icon.transform.localScale = Vector2.one * 1.2f;
        tValue.transform.localScale = Vector2.one * 1.2f;

        icon.DOColor(Color.white, 0.4f).SetDelay(0.1f);
        textIcon.DOColor(Color.white, 0.4f).SetDelay(0.1f);
        tValue.DOColor(Color.white, 0.4f).SetDelay(0.1f);

        icon.transform.DOScale(1f, 0.4f).SetDelay(0.1f);
        tValue.transform.DOScale(1f, 0.4f).SetDelay(0.1f);

        GameFlowManager.Instance.BalanceDownByUpgrade();
    }
}
