using Photon.Pun;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Door Linked_Door;
    private Collider2D col;
    private GameObject playersInRange;

    private ScreenFader screenFader; // 스크린 페이더 컴포넌트
    private float fadeTime = 0.5f;

    public PolygonCollider2D confinderArea;
    string area = "";

    public MonsterSpawner stageSpawner;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("Collider2D가 Door에 없습니다!");
        }

        screenFader = FindFirstObjectByType<ScreenFader>();
        if (screenFader == null)
        {
            Debug.LogError("ScreenFader를 찾을 수 없습니다!");
        }
        area = confinderArea.name;
    }

    private void Update()
    {
        if (playersInRange != null && Input.GetKeyDown(KeyCode.W))
        {
            if (playersInRange.GetComponent<PlayerTest>() != null)
            {
                playersInRange.GetComponent<Player>().pView.RPC("Setting_Area_Name", RpcTarget.AllBuffered, area, playersInRange.GetComponent<Player>().pView.ViewID,Linked_Door.gameObject.name);
                //playersInRange.GetComponent<PlayerTest>().Setting_Area_Name(area, playersInRange.GetComponent<Player>().pView.ViewID);
            }
            StartCoroutine(TeleportPlayerCoroutine(playersInRange));
            AudioManager.Instance.PlaySFX("Door_Sound");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<PhotonView>().IsMine)
        {
            playersInRange = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<PhotonView>().IsMine)
        {
            playersInRange = null;
        }
    }

    private IEnumerator TeleportPlayerCoroutine(GameObject player)
    {
        if (Linked_Door == null)
        {
            Debug.LogError("Linked_Door가 설정되지 않았습니다!");
            yield break;
        }

        if (player != null)
        {
            

            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {

                // 1. 페이드 아웃
                if (screenFader != null)
                {
                    yield return StartCoroutine(screenFader.FadeOut(fadeTime));
                }

                // 2. 위치 이동 및 동기화
                player.transform.position = Linked_Door.transform.position; 
                Vector3 tmpPostion = Linked_Door.transform.position + (Vector3.up * 0.2f);
                GetComponent<PhotonView>().RPC("ForceSyncPosition", RpcTarget.All, photonView.ViewID, tmpPostion);
                /*PhotonNetwork.RaiseEvent(0, new object[] { photonView.ViewID, Linked_Door.transform.position },
                    new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                    ExitGames.Client.Photon.SendOptions.SendReliable);*/

                // 3. 플레이어의 카메라 설정
                if (confinderArea != null)
                {
                    CinemachineConfiner2D tmpCam = GameObject.Find("PlayerCamera").GetComponent<CinemachineConfiner2D>();
                    tmpCam.BoundingShape2D = confinderArea;
                }

                Debug.Log("Current Area Name : " + Linked_Door.area + " = " + GameManager.Instance.GetAreaPlayer(Linked_Door.area));
                Debug.Log("Next Area Name" + area + " = " + GameManager.Instance.GetAreaPlayer(area));



                if (GameManager.Instance.GetAreaPlayer(area) == 1) // 이동할층
                {
                    Linked_Door.stageSpawner.photonView.RPC("DeactivateSelfAndChildren", RpcTarget.All);
                }
                if (GameManager.Instance.GetAreaPlayer(Linked_Door.area) == 0) //현재층
                {
                    stageSpawner.photonView.RPC("ActFalseWithChildren", RpcTarget.All);
                }


                Debug.Log(player.name + "이(가) " + Linked_Door.name + "로 이동했습니다.");
                

                // 3. 페이드 인
                if (screenFader != null)
                {
                    yield return StartCoroutine(screenFader.FadeIn(fadeTime));
                }
                if (!AudioManager.Instance.bgmSource.isPlaying)
                {
                    AudioManager.Instance.PlayBGM("GameScene");
                }
            }
        }
        else
        {
            Debug.LogError("이동할 플레이어가 없습니다!");
        }
    }

    [PunRPC]
    private void ForceSyncPosition(int playerViewId, Vector3 newPosition)
    {
        PhotonView.Find(playerViewId).transform.position = newPosition;
    }
}