using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �κ��丮�� ���� ������ ������ ��Ÿ���� Ŭ����
public class ItemSlot : MonoBehaviour
{
    public ItemData item;                    // ���Կ� ��� ������ ������

    public Button Button;                    // Ŭ�� ó���� ��ư
    public Image icon;                       // ������ ������ �̹���
    public TextMeshProUGUI quantityText;     // ������ ���� �ؽ�Ʈ
    public Outline outline;                  // ���� �� �׵θ� ǥ�ÿ�

    public UIInventory inventory;            // ����� �κ��丮 UI

    public int index;                        // ���� �ε���
    public bool equipped;                    // ���� ����
    public int quantity;                     // ������ ����

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    /// <summary>
    /// ������ Ȱ��ȭ�� �� Outline ���� ����
    /// </summary>
    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    /// <summary>
    /// ���Կ� ������ ������ ǥ��
    /// </summary>
    public void set()
    {
        icon.gameObject.SetActive(true);                         // ������ ǥ��
        icon.sprite = item.icon;                                 // ������ �̹��� ����
        Debug.Log($"[ItemSlot] Setting icon: {item.displayName}, icon null? {item.icon == null}");
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // ���� ǥ�� (1 �̻��� ����)

        if (outline != null)
        {
            outline.enabled = equipped;                          // ���� ���ο� ���� Outline ǥ��
        }
    }

    /// <summary>
    /// ������ �ʱ�ȭ�Ͽ� �� ���·� ����
    /// </summary>
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    /// <summary>
    /// ������ Ŭ���Ǿ��� �� �κ��丮�� ���� �ε����� ����
    /// </summary>
    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
