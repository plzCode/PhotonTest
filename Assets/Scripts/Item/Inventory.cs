using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<ItemScriptable> items = new List<ItemScriptable>();
    public Transform[] itemPositions = new Transform[10]; // 아이템 UI의 위치 배열
    public Player player;

    public void AddItem(ItemScriptable newItem)
    {
        if (newItem.isAutoUse)
        {
            UseItem(newItem); // 즉시 사용
        }
        else
        {
            if (items.Count >= 10)
            {
                Debug.LogWarning("Inventory is full. Cannot add more items.");
                return; // 최대 10개까지만 허용
            }
            //if (!items.Contains(newItem)) //중복 허용여부
            {
                items.Add(newItem); // 인벤토리에 추가
                UpdateInventoryUI(); // UI 업데이트
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
    // 인벤토리 UI를 원형으로 배치
    private void UpdateInventoryUI()
    {
        // UI 부모 객체 (Canvas 또는 Inventory UI Panel)
        Transform inventoryUIParent = this.transform;
        if (inventoryUIParent == null)
        {
            Debug.LogError("InventoryUIParent not found in the scene.");
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            // 아이템 UI 생성 또는 가져오기
            GameObject itemUI = new GameObject(items[i].itemName);
            if (itemUI == null)
            {
                Debug.LogError("ItemUI prefab not found in Resources/UI.");
                return;
            }
            // 아이템 UI의 위치를 미리 지정된 위치로 설정
            itemUI.AddComponent<RectTransform>();
            itemUI.GetComponent<RectTransform>().anchoredPosition = itemPositions[i].position;
            // 컴포넌트 추가
            itemUI.AddComponent<Image>().sprite = items[i].icon; // 아이템 아이콘 설정
            itemUI.AddComponent<Button>(); // Button 추가
            itemUI.GetComponent<Button>().onClick.AddListener(() => UseItem(items[i])); // 클릭 시 아이템 사용
            itemUI.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            itemUI.transform.SetParent(inventoryUIParent); // 부모 설정



        }
    }
}
