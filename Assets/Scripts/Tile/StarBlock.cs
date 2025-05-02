using Photon.Pun;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;


public class StarBlock : BlockManager
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
        EffectAdd("Delete Effect 30x30_0", transform.position);
        Destroy(gameObject);
    }
}
