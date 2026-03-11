using UnityEngine;

public class BossBoulder : MonoBehaviour
{
    public float speed = 4f;
    int damage;
    Vector2 direction;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    public void SetDirection(Vector2 dir, int dmg)
    {
        direction = dir;
        damage = dmg;
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