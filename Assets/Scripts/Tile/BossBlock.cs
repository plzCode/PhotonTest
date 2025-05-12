using Photon.Pun;
using UnityEngine;

public class BossBlock : BlockManager
{
    private int blockCount = 0;
    public void Update()
    {
       Debug.Log("Block Count: " + blockCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            blockCount++;

            if (PhotonNetwork.IsMasterClient && blockCount > 3) // 마스터 클라이언트만 RPC 호출
            {
                pv.RPC("AddBlock", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void AddBlock()
    {
        StartCoroutine(AddBlockDelay());
    }

    private System.Collections.IEnumerator AddBlockDelay()
    {
        yield return new WaitForSeconds(2f);

        pv.RPC("EffectAdd", RpcTarget.All, "BossRockBlock", transform.position);

        Destroy(gameObject);
    }
}
