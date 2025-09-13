using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Timmer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Timer를 표시할 UI Text 컴포넌트
    private float remainingTime = 120f; // 2분 = 120초
    private bool timerRunning = false;

    private void Start()
    {
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        timerRunning = true;
        while (remainingTime > 0)
        {
            UpdateTimerText();
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }
        UpdateTimerText(); // Ensure the final time is displayed as 00:00
        timerRunning = false;

        // 여기에 타이머가 끝났을 때 실행할 코드를 추가합니다.
        Debug.Log("타이머 종료");
    }
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (remainingTime <= 10f)
        {
            timerText.color = Color.red; // 10초 이하일 때 빨간색으로 변경
            timerText.transform.localScale = Vector2.one * 1.1f;
            timerText.transform.DOScale(1f, 0.3f);
        }
        else
        {
            timerText.color = Color.white; // 그렇지 않으면 흰색으로 유지
        }

    }

}
