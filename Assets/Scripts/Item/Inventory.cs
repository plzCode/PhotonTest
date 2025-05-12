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
        if (player != null && player.pView.IsMine == false) return;
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
                    player.pView.RPC("TakeHeal", RpcTarget.All, item.effectValue); // ü�� ȸ��

                }
                else
                {
                    Debug.LogWarning("Player reference is null.");
                }
                break;
            case ItemType.Ability:
                if (player != null && player.GetComponentInChildren<PlayerAnimatorController>())
                {
                    player.EatKirbyFormNum = (int)item.effectValue; // �ɷ� ����
                    player.pView.RPC("SyncFormNum", RpcTarget.AllBuffered, player.EatKirbyFormNum); // �ɷ� ����
                    player.GetComponentInChildren<PlayerAnimatorController>().ChangeForm(); // �ɷ� ���
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
                    player.pView.RPC("AddLife", RpcTarget.AllBuffered, item.effectValue); // ���� ����
                }
                else
                {
                    Debug.LogWarning("Player reference is null.");
                }
                break;
        }

        RemoveItem(item); // ��� �� ����
        UpdateInventoryUI();
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
        // ���� UI ����
        foreach (Transform child in inventoryUIParent)
        {
            if(child.GetComponent<Image>() != null)
            {
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < items.Count; i++)
        {

            // ���� ������ ĸó
            int index = i;

            // ������ UI ����
            GameObject itemUI = new GameObject(items[index].itemName);
            itemUI.layer = LayerMask.NameToLayer("UI"); // UI ���̾� ����
            itemUI.tag = "Item"; // �±� ����

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
            float scaleFactor = Mathf.Min(Screen.width, Screen.height) / 1080f;
            itemUI.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f) * scaleFactor;
            itemUI.transform.SetParent(inventoryUIParent);
        }


        
    }
}
