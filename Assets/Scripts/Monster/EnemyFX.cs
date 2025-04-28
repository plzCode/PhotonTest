using System.Collections;
using UnityEngine;

public class EnemyFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("ÇÃ·¡½Ã FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originMat;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
