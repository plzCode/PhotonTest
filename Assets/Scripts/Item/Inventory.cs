using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<ItemScriptable> items = new List<ItemScriptable>();
    public Transform[] itemPositions = new Transform[10]; // ������ UI�� ��ġ �迭
    public Player player;

    public void AddItem(ItemScriptable newItem)
    {
        if (newItem.isAutoUse)
        {
            UseItem(newItem); // ��� ���
        }
        else
        {
            if (items.Count >= 10)
            {
                Debug.LogWarning("Inventory is full. Cannot add more items.");
                return; // �ִ� 10�������� ���
            }
            //if (!items.Contains(newItem)) //�ߺ� ��뿩��
            {
                items.Add(newItem); // �κ��丮�� �߰�
                UpdateInventoryUI(); // UI ������Ʈ
            }
        }
    }

    public void RemoveItem(ItemScriptable item)
    {
        items.Remove(item);
    }
        
    public void UseItem(ItemScriptable item)
    {
        Debug.Log(player.GetComponent<PhotonView>().ViewID);
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
    // �κ��丮 UI�� �������� ��ġ
    private void UpdateInventoryUI()
    {
        // UI �θ� ��ü (Canvas �Ǵ� Inventory UI Panel)
        Transform inventoryUIParent = this.transform;
        if (inventoryUIParent == null)
        {
            Debug.LogError("InventoryUIParent not found in the scene.");
            return;
        }
        for (int i = 0; i < items.Count; i++)
        {

            // ���� ������ ĸó
            int index = i;

            // ������ UI ����
            GameObject itemUI = new GameObject(items[index].itemName);

            // RectTransform ����
            RectTransform rectTransform = itemUI.AddComponent<RectTransform>();
            rectTransform.anchoredPosition = itemPositions[index].position;

            // ������ ������ ����
            Image image = itemUI.AddComponent<Image>();
            image.sprite = items[index].icon;

            // ��ư �߰� �� �̺�Ʈ ���
            Button button = itemUI.AddComponent<Button>();
            button.onClick.AddListener(() =>
            {
                Debug.Log($"Button clicked for item: {items[index].itemName}");
                UseItem(items[index]);
                Destroy(itemUI); // ��� �� UI ����
            });

            // UI ũ�� �� �θ� ����
            itemUI.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            itemUI.transform.SetParent(inventoryUIParent);
        }


        
    }
}
