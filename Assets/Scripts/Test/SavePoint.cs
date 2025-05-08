using System.Collections;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField]
    public bool isTriggered = false;
    public string areaName = "";
    public PolygonCollider2D confinderArea;


    public void Start()
    {
        if(confinderArea != null) areaName = confinderArea.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.spwanTransform = this;
                StartCoroutine(RotationObject());
            }
        }

    }

    IEnumerator RotationObject()
    {
        float elapsedTime = 0f; // ��� �ð� �ʱ�ȭ
        float duration = 2f;    // ȸ�� ���� �ð� (2��)
        float rotationSpeed = 25f; // ȸ�� �ӵ� (���� 1���� 10���� ����)

        while (elapsedTime < duration)
        {
            transform.Rotate(new Vector3(0, 1, 0), rotationSpeed, Space.Self); // ���� ��ǥ�� ���� ȸ��
            yield return new WaitForSeconds(0.01f);    // 0.01�� ���
            elapsedTime += 0.01f;
        }
        this.gameObject.SetActive(false);
    }
}
