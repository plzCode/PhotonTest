using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Monster_HealthBar : MonoBehaviour
{
    [Header("ü�� ����")]
    public Image hpBar;

    private Coroutine currentAnimation;

    /// <summary>
    /// ü�¹� ������Ʈ ��û (õõ�� ����)
    /// </summary>
    /// <param name="_MaxHp">�ִ� ü��</param>
    /// <param name="_currentHp">���� ü��</param>
    public void UpdateHealthBar(float _MaxHp, float _currentHp)
    {
        float targetFill = _currentHp / _MaxHp;

        // ���� �ִϸ��̼��� ������ �ߴ�
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        // ���ο� �ִϸ��̼� ����
        currentAnimation = StartCoroutine(SmoothFillAmount(targetFill));
    }

    private IEnumerator SmoothFillAmount(float targetFill)
    {
        float duration = 0.3f; // �پ��� �ӵ� ����
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