using Photon.Pun;
using UnityEngine;

public class Boomerang_Obj : MonoBehaviour
{
    [Header("부메랑 정보")]
    [SerializeField] private float attackPower;

    private Rigidbody2D rb;
    private Animator anim;

    private float currentSpeed;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float targetSpeed = -10f;
    [SerializeField] private float lerpSpeed = 1.5f; // 속도 전환 속도
    private bool isThrown = false;
    public float facingDir = 1;

    private bool isDestroyed = false;  // 오브젝트가 이미 삭제되었는지 여부를 추적
    public PhotonView photonView;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if (isThrown && rb != null)
        {
            // currentSpeed를 targetSpeed로 서서히 변화시킴
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * lerpSpeed);

            // 속도 적용 (방향 고려)
            rb.linearVelocity = new Vector2(currentSpeed * facingDir, 0);
        }
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
    }

    [PunRPC]
    public void ThrowBoomerang(float _facingDir)
    {
        facingDir = _facingDir;
        if (anim != null)
            anim.SetTrigger("Throw");
        currentSpeed = maxSpeed;
        isThrown = true;



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("플레이어에게 " + attackPower + "만큼 데미지를 줍니다.");
            if (collision.GetComponent<PhotonView>() != null)
                collision.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, attackPower); // 데미지 처리
            if (PhotonNetwork.IsMasterClient)
            {
                isDestroyed = true;  // 삭제 상태로 설정
                PhotonNetwork.Destroy(gameObject); // 네트워크 전체에서 삭제
            }
        }
    }
}
