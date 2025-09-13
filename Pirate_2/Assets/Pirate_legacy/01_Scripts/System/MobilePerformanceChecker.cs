using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePerformanceChecker : MonoBehaviour
{
    [SerializeField] GameObject Water_PC, water_Mobile, water_Mobie_Pattern;
    private int frameThreshold = 45; // 프레임 임계값
    [SerializeField] private float checkDuration = 1f; // 체크 시간 (1초)
    
    private float belowThresholdTime = 0f; // 프레임이 임계값 이하로 유지된 시간
    private bool isLowPerformance = false; // 낮은 성능으로 판단되는지 여부

    void Update()
    {
        if (isLowPerformance) return;
        
        // 실제 프레임 레이트가 임계값보다 낮은지 확인
        float currentFrameRate = 1.0f / Time.deltaTime;
        if (currentFrameRate < frameThreshold)
        {
            belowThresholdTime += Time.deltaTime;
        }
        else
        {
            belowThresholdTime = 0f; // 프레임이 다시 임계값 위로 올라가면 시간 초기화
        }

        // 프레임이 임계값 이하로 1초 이상 유지되면 성능이 낮은 장치로 판단
        if (belowThresholdTime >= checkDuration && !isLowPerformance)
        {
            isLowPerformance = true;
            HandleLowPerformance();
        }
    }

    private void HandleLowPerformance()
    {
        Water_PC.SetActive(false);
        water_Mobile.SetActive(true);
        water_Mobie_Pattern.SetActive(true);
    }
}
