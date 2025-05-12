using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Security.Cryptography;
using Photon.Realtime;
using System.Collections;

public class CutScenePlayer : MonoBehaviourPunCallbacks
{
    [Header("영상 설정")]
    public string videoFileName = "ending.mp4";

    [Header("다음 씬 설정")]
    public string nextSceneName = "TestPhoton";

    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }

        if (!videoFileName.EndsWith(".mp4"))
            videoFileName += ".mp4";
    }

    void Start()
    {
        videoPlayer.targetCamera = Camera.main;
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;

        if(videoFileName == "Opneing.mp4")
        {
            videoPlayer.playOnAwake = true;
            PlayCutScene();
        }
    }

    /*void OnVideoEnd(VideoPlayer vp)
    {
        //Debug.Log("✅ [OnVideoEnd] 영상 종료 감지");
        //StartCoroutine(HandleDisconnectAndSceneLoad());
        Debug.Log("✅ [OnVideoEnd] 영상 종료 감지 - 바로 씬 전환");
        NetworkManager.Instance.OnLeftLobby();
        NetworkManager.Instance.ConnectToPhoton();
        SceneManager.LoadScene(nextSceneName);


    }*/

    /* public override void OnDisconnected(DisconnectCause cause)
     {
         Debug.Log("[OnDisconnected] 연결 해제됨: " + cause);

         if (isWaitingToLoadScene)
         {
             isWaitingToLoadScene = false;

             Debug.Log("➡️ 씬 이동 시작");
             NetworkManager.Instance.OnLeftLobby();
             NetworkManager.Instance.ConnectToPhoton();

             SceneManager.LoadScene(nextSceneName);
         }
     }

     IEnumerator HandleDisconnectAndSceneLoad()
     {
         if (PhotonNetwork.IsConnected)
         {
             PhotonNetwork.Disconnect();
             Debug.Log("🔌 Disconnect 요청");

             while (PhotonNetwork.IsConnected)
             {
                 yield return null; // 연결 해제 대기
             }
         }

         Debug.Log("➡️ 연결 해제 완료 → 씬 전환");
         SceneManager.LoadScene(nextSceneName);
     }*/
    void OnVideoEnd(VideoPlayer vp)
    {

        switch (videoFileName) 
        {
            case "Opening.mp4":
                SceneManager.LoadScene(nextSceneName);
                break;
            case "Ending.mp4":
                if (AudioManager.Instance != null)
                {
                    Destroy(AudioManager.Instance.gameObject);
                    AudioManager.Instance = null;
                }
                NetworkManager.Instance.DisconnectAndLoadScene(nextSceneName);

                break;
            default:
                Debug.Log("기타 영상 종료 감지");
                break;
        }
        
        
    }

    
    public void PlayCutScene()
    {
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        videoPlayer.url = videoPath;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }
}
