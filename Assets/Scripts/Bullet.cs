using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1;
    public float knockbackForce = 0.5f;
    public float splashRadius = 1.2f;
    Vector2 direction;

    void Start()
    {
        Destroy(gameObject, 2f);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Zombie zombie = other.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombie.TakeDamage(damage, direction, knockbackForce);
            SplashDamage(zombie.gameObject);
            Destroy(gameObject);
            return;
        }

        Smoker smoker = other.GetComponent<Smoker>();
        if (smoker != null)
        {
            smoker.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        Boss boss = other.GetComponent<Boss>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void SplashDamage(GameObject hitObject)
    {
        int splashAmount = Mathf.Max(1, Mathf.RoundToInt(damage * 0.018f)); // %1.8
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, splashRadius);

        foreach (Collider2D col in hits)
        {
            if (col.gameObject == hitObject) continue;

            Zombie z = col.GetComponent<Zombie>();
            if (z != null)
                z.TakeDamage(splashAmount, direction * 0.3f, 0.1f);

            Smoker s = col.GetComponent<Smoker>();
            if (s != null)
                s.TakeDamage(splashAmount);
        }
    }
}