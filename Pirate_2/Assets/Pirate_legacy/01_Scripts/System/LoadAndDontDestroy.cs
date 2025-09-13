using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CodeStage.AntiCheat.ObscuredTypes;

public class LoadAndDontDestroy : MonoBehaviour
{
    public bool isDeleteAllData;
    // Start is called before the first frame update
    void Start()
    {
        if ( isDeleteAllData ) ObscuredPrefs.DeleteAll();
        
        StartCoroutine(LoadCor());
        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator LoadCor()
    {
        Application.targetFrameRate = 60;
        for ( int i =0; i < this.gameObject.transform.childCount; i ++ )
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);

        }
        yield return new WaitForSeconds(0.6f);

        if ( GameDataManager.Instance.isTutorialMode == true )
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
        yield break;
    }
}
