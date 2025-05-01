using UnityEngine;
public class ItemPickup : MonoBehaviour
{
    public ItemScriptable item;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ItemPickup OnTriggerEnter");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Item picked up: " + item.itemName);
            other.GetComponent<Player>().inventory.AddItem(item);
            Destroy(gameObject);
        }
    }
}