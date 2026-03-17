using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Boss : MonoBehaviour
{
    public float speed = 2f;
    public int health = 50;
    public float boulderInterval = 15f;
    public float shockwaveInterval = 10f;
    public float shockwaveRadius = 3f;
    public int shockwaveDamage = 2;
    public int boulderDamage = 3;
    public GameObject boulderPrefab;
    public GameObject blindBoxPrefab; // YENİ
    public Slider hpBar;

    Transform player;
    WaveSpawner waveSpawner;
    Rigidbody2D rb;
    float lastBoulderTime;
    float lastShockwaveTime;
    float damageCooldown = 0.5f;
    float lastDamageTime;
    bool touchingPlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        waveSpawner = FindObjectOfType<WaveSpawner>();
        rb = GetComponent<Rigidbody2D>();

        lastBoulderTime = Time.time;
        lastShockwaveTime = Time.time + 4f;

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

        if (Time.time - lastBoulderTime >= boulderInterval)
        {
            lastBoulderTime = Time.time;
            ThrowBoulder();
        }

        if (Time.time - lastShockwaveTime >= shockwaveInterval)
        {
            lastShockwaveTime = Time.time;
            Shockwave();
        }
    }

    void ThrowBoulder()
    {
        if (boulderPrefab == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        GameObject boulder = Instantiate(boulderPrefab, transform.position, Quaternion.identity);
        boulder.GetComponent<BossBoulder>().SetDirection(direction, boulderDamage);
    }

    void Shockwave()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, shockwaveRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<PlayerController>().TakeDamage(shockwaveDamage);
        }
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
            waveSpawner.OnBossDied();

        // Blind Box drop
        if (blindBoxPrefab != null)
        {
            Vector2 offset = Random.insideUnitCircle * 0.5f;
            Instantiate(blindBoxPrefab, transform.position + (Vector3)offset, Quaternion.identity);
            Debug.Log("Boss Blind Box düşürdü!");
        }

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shockwaveRadius);
    }
}