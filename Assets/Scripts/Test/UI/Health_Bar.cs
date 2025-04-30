using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;  // 체력 바 UI 이미지 (Fill Amount로 동기화)
    [SerializeField] private float maxHealth = 100f; // 최대 체력
    [SerializeField] private float fillSpeed = 5f;
    [SerializeField] private Player player;

    private Coroutine currentAnimation;

    // 외부에서 체력 정보를 전달받아 체력바를 천천히 갱신
    public void UpdateHealthBar(float currentHealth)
    {
        float targetRatio = currentHealth / maxHealth;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(SmoothFill(targetRatio));
    }

    // 코루틴으로 서서히 fillAmount 변경
    private IEnumerator SmoothFill(float target)
    {
        while (Mathf.Abs(healthBarImage.fillAmount - target) > 0.01f)
        {
            healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, target, Time.deltaTime * fillSpeed);
            yield return null;
        }

        healthBarImage.fillAmount = target; // 마지막 보정
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

}
