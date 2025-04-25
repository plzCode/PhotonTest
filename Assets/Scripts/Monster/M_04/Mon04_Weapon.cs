using UnityEngine;

public class Mon04_Weapon : MonsterWeapon
{
    Rigidbody2D rb;
    Camera mainCam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
    }

    void Update()
    {

        Vector3 vp = mainCam.WorldToViewportPoint(transform.position);
        if (vp.x < 0f || vp.x > 1f || vp.y < 0f || vp.y > 1f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

}
