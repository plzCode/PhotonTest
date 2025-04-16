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
        throw new System.NotImplementedException();
    }

    private void FixedUpdate()
    {
        if (playerView.IsMine) 
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                Debug.Log("Å×½ºÆ®" + playerView.name);
            }
        }
    }

}
