using UnityEngine;
using Photon.Pun;

public class EatKirbyAttack : MonoBehaviourPun
{
    private Player player => GetComponent<Player>();

    public float moveSpeed = 8f;
    public float lifeTime = 5f; // 10�� �ڿ� ����
    private float timer = 0f;

    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);

        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            PhotonNetwork.Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}