using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 인벤토리 UI를 관리하는 클래스
/// 아이템 획득, 사용, 장비, 드롭 등의 기능과 슬롯 UI를 업데이트
/// </summary>
public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;                         // 인벤토리 슬롯 배열
    public GameObject inventoryWindow;               // 인벤토리 창 오브젝트
    public Transform slotPanel;                      // 슬롯들을 담고 있는 부모 오브젝트
    public Transform dropPosition;                   // 아이템을 버릴 위치

    [Header("선택된 아이템 UI")]
    public TextMeshProUGUI selectedItemName;         // 선택된 아이템 이름
    public TextMeshProUGUI selectedItemDescription;  // 선택된 아이템 설명
    public TextMeshProUGUI selectedStatName;         // 아이템 효과 이름
    public TextMeshProUGUI selectedStatValue;        // 아이템 효과 수치

    public GameObject useButton;                     // 사용 버튼
    public GameObject dropButton;                    // 버리기 버튼

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;                           // 현재 선택된 아이템
    int selectedItemIndex = 0;                       // 선택된 슬롯 인덱스

    int curEquipIndex;                               // 현재 장착된 슬롯 인덱스

    /// <summary>
    /// 인벤토리 초기 설정 및 이벤트 연결, 슬롯 초기화
    /// </summary>
    void Start()
    {
        // 플레이어 컨트롤러 및 상태 정보 가져오기
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // 인벤토리 토글 이벤트와 아이템 추가 이벤트 연결
        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;
        Debug.Log("아이템 호출");

        // 인벤토리 창 비활성화 초기화
        inventoryWindow.SetActive(false);

        // 슬롯 배열 초기화 및 슬롯마다 인덱스, 인벤토리 참조 지정
        slots = new ItemSlot[slotPanel.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        // 선택된 아이템 정보 UI 초기화
        ClearSelectedItemWindow();
    }

    /// <summary>
    /// 선택된 아이템 정보를 초기화하고 버튼을 비활성화
    /// </summary>
    void ClearSelectedItemWindow()
    {
        // 아이템 정보 텍스트 초기화
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        // 모든 기능 버튼 숨김 처리
        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    /// <summary>
    /// 인벤토리 창의 열림/닫힘 상태를 토글
    /// </summary>
    public void Toggle()
    {
        // 현재 상태 반전하여 창 열거나 닫음
        inventoryWindow.SetActive(!IsOpen());
    }

    /// <summary>
    /// 인벤토리 창이 열려 있는지 여부를 반환
    /// </summary>
    /// <returns>열려 있으면 true</returns>
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    /// <summary>
    /// 새로운 아이템을 인벤토리에 추가
    /// 스택 가능한 경우 기존 슬롯에 추가하고, 아니면 빈 슬롯이나 바닥에 드롭
    /// </summary>
    void AddItem()
    {
        Debug.Log("[AddItem] 함수 호출됨");
        // 플레이어가 획득한 아이템 데이터 참조
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data == null)
        {
            Debug.LogWarning("[AddItem] itemData가 null입니다.");
            return;
        }

        // 스택 가능한 아이템이면 기존 슬롯 중 수량이 덜 찬 슬롯 찾기
        if (data.canStack)
        {
            ItemSlot slot = getItemStack(data);
            if (slot != null)
            {
                slot.quantity++;  // 수량 증가
                UpdateUI();       // UI 갱신
                CharacterManager.Instance.Player.itemData = null; // 아이템 초기화
                return;
            }
        }

        // 빈 슬롯 찾기
        ItemSlot emptySlot = getEmptySlot();

        if (emptySlot != null)
        {
            // 빈 슬롯에 아이템 할당 및 수량 1로 설정
            emptySlot.item = data;
            emptySlot.quantity = 1;
            Debug.Log($"[AddItem] 빈 슬롯에 아이템 추가됨: {data.name}");
            UpdateUI();   // UI 갱신
            CharacterManager.Instance.Player.itemData = null; // 아이템 초기화
            return;
        }

        // 슬롯이 없으면 바닥에 아이템 드롭
        Debug.LogWarning("[AddItem] 인벤토리에 공간이 없어 아이템을 드롭합니다.");
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null; // 아이템 초기화
    }


    /// <summary>
    /// 인벤토리 UI를 현재 슬롯 상태에 따라 갱신
    /// </summary>
    void UpdateUI()
    {
        // 모든 슬롯을 순회하며 아이템 존재 시 set 호출, 없으면 Clear 호출
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                slots[i].set();
            else
                slots[i].Clear();
        }
    }

    /// <summary>
    /// 같은 아이템이 존재하고 수량이 최대치 미만인 슬롯을 찾음
    /// </summary>
    /// <param name="data">찾을 아이템 데이터</param>
    /// <returns>해당 슬롯이 있으면 반환, 없으면 null</returns>
    ItemSlot getItemStack(ItemData data)
    {
        // 슬롯을 순회하며 같은 아이템이면서 최대 스택 미만인 슬롯 반환
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
                return slots[i];
        }
        return null;
    }

    /// <summary>
    /// 비어 있는 슬롯을 반환
    /// </summary>
    /// <returns>비어 있는 슬롯 또는 null</returns>
    ItemSlot getEmptySlot()
    {
        // 아이템이 없는 빈 슬롯 찾기
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }
        return null;
    }

    /// <summary>
    /// 아이템을 현재 위치에 드롭
    /// </summary>
    /// <param name="data">드롭할 아이템 데이터</param>
    void ThrowItem(ItemData data)
    {
        // 아이템 드롭 프리팹을 dropPosition 위치에 무작위 회전으로 생성
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    /// <summary>
    /// 인벤토리에서 아이템을 선택하여 상세 정보를 표시
    /// </summary>
    /// <param name="index">선택한 슬롯 인덱스</param>
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;  // 빈 슬롯이면 무시

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        // 선택된 아이템 기본 정보 표시
        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        // 아이템 효과 정보 초기화
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        // 아이템에 포함된 효과들을 이름과 값으로 표시 (JumpBoost는 지속 시간으로 표시)
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            var effect = selectedItem.consumables[i];

            selectedStatName.text += effect.type.ToString() + "\n";

            if (effect.type == ConsumableType.JumpBoost)
            {
                // JumpBoost는 "5초간 점프 강화" 형식으로 표시
                selectedStatValue.text += $"{effect.value}초간 점프 강화\n";
            }
            else
            {
                // 일반 회복 효과는 "+수치" 형식으로 표시
                selectedStatValue.text += $"+{effect.value}\n";
            }
        }

        // 아이템 타입에 따른 버튼 활성화 처리
        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        dropButton.SetActive(true);
    }

    /// <summary>
    /// 선택된 아이템을 사용하고 효과를 적용
    /// </summary>
    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            ItemEffectHandler.Apply(controller, condition, selectedItem); // 효과 처리 일원화
            RemoveSelectedItem();  // 사용 후 제거
        }
    }

    /// <summary>
    /// 선택된 아이템을 드롭
    /// </summary>
    public void OnDropButton()
    {
        ThrowItem(selectedItem);  // 아이템 드롭
        RemoveSelectedItem();     // 인벤토리에서 제거
    }

    /// <summary>
    /// 선택된 아이템의 수량을 줄이고, 0이 되면 제거
    /// </summary>
    void RemoveSelectedItem()
    {
        // 수량 감소
        slots[selectedItemIndex].quantity--;

        // 수량이 0 이하가 되면 슬롯 초기화 및 선택 정보 초기화
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();  // UI 초기화
        }

        UpdateUI();  // UI 갱신
    }
}