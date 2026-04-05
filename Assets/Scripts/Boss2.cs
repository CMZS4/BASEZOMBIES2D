using UnityEngine;
using UnityEngine.UI;

public class Boss2 : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 450;
    public int currentHealth;
    public float baseSpeed = 2f;
    public float attackRadius = 2f; // geniş alan saldırı
    public float attackCooldown = 1.5f;
    public Slider hpBar;

    [Header("Vampire")]
    public float healRadius = 5f; // etraftaki zombilere can verme menzili
    public int zombieHealAmount = 30; // zombilere verilecek can (normal faz)
    public int zombieHealAmountPhase2 = 15; // yarı canda

    float currentSpeed;
    float speedBoostStacks = 0f;
    float lastAttackTime;
    bool isPhase2 = false;

    Transform player;
    WaveSpawner waveSpawner;
    Rigidbody2D rb;
    PlayerController playerController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        waveSpawner = FindObjectOfType<WaveSpawner>();
        playerController = player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        currentSpeed = baseSpeed;

        if (hpBar != null)
        {
            hpBar.maxValue = maxHealth;
            hpBar.value = maxHealth;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Yarı can fazı kontrolü
        if (!isPhase2 && currentHealth <= maxHealth / 2)
        {
            isPhase2 = true;
            Debug.Log("Boss2 Phase 2!");
        }

        // Saldırı
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= attackRadius && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            Attack();
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * currentSpeed;
    }

    void Attack()
    {
        if (playerController == null) return;

        // Hasar hesapla
        float damagePercent = isPhase2 ? 0.05f : 0.10f;
        int damage = Mathf.Max(1, Mathf.RoundToInt(playerController.maxHealth * damagePercent));

        // Oyuncuya hasar ver
        playerController.TakeDamage(damage);

        // Can çal
        int healAmount = isPhase2 ? 25 : 50;

        if (currentHealth < maxHealth)
        {
            // Canı dolu değil → kendini heal et
            currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
            if (hpBar != null) hpBar.value = currentHealth;
        }
        else
        {
            // Canı full → etraftaki zombilere ver
            HealNearbyZombies();
        }

        // Hız artır
        speedBoostStacks += 0.05f;
        currentSpeed = baseSpeed * (1f + speedBoostStacks);
        Debug.Log("Boss2 hız: " + currentSpeed + " (stack: " + speedBoostStacks + ")");
    }

    void HealNearbyZombies()
    {
        int healAmt = isPhase2 ? zombieHealAmountPhase2 : zombieHealAmount;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, healRadius);
        foreach (Collider2D hit in hits)
        {
            Zombie z = hit.GetComponent<Zombie>();
            if (z != null)
            {
                z.health = Mathf.Min(z.health + healAmt, z.health + healAmt);
                if (z.hpBar != null) z.hpBar.value = z.health;
            }
        }
        Debug.Log("Boss2 etraftaki zombileri iyilestirdi!");
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

        // Blind Box drop
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRadius);
    }
}