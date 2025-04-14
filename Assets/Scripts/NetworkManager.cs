using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ConnectToPhoton();
    }

    public void ConnectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to Photon...");
    }

    // 마스터 서버에 연결 완료
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby();
    }

    // 로비 입장 완료
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        // UI 활성화 같은 작업 가능
    }

    // 방 생성
    public void CreateRoom(string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(roomName, options);
    }

    // 방 참가
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    // 랜덤 방 참가
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // 방 참가 성공
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);

        // 로딩 씬 이동 등
        PhotonNetwork.LoadLevel("GameScene"); // 모든 유저가 자동으로 이동
    }

    // 방 참가 실패
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join Random Failed: " + message);
        CreateRoom("Room_" + Random.Range(1000, 9999));
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed: " + message);
    }
}