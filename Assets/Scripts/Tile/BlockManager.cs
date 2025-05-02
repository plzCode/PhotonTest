using Photon.Pun;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public PhotonView pv;
    public void Awake()
    {
        pv = GetComponent<PhotonView>(); // PhotonView √ ±‚»≠
    }

    public virtual void Delete()
    {

    }

    [PunRPC]
    protected virtual void EffectAdd(string effectName, Vector3 effectPos)
    {
        PhotonNetwork.Instantiate("Tile_Effect/" + effectName, effectPos, Quaternion.identity);
    }
}
