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
            UseItem(newItem); // ��� ���
        }
        else
        {
            if (!items.Contains(newItem))
                items.Add(newItem); // �κ��丮�� �߰�
        }
    }

    public void RemoveItem(ItemScriptable item)
    {
        items.Remove(item);
    }

    public void UseItem(ItemScriptable item)
    {
        Debug.Log($"���: {item.itemName}");

        switch (item.type)
        {
            case ItemType.Heal:
                if (player != null)
                {
                    player.PlayerHP += item.effectValue; // �÷��̾� ü�� ȸ��
                    if (player.PlayerHP > player.PlayerMaxHP)
                        player.PlayerHP = player.PlayerMaxHP; // �ִ� ü�� �ʰ� ����
                    
                    if (player.health_Bar != null)
                    {
                        player.health_Bar.UpdateHealthBar(player.PlayerHP);
                    }
                    Debug.Log($"�÷��̾� ü��: {player.PlayerHP}");
                }
                else
                {
                    Debug.LogWarning("Player reference is null.");
                }
                break;
            case ItemType.Ability:
                break;
        }

        RemoveItem(item); // ��� �� ����
    }
}
