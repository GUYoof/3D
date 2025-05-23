using UnityEngine;


// �������� �Һ� ȿ���� ó���ϴ� ���� Ŭ����
public static class ItemEffectHandler
{
    /// <summary>
    /// �Һ� �������� ȿ���� �÷��̾�� ����
    /// </summary>
    /// <param name="player">���� ��ȭ �� ��Ʈ�� ���� ȿ���� ������ �÷��̾�</param>
    /// <param name="condition">ü��, ���¹̳� �� ���� ȿ���� ������ ������Ʈ</param>
    /// <param name="data">������ ������ ������</param>
    public static void Apply(PlayerController player, PlayerCondition condition, ItemData data)
    {
        // �Һ��� �������� �ƴϰų�, �Һ� ȿ�� ������ ������ ó�� �ߴ�
        if (data.type != ItemType.Consumable || data.consumables == null)
            return;

        // �� �Һ� ȿ���� ��ȸ�ϸ鼭 ����
        foreach (var effect in data.consumables)
        {
            switch (effect.type)
            {
                case ConsumableType.JumpBoost:
                    // ���� ��ȭ ȿ�� ���� (���� �ð� ���)
                    player?.ApplyJumpBoost(effect.value);
                    break;

                case ConsumableType.Health:
                    // ü�� ȸ�� ȿ�� ����
                    condition?.Heal(effect.value);
                    break;

                case ConsumableType.Stamina:
                    // ���¹̳� ȸ�� ȿ�� ����
                    condition?.Eat(effect.value);
                    break;
            }
        }
    }
}
