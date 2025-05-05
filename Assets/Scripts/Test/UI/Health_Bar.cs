using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;  // ü�� �� UI �̹��� (Fill Amount�� ����ȭ)
    [SerializeField] private float maxHealth = 100f; // �ִ� ü��
    [SerializeField] private float fillSpeed = 5f;
    [SerializeField] public Player player;

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
        // ���� ü�� ����
        float currentFill = healthBarImage.fillAmount;

        // ü�� ��ȭ ���� Ȯ��
        bool isHealing = target > currentFill;

        // SFX ��� ���� (ȸ���� ����)
        AudioSource audioSource = AudioManager.Instance.sfxSource;
        AudioClip sfxClip = AudioManager.Instance.sfxClips["Healing_Sound"]; // SFX Ŭ�� �̸�
        if (isHealing && PhotonNetwork.LocalPlayer.IsLocal)
        {
            audioSource.clip = sfxClip;
            audioSource.loop = true; // ���� ����
            audioSource.Play();
        }

        // ü�¹� �ִϸ��̼�
        while (Mathf.Abs(healthBarImage.fillAmount - target) > 0.01f)
        {
            healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, target, Time.deltaTime * fillSpeed);
            yield return null;
        }

        healthBarImage.fillAmount = target; // ������ ����

        // SFX ��� ����
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
