using UnityEngine;
using UnityEngine.UI;

public class Boss3 : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 800;
    public int currentHealth;
    public float speed = 1.5f;
    public Slider hpBar;

    [Header("Acid Alan")]
    public float acidRadius = 4f;
    public float acidDamageInterval = 1f;
    public int acidDamagePerTick = 5;

    [Header("Acid Atis")]
    public GameObject acidBulletPrefab;
    public float acidShootInterval = 4f;

    float lastAcidTick;
    float lastAcidShoot;

    Transform player;
    WaveSpawner waveSpawner;
    Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        waveSpawner = FindObjectOfType<WaveSpawner>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        if (hpBar != null)
        {
            hpBar.maxValue = maxHealth;
            hpBar.value = maxHealth;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Acid alan hasarı — oyuncuya
        if (Time.time - lastAcidTick >= acidDamageInterval)
        {
            lastAcidTick = Time.time;
            AcidAreaTick();
        }

        // Acid atışı
        if (Time.time - lastAcidShoot >= acidShootInterval)
        {
            lastAcidShoot = Time.time;
            ShootAcid();
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    void AcidAreaTick()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, acidRadius);
        foreach (Collider2D hit in hits)
        {
            // Oyuncuya hasar
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerController>()?.TakeDamage(1);
            }

            // Zombileri erit — acid hasarından ölürse boss can alır
            Zombie zombie = hit.GetComponent<Zombie>();
            if (zombie != null)
            {
                int beforeHealth = zombie.health;
                zombie.health -= acidDamagePerTick;

                if (zombie.hpBar != null)
                    zombie.hpBar.value = zombie.health;

                // Acid hasarından öldü → boss can alır
                if (zombie.health <= 0 && beforeHealth > 0)
                {
                    int healAmount = Mathf.RoundToInt(beforeHealth * 0.1f);
                    currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
                    if (hpBar != null) hpBar.value = currentHealth;
                    Debug.Log("Boss3 zombie eritti! +" + healAmount + " can");

                    // Zombie'yi direkt yok et (OnZombieDied çağır)
                    if (waveSpawner != null)
                        waveSpawner.OnZombieDied();
                    Destroy(zombie.gameObject);
                }
            }
        }
    }

    void ShootAcid()
    {
        if (acidBulletPrefab == null || player == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        GameObject bullet = Instantiate(acidBulletPrefab, transform.position, Quaternion.identity);
        AcidBullet ab = bullet.GetComponent<AcidBullet>();
        if (ab != null) ab.SetDirection(direction);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (hpBar != null) hpBar.value = currentHealth;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (waveSpawner != null)
            waveSpawner.OnBossDied();

        GameObject blindBox = Resources.Load<GameObject>("BlindBox");
        if (blindBox != null)
        {
            Vector2 offset = Random.insideUnitCircle * 0.5f;
            Instantiate(blindBox, transform.position + (Vector3)offset, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, acidRadius);
    }
}