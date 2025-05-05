using Photon.Pun;
using UnityEngine;

public class Bonkers_Bomb : MonoBehaviour
{
    [Header("��ź ����")]
    [SerializeField] private float attackPower;
    [SerializeField]private Rigidbody2D rb;
    private Boss_Bonkers boss;
    private PhotonView photonView;

    private void Awake()
    {
        boss = FindAnyObjectByType<Boss_Bonkers>();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        rb.linearVelocity = new Vector2(5 * boss.facingDir, 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            PhotonView pv = collision.GetComponent<PhotonView>();
            Debug.Log("��ź�� ���� �÷��̾�� " + attackPower + "��ŭ �������� �ݴϴ�.");
            if (pv != null)
                pv.RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, attackPower); // ������ ó��
            if (PhotonNetwork.IsMasterClient)
            {
                //isDestroyed = true;  // ���� ���·� ����
                photonView.RPC("EffectAdd", RpcTarget.All, "Delete Effect 30x30_0", transform.position);
                PhotonNetwork.Destroy(gameObject); // ��Ʈ��ũ ��ü���� ����
            }
        }
    }

    [PunRPC]
    protected virtual void EffectAdd(string effectName, Vector3 effectPos)
    {
        PhotonNetwork.Instantiate("Tile_Effect/" + effectName, effectPos, Quaternion.identity);
    }
}
