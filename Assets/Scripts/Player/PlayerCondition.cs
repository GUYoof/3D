using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� ���� �� �ִ� ��ü �������̽�
/// </summary>
public interface IDamagalbe
{
    void TakePysicalDamage(int damage);
}

/// <summary>
/// �÷��̾��� ü��, ���, ���¹̳� ���¸� �����ϴ� Ŭ����
/// </summary>
public class PlayerCondition : MonoBehaviour, IDamagalbe
{
    public UICondition uiCondition;             // ���� ��ġ�� ������ UICondition ����
    public event Action onTakeDamage;           // �������� ���� �� ȣ��� �̺�Ʈ

    // ���� ����
    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }


    // �� �����Ӹ��� ���� ��ġ�� ������
    void Update()
    {
        // ���¹̳� �ڿ� ȸ��
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        // ü���� 0 ���ϰ� �Ǹ� ��� ó��
        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// ü���� ȸ����Ŵ
    /// </summary>
    /// <param name="amount">ȸ���� ��</param>
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    /// <summary>
    /// �÷��̾� ��� ó��
    /// </summary>
    public void Die()
    {
        Debug.Log("�׾���");
    }

    /// <summary>
    /// �������� ������ ü�� ���� �� �̺�Ʈ ȣ��
    /// </summary>
    /// <param name="damage">���� ������ ��</param>
    public void TakePysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    /// <summary>
    /// ���¹̳� ��� �õ�. ����� ��� ���ҽ�Ű�� true ��ȯ
    /// </summary>
    /// <param name="amount">����� ���¹̳� ��</param>
    /// <returns>���� ����</returns>
    public bool UseStamina(float amount)
    {
        // ���¹̳ʰ� �����ϸ� false ��ȯ
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }

        // ����ϸ� ���¹̳� ���� �� true ��ȯ
        stamina.Subtract(amount);
        return true;
    }
}
