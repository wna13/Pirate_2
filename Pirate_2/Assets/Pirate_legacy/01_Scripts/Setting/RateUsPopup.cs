using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateUsPopup : MonoBehaviour
{
    [SerializeField] Button btnRate;
    
    private void Start() 
    {
        btnRate.onClick.AddListener(GetBtnRate);
    }
    public void GetBtnRate()
    {
        GameDataManager.Instance.RateSuccess();
        #if UNITY_ANDROID
			Application.OpenURL("https://play.google.com/store/apps/details?id=com.Sept09.Pirateio");
			
		#elif UNITY_IOS
			Application.OpenURL("https://apps.apple.com/kr/app/apple-store/id6630386184");
		#endif
    }
}
