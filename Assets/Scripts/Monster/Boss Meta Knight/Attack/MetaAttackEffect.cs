using Photon.Pun;
using UnityEngine;

public class MetaAttackEffect : MonoBehaviour
{
    [SerializeField] private float attackPower = 20f;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    private BossMetaKnight boss;

    private float dir = 1f; // 기본은 오른쪽

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
                PhotonNetwork.Destroy(gameObject); // 네트워크 전체에서 삭제
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("플레이어에게 " + attackPower + "만큼 데미지를 줍니다.");
            if (collision.GetComponent<PhotonView>() != null)
                collision.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, attackPower); // 데미지 처리
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
