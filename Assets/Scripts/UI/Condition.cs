using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���°��� �����ϴ� Ŭ����
public class Condition : MonoBehaviour
{
    public float curValue;      // ���� ��
    public float startValue;    // ���� ��
    public float maxValue;      // �ִ� ��
    public float passiveValue;  // �ڿ� ȸ�� �Ǵ� ���� ��
    public Image uiBar;         // ���¸� ǥ���� UI �� �̹���

    void Start()
    {
        curValue = startValue;
    }

    /// �� ������ UI �ٸ� ���� ���� ������ �°� ����
    void Update()
    {
        // UI �� fillAmount ������Ʈ
        uiBar.fillAmount = GetPercentage();
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
}
