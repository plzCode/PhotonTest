using UnityEngine;
using Photon.Pun;

public class KirbyDamageStarmini : MonoBehaviour
{
    public float moveRadius = 0.5f; // ��鸮�� �ִ� �Ÿ�
    public float moveSpeed = 6f;     // ��鸮�� �ӵ�

    private Vector3 startPos;
    private Vector2 randomDirection; // ���� ���� ����
    private float randomOffset;      // ���� �ٸ� �������� ����� ���� �ð� ������

    void Start()
    {
        startPos = transform.localPosition;
        randomDirection = Random.insideUnitCircle.normalized; // (x, y) ���� ����
        randomOffset = Random.Range(0f, Mathf.PI * 2f); // �ð��� ���� ������ �༭ �ٸ��� ��鸮��
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
