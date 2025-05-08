using UnityEngine;

public class DDD_Air : MonoBehaviour
{
    public float moveSpeed = 3f;

    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }
}
