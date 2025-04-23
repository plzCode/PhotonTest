using UnityEngine;

public class Boomerang_Obj : MonoBehaviour
{
    [Header("�θ޶� ����")]
    [SerializeField] private float attackPower;

    private Rigidbody2D rb;
    private Animator anim;

    private float currentSpeed;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float targetSpeed = -10f;
    [SerializeField] private float lerpSpeed = 1.5f; // �ӵ� ��ȯ �ӵ�
    private bool isThrown = false;
    public float facingDir = 1;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (isThrown && rb != null)
        {
            // currentSpeed�� targetSpeed�� ������ ��ȭ��Ŵ
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * lerpSpeed);

            // �ӵ� ���� (���� ���)
            rb.linearVelocity = new Vector2(currentSpeed * facingDir, 0);
        }
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {        
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);        
    }

    public void ThrowBoomerang()
    {
        if (anim != null)
            anim.SetTrigger("Throw");

        currentSpeed = maxSpeed;
        isThrown = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� " + attackPower + "��ŭ �������� �ݴϴ�.");
            collision.GetComponent<Player>().TakeDamage(transform.position, attackPower);
            Destroy(gameObject);
        }
    }
}
