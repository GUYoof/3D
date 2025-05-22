using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상호작용 가능한 오브젝트에 필요한 기능 정의
public interface IInteractable
{

    public string GetInteractPrompt();

    public void OnInteract();
}

// 게임 내 상호작용 가능한 아이템 오브젝트
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;  // 아이템 데이터 참조

    public string GetInteractPrompt()
    {
        // 아이템 이름과 설명을 묶어서 반환
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    /// <summary>
    /// 아이템과 상호작용 시 플레이어 인벤토리에 아이템 추가 및 오브젝트 제거
    /// </summary>
    public void OnInteract()
    {
        //// 플레이어에 아이템 데이터 전달
        //CharacterManager.Instance.Player.itemData = data;

        //// 아이템 추가 이벤트 호출
        //CharacterManager.Instance.Player.addItem?.Invoke();

        // 씬에서 오브젝트 제거
        Destroy(gameObject);
    }
}
