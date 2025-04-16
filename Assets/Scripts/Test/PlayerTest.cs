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
            // 로컬 플레이어의 데이터를 네트워크로 전송
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // 네트워크에서 데이터를 수신
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
                Debug.Log("테스트" + playerView.name);
            }
        }
    }
}

