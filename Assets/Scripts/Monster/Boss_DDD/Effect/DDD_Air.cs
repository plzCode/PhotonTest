using UnityEngine;

public class DDD_Air : MonoBehaviour
{
    private Boss_DDD boss;

    public float moveSpeed = 3f;

    private void Awake()
    {
        boss = FindAnyObjectByType<Boss_DDD>();
    }
    private void Start()
    {
        if (boss.facingDir == 1)
        {

        }
        else if (boss.facingDir == -1)
        {
            Flip();
        }
    }
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
    }
}
