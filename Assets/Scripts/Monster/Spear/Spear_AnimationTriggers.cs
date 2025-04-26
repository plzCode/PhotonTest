using Photon.Pun;
using UnityEngine;

public class Spear_AnimationTriggers : MonoBehaviour
{
    [SerializeField] private GameObject spearPrefab;
    private Monster_Spear enemy => GetComponentInParent<Monster_Spear>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void CalledFunction()
    {
        Debug.Log("CalledFunction ȣ���");
        // RPC ȣ���� ������ Ŭ���̾�Ʈ������ ó��
        if (PhotonNetwork.IsMasterClient)
        {
            // ���� ȣ���� �����, RPC�� ��ȯ�մϴ�.
            enemy.photonView.RPC(
                "ThrowSpear",
                RpcTarget.All  // �Ǵ� RpcTarget.All
            );
        }
    }


}
