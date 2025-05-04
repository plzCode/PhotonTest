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
            case "LobbyScene":
                AudioManager.Instance.PlayBGM("LobbyBGM");
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
