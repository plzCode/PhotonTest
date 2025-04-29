using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Spear : MonsterWeapon
{

    private Rigidbody2D rb;
    private Transform target;
    private PhotonView photonView;

    private bool isDestroyed = false;  // ������Ʈ�� �̹� �����Ǿ����� ���θ� ����

    [SerializeField] private float flightTime = 1.0f; // ��â�� ��ǥ ������ ������ �ð�
    private bool isStuck = false; // â�� ���� ���� ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {

        target = FindClosestPlayer();
        if (target != null)
        {
            LaunchParabolic();
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }

    }

    private void Update()
    {
        if (isStuck)
            return;

        //â ȸ�� �� Ŭ�󿡼� ����

        transform.right = rb.linearVelocity;

    }


    private void LaunchParabolic()
    {
        Vector2 start = transform.position;
        Vector2 end = target.position;
        Vector2 distance = end - start;

        float vx = distance.x / flightTime;
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float vy = (distance.y + 0.5f * gravity * Mathf.Pow(flightTime, 2)) / flightTime;

        Vector2 velocity = new Vector2(vx, vy);
        rb.linearVelocity = velocity;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(player.transform.position, currentPos);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = player.transform;
            }
        }

        return closest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!PhotonNetwork.IsMasterClient) return; // �����͸� �浹 ó��
        if (isStuck || isDestroyed) return;
        if (collision.CompareTag("Player") && !isStuck && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("�÷��̾�� " + power + "��ŭ �������� �ݴϴ�.");
            if (collision.GetComponent<PhotonView>() != null)
                collision.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, power); // ������ ó��
            if (!isDestroyed && PhotonNetwork.IsMasterClient)
            {
                isDestroyed = true;  // ���� ���·� ����
                PhotonNetwork.Destroy(gameObject); // ��Ʈ��ũ ��ü���� ����
            }
        }

        else if (collision.CompareTag("Ground") && !isDestroyed)
        {
            if (!isStuck)
            {
                FreezeSpear();
                if (photonView != null)
                    photonView.RPC("FreezeSpear", RpcTarget.Others);

                if (PhotonNetwork.IsMasterClient)
                    StartCoroutine(DestroyAfterDelay(2f));
            }
        }
    }
    [PunRPC]
    private void FreezeSpear()
    {
        if (isStuck) return; // �̹� ���� ��� ����
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // ��ġ+ȸ�� �� ����
        isStuck = true;


    }


    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ���� ���°� �ƴ϶�� ���� ��û
        if (!isDestroyed && PhotonNetwork.IsMasterClient)
        {
            isDestroyed = true;  // ���� ���·� ����
            PhotonNetwork.Destroy(gameObject); // ��Ʈ��ũ ��ü���� ����
        }
    }
}
