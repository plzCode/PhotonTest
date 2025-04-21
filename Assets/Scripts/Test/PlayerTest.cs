using Photon.Pun;
using UnityEngine;

public class PlayerTest : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView playerView;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
    }

/*    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
    }*/

    private void FixedUpdate()
    {
        
    }
    [PunRPC]
    void SettingColor(int number, int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView == null) return;

        SpriteRenderer sr = targetView.GetComponentInChildren<SpriteRenderer>();
        if (sr == null) return;

        switch (number)
        {
            case 1:
                sr.color = new Color32(255, 255, 255, 255);
                break;
            case 2:
                sr.color = new Color32(255, 255, 0, 255);
                break;
            case 3:
                sr.color = new Color32(0, 0, 255, 255);
                break;
            case 4:
                sr.color = new Color32(0, 255, 0, 255);
                break;
        }
    }
}

