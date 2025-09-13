using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;
    public bool isGameStart;
    public int inGamePlayerLevel;
    public bool isPlayerDeadOnce;
    public int coinGotThisGame, expGotThisGame;
    [SerializeField] GameObject touchPad;
    public int balanceFactorThisTime;
    float playingTime;
    int balanceUP;

    void Start()
    {
        Instance = this;
        isGameStart = false;
        isPlayerDeadOnce = false;
        touchPad.SetActive(false);

        ShipModelData.Instance.PlayerEquipShipIndexReload();

        MapSpawn();
        ShipInit();
        ShipPreSpawn( true );
        UIManager.Instance.coinUI.CanvasGroupON(true);
        coinGotThisGame = 0;
        balanceFactorThisTime = 0;
        playingTime = 0f;
        balanceUP = 0;

        if ( GameDataManager.Instance.playCount > 4 )
        {
            if ( GameDataManager.Instance.isNoAdsMode == false )
            {
                if ( GameDataManager.Instance.isJustWachedAd == false )
                {
                    if ( GameDataManager.Instance.playCount < 10 )
                    {
                        if ( GameDataManager.Instance.playCount % 2 == 0 )
                        {
                            AdsTotalManager.Instance.InterstitialAdShow();
                        }
                    }
                    else
                    {
                        AdsTotalManager.Instance.InterstitialAdShow();
                    }
                }
            }
        }
        GameDataManager.Instance.isJustWachedAd = false;
        SoundManager.Instance.PlayBGM();
    }
    void MapSpawn()
    {
        GameObject _map = Instantiate(StageData.Instance.data[StageData.Instance.currentStage].stageprefab, null);
        _map.transform.position = Vector3.zero;
        _map.transform.eulerAngles = Vector3.zero;
        _map.transform.localScale = Vector3.one;
    }
    public void BalanceDownByUpgrade()
    {
        balanceFactorThisTime ++;
        if ( balanceFactorThisTime < 3 ) BalanceFactorManager.Instance.BalanceFactorchange (-1);
    }
    public void GetCoinThisGameValue( int _value )
    {
        coinGotThisGame += _value;
    }
    public void EXPGotThisGameValue( int _value )
    {
        expGotThisGame += _value;
    }
    public void ShipPreSpawn( bool _isFirstSpawn )
    {
        isGameStart = false;

        int spawnCount = 3 + BalanceFactorManager.Instance.balanceFactor;
        spawnCount = Mathf.Clamp(spawnCount, 3, 5);
        ShipManager.Instance.SpawnManyEnemyShip(spawnCount, _isFirstSpawn);
    }

    void ShipInit()
    {
        ShipManager.Instance.ShipRemoveAllAndInit();
    }

    public void GameStart()
    {
        isGameStart = true;
        ShipManager.Instance.EnemyAutoSpawn();
        UIManager.Instance.coinUI.CanvasGroupON(false);
        touchPad.SetActive(true);
        GameDataManager.Instance.PlayCountUP();
    }

    private void Update() 
    {
        if ( isGameStart == false ) return;
        playingTime += Time.deltaTime;

        if ( playingTime >= 45f )
        {
            playingTime = 0f;
            BalanceFactorManager.Instance.BalanceFactorchange(1);
            balanceUP ++;
        }
    }

    public void PlayerDead()
    {
        isGameStart = false;
        touchPad.SetActive(false);

        if (!isPlayerDeadOnce)
        {
            isPlayerDeadOnce = true;
            //콘티뉴 UI 띄움.
            EndingUIManager.Instance.ContinueUION();

            if ( balanceUP == 0 ) 
            {
                BalanceFactorManager.Instance.BalanceFactorchange(-3);
                GameDataManager.Instance.isJustWachedAd = true;
            }
            if ( balanceUP == 1 )
            {
                BalanceFactorManager.Instance.BalanceFactorchange(-2);
            }
            if ( balanceUP == 2 )
            {
                BalanceFactorManager.Instance.BalanceFactorchange(-1);
            }
        }
        else
        {
            //엔딩 UI 띄움.
            EndingUIManager.Instance.ResultUION();
        }
    }

    public void GameContinue()
    {
        ShipInit();
        ShipPreSpawn(false);
        isGameStart = true;
        touchPad.SetActive(true);

        ShipManager.Instance.EnemyAutoSpawn();
    }

    public void GameEnd()
    {
        isGameStart = false;
        touchPad.SetActive(false);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene("Game");
    }

    public void InGamePlayerLevelUP()
    {
        inGamePlayerLevel++;
    }
}
