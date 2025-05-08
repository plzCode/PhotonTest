using System.Collections;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.spwanTransform.Add(transform);
                StartCoroutine(RotationObject());
            }
        }

    }

    IEnumerator RotationObject()
    {
        float elapsedTime = 0f; // 경과 시간 초기화
        float duration = 2f;    // 회전 지속 시간 (2초)
        float rotationSpeed = 25f; // 회전 속도 (기존 1에서 10으로 증가)

        while (elapsedTime < duration)
        {
            transform.Rotate(new Vector3(0, 1, 0), rotationSpeed, Space.Self); // 월드 좌표계 기준 회전
            yield return new WaitForSeconds(0.01f);    // 0.01초 대기
            elapsedTime += 0.01f;
        }
        this.gameObject.SetActive(false);
    }
}
