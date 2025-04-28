using Photon.Pun;
using UnityEngine;

public class SyncRotationOnly : MonoBehaviourPun, IPunObservable
{
    private Quaternion networkRotation;

    void Update()
    {
        if (!photonView.IsMine)
        {
            // 네트워크로 받은 회전만 적용
            transform.rotation = networkRotation;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 내 회전을 보냄
            stream.SendNext(transform.rotation);
        }
        else
        {
            // 상대방 회전 받기
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}