using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LifeNum : MonoBehaviour
{
    public List<GameObject> lifeNumList = new List<GameObject>();
    public Player owner = null;

    private void Awake()
    {
        lifeNumList.Clear();

        foreach (Transform child in transform)
        {
            lifeNumList.Add(child.gameObject);
        }

    }

    public void UpdateLifeNum(int num)
    {
        for (int i = 0; i < lifeNumList.Count; i++)
        {
            lifeNumList[i].SetActive(i < num);
        }
    }
    public void SetPlayer(Player player)
    {
        this.owner = player;
    }

}
