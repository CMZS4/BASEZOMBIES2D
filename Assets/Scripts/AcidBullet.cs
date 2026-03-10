using UnityEngine;

public class AcidBullet : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;
    Vector2 direction;

    void Start()
    {
        Destroy(gameObject, 4f);
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
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}