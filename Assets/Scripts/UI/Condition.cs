using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ���°��� �����ϴ� Ŭ����
public class Condition : MonoBehaviour
{
    // �Ϲ� ���°�
    public float curValue;      // ���� ��
    public float startValue;    // ���� ��
    public float maxValue;      // �ִ� ��
    public float passiveValue;  // �ڿ� ȸ�� �Ǵ� ���� ��
    public Image uiBar;         // ���¸� ǥ���� UI �� �̹���

    [Header("Jump Boost UI")]
    public Image jumpBoostBar;       // ���� �ν�Ʈ ���¹� (Image Ÿ��, fillAmount�� ǥ��)
    public TextMeshProUGUI jumpBoostText;       // ���� �ð� �ؽ�Ʈ
    private float jumpBoostDuration; // �� ���� �ð�
    private float jumpBoostTimeLeft; // ���� �ð�
    private bool jumpBoostActive;

    void Start()
    {
        curValue = startValue;
    }

    /// �� ������ UI �ٸ� ���� ���� ������ �°� ����
    void Update()
    {
        // UI �� fillAmount ������Ʈ
        uiBar.fillAmount = GetPercentage();

        // ���� �ν�Ʈ UI ����
        if (jumpBoostActive)
        {
            jumpBoostTimeLeft -= Time.deltaTime;

            if (jumpBoostTimeLeft <= 0)
            {
                jumpBoostTimeLeft = 0;
                jumpBoostActive = false;
                jumpBoostBar.fillAmount = 0;
                jumpBoostText.text = "";
            }
            else
            {
                jumpBoostBar.fillAmount = jumpBoostTimeLeft / jumpBoostDuration;
                jumpBoostText.text = $"{jumpBoostTimeLeft:F1}s";
            }
        }
    }

    /// <summary>
    /// ���� ���� �ִ� ������ �����ϴ� ���� ���
    /// </summary>
    float GetPercentage()
    {
        return curValue / maxValue;
    }

    /// <summary>
    /// ���°��� ������Ŵ (�ִ밪�� ���� ����)
    /// </summary>
    /// <param name="amount">������ų ��</param>
    public void Add(float amount)
    {
        curValue = MathF.Min(curValue + amount, maxValue);
    }

    /// <summary>
    /// ���°��� ���ҽ�Ŵ (0 �Ʒ��� �������� ����)
    /// </summary>
    /// <param name="amount">���ҽ�ų ��</param>
    public void Subtract(float amount)
    {
        curValue = MathF.Max(curValue - amount, 0f);
    }

    /// <summary>
    /// ���� �ν�Ʈ UI ����
    /// </summary>
    /// <param name="duration">���� ȿ�� ���� �ð�</param>
    public void StartJumpBoostUI(float duration)
    {
        jumpBoostDuration = duration;
        jumpBoostTimeLeft = duration;
        jumpBoostActive = true;

        if (jumpBoostBar != null) jumpBoostBar.fillAmount = 1f;
        if (jumpBoostText != null) jumpBoostText.text = $"{duration:F1}s";
    }
}
