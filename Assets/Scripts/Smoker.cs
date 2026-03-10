using UnityEngine;
using UnityEngine.UI;

public class Smoker : MonoBehaviour
{
    public float speed = 1.5f;
    public int health = 4;
    public float shootInterval = 5f;
    public GameObject acidBulletPrefab;
    public Slider hpBar;

    Transform player;
    WaveSpawner waveSpawner;
    Rigidbody2D rb;
    float lastShootTime;
    float damageCooldown = 0.5f;
    float lastDamageTime;
    bool touchingPlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        waveSpawner = FindObjectOfType<WaveSpawner>();
        rb = GetComponent<Rigidbody2D>();

        if (hpBar != null)
        {
            hpBar.maxValue = health;
            hpBar.value = health;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        if (touchingPlayer)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                lastDamageTime = Time.time;
                player.GetComponent<PlayerController>().TakeDamage(1);
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        if (Time.time - lastShootTime >= shootInterval)
        {
            lastShootTime = Time.time;
            ShootAcid();
        }
    }

    void ShootAcid()
    {
        if (acidBulletPrefab == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        GameObject acid = Instantiate(acidBulletPrefab, transform.position, Quaternion.identity);
        acid.GetComponent<AcidBullet>().SetDirection(direction);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (hpBar != null) hpBar.value = health;
        if (health <= 0) Die();
    }

    void Die()
    {
        if (waveSpawner != null)
            waveSpawner.OnZombieDied();
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = false;
    }
}