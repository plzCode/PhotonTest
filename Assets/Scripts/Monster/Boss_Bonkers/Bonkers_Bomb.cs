using Photon.Pun;
using UnityEngine;

public class Bonkers_Bomb : MonoBehaviour
{
    float LiftTime = 5f; // ��ź�� �������� �ð�


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

    private void Update()
    {
        LiftTime -= Time.deltaTime;
        if (LiftTime <= 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                AudioManager.Instance.RPC_PlaySFX("Boom5");
                photonView.RPC("EffectAdd", RpcTarget.All, "Bonkers Bomb Effect2 95x95_0", transform.position);
                PhotonNetwork.Destroy(gameObject); // ��Ʈ��ũ ��ü���� ����
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            AudioManager.Instance.RPC_PlaySFX("Boom5");
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonView pv = collision.collider.GetComponent<PhotonView>();
                Debug.Log("��ź�� ���� �÷��̾�� " + attackPower + "��ŭ �������� �ݴϴ�.");
                if (pv != null)
                    pv.RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, attackPower); // ������ ó��
                if (PhotonNetwork.IsMasterClient)
                {
                    //isDestroyed = true;  // ���� ���·� ����
                    photonView.RPC("EffectAdd", RpcTarget.All, "Bonkers Bomb Effect2 95x95_0", transform.position);
                    PhotonNetwork.Destroy(gameObject); // ��Ʈ��ũ ��ü���� ����
                }
            }
        }
    }

    [PunRPC]
    protected virtual void EffectAdd(string effectName, Vector3 effectPos)
    {
        PhotonNetwork.Instantiate("Monster_Effect/" + effectName, effectPos, Quaternion.identity);
    }
}
