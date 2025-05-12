using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator anim;
    public GameObject reward;
    public CutScenePlayer cutScenePlayer;
    public bool isTriggered = false;

    public void Start()
    {
        anim = GetComponent<Animator>();

    }

    public void Open()
    {
        if (reward != null)
        {
            GameObject spawnedReward = null;

            // PhotonNetwork를 사용하는 경우와 아닌 경우를 구분하여 Cake 생성            
            if (reward.GetComponent<PhotonView>())
            {
                spawnedReward = PhotonNetwork.Instantiate(reward.name, transform.position, Quaternion.identity);
            }
            else
            {
                spawnedReward = Instantiate(reward, transform.position, Quaternion.identity);
            }
            spawnedReward.transform.SetParent(transform); // Chest의 자식으로 설정

            if (cutScenePlayer == null)
            {
                try
                {
                    cutScenePlayer = GetComponentInChildren<CutScenePlayer>();

                }
                catch { }
            }
               
            // Cake를 위로 튀어나오게 이동
            StartCoroutine(MoveRewardUpwards(spawnedReward));
        }
    }

    private IEnumerator MoveRewardUpwards(GameObject rewardObject)
    {
        float moveDuration = 0.7f; // 이동 시간
        float moveDistance = 2f;   // 이동 거리
        float elapsedTime = 0f;

        Vector3 startPosition = rewardObject.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * moveDistance;

        while (elapsedTime < moveDuration)
        {
            rewardObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            rewardObject.transform.localScale *= 1.01f;
            yield return null;
        }

        rewardObject.transform.position = targetPosition; // 최종 위치 보정
        StartCoroutine(WaitAndAction(0.45f, "CutScene")); // 컷신 재생
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            AudioManager.Instance.StopBGM();
            collision.GetComponent<Player>().pView.RPC("Dance", RpcTarget.AllBuffered, transform.position);
            //StartCoroutine(WaitAndAction(5.3f, "Open"));
            GetComponent<PhotonView>().RPC("RPC_WaitAndAction", RpcTarget.AllBuffered, 5.3f, "Open");

        }
    }

    [PunRPC]
    public void RPC_WaitAndAction(float time, string func)
    {
        StartCoroutine(WaitAndAction(time, func));
    }

    IEnumerator WaitAndAction(float time,string func)
    {
        yield return new WaitForSeconds(time);
        switch(func)
        {
            case "Open":
                isTriggered = true;
                anim.SetBool("Open", true); 
                break;
            case "CutScene":
                if(cutScenePlayer !=null)
                {
                    AudioManager.Instance.RPC_PlaySFX("Get_Item_Sound");
                    cutScenePlayer.PlayCutScene();
                }

                break;
        }
        

    }
}
