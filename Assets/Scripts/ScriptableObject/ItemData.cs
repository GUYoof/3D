using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �������� ������ �����ϴ� ������
public enum ItemType
{
    Equipable,   // ��� ���� ������
    Consumable,  // �Һ� ������
    Resource     // �ڿ� ������
}

// �Һ� �������� ȿ�� Ÿ�� ����
public enum ConsumableType
{
    Health,  // ü�� ȸ��
}

// �Һ� �������� �� ���� (ȿ�� ���� �� ��ġ)
[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;  // ȸ�� ����
    public float value;          // ȸ����
}

/// <summary>
/// �κ��丮 �������� ������ ���� ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;         // ������ �̸�
    public string description;         // ������ ����
    public ItemType type;              // ������ �з�
    public Sprite icon;                // �κ��丮�� ǥ���� ������
    public GameObject dropPrefab;      // �ʵ忡 ��ӵ� �� ����� ������

    [Header("Stacking")]
    public bool canStack;              // ���� �� ���� �� �ִ��� ����
    public int maxStackAmount;         // �ִ� ���� ��

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;  // �Һ� ������ ȿ����

    [Header("Equip")]
    public GameObject equipPrefab;     // ��� ���� �� ����� ������
}
