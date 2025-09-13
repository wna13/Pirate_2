using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingCor());
    }

    IEnumerator LoadingCor()
    {
        yield return new WaitForEndOfFrame();

        for ( int i =0 ; i < this.transform.childCount; i ++ )
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
