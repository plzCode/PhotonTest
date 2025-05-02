using UnityEngine;
using Photon.Pun;

public class EatKirbyAttack : PlayerRagedManager
{
    public float moveSpeed = 8f;
    public float lifeTime = 5f; // 10�� �ڿ� ����
    private float timer = 0f;

    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);

        timer += Time.deltaTime;
        if (timer > lifeTime && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().photonView.RPC("TakeDamage", RpcTarget.All, player.curAbility.attackPower);
            }
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.Destroy(gameObject);
        }
    }
}