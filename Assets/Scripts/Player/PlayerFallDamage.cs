using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 낙하 데미지를 계산하여 PlayerCondition에 전달
/// </summary>
public class PlayerFallDamage : MonoBehaviour
{
    // 낙하 데미지를 받을 수 있는 PlayerCondition 컴포넌트
    private PlayerCondition playerCondition;

    // 이 거리 이하로 떨어지면 데미지 없음
    public float fallThreshold = 5f;

    // 낙하 거리 단위당 데미지 배수
    public float damageMultiplier = 10f;

    private float lastYPosition;     // 낙하 시작 Y좌표
    private bool isFalling = false;  // 낙하 중인지 여부

    void Awake()
    {
        // 같은 오브젝트에서 PlayerCondition 컴포넌트를 가져옴
        playerCondition = GetComponent<PlayerCondition>();

        if (playerCondition == null)
        {
            Debug.LogError("PlayerCondition 컴포넌트를 찾을 수 없습니다!");
        }
    }

    void Update()
    {
        // 낙하 중이면 시작 위치 저장
        if (IsFalling())
        {
            if (!isFalling)
            {
                lastYPosition = transform.position.y;
                isFalling = true;
            }
        }
    }

    /// <summary>
    /// 충돌 시 낙하 거리 계산 후 데미지 적용
    /// </summary>
    /// <param name="collision">충돌 정보</param>
    void OnCollisionEnter(Collision collision)
    {
        if (isFalling)
        {
            float fallDistance = lastYPosition - transform.position.y;

            if (fallDistance > fallThreshold)
            {
                float damage = (fallDistance - fallThreshold) * damageMultiplier;

                // 연동된 PlayerCondition에 데미지를 전달
                playerCondition?.TakePysicalDamage(Mathf.RoundToInt(damage));
            }

            isFalling = false;
        }
    }

    /// <summary>
    /// Rigidbody가 아래로 움직이는지 확인하여 낙하 중인지 판단
    /// </summary>
    /// <returns>낙하 중이면 true</returns>
    bool IsFalling()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        return rb != null && rb.velocity.y < -0.1f;
    }
}
