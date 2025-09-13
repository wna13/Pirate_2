using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField] SettingButton bASMR, bSFX, bHaptic, bRate;

    void Start()
    {
        ASMRSetting();
        SFXSetting();
        HapticSetting();
        RateSetting();
    }
    void ASMRSetting()
    {
        bASMR.GetComponent<Button>().onClick.AddListener(GetBtnASMR);

        bASMR.isOnOffType = true;
        bASMR.Setting( SoundManager.Instance.isBGMOn );
    }
    void GetBtnASMR()
    {
        bool onoff = ! SoundManager.Instance.isBGMOn;
        SoundManager.Instance.DataSaveBGM( onoff );
        bASMR.Setting(onoff);
    }
    void SFXSetting()
    {
        bSFX.GetComponent<Button>().onClick.AddListener(GetBtnSFX);

        bSFX.isOnOffType = true;
        bSFX.Setting( SoundManager.Instance.isEffectOn );
    }
    void GetBtnSFX()
    {
        bool onoff = ! SoundManager.Instance.isEffectOn;
        SoundManager.Instance.DataSaveSFX( onoff );
        bSFX.Setting(onoff);
    }
    void HapticSetting()
    {
        bHaptic.GetComponent<Button>().onClick.AddListener(GetBtnHaptic);

        bHaptic.isOnOffType = true;
        bHaptic.Setting( VibrationManager.Instance.isVibrateON );
    }
    void GetBtnHaptic()
    {
        bool onoff = ! VibrationManager.Instance.isVibrateON;
        SoundManager.Instance.DataSaveSFX( onoff );
        bHaptic.Setting(onoff);
    }
    void RateSetting()
    {
        bRate.GetComponent<Button>().onClick.AddListener(GetBtnRate);
        bRate.isOnOffType = false;
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
