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

            // PhotonNetwork�� ����ϴ� ���� �ƴ� ��츦 �����Ͽ� Cake ����
            if (reward.GetComponent<PhotonView>())
            {
                spawnedReward = PhotonNetwork.Instantiate(reward.name, transform.position, Quaternion.identity);
            }
            else
            {
                spawnedReward = Instantiate(reward, transform.position, Quaternion.identity);
            }

            // Cake�� ���� Ƣ����� �̵�
            StartCoroutine(MoveRewardUpwards(spawnedReward));
        }
    }

    private IEnumerator MoveRewardUpwards(GameObject rewardObject)
    {
        float moveDuration = 1f; // �̵� �ð�
        float moveDistance = 2f;   // �̵� �Ÿ�
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

        rewardObject.transform.position = targetPosition; // ���� ��ġ ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().pView.RPC("Dance", RpcTarget.AllBuffered);

        }
    }

    


}
