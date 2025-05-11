using Photon.Pun;
using UnityEngine;

public class MetaAttackEffect : MonoBehaviour
{
    [SerializeField] private float attackPower = 20f;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    private BossMetaKnight boss;

    private float dir = 1f; // �⺻�� ������

    private void Awake()
    {
        boss = FindAnyObjectByType<BossMetaKnight>();
    }

    void Start()
    {
        if(boss.facingDir == 1)
        {

        }
        else if (boss.facingDir == -1)
        {
            Flip();
            dir = -1f;
        }
    }

    void Update()
    {
        transform.Translate(5f * Time.deltaTime, 0, 0);

        if (IsWallDetected())
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject); // ��Ʈ��ũ ��ü���� ����
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("�÷��̾�� " + attackPower + "��ŭ �������� �ݴϴ�.");
            if (collision.GetComponent<PhotonView>() != null)
                collision.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, attackPower); // ������ ó��
        }
    }

    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * dir, wallCheckDistance, whatIsGround);
    protected void OnDrawGizmos()
    {
        if (wallCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance * dir);
    }

    public virtual void Flip()
    {
        transform.Rotate(0, 180, 0);
    }
}
