using Photon.Pun;
using UnityEngine;

public class MetaAttack2_LeftEffect : MonoBehaviour
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
        transform.Translate(-4f * Time.deltaTime, 0, 0);

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

    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, whatIsGround);
    protected void OnDrawGizmos()
    {
        if (wallCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.left * wallCheckDistance);
    }
}
