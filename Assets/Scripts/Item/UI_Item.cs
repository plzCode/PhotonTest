using UnityEngine;

public class UI_Item : MonoBehaviour
{
    public ItemScriptable item;
    public Inventory inventory;

    public void OnClick()
    {
        inventory.UseItem(item);
    }
}
