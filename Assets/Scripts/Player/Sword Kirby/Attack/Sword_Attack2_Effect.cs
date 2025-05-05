using Photon.Pun;
using UnityEngine;

public class Sword_Attack2_Effect : PlayerRagedManager
{
    private Animator anim;

    [SerializeField] private float flightTime = 1.0f;

    public float speed = 10f;
    private float lifeTime = 10f;  // 초기 수명 설정

    public bool AttackDelay = false;
    public bool AttackEnemy = false;
    public bool ReturnPlayer = false;

    // Rigidbody2D와 PhotonView를 초기화합니다.
    public void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Start()
    {
    }

    public void Update()
    {
        if (!photonView.IsMine) return;

        transform.Translate(speed * Time.deltaTime, 0, 0);


        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime <= 0f && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return; // 현재 클라이언트가 소유하지 않은 객체는 충돌 처리하지 않음

        if (collision.gameObject.CompareTag("Enemy"))
        {
            lifeTime = 10f;  // 적과 충돌 시 생명 주기를 초기화합니다.

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().photonView.RPC("TakeDamage", RpcTarget.All, player.curAbility.attackPower);
            }
        }

        if (collision.gameObject.CompareTag("Ground") && PhotonNetwork.IsMasterClient)
        {
            player.CutterUpgrade = 0;
            PhotonNetwork.Destroy(gameObject);  // 제거
        }

        if (collision.gameObject.CompareTag("StarBlock") && PhotonNetwork.IsMasterClient)
        {
            player.CutterUpgrade = 0;
            PhotonNetwork.Destroy(gameObject);  // 제거

            BigStarBlock enemy = collision.gameObject.GetComponent<BigStarBlock>();
            if (enemy != null)
            {
                enemy.GetComponent<BigStarBlock>().pv.RPC("Delete", RpcTarget.All);
            }
        }
    }
}
