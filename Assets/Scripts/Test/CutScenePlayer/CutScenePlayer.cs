using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutScenePlayer : MonoBehaviour
{
    [Header("영상 설정")]
    public string videoFileName = "ending.mp4"; // StreamingAssets 폴더 내 영상 파일 이름

    [Header("다음 씬 설정")]
    public string nextSceneName = "TestPhoton"; // 영상 끝난 후 이동할 씬 이름

    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }
        videoFileName = videoFileName + ".mp4"; // 확장자 추가
    }

    void Start()
    {
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        videoPlayer.url = videoPath;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
