using Photon.Pun;
using UnityEngine;

public class Meta_ItemPickUp : MonoBehaviour
{

    public ItemScriptable item;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().inventory.AddItem(item);
            Destroy(gameObject);

            BossMetaKnight.Instance.readyState.Count++;

            if (other.GetComponent<PhotonView>().IsMine)
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
