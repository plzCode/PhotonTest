using Photon.Pun;
using UnityEngine;

public class BigStarBlock : BlockManager
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
        EffectAdd("Big Delete Effect 30x30_0", transform.position);
        Destroy(gameObject);
    }
}
