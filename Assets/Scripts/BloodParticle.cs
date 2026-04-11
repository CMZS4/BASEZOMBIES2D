using UnityEngine;

public class BloodParticle : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    float lifetime = 0.5f;
    float elapsed = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float randomForce = Random.Range(2f, 5f);
        rb.AddForce(randomDir * randomForce, ForceMode2D.Impulse);
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, elapsed / lifetime);
        sr.color = new Color(0.7f, 0f, 0f, alpha);
        if (elapsed >= lifetime) Destroy(gameObject);
    }
}