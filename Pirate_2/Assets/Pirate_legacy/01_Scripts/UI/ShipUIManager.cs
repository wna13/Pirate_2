using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ShipUIManager : MonoBehaviour
{
    [SerializeField] Button btnBUYCoin, btnUpgrade, btnMAX, btnUse, btnNext, btnPrev;
    [SerializeField] TextMeshProUGUI tUpgradePrice, tShipTitle, tShipLv, tHP, tDmg, tReload, tCtrl;
    [SerializeField] ShipModelBase[] shipModels;
    [SerializeField] int selectedIndex;
    int upgradePrice;
    [SerializeField] Transform shipModelsParent;
    [SerializeField] ShipModelData.ShipData selectedShipModelData;

    void Start()
    {
        selectedIndex = ShipModelData.Instance.playerEquippedShipIndex;
        selectedShipModelData = ShipModelData.Instance.shipData[selectedIndex];
        ButtonRefresh();
        ModelSpawn();

        btnNext.onClick.AddListener(GetBtnNext);
        btnPrev.onClick.AddListener(GetBtnPrev);
        btnBUYCoin.onClick.AddListener(GetBtnBUYCoin);
        btnUpgrade.onClick.AddListener(GetBtnUpgrade);
        btnUse.onClick.AddListener(GetBtnUse);
    }

    public void OpenInit()
    {
        ShipManager.Instance.allShipsParent.gameObject.SetActive(false);
        shipModelsParent.gameObject.SetActive(true);
        // shipModelsParent.transform.position = Vector3.zero;

        selectedIndex = ShipModelData.Instance.playerEquippedShipIndex;
        selectedShipModelData = ShipModelData.Instance.shipData[selectedIndex];

        ButtonRefresh();
        ModelFlagChange();
        ShipModelPosChangeQUICK();
    }


    public void Close()
    {
        ShipManager.Instance.allShipsParent.gameObject.SetActive(true);
        shipModelsParent.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                UIManager.Instance.startUIManager.GetBtnShipToHome();
            }
        }
    }
    void ModelSpawn()
    {
        shipModelsParent.gameObject.SetActive(false);
        shipModels = new ShipModelBase[ ShipModelData.Instance.shipData.Count ];
        for ( int i =0  ; i < ShipModelData.Instance.shipData.Count; i ++ )
        {
            ShipModelData.ShipData _shipData = ShipModelData.Instance.shipData[i];
            int _lv = _shipData.level < 0 ? _shipData.levelMinimum : _shipData.level;

            GameObject model = Instantiate( _shipData.model[_lv], shipModelsParent);
            model.transform.SetParent( shipModelsParent );
            model.transform.localPosition = new Vector3 ( i * 10, 0, 0);
            model.transform.eulerAngles = new Vector3 (0f, 145f, 0f);
            model.transform.localScale = Vector3.one;
            model.GetComponent<ShipModelBase>().SailChange();

            shipModels[i] =model.GetComponent<ShipModelBase>();

        }
    }

    public void ModelRespawnByUpgrade()
    {
        GameObject _destroy = shipModels[selectedIndex].gameObject;
        shipModels[selectedIndex] = null;
        Destroy(_destroy);

        ShipModelData.ShipData _shipData = ShipModelData.Instance.shipData[selectedIndex];

        GameObject model = Instantiate( _shipData.model[_shipData.level], shipModelsParent);
        model.transform.SetParent( shipModelsParent );
        model.transform.localPosition = new Vector3 ( selectedIndex * 10, 0, 0);
        model.transform.eulerAngles = new Vector3 (0f, 145f, 0f);
        model.transform.localScale = Vector3.one;
        model.GetComponent<ShipModelBase>().SailChange();
        model.GetComponent<ShipModelBase>().SpawnPtclPlay();

        shipModels[selectedIndex] = model.GetComponent<ShipModelBase>();
    }

    void ModelFlagChange()
    {
        foreach ( ShipModelBase model in shipModels )
        {
            model.SailChange();
        }
    }

    public void GetBtnNext()
    {
        selectedIndex ++;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, ShipModelData.Instance.shipData.Count);
        selectedShipModelData = ShipModelData.Instance.shipData[selectedIndex];

        ButtonRefresh();
        ShipModelPosChange();
    }
    public void GetBtnPrev()
    {
        selectedIndex --;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, ShipModelData.Instance.shipData.Count);
        selectedShipModelData = ShipModelData.Instance.shipData[selectedIndex];

        ButtonRefresh();
        ShipModelPosChange();
    }


    public void GetBtnUpgrade()
    {
        if ( PlayerDataManager.Instance.coin >= upgradePrice )
        {
            UIManager.Instance.coinUI.GoodsValueChange(-upgradePrice);
            ShipModelData.Instance.ShipUpgrade( selectedIndex );
            ShipManager.Instance.PlayerSpawn(true);
            ModelRespawnByUpgrade();
            GameFlowManager.Instance.BalanceDownByUpgrade();
        }   
        else
        {
            UIManager.Instance.coinUI.GoodsNotEnough();
        }
        return;
    }

    public void GetBtnUse()
    {
        if ( ShipModelData.Instance.shipData[selectedIndex].level < 0 ) return;

        ShipModelData.Instance.ChangeEquippedShipIndex(selectedIndex);

        ShipManager.Instance.PlayerSpawn(true);
        return;
    }


    public void ButtonRefresh()
    {
        tShipTitle.text = selectedShipModelData.name;

        int lv = selectedShipModelData.level < 0 ? selectedShipModelData.levelMinimum : selectedShipModelData.level;

        int _lv = lv + 1;
        tShipLv.text = "Lv." + _lv;
        tHP.text = ( selectedShipModelData.defaultHP * (1+lv) ).ToString("N0");
        
        tDmg.text = selectedShipModelData.defaultDMG.ToString("N0");
        tReload.text = selectedShipModelData.reloadTime + "sec";
        tCtrl.text = selectedShipModelData.controllFactor.ToString();


        if ( selectedShipModelData.level < 0 )  // 구매전
        {
            btnBUYCoin.gameObject.SetActive(false);
            // iapButton.gameObject.SetActive(false);

            btnUpgrade.gameObject.SetActive(false);
            btnUse.gameObject.SetActive(false);

            if ( selectedShipModelData.buyType == ShipModelData.ShipData.BuyType.Coin )
            {
                btnBUYCoin.gameObject.SetActive(true);
                CoinButtonSetting();
            }
            if ( selectedShipModelData.buyType == ShipModelData.ShipData.BuyType.IAP )
            {
                // iapButton.gameObject.SetActive(true);
                IAPButtonSetting();
            }
        }
        if ( selectedShipModelData.level >= 0 )
        {
            btnBUYCoin.gameObject.SetActive(false);
            // iapButton.gameObject.SetActive(false);

            if ( selectedShipModelData.level >= selectedShipModelData.levelMAX ) 
            {
                btnMAX.gameObject.SetActive( true );
                btnUpgrade.gameObject.SetActive ( false );
            }
            else
            {
                btnMAX.gameObject.SetActive( false );
                btnUpgrade.gameObject.SetActive ( true );
                upgradePrice = ShipModelData.Instance.modelUpgradePrice[selectedShipModelData.level];
                tUpgradePrice.text = upgradePrice.ToString("N0");
            }

            if ( selectedIndex == ShipModelData.Instance.playerEquippedShipIndex ) btnUse.gameObject.SetActive(false);
            else btnUse.gameObject.SetActive(true);
        }

        btnPrev.gameObject.SetActive( selectedIndex > 0);
        btnNext.gameObject.SetActive( selectedIndex < ShipModelData.Instance.shipData.Count-1 );
    }

    void ShipModelPosChange()
    {
        float _x = selectedIndex * 10f;
        shipModelsParent.transform.DOLocalMoveX( - _x, 0.3f);
    }

    void ShipModelPosChangeQUICK()
    {
        float _x = selectedIndex * 10f;
        shipModelsParent.transform.localPosition = new Vector3 ( - _x, 0f, 0f);
    }

    // [SerializeField] CodelessIAPButton iapButton;
    [SerializeField] TextMeshProUGUI tIapPrice, tCoinPrice;
    [SerializeField] string iapID;
    void IAPButtonSetting()
    {
        iapID = selectedShipModelData.iapID;
        // iapButton.productId = iapID;
        tIapPrice.text = "$ "+selectedShipModelData.iapPrice;
    }
    void CoinButtonSetting()
    {
        tCoinPrice.text = selectedShipModelData.coinPrice.ToString("N0");
    }
    public void GetBtnBUYCoin()
    {   
        if ( PlayerDataManager.Instance.coin >= selectedShipModelData.coinPrice )
        {
            UIManager.Instance.coinUI.GoodsValueChange(-selectedShipModelData.coinPrice);
            GameFlowManager.Instance.BalanceDownByUpgrade();

            BuySuccess();
        }   
    }
    public void BuySuccess()
    {
        ShipModelData.Instance.BuyShipSuccess(selectedIndex);
        GameFlowManager.Instance.BalanceDownByUpgrade();

        if ( selectedShipModelData.buyType == ShipModelData.ShipData.BuyType.IAP ) UIManager.Instance.FakeLoadingOFF();
        UIManager.Instance.ToastShow("Purchase Complete.");
    }
    public void FakeLoadingON()
    {
        UIManager.Instance.FakeloadingON(5f);
    }

    public void BuyFail()
    {
        UIManager.Instance.ToastShow("Sorry. Try Again.");
    }
}
