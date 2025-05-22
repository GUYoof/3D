using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 상태값을 관리하는 클래스
public class Condition : MonoBehaviour
{
    public float curValue;      // 현재 값
    public float startValue;    // 시작 값
    public float maxValue;      // 최대 값
    public float passiveValue;  // 자연 회복 또는 감소 값
    public Image uiBar;         // 상태를 표시할 UI 바 이미지

    void Start()
    {
        curValue = startValue;
    }

    /// 매 프레임 UI 바를 현재 상태 비율에 맞게 갱신
    void Update()
    {
        // UI 바 fillAmount 업데이트
        uiBar.fillAmount = GetPercentage();
    }

    /// <summary>
    /// 현재 값이 최대 값에서 차지하는 비율 계산
    /// </summary>
    float GetPercentage()
    {
        return curValue / maxValue;
    }

    /// <summary>
    /// 상태값을 증가시킴 (최대값을 넘지 않음)
    /// </summary>
    /// <param name="amount">증가시킬 양</param>
    public void Add(float amount)
    {
        curValue = MathF.Min(curValue + amount, maxValue);
    }

    /// <summary>
    /// 상태값을 감소시킴 (0 아래로 내려가지 않음)
    /// </summary>
    /// <param name="amount">감소시킬 양</param>
    public void Subtract(float amount)
    {
        curValue = MathF.Max(curValue - amount, 0f);
    }
}
