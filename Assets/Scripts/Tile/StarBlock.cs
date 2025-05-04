using Photon.Pun;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;


public class StarBlock : BlockManager
{
    public PhotonView photonView;

    [PunRPC]
    public override void Delete()
    {
        base.Delete();

        DestroyBlock();
    }

    [PunRPC]
    public void DestroyBlock()
    {
        // 삭제 이펙트 등 추가
        pv.RPC("EffectAdd", RpcTarget.All, "Delete Effect 30x30_0", transform.position);

        if (pv.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
