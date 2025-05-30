using Photon.Pun;
using UnityEngine;

public class MetaAttack2Effect : MonoBehaviour
{
    [SerializeField] private float attackPower = 30f;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
        transform.Translate(4f * Time.deltaTime, 0, 0);

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

    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, whatIsGround);
    protected void OnDrawGizmos()
    {
        if (wallCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);
    }
}
