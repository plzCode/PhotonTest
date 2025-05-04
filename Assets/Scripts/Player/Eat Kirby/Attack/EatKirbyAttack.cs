using UnityEngine;
using Photon.Pun;

public class EatKirbyAttack : PlayerRagedManager
{
    public float moveSpeed = 8f;
    public float lifeTime = 5f; // 10초 뒤에 삭제
    private float timer = 0f;

    void Update()
    {
        if (!photonView.IsMine) return;

        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        timer += Time.deltaTime;

        if (timer > lifeTime)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().photonView.RPC("TakeDamage", RpcTarget.All, player.curAbility.attackPower);
            }
                PhotonNetwork.Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
                PhotonNetwork.Destroy(gameObject);
        }
    }
}