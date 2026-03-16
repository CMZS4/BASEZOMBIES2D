using UnityEngine;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    public enum ZombieType { Basic, Runner, Tank }
    public ZombieType zombieType = ZombieType.Basic;

    public float speed = 2f;
    public int health = 3;
    public float damageCooldown = 0.5f;
    public Slider hpBar;

    Transform player;
    WaveSpawner waveSpawner;
    Rigidbody2D rb;
    float lastDamageTime;
    bool touchingPlayer = false;

    // Her zombie tipinin fragment drop bonusu
    float GetZombieDropBonus()
    {
        switch (zombieType)
        {
            case ZombieType.Runner: return 3f;  // +%3
            case ZombieType.Tank:   return 2f;  // +%2
            default:                return 0f;  // Basic +%0
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        waveSpawner = FindObjectOfType<WaveSpawner>();
        rb = GetComponent<Rigidbody2D>();

        switch (zombieType)
        {
            case ZombieType.Runner:
                speed = 3.9f;
                health = 2;
                break;
            case ZombieType.Tank:
                speed = 1f;
                health = 6;
                break;
        }

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
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null) pc.TakeDamage(1);
            }
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
            waveSpawner.OnZombieDied();

        // Health pack drop
        float dropChance = Mathf.Max(0.05f - (waveSpawner.currentWave * 0.001f), 0.005f);
        if (Random.value < dropChance)
        {
            GameObject healthPackPrefab = Resources.Load<GameObject>("HealthPack");
            if (healthPackPrefab != null)
                Instantiate(healthPackPrefab, transform.position, Quaternion.identity);
        }

        // Fragment drop
        WeaponSystem weaponSystem = player?.GetComponent<WeaponSystem>();
        float weaponBonus = weaponSystem != null ? weaponSystem.ActiveWeapon.dropRateBonus : 0f;
        float zombieBonus = GetZombieDropBonus();
        float totalDropRate = (14f + weaponBonus + zombieBonus) / 100f;

        if (Random.value < totalDropRate)
        {
            GameObject fragmentPrefab = Resources.Load<GameObject>("Fragment");
            if (fragmentPrefab != null)
                Instantiate(fragmentPrefab, transform.position, Quaternion.identity);
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
}