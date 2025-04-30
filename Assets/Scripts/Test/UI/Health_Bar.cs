using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;  // ü�� �� UI �̹��� (Fill Amount�� ����ȭ)
    [SerializeField] private float maxHealth = 100f; // �ִ� ü��
    [SerializeField] private float fillSpeed = 5f;
    [SerializeField] private Player player;

    private Coroutine currentAnimation;

    // �ܺο��� ü�� ������ ���޹޾� ü�¹ٸ� õõ�� ����
    public void UpdateHealthBar(float currentHealth)
    {
        float targetRatio = currentHealth / maxHealth;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(SmoothFill(targetRatio));
    }

    // �ڷ�ƾ���� ������ fillAmount ����
    private IEnumerator SmoothFill(float target)
    {
        while (Mathf.Abs(healthBarImage.fillAmount - target) > 0.01f)
        {
            healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, target, Time.deltaTime * fillSpeed);
            yield return null;
        }

        healthBarImage.fillAmount = target; // ������ ����
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

}
