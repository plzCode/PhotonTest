using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject[] players; // 기존 방식 유지
    public List<GameObject> playerList = new List<GameObject>(); // 새 리스트 방식

    public List<Transform> spwanTransform = new List<Transform>(); // 스폰 위치 리스트

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

    public void RegisterPlayer(GameObject player)
    {
        if (!playerList.Contains(player))
        {
            playerList.Add(player);
            Debug.Log($"플레이어 등록됨: {player.name}");
        }
    }

    public GameObject GetClosestPlayer(Vector3 fromPosition)
    {
        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var p in playerList)
        {
            if (p == null) continue;

            float distance = Vector3.Distance(fromPosition, p.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = p;
            }
        }

        return closest;
    }

    public int GetAreaPlayer(string areaName)
    {
        int playerCount = 0;
        foreach (GameObject player in playerList)
        {
            if (player.GetComponent<PlayerTest>() != null)
            {
                if (player.GetComponent<PlayerTest>().area.Equals(areaName))
                {
                    playerCount++;
                }
            }
        }
        return playerCount;
    }
    
}