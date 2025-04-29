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
        enemyNumber = GetComponent<EnemyNumber>();  // 자기 자신에 붙어 있는 EnemyNumber 가져오기

        if (enemyNumber == null)
        {
            Debug.LogError("EnemyNumber 컴포넌트가 이 오브젝트에 존재하지 않습니다.");
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
