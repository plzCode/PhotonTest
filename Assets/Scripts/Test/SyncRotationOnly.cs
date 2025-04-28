using Photon.Pun;
using UnityEngine;

public class SyncRotationOnly : MonoBehaviourPun, IPunObservable
{
    private Quaternion networkRotation;

    void Update()
    {
        if (!photonView.IsMine)
        {
            // ��Ʈ��ũ�� ���� ȸ���� ����
            transform.rotation = networkRotation;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �� ȸ���� ����
            stream.SendNext(transform.rotation);
        }
        else
        {
            // ���� ȸ�� �ޱ�
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}