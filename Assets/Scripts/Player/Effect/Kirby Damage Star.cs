using Photon.Pun;
using UnityEngine;

public class KirbyDamageStar : MonoBehaviour
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

        if (enemyNumber == null)
        {
            Debug.LogError("EnemyNumber ������Ʈ�� �� ������Ʈ�� �������� �ʽ��ϴ�.");
        }
    }

    void Start()
    {
        enemyNumber.Number = player.EatKirbyFormNum;
        player.EatKirbyFormNum = 0;
    }

    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);

        dleeteTime += Time.deltaTime;

        if (dleeteTime > 8f)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            randomSpeed = Random.Range(-3f, 3f);
            moveSpeed = randomSpeed;
        }
    }
}
