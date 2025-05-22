using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ȣ�ۿ� ������ ������Ʈ�� �ʿ��� ��� ����
public interface IInteractable
{

    public string GetInteractPrompt();

    public void OnInteract();
}

// ���� �� ��ȣ�ۿ� ������ ������ ������Ʈ
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;  // ������ ������ ����

    public string GetInteractPrompt()
    {
        // ������ �̸��� ������ ��� ��ȯ
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    /// <summary>
    /// �����۰� ��ȣ�ۿ� �� �÷��̾� �κ��丮�� ������ �߰� �� ������Ʈ ����
    /// </summary>
    public void OnInteract()
    {
        //// �÷��̾ ������ ������ ����
        //CharacterManager.Instance.Player.itemData = data;

        //// ������ �߰� �̺�Ʈ ȣ��
        //CharacterManager.Instance.Player.addItem?.Invoke();

        // ������ ������Ʈ ����
        Destroy(gameObject);
    }
}
