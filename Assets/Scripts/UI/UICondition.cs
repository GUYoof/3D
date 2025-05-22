using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �÷��̾��� UI �󿡼� ǥ�õǴ� ����
public class UICondition : MonoBehaviour
{
    public Condition health;   // ü�� ����
    public Condition hunger;   // ��� ����
    public Condition stamina;  // ���¹̳� ����

    /// <summary>
    /// ���� �� CharacterManager���� �÷��̾��� condition�� ���� UICondition ����
    /// </summary>
    void Start()
    {
        // �÷��̾��� condition�� �� UICondition ����
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }

    void Update()
    {

    }
}
