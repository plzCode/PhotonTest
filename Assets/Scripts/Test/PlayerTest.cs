using Photon.Pun;
using UnityEngine;

public class PlayerTest : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView playerView;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ���� �÷��̾��� �����͸� ��Ʈ��ũ�� ����
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // ��Ʈ��ũ���� �����͸� ����
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void FixedUpdate()
    {
        if (playerView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("�׽�Ʈ" + playerView.name);
            }
        }
    }
}

