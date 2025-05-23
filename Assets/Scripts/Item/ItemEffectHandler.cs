using UnityEngine;


// 아이템의 소비 효과를 처리하는 정적 클래스
public static class ItemEffectHandler
{
    /// <summary>
    /// 소비 아이템의 효과를 플레이어에게 적용
    /// </summary>
    /// <param name="player">점프 강화 등 컨트롤 관련 효과를 적용할 플레이어</param>
    /// <param name="condition">체력, 스태미나 등 상태 효과를 적용할 컴포넌트</param>
    /// <param name="data">적용할 아이템 데이터</param>
    public static void Apply(PlayerController player, PlayerCondition condition, ItemData data)
    {
        // 소비형 아이템이 아니거나, 소비 효과 정보가 없으면 처리 중단
        if (data.type != ItemType.Consumable || data.consumables == null)
            return;

        // 각 소비 효과를 순회하면서 적용
        foreach (var effect in data.consumables)
        {
            switch (effect.type)
            {
                case ConsumableType.JumpBoost:
                    // 점프 강화 효과 적용 (지속 시간 기반)
                    player?.ApplyJumpBoost(effect.value);
                    break;

                case ConsumableType.Health:
                    // 체력 회복 효과 적용
                    condition?.Heal(effect.value);
                    break;

                case ConsumableType.Stamina:
                    // 스태미나 회복 효과 적용
                    condition?.Eat(effect.value);
                    break;
            }
        }
    }
}
