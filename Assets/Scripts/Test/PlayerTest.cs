using Photon.Pun;
using UnityEngine;

public class PlayerTest : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView playerView;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
    }

/*    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 로컬 플레이어의 데이터를 네트워크로 전송
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // 네트워크에서 데이터를 수신
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }*/

    private void FixedUpdate()
    {
        
    }
    [PunRPC]
    void SettingColor(int number, int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView == null) return;

        SpriteRenderer sr = targetView.GetComponentInChildren<SpriteRenderer>();
        if (sr == null) return;

        switch (number)
        {
            case 1:
                sr.color = new Color32(255, 255, 255, 255);
                break;
            case 2:
                sr.color = new Color32(255, 255, 0, 255);
                break;
            case 3:
                sr.color = new Color32(0, 0, 255, 255);
                break;
            case 4:
                sr.color = new Color32(0, 255, 0, 255);
                break;
        }
    }

    [PunRPC]
    public void CreateHealthBar(int number, int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);
        if (playerView == null)
        {
            Debug.LogError($"Player with ViewID {playerViewID} not found.");
            return;
        }

        Player player = playerView.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player component not found on the target object.");
            return;
        }

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in the scene.");
            return;
        }

        // 체력 UI 생성
        GameObject healthBar = Instantiate(Resources.Load("UI/HealthBar" + number.ToString()), canvas.transform) as GameObject;
        healthBar.transform.SetParent(canvas.transform);

        // UI 위치 조정 (플레이어 리스트에 따라 위치 변경)
        float verticalSpacing = Screen.height * 0.14f; // 화면 높이의 14%를 간격으로 사용
        float i = verticalSpacing * GameManager.Instance.playerList.Count;
        healthBar.transform.position = new Vector3(Screen.width * 0.984f, Screen.height * 0.945f - i, 0);

        // Health_Bar 설정
        Health_Bar healthBarScript = healthBar.GetComponentInChildren<Health_Bar>();
        healthBarScript.UpdateHealthBar(100f);
        healthBarScript.SetPlayer(player);
        player.health_Bar = healthBarScript;
    }
}

