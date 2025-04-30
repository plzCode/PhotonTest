using UnityEngine;
using Photon.Pun;

public class KirbyDamageStarmini : MonoBehaviour
{
    public float moveRadius = 0.5f; // 흔들리는 최대 거리
    public float moveSpeed = 6f;     // 흔들리는 속도

    private Vector3 startPos;
    private Vector2 randomDirection; // 랜덤 방향 설정
    private float randomOffset;      // 서로 다른 움직임을 만들기 위한 시간 오프셋

    void Start()
    {
        startPos = transform.localPosition;
        randomDirection = Random.insideUnitCircle.normalized; // (x, y) 방향 랜덤
        randomOffset = Random.Range(0f, Mathf.PI * 2f); // 시간에 랜덤 오프셋 줘서 다르게 흔들리게
    }

    void Update()
    {
        float time = Time.time * moveSpeed + randomOffset;
        Vector3 offset = new Vector3(
            Mathf.Sin(time) * randomDirection.x,
            Mathf.Cos(time) * randomDirection.y,
            0
        ) * moveRadius;

        transform.localPosition = startPos + offset;
    }
}
