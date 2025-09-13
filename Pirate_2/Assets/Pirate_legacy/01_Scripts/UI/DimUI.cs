using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DimUI : MonoBehaviour
{
    [SerializeField] Image imgDim;
    Camera camera;
    private void Start() 
    {
        camera= Camera.main;
    }

    public void DimStart()
    {
        if (imgDim.gameObject.activeSelf == false ) imgDim.gameObject.SetActive(true);
        imgDim.DOKill();
        imgDim.color = UIDataManagerTable.Instance.hitUIDimStartColor;
        imgDim.DOFade(0f, 1.3f).OnComplete(()=> imgDim.gameObject.SetActive(false));

        camera.DOKill();
        camera.DOShakePosition(0.4f, 0.3f);
    }
}
