using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어의 UI 상에서 표시되는 상태
public class UICondition : MonoBehaviour
{
    public Condition health;   // 체력 상태
    public Condition hunger;   // 허기 상태
    public Condition stamina;  // 스태미너 상태

    /// <summary>
    /// 시작 시 CharacterManager에서 플레이어의 condition에 현재 UICondition 연결
    /// </summary>
    void Start()
    {
        // 플레이어의 condition에 이 UICondition 연결
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }

    void Update()
    {

    }
}
