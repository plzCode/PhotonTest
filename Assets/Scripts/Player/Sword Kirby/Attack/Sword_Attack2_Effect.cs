using Photon.Pun;
using UnityEngine;

public class Sword_Attack2_Effect : PlayerRagedManager
{
    private Animator anim;

    [SerializeField] private float flightTime = 1.0f;

    public float speed = 10f;
    private float lifeTime = 10f;  // �ʱ� ���� ����

    public bool AttackDelay = false;
    public bool AttackEnemy = false;
    public bool ReturnPlayer = false;

    // Rigidbody2D�� PhotonView�� �ʱ�ȭ�մϴ�.
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
        if (!photonView.IsMine) return; // ���� Ŭ���̾�Ʈ�� �������� ���� ��ü�� �浹 ó������ ����

        if (collision.gameObject.CompareTag("Enemy"))
        {
            lifeTime = 10f;  // ���� �浹 �� ���� �ֱ⸦ �ʱ�ȭ�մϴ�.

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().photonView.RPC("TakeDamage", RpcTarget.All, player.curAbility.attackPower);
            }
        }

        if (collision.gameObject.CompareTag("Ground") && PhotonNetwork.IsMasterClient)
        {
            player.CutterUpgrade = 0;
            PhotonNetwork.Destroy(gameObject);  // ����
        }

        if (collision.gameObject.CompareTag("StarBlock") && PhotonNetwork.IsMasterClient)
        {
            player.CutterUpgrade = 0;
            PhotonNetwork.Destroy(gameObject);  // ����

            BigStarBlock enemy = collision.gameObject.GetComponent<BigStarBlock>();
            if (enemy != null)
            {
                enemy.GetComponent<BigStarBlock>().pv.RPC("Delete", RpcTarget.All);
            }
        }
    }
}
