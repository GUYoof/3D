using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� ���� �������� ����Ͽ� PlayerCondition�� ����
/// </summary>
public class PlayerFallDamage : MonoBehaviour
{
    // ���� �������� ���� �� �ִ� PlayerCondition ������Ʈ
    private PlayerCondition playerCondition;

    // �� �Ÿ� ���Ϸ� �������� ������ ����
    public float fallThreshold = 5f;

    // ���� �Ÿ� ������ ������ ���
    public float damageMultiplier = 10f;

    private float lastYPosition;     // ���� ���� Y��ǥ
    private bool isFalling = false;  // ���� ������ ����

    void Awake()
    {
        // ���� ������Ʈ���� PlayerCondition ������Ʈ�� ������
        playerCondition = GetComponent<PlayerCondition>();

        if (playerCondition == null)
        {
            Debug.LogError("PlayerCondition ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    void Update()
    {
        // ���� ���̸� ���� ��ġ ����
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
    /// �浹 �� ���� �Ÿ� ��� �� ������ ����
    /// </summary>
    /// <param name="collision">�浹 ����</param>
    void OnCollisionEnter(Collision collision)
    {
        if (isFalling)
        {
            float fallDistance = lastYPosition - transform.position.y;

            if (fallDistance > fallThreshold)
            {
                float damage = (fallDistance - fallThreshold) * damageMultiplier;

                // ������ PlayerCondition�� �������� ����
                playerCondition?.TakePysicalDamage(Mathf.RoundToInt(damage));
            }

            isFalling = false;
        }
    }

    /// <summary>
    /// Rigidbody�� �Ʒ��� �����̴��� Ȯ���Ͽ� ���� ������ �Ǵ�
    /// </summary>
    /// <returns>���� ���̸� true</returns>
    bool IsFalling()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        return rb != null && rb.velocity.y < -0.1f;
    }
}
