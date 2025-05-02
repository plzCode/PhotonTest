using Photon.Pun;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;


public class StarBlock : BlockManager
{
    public PhotonView photonView;

    public void Start()
    {
        photonView = GetComponent<PhotonView>(); // PhotonView 초기화
    }

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
        EffectAdd("Delete Effect 30x30_0", transform.position);
        Destroy(gameObject);
    }
}
