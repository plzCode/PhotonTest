using Photon.Pun;
using UnityEngine;

public class KirbyDamageStar : Item
{
    private Player player;
    private EnemyNumber enemyNumber;

    public float SaveNumber;

    public float moveSpeed = 3f;
    public float randomSpeed = 0f;

    public float dleeteTime;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        enemyNumber = GetComponent<EnemyNumber>();  // �ڱ� �ڽſ� �پ� �ִ� EnemyNumber ��������
    }

    void Start()
    {
        enemyNumber.Number = player.EatKirbyFormNum;
        player.EatKirbyFormNum = 0;
    }

    void Update()
    {
        if (this.photonView.IsMine)
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }

        dleeteTime += Time.deltaTime;

        if (dleeteTime > 8f && this.photonView.IsMine)
        {
            DestroySelf();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!this.photonView.IsMine) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            randomSpeed = Random.Range(-3f, 3f);
            moveSpeed = randomSpeed;
        }
    }
}
