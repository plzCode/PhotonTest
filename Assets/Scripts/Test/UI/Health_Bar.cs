using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;  // 체력 바 UI 이미지 (Fill Amount로 동기화)
    [SerializeField] private float maxHealth = 100f; // 최대 체력
    [SerializeField] private float fillSpeed = 5f;
    [SerializeField] public Player player;

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
        // 현재 체력 비율
        float currentFill = healthBarImage.fillAmount;

        // 체력 변화 방향 확인
        bool isHealing = target > currentFill;

        // SFX 재생 시작 (회복일 때만)
        AudioSource audioSource = AudioManager.Instance.sfxSource;
        AudioClip sfxClip = AudioManager.Instance.sfxClips["Healing_Sound"]; // SFX 클립 이름
        if (isHealing && PhotonNetwork.LocalPlayer.IsLocal)
        {
            audioSource.clip = sfxClip;
            audioSource.loop = true; // 루프 설정
            audioSource.Play();
        }

        // 체력바 애니메이션
        while (Mathf.Abs(healthBarImage.fillAmount - target) > 0.01f)
        {
            healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, target, Time.deltaTime * fillSpeed);
            yield return null;
        }

        healthBarImage.fillAmount = target; // 마지막 보정

        // SFX 재생 중지
        if (isHealing && audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

}
