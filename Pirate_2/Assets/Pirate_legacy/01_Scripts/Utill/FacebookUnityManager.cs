using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookUnityManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if ( FB.IsInitialized == false) FB.Init();
        else
        {
            FB.ActivateApp();
        }
    }

}
