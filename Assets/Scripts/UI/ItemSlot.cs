using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리의 개별 아이템 슬롯을 나타내는 클래스
public class ItemSlot : MonoBehaviour
{
    public ItemData item;                    // 슬롯에 담긴 아이템 데이터

    public Button Button;                    // 클릭 처리용 버튼
    public Image icon;                       // 아이템 아이콘 이미지
    public TextMeshProUGUI quantityText;     // 아이템 수량 텍스트
    public Outline outline;                  // 장착 시 테두리 표시용

    public UIInventory inventory;            // 연결된 인벤토리 UI

    public int index;                        // 슬롯 인덱스
    public bool equipped;                    // 장착 여부
    public int quantity;                     // 아이템 수량

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    /// <summary>
    /// 슬롯이 활성화될 때 Outline 상태 갱신
    /// </summary>
    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    /// <summary>
    /// 슬롯에 아이템 정보를 표시
    /// </summary>
    public void set()
    {
        icon.gameObject.SetActive(true);                         // 아이콘 표시
        icon.sprite = item.icon;                                 // 아이콘 이미지 설정
        Debug.Log($"[ItemSlot] Setting icon: {item.displayName}, icon null? {item.icon == null}");
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // 수량 표시 (1 이상일 때만)

        if (outline != null)
        {
            outline.enabled = equipped;                          // 장착 여부에 따라 Outline 표시
        }
    }

    /// <summary>
    /// 슬롯을 초기화하여 빈 상태로 만듦
    /// </summary>
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    /// <summary>
    /// 슬롯이 클릭되었을 때 인벤토리에 선택 인덱스를 전달
    /// </summary>
    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
