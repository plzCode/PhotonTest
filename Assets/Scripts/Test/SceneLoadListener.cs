using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadListener : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "TestPhoton":
                AudioManager.Instance.PlayBGM("LobbyBGM");
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayBGM("MainTitle_2");
                }
                NetworkManager.Instance.ConnectToPhoton();
                break;
            case "GameScene":
                AudioManager.Instance.PlayBGM("GameScene");
                break;
            default:
                AudioManager.Instance.StopBGM(); // ÇÊ¿ä ½Ã
                break;
        }
    }
}
