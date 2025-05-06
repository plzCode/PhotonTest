using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Monster_HealthBar : MonoBehaviour
{
    [Header("체력 정보")]
    public Image hpBar;

    private Coroutine currentAnimation;

    /// <summary>
    /// 체력바 업데이트 요청 (천천히 감소)
    /// </summary>
    /// <param name="_MaxHp">최대 체력</param>
    /// <param name="_currentHp">현재 체력</param>
    public void UpdateHealthBar(float _MaxHp, float _currentHp)
    {
        float targetFill = _currentHp / _MaxHp;

        // 이전 애니메이션이 있으면 중단
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        // 새로운 애니메이션 시작
        currentAnimation = StartCoroutine(SmoothFillAmount(targetFill));
    }

    private IEnumerator SmoothFillAmount(float targetFill)
    {
        float duration = 0.3f; // 줄어드는 속도 조절
        float startFill = hpBar.fillAmount;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            hpBar.fillAmount = Mathf.Lerp(startFill, targetFill, elapsed / duration);
            yield return null;
        }

        hpBar.fillAmount = targetFill;
        currentAnimation = null;
    }
}