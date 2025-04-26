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
        Debug.Log("CalledFunction 호출됨");
        // RPC 호출을 마스터 클라이언트에서만 처리
        if (PhotonNetwork.IsMasterClient)
        {
            // 직접 호출을 지우고, RPC로 전환합니다.
            enemy.photonView.RPC(
                "ThrowSpear",
                RpcTarget.All  // 또는 RpcTarget.All
            );
        }
    }


}
