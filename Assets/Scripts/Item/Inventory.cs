using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemScriptable> items = new List<ItemScriptable>();
    public Player player;

    public void AddItem(ItemScriptable newItem)
    {
        if (newItem.isAutoUse)
        {
            UseItem(newItem); // 즉시 사용
        }
        else
        {
            if (!items.Contains(newItem))
                items.Add(newItem); // 인벤토리에 추가
        }
    }

    public void RemoveItem(ItemScriptable item)
    {
        items.Remove(item);
    }

    public void UseItem(ItemScriptable item)
    {
        Debug.Log($"사용: {item.itemName}");

        switch (item.type)
        {
            case ItemType.Heal:
                if (player != null)
                {
                    player.PlayerHP += item.effectValue; // 플레이어 체력 회복
                    if (player.PlayerHP > player.PlayerMaxHP)
                        player.PlayerHP = player.PlayerMaxHP; // 최대 체력 초과 방지
                    
                    if (player.health_Bar != null)
                    {
                        player.health_Bar.UpdateHealthBar(player.PlayerHP);
                    }
                    Debug.Log($"플레이어 체력: {player.PlayerHP}");
                }
                else
                {
                    Debug.LogWarning("Player reference is null.");
                }
                break;
            case ItemType.Ability:
                break;
        }

        RemoveItem(item); // 사용 후 삭제
    }
}
