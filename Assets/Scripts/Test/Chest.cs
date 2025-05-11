using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator anim;
    public GameObject reward;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Open()
    {
        if (reward != null)
        {
            
            GameObject spawnedReward;

            // PhotonNetwork를 사용하는 경우와 아닌 경우를 구분하여 Cake 생성
            if (reward.GetComponent<PhotonView>())
            {
                spawnedReward = PhotonNetwork.Instantiate(reward.name, transform.position, Quaternion.identity);
            }
            else
            {
                spawnedReward = Instantiate(reward, transform.position, Quaternion.identity);
            }

            // Cake를 위로 튀어나오게 이동
            StartCoroutine(MoveRewardUpwards(spawnedReward));
        }
    }

    private IEnumerator MoveRewardUpwards(GameObject rewardObject)
    {
        float moveDuration = 1f; // 이동 시간
        float moveDistance = 2f;   // 이동 거리
        float elapsedTime = 0f;

        Vector3 startPosition = rewardObject.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * moveDistance;

        while (elapsedTime < moveDuration)
        {
            rewardObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            rewardObject.transform.localScale *= 1.007f;
            yield return null;
        }

        rewardObject.transform.position = targetPosition; // 최종 위치 보정
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().pView.RPC("Dance", RpcTarget.AllBuffered);

        }
    }

    


}
