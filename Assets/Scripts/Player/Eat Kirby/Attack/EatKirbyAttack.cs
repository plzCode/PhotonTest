using UnityEngine;
using Photon.Pun;

public class EatKirbyAttack : MonoBehaviourPun
{
    private Player player => GetComponentInParent<Player>();

    public float moveSpeed = 8f;
    public float lifeTime = 10f; // 10초 뒤에 삭제
    private float timer = 0f;

    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);

        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            Destroy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("적에게" + player.curAbility.attackPower + "만큼 데미지를 줌");
        }

        // Ground 레이어 체크 (충돌했을 때 삭제)
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}