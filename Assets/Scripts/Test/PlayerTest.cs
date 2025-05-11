using Photon.Pun;
using System.Linq;
using UnityEngine;

public class PlayerTest : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView playerView;
    public string area;
    public Door currentDoor;
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
                sr.color = new Color32(0, 191, 254, 255);
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
        healthBarScript.player = player; // 플레이어 스크립트와 연결
        
        // LifeNum 설정
        LifeNum lifenum = healthBar.GetComponentInChildren<LifeNum>();
        lifenum.SetPlayer(player);
        player.lifeNum = lifenum;
        lifenum.UpdateLifeNum(player.PlayerLife);
    }

    [PunRPC]
    public void CreateInventory(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);
        // 인벤토리 프리팹 로드 및 소환
        GameObject inventoryPrefab = Resources.Load<GameObject>("UI/Inventory");
        if (inventoryPrefab == null)
        {
            Debug.LogError("Inventory prefab not found in Resources/UI.");
            return;
        }
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject inventoryInstance = Instantiate(inventoryPrefab, canvas.transform);
        Inventory inventoryScript = inventoryInstance.GetComponent<Inventory>();

        if (inventoryScript == null)
        {
            Debug.LogError("Inventory component not found on the instantiated prefab.");
            return;
        }

        // 로컬 플레이어가 아닌 경우 Inventory 비활성화
        if (!playerView.IsMine)
        {
            inventoryInstance.SetActive(false);
        }

        // 인벤토리 등록
        GetComponent<Player>().inventory = inventoryScript;
        inventoryScript.player = GetComponent<Player>();
    }

    [PunRPC]
    public void Setting_Area_Name(string area, int pViewId,string doorName)
    {

        this.area = area;
        currentDoor = GameObject.Find(doorName).GetComponent<Door>();
    }

    [PunRPC]
    public void Dance(Vector3 pos)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            AudioManager.Instance.RPC_PlaySFX("Kirby_Dance");
        }

        var sortedPlayers = GameManager.Instance.playerList.OrderBy(p => p.GetComponent<PhotonView>().ViewID).ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            GameObject playerObject = sortedPlayers[i];
            if (playerObject.TryGetComponent<Player>(out Player tmp_player))
            {
                float offset = 1.5f * ((i / 2) + 1);
                float x = (i % 2 == 0) ? pos.x + offset : pos.x - offset;
                playerObject.transform.position = new Vector3(x, pos.y, pos.z);

                if (tmp_player.curAbility != null)
                {
                    tmp_player.curAbility.OnAbilityDestroyed(tmp_player);
                    Destroy(tmp_player.curAbility);
                }
                tmp_player.isBusy = true;
                tmp_player.stateMachine.ChangeState(tmp_player.danceState);
            }
        }
    }

}

