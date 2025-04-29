using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Spear : MonsterWeapon
{

    private Rigidbody2D rb;
    private Transform target;
    private PhotonView photonView;

    private bool isDestroyed = false;  // 오브젝트가 이미 삭제되었는지 여부를 추적

    [SerializeField] private float flightTime = 1.0f; // 투창이 목표 지점에 도달할 시간
    private bool isStuck = false; // 창이 땅에 박힌 상태

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

        //창 회전 각 클라에서 적용

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
        //if (!PhotonNetwork.IsMasterClient) return; // 마스터만 충돌 처리
        if (isStuck || isDestroyed) return;
        if (collision.CompareTag("Player") && !isStuck && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("플레이어에게 " + power + "만큼 데미지를 줍니다.");
            if (collision.GetComponent<PhotonView>() != null)
                collision.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, power); // 데미지 처리
            if (!isDestroyed && PhotonNetwork.IsMasterClient)
            {
                isDestroyed = true;  // 삭제 상태로 설정
                PhotonNetwork.Destroy(gameObject); // 네트워크 전체에서 삭제
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
        if (isStuck) return; // 이미 박힌 경우 무시
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 위치+회전 다 고정
        isStuck = true;


    }


    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 삭제 상태가 아니라면 삭제 요청
        if (!isDestroyed && PhotonNetwork.IsMasterClient)
        {
            isDestroyed = true;  // 삭제 상태로 설정
            PhotonNetwork.Destroy(gameObject); // 네트워크 전체에서 삭제
        }
    }
}
