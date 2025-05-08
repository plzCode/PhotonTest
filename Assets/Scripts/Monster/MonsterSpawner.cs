using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator ReSpawner(GameObject obj)
    {
        Debug.Log("���������");
        obj.SetActive(false);
        yield return new WaitForSeconds(3f);
        obj.SetActive(true);
        Debug.Log("�ٽ� ��Ÿ��");
    }

}
