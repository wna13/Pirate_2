using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using Firebase.Extensions;

public class FireBaseUnityManager : MonoBehaviour
{
    
    private Firebase.FirebaseApp app; // FirebaseApp 인스턴스 변수 추가
    private bool firebaseReady = false; // Firebase 초기화 완료 플래그

    // Start is called before the first frame update
    void Start()
    {
        
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                firebaseReady = true; // Firebase 초기화 완료 플래그 설정

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        FirebaseAnalytics.LogEvent("Pirate_io_start");
    }
}