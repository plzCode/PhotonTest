using Photon.Pun;
using UnityEngine;

public class PlayerRegister : MonoBehaviourPun
{
    private void Start()
    {
        // 모든 클라이언트에 자기 자신을 GameManager에 등록시키는 RPC
        photonView.RPC("RegisterToGameManager", RpcTarget.AllBuffered, photonView.ViewID);
    }

    [PunRPC]
    public void RegisterToGameManager(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            GameManager.Instance.RegisterPlayer(targetView.gameObject);
        }

        GameObject bonkersObj = GameObject.Find("Bonkers");
        if (bonkersObj != null)
        {
            Boss_Bonkers bonkers = bonkersObj.GetComponent<Boss_Bonkers>();
            bonkers.UpdateCurrentPlayersCollision();
        }
        

    }
}