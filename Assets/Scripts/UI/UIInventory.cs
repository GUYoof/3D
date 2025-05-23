using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// �κ��丮 UI�� �����ϴ� Ŭ����
/// ������ ȹ��, ���, ���, ��� ���� ��ɰ� ���� UI�� ������Ʈ
/// </summary>
public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;                         // �κ��丮 ���� �迭
    public GameObject inventoryWindow;               // �κ��丮 â ������Ʈ
    public Transform slotPanel;                      // ���Ե��� ��� �ִ� �θ� ������Ʈ
    public Transform dropPosition;                   // �������� ���� ��ġ

    [Header("���õ� ������ UI")]
    public TextMeshProUGUI selectedItemName;         // ���õ� ������ �̸�
    public TextMeshProUGUI selectedItemDescription;  // ���õ� ������ ����
    public TextMeshProUGUI selectedStatName;         // ������ ȿ�� �̸�
    public TextMeshProUGUI selectedStatValue;        // ������ ȿ�� ��ġ

    public GameObject useButton;                     // ��� ��ư
    public GameObject dropButton;                    // ������ ��ư

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;                           // ���� ���õ� ������
    int selectedItemIndex = 0;                       // ���õ� ���� �ε���

    int curEquipIndex;                               // ���� ������ ���� �ε���

    /// <summary>
    /// �κ��丮 �ʱ� ���� �� �̺�Ʈ ����, ���� �ʱ�ȭ
    /// </summary>
    void Start()
    {
        // �÷��̾� ��Ʈ�ѷ� �� ���� ���� ��������
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // �κ��丮 ��� �̺�Ʈ�� ������ �߰� �̺�Ʈ ����
        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;
        Debug.Log("������ ȣ��");

        // �κ��丮 â ��Ȱ��ȭ �ʱ�ȭ
        inventoryWindow.SetActive(false);

        // ���� �迭 �ʱ�ȭ �� ���Ը��� �ε���, �κ��丮 ���� ����
        slots = new ItemSlot[slotPanel.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        // ���õ� ������ ���� UI �ʱ�ȭ
        ClearSelectedItemWindow();
    }

    /// <summary>
    /// ���õ� ������ ������ �ʱ�ȭ�ϰ� ��ư�� ��Ȱ��ȭ
    /// </summary>
    void ClearSelectedItemWindow()
    {
        // ������ ���� �ؽ�Ʈ �ʱ�ȭ
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        // ��� ��� ��ư ���� ó��
        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    /// <summary>
    /// �κ��丮 â�� ����/���� ���¸� ���
    /// </summary>
    public void Toggle()
    {
        // ���� ���� �����Ͽ� â ���ų� ����
        inventoryWindow.SetActive(!IsOpen());
    }

    /// <summary>
    /// �κ��丮 â�� ���� �ִ��� ���θ� ��ȯ
    /// </summary>
    /// <returns>���� ������ true</returns>
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    /// <summary>
    /// ���ο� �������� �κ��丮�� �߰�
    /// ���� ������ ��� ���� ���Կ� �߰��ϰ�, �ƴϸ� �� �����̳� �ٴڿ� ���
    /// </summary>
    void AddItem()
    {
        Debug.Log("[AddItem] �Լ� ȣ���");
        // �÷��̾ ȹ���� ������ ������ ����
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data == null)
        {
            Debug.LogWarning("[AddItem] itemData�� null�Դϴ�.");
            return;
        }

        // ���� ������ �������̸� ���� ���� �� ������ �� �� ���� ã��
        if (data.canStack)
        {
            ItemSlot slot = getItemStack(data);
            if (slot != null)
            {
                slot.quantity++;  // ���� ����
                UpdateUI();       // UI ����
                CharacterManager.Instance.Player.itemData = null; // ������ �ʱ�ȭ
                return;
            }
        }

        // �� ���� ã��
        ItemSlot emptySlot = getEmptySlot();

        if (emptySlot != null)
        {
            // �� ���Կ� ������ �Ҵ� �� ���� 1�� ����
            emptySlot.item = data;
            emptySlot.quantity = 1;
            Debug.Log($"[AddItem] �� ���Կ� ������ �߰���: {data.name}");
            UpdateUI();   // UI ����
            CharacterManager.Instance.Player.itemData = null; // ������ �ʱ�ȭ
            return;
        }

        // ������ ������ �ٴڿ� ������ ���
        Debug.LogWarning("[AddItem] �κ��丮�� ������ ���� �������� ����մϴ�.");
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null; // ������ �ʱ�ȭ
    }


    /// <summary>
    /// �κ��丮 UI�� ���� ���� ���¿� ���� ����
    /// </summary>
    void UpdateUI()
    {
        // ��� ������ ��ȸ�ϸ� ������ ���� �� set ȣ��, ������ Clear ȣ��
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                slots[i].set();
            else
                slots[i].Clear();
        }
    }

    /// <summary>
    /// ���� �������� �����ϰ� ������ �ִ�ġ �̸��� ������ ã��
    /// </summary>
    /// <param name="data">ã�� ������ ������</param>
    /// <returns>�ش� ������ ������ ��ȯ, ������ null</returns>
    ItemSlot getItemStack(ItemData data)
    {
        // ������ ��ȸ�ϸ� ���� �������̸鼭 �ִ� ���� �̸��� ���� ��ȯ
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
                return slots[i];
        }
        return null;
    }

    /// <summary>
    /// ��� �ִ� ������ ��ȯ
    /// </summary>
    /// <returns>��� �ִ� ���� �Ǵ� null</returns>
    ItemSlot getEmptySlot()
    {
        // �������� ���� �� ���� ã��
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }
        return null;
    }

    /// <summary>
    /// �������� ���� ��ġ�� ���
    /// </summary>
    /// <param name="data">����� ������ ������</param>
    void ThrowItem(ItemData data)
    {
        // ������ ��� �������� dropPosition ��ġ�� ������ ȸ������ ����
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    /// <summary>
    /// �κ��丮���� �������� �����Ͽ� �� ������ ǥ��
    /// </summary>
    /// <param name="index">������ ���� �ε���</param>
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;  // �� �����̸� ����

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        // ���õ� ������ �⺻ ���� ǥ��
        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        // ������ ȿ�� ���� �ʱ�ȭ
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        // �����ۿ� ���Ե� ȿ������ �̸��� ������ ǥ�� (JumpBoost�� ���� �ð����� ǥ��)
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            var effect = selectedItem.consumables[i];

            selectedStatName.text += effect.type.ToString() + "\n";

            if (effect.type == ConsumableType.JumpBoost)
            {
                // JumpBoost�� "5�ʰ� ���� ��ȭ" �������� ǥ��
                selectedStatValue.text += $"{effect.value}�ʰ� ���� ��ȭ\n";
            }
            else
            {
                // �Ϲ� ȸ�� ȿ���� "+��ġ" �������� ǥ��
                selectedStatValue.text += $"+{effect.value}\n";
            }
        }

        // ������ Ÿ�Կ� ���� ��ư Ȱ��ȭ ó��
        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        dropButton.SetActive(true);
    }

    /// <summary>
    /// ���õ� �������� ����ϰ� ȿ���� ����
    /// </summary>
    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            ItemEffectHandler.Apply(controller, condition, selectedItem); // ȿ�� ó�� �Ͽ�ȭ
            RemoveSelectedItem();  // ��� �� ����
        }
    }

    /// <summary>
    /// ���õ� �������� ���
    /// </summary>
    public void OnDropButton()
    {
        ThrowItem(selectedItem);  // ������ ���
        RemoveSelectedItem();     // �κ��丮���� ����
    }

    /// <summary>
    /// ���õ� �������� ������ ���̰�, 0�� �Ǹ� ����
    /// </summary>
    void RemoveSelectedItem()
    {
        // ���� ����
        slots[selectedItemIndex].quantity--;

        // ������ 0 ���ϰ� �Ǹ� ���� �ʱ�ȭ �� ���� ���� �ʱ�ȭ
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();  // UI �ʱ�ȭ
        }

        UpdateUI();  // UI ����
    }
}