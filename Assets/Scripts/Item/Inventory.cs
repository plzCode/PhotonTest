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
        if (player != null && player.pView.IsMine == false) return;
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
        if (player != null && player.pView.IsMine == false) return;
        items.Remove(item);
    }


    public void UseItem(ItemScriptable item)
    {
        if (player != null && player.pView.IsMine == false) return;
        switch (item.type)
        {
            case ItemType.Heal:
                if (player != null)
                {
                    player.pView.RPC("TakeHeal", RpcTarget.All, item.effectValue); // 체력 회복

                }
                else
                {
                    Debug.LogWarning("Player reference is null.");
                }
                break;
            case ItemType.Ability:
                if (player != null && player.GetComponentInChildren<PlayerAnimatorController>())
                {
                    player.EatKirbyFormNum = (int)item.effectValue; // 능력 변경
                    player.pView.RPC("SyncFormNum", RpcTarget.AllBuffered, player.EatKirbyFormNum); // 능력 변경
                    player.GetComponentInChildren<PlayerAnimatorController>().ChangeForm(); // 능력 사용
                }
                else
                {
                    Debug.LogWarning("Player reference is null.");
                }
                    break;
            case ItemType.LifeUp:
                if (player != null)
                {
                    AudioManager.Instance.RPC_PlaySFX("LifeUp");
                    player.pView.RPC("AddLife", RpcTarget.AllBuffered, item.effectValue); // 생명 증가
                }
                else
                {
                    Debug.LogWarning("Player reference is null.");
                }
                break;
        }

        RemoveItem(item); // 사용 후 삭제
        UpdateInventoryUI();
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
        // 기존 UI 제거
        foreach (Transform child in inventoryUIParent)
        {
            if(child.GetComponent<Image>() != null)
            {
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < items.Count; i++)
        {

            // 로컬 변수로 캡처
            int index = i;

            // 아이템 UI 생성
            GameObject itemUI = new GameObject(items[index].itemName);
            itemUI.layer = LayerMask.NameToLayer("UI"); // UI 레이어 설정
            itemUI.tag = "Item"; // 태그 설정

            // RectTransform 설정
            RectTransform rectTransform = itemUI.AddComponent<RectTransform>();
            rectTransform.anchoredPosition = itemPositions[index].position;

            // 아이템 아이콘 설정
            Image image = itemUI.AddComponent<Image>();
            image.sprite = items[index].icon;

            // 버튼 추가 및 이벤트 등록
            Button button = itemUI.AddComponent<Button>();
            button.onClick.AddListener(() =>
            {
                Debug.Log($"Button clicked for item: {items[index].itemName}");
                UseItem(items[index]);
                Destroy(itemUI); // 사용 후 UI 제거
            });

            // UI 크기 및 부모 설정
            float scaleFactor = Mathf.Min(Screen.width, Screen.height) / 1080f;
            itemUI.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f) * scaleFactor;
            itemUI.transform.SetParent(inventoryUIParent);
        }


        
    }
}
