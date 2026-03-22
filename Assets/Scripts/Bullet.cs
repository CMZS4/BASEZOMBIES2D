using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1;
    public float knockbackForce = 0.5f; // PlayerController tarafından set edilecek
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
}