using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PirateConfig.Data;


public class CameraMover : MonoBehaviour
{
    public static CameraMover Instance;
    [SerializeField] Transform uiTrans, gameTrans;
    public Camera camera;
    private void Start() 
    {
        Instance = this;    
        if ( camera == null ) camera = Camera.main;

        FovChange();
        CamUIView();
    }

    public void FovChange()
    {
        int _lv = 0;
        if ( PlayerMover.Instance ) _lv = PlayerMover.Instance.shipMover.level;
        else _lv = ShipModelData.Instance.shipData[ShipModelData.Instance.playerEquippedShipIndex].level;

        float _fov = ShipData.DefaultStat.CamFov[_lv] * ( 1.2f + (_lv * 0.01f));
        camera.DOFieldOfView(_fov, 0.5f);//SetDelay(0.3f);
    }



    void Update()
    {
        if ( PlayerMover.Instance != null)  this.transform.position = PlayerMover.Instance.transform.position;
    }
    [SerializeField] Light mainUILight;
    public void CamUIView ()
    {
        camera.transform.SetParent(uiTrans);
        camera.transform.localScale = Vector3.zero;
        camera.transform.localEulerAngles = Vector3.zero;
        // mainUILight.gameObject.SetActive(true);
        // mainUILight.intensity = 0.5f;
    }

    public void CamInGameView()
    {
        camera.transform.SetParent(gameTrans);
        camera.transform.DOLocalMove(Vector3.zero, 2f);
        camera.transform.DOLocalRotate(Vector3.zero, 2f);
        // mainUILight.DOIntensity(0f, 0.5f).OnComplete(()=>mainUILight.gameObject.SetActive(false));

    }


}
