using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StageMapUI : MonoBehaviour
{
    [SerializeField] Image currentStage, nextStage;
    [SerializeField] GameObject updateSoon;
    [SerializeField] Transform tDotParent;
    [SerializeField] List<Image> dots = new List<Image>();
    [SerializeField] Color cFalse, cTrue;

    public void Init()
    {
        int currentStageIndex = StageData.Instance.currentStage;
        StageData.Data data = StageData.Instance.data[currentStageIndex];

        currentStage.sprite = data.stageImg;

        if (StageData.Instance.currentStage == StageData.Instance.data.Count - 1) // 마지막 스테이지
        {
            nextStage.gameObject.SetActive(false);
            updateSoon.SetActive(true);

            tDotParent.gameObject.SetActive(false);
            return;
        }

        nextStage.sprite = StageData.Instance.data[currentStageIndex + 1].stageImg;
        nextStage.gameObject.SetActive(true);
        updateSoon.SetActive(false);

        dots.Clear();
        for (int i = 0; i < tDotParent.childCount; i++)
        {
            Image dot = tDotParent.GetChild(i).gameObject.GetComponent<Image>();
            if (i < data.filledCount) dot.color = cTrue;
            else dot.color = cFalse;
            dots.Add(dot);
        }
    }

    public void FilledCountUPAnim()
    {
        if ( StageData.Instance.isLastStage == false )
        {
            StartCoroutine(FilledCountUPCor());
        }
    }

    bool isNewStageOpened;

    IEnumerator FilledCountUPCor()
    {
        isNewStageOpened = false;
        int currentStageIndex = StageData.Instance.currentStage;
        StageData.Data data = StageData.Instance.data[currentStageIndex];

        int goCount = GameFlowManager.Instance.expGotThisGame / data.needCountperDot;

        for (int i = data.filledCount; i < data.filledCount + goCount; i++)
        {
            if (i >= dots.Count)
            {
                // 꽉 차면 다음 스테이지로 이동
                isNewStageOpened = true;

                // 마지막 점을 처리한 후 바로 새로운 스테이지로 넘어가므로 break
                break;
            }

            // 점 순차적으로 켜주는 애니메이션
            if ( dots[i] != null )
            {
                dots[i].DOKill();
                dots[i].color = Color.white;
                dots[i].DOColor(cTrue, 0.4f);
                dots[i].transform.localScale = Vector2.one * 1.3f;
                dots[i].transform.DOScale(1f, 0.35f);

                yield return new WaitForSeconds(0.1f);
            }

        }
        StageData.Instance.FillCountUP(goCount);

        if (isNewStageOpened && StageData.Instance.isLastStage == false )
        {
            StageData.Instance.stageChange();
            // 새로운 스테이지 팝업띄움.
        }

        yield break;
    }
}
