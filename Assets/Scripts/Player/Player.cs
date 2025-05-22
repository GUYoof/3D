using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �÷��̾��� �ֿ� ������Ʈ(PlayerController, PlayerCondition)�� �����ϰ�
/// CharacterManager�� ���� ���� ������ �����ϴ� Ŭ����
/// </summary>
public class Player : MonoBehaviour
{
    public PlayerController controller;  // �÷��̾� �Է� �� �̵� ����
    public PlayerCondition condition;    // �÷��̾� ����(ü��, ���¹̳� ��) ����

    public ItemData itemData;
    public Action addItem;

    public Transform dropPosition; // ��ӵǴ� �������� ��ġ

    private void Awake()
    {
        // CharacterManager�� ���� ���� Player ���� ����
        CharacterManager.Instance.Player = this;

        // �ʼ� ������Ʈ ��������
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
