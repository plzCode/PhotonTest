using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutScenePlayer : MonoBehaviour
{
    [Header("���� ����")]
    public string videoFileName = "ending.mp4"; // StreamingAssets ���� �� ���� ���� �̸�

    [Header("���� �� ����")]
    public string nextSceneName = "TestPhoton"; // ���� ���� �� �̵��� �� �̸�

    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }
        videoFileName = videoFileName + ".mp4"; // Ȯ���� �߰�
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
