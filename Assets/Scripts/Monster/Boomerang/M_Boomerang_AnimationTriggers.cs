using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class M_Boomerang_AnimationTriggers : MonoBehaviour
{
    private Monster_Boomerang enemy => GetComponentInParent<Monster_Boomerang>();
    [SerializeField] private GameObject boomerangPrefab;
    [SerializeField] private Transform startPos;
    private GameObject currentBoomerang;
    private Boomerang_Obj boomerang_Script;
    

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void PickupBoomerang()
    {
        currentBoomerang = Instantiate(boomerangPrefab,startPos.position,Quaternion.identity);
        boomerang_Script = currentBoomerang.GetComponent<Boomerang_Obj>();
        boomerang_Script.SetVelocity(0, 0);
        if (enemy.facingDir==-1) 
        {
            currentBoomerang.transform.Rotate(0, 180, 0);
            boomerang_Script.facingDir = -1;
        }
    }

    private void PickupBoomerangRPC()
    {
        //Debug.Log("CalledFunction 호출됨");
        // RPC 호출을 마스터 클라이언트에서만 처리
        if (PhotonNetwork.IsMasterClient)
        {
            // 직접 호출을 지우고, RPC로 전환합니다.
            enemy.photonView.RPC(
                "PickupBoomerang",
                RpcTarget.All  // 또는 RpcTarget.All
            );
        }
    }

    private void ThrowBoomerangRPC()
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    // 직접 호출을 지우고, RPC로 전환합니다.
        //    enemy.photonView.RPC(
        //        "ThrowBoomerang",
        //        RpcTarget.All  // 또는 RpcTarget.All
        //    );
        //}
        enemy.ThrowBoomerang();

    }
    
    
}
