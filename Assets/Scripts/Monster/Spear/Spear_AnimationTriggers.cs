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

    private void ThrowSpear()
    {
        PhotonNetwork.Instantiate("Monster_Effect/" + spearPrefab.name, transform.position, Quaternion.identity);
    }
}
