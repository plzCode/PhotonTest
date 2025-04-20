using UnityEngine;

public class KirbyAirEffect : MonoBehaviour
{
    public float moveSpeed = 3f;
    public GameObject Effect2;
    public Transform Effect2Pos;
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }

    void Effect2Add()
    {
        Instantiate(Effect2, Effect2Pos.position, Quaternion.identity);
    }
}
