using Photon.Pun;
using UnityEngine;
public class ItemPickup : MonoBehaviour
{
    public ItemScriptable item;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().inventory.AddItem(item);
            Destroy(gameObject);

            if(other.GetComponent<PhotonView>().IsMine)
            {
                switch (item.type)
                {
                    case ItemType.Heal:
                        AudioManager.Instance.RPC_PlaySFX("Get_Item_Sound");
                        break;
                    case ItemType.Ability:
                        AudioManager.Instance.RPC_PlaySFX("Get_Item_Sound");
                        break;
                    default:
                        Debug.LogWarning("Unknown item type: " + item.type);
                        break;
                }
            }
            
        }
    }
}