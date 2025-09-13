using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ShipUI : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] HPBarUI hpBarUI;
    [SerializeField] EXPBarUI expBarUI;
    ShipMover myShip;
    [SerializeField] CanvasGroup canvasGroup;

    private Coroutine showUICoroutine;

    private void Start()
    {
        if ( CameraMover.Instance != null ) mainCamera = CameraMover.Instance.camera;
    }

    public void UIInit(ShipMover _myShip)
    {
        myShip = _myShip;
        expBarUI.gameObject.SetActive(myShip.isPlayer);

        hpBarUI.HPBarInit();
        expBarUI.LevelInit( myShip );
        
        canvasGroup.alpha = 0;
    }


    public void HPBarValueChange(int fullHP, int currentHP)
    {
        float _ratio = (float)currentHP / (float)fullHP;
        hpBarUI.HPBarChange(_ratio);

        if (showUICoroutine != null)
        {
            StopCoroutine(showUICoroutine);
        }
        showUICoroutine = StartCoroutine(ShowUI());
    }

    public void EXPValueChange(int _EXP)
    {
        if (myShip.isPlayer)
        {
            if (showUICoroutine != null)
            {
                StopCoroutine(showUICoroutine);
            }
            showUICoroutine = StartCoroutine(ShowUI());
        }
        expBarUI.EXPChange(_EXP);
    }

    IEnumerator ShowUI()
    {
        if ( mainCamera == null )
        {
            if ( CameraMover.Instance != null ) mainCamera = CameraMover.Instance.camera;
            else
            {
                if ( TutoCamMove.Instance != null ) mainCamera = TutoCamMove.Instance.camera;
            }
        }
        canvasGroup.DOFade(1f, 0.2f);
        float _showTotal = 2.5f;

        bool show = true;

        while (show)
        {
            if ( myShip.isLive == false ) yield break;
            _showTotal -= Time.deltaTime;

            if (_showTotal < 0f)
            {
                canvasGroup.DOFade(0f, 0.5f).OnComplete(() => show = false);
            }

            if (mainCamera != null && canvasGroup.alpha > 0)
            {
                // HPBar가 카메라를 항상 바라보도록 회전합니다.
                transform.LookAt(mainCamera.transform);

                // 카메라와 HPBar가 같은 방향을 보도록 만듭니다.
                transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
            }

            yield return null;
        }
    }
}
