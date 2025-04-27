using UnityEngine;

public class GroundStarEffect : MonoBehaviour
{
    private float xMove;
    private float yMove;

    void Start()
    {
        xMove = Random.Range(-1f, 1f);
        yMove = Random.Range(-1f, 1f);
    }

    void Update()
    {
        transform.Translate(xMove * Time.deltaTime, yMove * Time.deltaTime, 0);
    }
}
