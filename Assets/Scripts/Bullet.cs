using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1;
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
            zombie.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        Smoker smoker = other.GetComponent<Smoker>();
        if (smoker != null)
        {
            smoker.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}