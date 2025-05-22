using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어의 주요 컴포넌트(PlayerController, PlayerCondition)를 관리하고
/// CharacterManager를 통해 전역 접근을 설정하는 클래스
/// </summary>
public class Player : MonoBehaviour
{
    public PlayerController controller;  // 플레이어 입력 및 이동 제어
    public PlayerCondition condition;    // 플레이어 상태(체력, 스태미너 등) 제어

    public ItemData itemData;
    public Action addItem;

    public Transform dropPosition; // 드롭되는 아이템의 위치

    private void Awake()
    {
        // CharacterManager를 통해 전역 Player 접근 설정
        CharacterManager.Instance.Player = this;

        // 필수 컴포넌트 가져오기
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
