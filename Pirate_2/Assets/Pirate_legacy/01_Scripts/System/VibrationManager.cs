using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat;
using CodeStage.AntiCheat.ObscuredTypes;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;
    private void OnEnable() {
        Instance = this;
        isVibrateON = ObscuredPrefs.GetBool("isVibrateON", true);
    }
    public bool isVibrateON;

    public void VibrateONOFF(bool _onoff)
    {
        isVibrateON = _onoff;
        ObscuredPrefs.SetBool("isVibrateON", _onoff);
    }

    public void VivrateOnece()
    {
        if ( isVibrateON == false ) return;
        Vibration.Vibrate(5);
    }
}
