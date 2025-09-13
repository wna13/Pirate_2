using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    [SerializeField] TutoShipManager tutoShipManager;
    [SerializeField] Transform tEXPP;
    [SerializeField] GameObject atkRange;
    void Start()
    {
        Instance = this;
        PlayerSpawn();
    }
    public int tutorialStep;

    void PlayerSpawn()
    {
        tutoShipManager.PlayerSpawn();
        tutorialStep = 0;

        tutorialON();
    }
    [SerializeField] GameObject[] tutorial;

    void tutorialON()
    {
        if ( tutorialStep > 0 ) 
        {
            tutorial[tutorialStep-1].SetActive(false);
        }
        if ( tutorial[tutorialStep] != null )
        {
            tutorial[tutorialStep].SetActive(true);
        }

        if ( tutorialStep ==  1)
        {
            EXPSpawn();
        }
        if ( tutorialStep == 2 )
        {
            EnemySpawn();
        }
    }
    public void FirstMove()
    {
        tutorialStep ++;
        Invoke("tutorialON", 1.7f);
    }
    public void TutoLevelUP()
    {
        tutorialStep ++;
        Invoke("tutorialON", 1f);
    }
    public void EXPSpawn()
    {
        StartCoroutine(EXPSpawnCor());
    }
    public void EnemySpawn()
    {
        tutoShipManager.EnemyShipSpawn();
        
        // 공격범위 
        GameObject _go = Instantiate(atkRange);

        if (_go != null)
        {
            _go.transform.SetParent(PlayerMover.Instance.transform);
            _go.transform.localPosition = Vector3.zero;
            _go.transform.localScale = Vector3.one;
            _go.transform.localEulerAngles = Vector3.zero;
        }
    }
    public void EnemyKill()
    {
        tutorialStep ++;
        Invoke("tutorialON", 0.5f);

        GameDataManager.Instance.TutorialClear();
    }

    public void GetBtnToGame()
    {
        SceneManager.LoadScene("Game");
    }

    Transform[] expSpawnpos;
    IEnumerator EXPSpawnCor()
    {
        expSpawnpos = new Transform[tEXPP.childCount];
        for ( int i =0  ; i < tEXPP.childCount; i ++ )
        {
            expSpawnpos[i] = tEXPP.GetChild(i).transform;
        }

        yield return new WaitForEndOfFrame();

        while ( true )
        {
            for ( int i = 0; i < expSpawnpos.Length; i ++ )
            {
                GameObject _go = null;
                if (ObjectPoolManager.Instance.expBox.TryGetNextObject(this.transform.position, Quaternion.identity, out _go))
                {
                    EXPobj _exp = _go.GetComponent<EXPobj>();
                    _go.transform.parent = this.transform;
                    _go.transform.position = expSpawnpos[i].position;
                    _exp.AutoSpawned();
                }
                yield return new WaitForSeconds(0.2f );

            }

            yield return null;
            yield break;
        }
    }

}
