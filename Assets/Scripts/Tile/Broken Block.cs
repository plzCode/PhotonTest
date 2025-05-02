using Photon.Pun;
using UnityEngine;

public class BrokenBlock : BlockManager
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PhotonNetwork.IsMasterClient) // ������ Ŭ���̾�Ʈ�� RPC ȣ��
            {
                pv.RPC("DestroyBlock", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void DestroyBlock()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    private System.Collections.IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        pv.RPC("EffectAdd", RpcTarget.All, "Delete Effect 30x30_0", transform.position);

        Destroy(gameObject);
    }

}
