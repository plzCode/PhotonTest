using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class BossSpawnEvent : MonoBehaviour
{
    public int playerNumber;
    public int stay_Player = 0;
    public float timeToWait = 1f;
    public GameObject bossPrefab;
    public Transform SpawnPoint;
    public bool isTriggered = false;

    public void Start()
    {
        

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        playerNumber = GameManager.Instance.playerList.Count;

        if (collision.CompareTag("Player"))
        {
            stay_Player++;
            if(playerNumber == stay_Player && !isTriggered)
            {
                

                StartCoroutine(playerBusy(timeToWait));

                if (bossPrefab != null)
                {
                    PlayBossBGM();
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GetComponent<PhotonView>().RPC("BossSpawn",RpcTarget.AllBuffered);
                        isTriggered = true;
                    }
                    
                }
                else
                {
                    Debug.LogError("Boss prefab is not assigned in the inspector.");
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            stay_Player--;
        }
    }

    [PunRPC]
    public void BossSpawn()
    {
        /*if(PhotonNetwork.IsMasterClient == false)
        {
            return;
        }
        GameObject boss = PhotonNetwork.Instantiate("Monster_Effect/Test_Monster/" + bossPrefab.name, SpawnPoint.position, Quaternion.identity);
        Enemy enemyComponent = boss.GetComponent<Enemy>();*/

        
        bossPrefab.gameObject.SetActive(true);

        
        if (bossPrefab != null)
        {
            // WaitAndAction 코루틴 실행
            //bossPrefab.GetComponentInChildren<PhotonView>().RPC("WaitAndAction", RpcTarget.All, timeToWait);
        }
        else
        {
            Debug.LogError("Enemy component is not attached to the bossPrefab.");
        }
    }

    public void PlayBossBGM()
    {
        if (bossPrefab.GetComponentInChildren<Boss_Bonkers>()) { AudioManager.Instance.PlayBGM("11. VS. Mid-Boss"); }
        else if (bossPrefab.GetComponentInChildren<Boss_DDD>()) { AudioManager.Instance.PlayBGM("21. VS. Boss"); }

    }

    public IEnumerator playerBusy(float time)
    {
        foreach (var player in GameManager.Instance.playerList)
        {
            player.GetComponent<Player>().isBusy = true;
        }
        yield return new WaitForSeconds(time);
        foreach (var player in GameManager.Instance.playerList)
        {
            player.GetComponent<Player>().isBusy = false;
        }
    }

}
