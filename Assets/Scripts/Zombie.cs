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

    const float PLAYER_SPEED = 10f;
    const float RUNNER_MAX_SPEED_RATIO = 0.9f;

    string[] ammoPrefabNames = { "Ammo_45ACP", "Ammo_545", "Ammo_762", "Ammo_12gauge", "Ammo_556" };

    Transform player;
    WaveSpawner waveSpawner;
    Rigidbody2D rb;
    float lastDamageTime;
    bool touchingPlayer = false;

    float GetZombieDropBonus()
    {
        switch (zombieType)
        {
            case ZombieType.Runner: return 3f;
            case ZombieType.Tank:   return 2f;
            default:                return 0f;
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        waveSpawner = FindObjectOfType<WaveSpawner>();
        rb = GetComponent<Rigidbody2D>();

        int wave = waveSpawner != null ? waveSpawner.currentWave : 1;
        float waveMultiplier = 1f + (wave - 1) * 0.01f;

        switch (zombieType)
        {
            case ZombieType.Runner:
                speed = Mathf.Min(3.9f, PLAYER_SPEED * RUNNER_MAX_SPEED_RATIO);
                health = Mathf.RoundToInt(2 * waveMultiplier);
                break;
            case ZombieType.Tank:
                speed = Mathf.Min(1f * waveMultiplier, PLAYER_SPEED * 0.5f);
                health = Mathf.RoundToInt(6 * waveMultiplier);
                break;
            default:
                speed = Mathf.Min(2f * waveMultiplier, PLAYER_SPEED * 0.7f);
                health = Mathf.RoundToInt(3 * waveMultiplier);
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
            {
                Vector2 offset = Random.insideUnitCircle * 0.5f;
                Instantiate(fragmentPrefab, transform.position + (Vector3)offset, Quaternion.identity);
            }
        }

        // Mermi drop - %6 ihtimalle
        if (Random.value < 0.06f)
        {
            int randomIndex = Random.Range(0, ammoPrefabNames.Length);
            string prefabName = ammoPrefabNames[randomIndex];
            GameObject ammoPrefab = Resources.Load<GameObject>(prefabName);
            if (ammoPrefab != null)
            {
                Vector2 offset2 = Random.insideUnitCircle * 0.5f;
                Instantiate(ammoPrefab, transform.position + (Vector3)offset2, Quaternion.identity);
            }
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