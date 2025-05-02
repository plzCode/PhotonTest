using Photon.Pun;
using UnityEngine;

public class BigStarBlock : BlockManager
{
    public PhotonView photonView;

    public void Start()
    {
        photonView = GetComponent<PhotonView>(); // PhotonView �ʱ�ȭ
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
        // ���� ����Ʈ �� �߰�
        EffectAdd("Big Delete Effect 30x30_0", transform.position);
        Destroy(gameObject);
    }
}
