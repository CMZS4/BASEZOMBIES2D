using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Can")]
    public int maxHealth = 5;
    public int currentHealth;

    Rigidbody2D rb;
    Vector2 movement;
    WeaponSystem weaponSystem;
    float lastFireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        weaponSystem = GetComponent<WeaponSystem>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 lookDir = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Otomatik ateş - basılı tut
        if (Input.GetMouseButton(0) && !ClaimUI.panelAcik)
        {
            if (Time.time - lastFireTime >= 1f / weaponSystem.ActiveWeapon.fireRate)
            {
                if (weaponSystem.CanShoot())
                {
                    Shoot(lookDir);
                    weaponSystem.UseAmmo();
                    lastFireTime = Time.time;
                }
                else
                {
                    Debug.Log("Mermi bitti! R ile doldur.");
                }
            }
        }

        // R ile doldur
        if (Input.GetKeyDown(KeyCode.R))
            weaponSystem.Reload();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * speed;
    }

    void Shoot(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(direction);
        bullet.GetComponent<Bullet>().damage = weaponSystem.ActiveWeapon.damage;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player can: " + currentHealth + "/" + maxHealth);
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        Debug.Log("PLAYER ÖLDÜ - Game Over");
        gameObject.SetActive(false);
    }
}