using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    public CinemachineCamera cinemCamera;
    public CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        
    }

    private void Start()
    {
        /*
        // PlayerCamera 오브젝트에서 CinemachineCamera 찾기
        GameObject camObj = GameObject.Find("PlayerCamera");
        if (camObj != null)
        {
            cinemCamera = camObj.GetComponent<CinemachineCamera>();
            noise = camObj.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        if (cinemCamera == null || noise == null)
        {
            Debug.LogWarning("CinemachineCamera 또는 Noise 컴포넌트를 찾을 수 없습니다.");
        }
        */
    }


    public void Shake(float duration = 0.2f, float intensity = 2f, float frequency = 2f)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeCoroutine(duration, intensity, frequency));
    }

    private IEnumerator ShakeCoroutine(float duration, float intensity, float frequency)
    {
        if (noise == null)
            yield break;

        noise.AmplitudeGain = intensity;
        noise.FrequencyGain = frequency;

        yield return new WaitForSeconds(duration);

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;

        shakeRoutine = null;
    }
}