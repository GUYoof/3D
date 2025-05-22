using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데미지를 받을 수 있는 객체 인터페이스
/// </summary>
public interface IDamagalbe
{
    void TakePysicalDamage(int damage);
}

/// <summary>
/// 플레이어의 체력, 허기, 스태미너 상태를 관리하는 클래스
/// </summary>
public class PlayerCondition : MonoBehaviour, IDamagalbe
{
    public UICondition uiCondition;             // 상태 수치를 포함한 UICondition 참조
    public event Action onTakeDamage;           // 데미지를 받을 때 호출될 이벤트

    // 상태 참조
    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }


    // 매 프레임마다 상태 수치를 갱신함
    void Update()
    {
        // 스태미너 자연 회복
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        // 체력이 0 이하가 되면 사망 처리
        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// 체력을 회복시킴
    /// </summary>
    /// <param name="amount">회복할 양</param>
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    /// <summary>
    /// 플레이어 사망 처리
    /// </summary>
    public void Die()
    {
        Debug.Log("죽었다");
    }

    /// <summary>
    /// 데미지를 받으면 체력 감소 및 이벤트 호출
    /// </summary>
    /// <param name="damage">받은 데미지 양</param>
    public void TakePysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    /// <summary>
    /// 스태미너 사용 시도. 충분할 경우 감소시키고 true 반환
    /// </summary>
    /// <param name="amount">사용할 스태미너 양</param>
    /// <returns>성공 여부</returns>
    public bool UseStamina(float amount)
    {
        // 스태미너가 부족하면 false 반환
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }

        // 충분하면 스태미너 차감 후 true 반환
        stamina.Subtract(amount);
        return true;
    }
}
