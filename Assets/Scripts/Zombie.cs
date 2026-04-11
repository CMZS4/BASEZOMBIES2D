using UnityEngine;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    public enum ZombieType { Basic, Runner, Tank }
    public ZombieType zombieType = ZombieType.Basic;

    public float speed = 2f;
    public int health = 50;
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
        float waveMultiplier = 1f + (wave - 1) * 0.05f;

        switch (zombieType)
        {
            case ZombieType.Runner:
                speed = Mathf.Min(3.9f, PLAYER_SPEED * RUNNER_MAX_SPEED_RATIO);
                health = Mathf.RoundToInt(35 * waveMultiplier);
                break;
            case ZombieType.Tank:
                speed = Mathf.Min(1f * waveMultiplier, PLAYER_SPEED * 0.5f);
                health = Mathf.RoundToInt(150 * waveMultiplier);
                break;
            default:
                speed = Mathf.Min(2f * waveMultiplier, PLAYER_SPEED * 0.7f);
                health = Mathf.RoundToInt(50 * waveMultiplier);
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

        if (rb.linearVelocity.magnitude < 0.1f || Vector2.Dot(rb.linearVelocity.normalized, direction) > 0)
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

    public void TakeDamage(int amount, Vector2 knockbackDir, float knockbackForce)
    {
        health -= amount;
        if (hpBar != null) hpBar.value = health;

        if (rb != null && knockbackForce > 0)
            rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);

        SpawnDamageNumber(amount);

        if (health <= 0) Die();
    }

    public void TakeDamage(int amount)
    {
        TakeDamage(amount, Vector2.zero, 0f);
    }

    void SpawnDamageNumber(int damage)
    {
        GameObject prefab = Resources.Load<GameObject>("DamageNumber");
        if (prefab == null) return;

        Vector3 spawnPos = transform.position + new Vector3(
            Random.Range(-0.3f, 0.3f), 0.5f, 0f);

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
        DamageNumber dn = obj.GetComponent<DamageNumber>();
        if (dn == null) return;

        Color col;
        if (damage >= 50)      col = new Color(1f, 0.3f, 0f);
        else if (damage >= 20) col = new Color(1f, 0.85f, 0f);
        else                   col = Color.white;

        dn.Init(damage, col);
    }

    void Die()
    {
        if (waveSpawner != null)
            waveSpawner.OnZombieDied();

        float dropChance = Mathf.Max(0.05f - (waveSpawner.currentWave * 0.001f), 0.005f);
        if (Random.value < dropChance)
        {
            GameObject healthPackPrefab = Resources.Load<GameObject>("HealthPack");
            if (healthPackPrefab != null)
                Instantiate(healthPackPrefab, transform.position, Quaternion.identity);
        }

        WeaponSystem weaponSystem = player?.GetComponent<WeaponSystem>();
        float weaponBonus = weaponSystem != null ? weaponSystem.ActiveWeapon.dropRateBonus : 0f;
        float zombieBonus = GetZombieDropBonus();
        int wave = waveSpawner != null ? waveSpawner.currentWave : 1;
        float baseRate = Mathf.Min(3f + (wave - 1) * 0.5f, 14f);
        float totalDropRate = (baseRate + weaponBonus + zombieBonus) / 100f;

        if (Random.value < totalDropRate)
        {
            GameObject fragmentPrefab = Resources.Load<GameObject>("Fragment");
            if (fragmentPrefab != null)
            {
                Vector2 offset = Random.insideUnitCircle * 0.5f;
                Instantiate(fragmentPrefab, transform.position + (Vector3)offset, Quaternion.identity);
            }
        }

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