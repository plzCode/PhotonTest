using UnityEngine;
using Photon.Pun;

public class EatKirbyAttack : MonoBehaviourPun
{

    public float moveSpeed = 8f;
    public float lifeTime = 5f; // 10초 뒤에 삭제
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

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy") && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground") && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}