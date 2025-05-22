using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 아이템의 종류를 정의하는 열거형
public enum ItemType
{
    Equipable,   // 장비 가능 아이템
    Consumable,  // 소비 아이템
    Resource     // 자원 아이템
}

// 소비 아이템의 효과 타입 정의
public enum ConsumableType
{
    Health,  // 체력 회복
}

// 소비 아이템의 상세 정보 (효과 종류 및 수치)
[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;  // 회복 종류
    public float value;          // 회복량
}

/// <summary>
/// 인벤토리 아이템의 데이터 정의 ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;         // 아이템 이름
    public string description;         // 아이템 설명
    public ItemType type;              // 아이템 분류
    public Sprite icon;                // 인벤토리에 표시할 아이콘
    public GameObject dropPrefab;      // 필드에 드롭될 때 사용할 프리팹

    [Header("Stacking")]
    public bool canStack;              // 여러 개 쌓을 수 있는지 여부
    public int maxStackAmount;         // 최대 스택 수

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;  // 소비 아이템 효과들

    [Header("Equip")]
    public GameObject equipPrefab;     // 장비 착용 시 사용할 프리팹
}
