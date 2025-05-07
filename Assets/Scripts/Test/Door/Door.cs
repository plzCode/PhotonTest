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

    private ScreenFader screenFader; // ��ũ�� ���̴� ������Ʈ
    private float fadeTime = 0.5f;

    public PolygonCollider2D confinderArea;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("Collider2D�� Door�� �����ϴ�!");
        }

        screenFader = FindFirstObjectByType<ScreenFader>();
        if (screenFader == null)
        {
            Debug.LogError("ScreenFader�� ã�� �� �����ϴ�!");
        }
    }

    private void Update()
    {
        if (playersInRange != null && Input.GetKeyDown(KeyCode.W))
        {
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
            Debug.LogError("Linked_Door�� �������� �ʾҽ��ϴ�!");
            yield break;
        }

        if (player != null)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                // 1. ���̵� �ƿ�
                if (screenFader != null)
                {
                    yield return StartCoroutine(screenFader.FadeOut(fadeTime));
                }
                
                // 2. ��ġ �̵� �� ����ȭ
                player.transform.position = Linked_Door.transform.position;
                GetComponent<PhotonView>().RPC("ForceSyncPosition", RpcTarget.All, photonView.ViewID, Linked_Door.transform.position);
                /*PhotonNetwork.RaiseEvent(0, new object[] { photonView.ViewID, Linked_Door.transform.position },
                    new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                    ExitGames.Client.Photon.SendOptions.SendReliable);*/

                // 3. �÷��̾��� ī�޶� ����
                if(confinderArea != null)
                {
                    CinemachineConfiner2D tmpCam = GameObject.Find("PlayerCamera").GetComponent<CinemachineConfiner2D>();
                    tmpCam.BoundingShape2D = confinderArea;
                }

                Debug.Log(player.name + "��(��) " + Linked_Door.name + "�� �̵��߽��ϴ�.");

                // 3. ���̵� ��
                if (screenFader != null)
                {
                    yield return StartCoroutine(screenFader.FadeIn(fadeTime));
                }
            }
        }
        else
        {
            Debug.LogError("�̵��� �÷��̾ �����ϴ�!");
        }
    }

    [PunRPC]
    private void ForceSyncPosition(int playerViewId, Vector3 newPosition)
    {
        PhotonView.Find(playerViewId).transform.position = newPosition;
    }
}