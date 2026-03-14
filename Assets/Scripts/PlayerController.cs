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

        if (Input.GetMouseButton(0) && !ClaimUI.panelAcik)
        {
            if (Time.time - lastFireTime >= 1f / weaponSystem.ActiveWeapon.fireRate)
            {
                if (weaponSystem.CanShoot())
                {
                    if (weaponSystem.ActiveWeapon.isShotgun)
                        ShootShotgun(lookDir);
                    else
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

    void ShootShotgun(Vector2 direction)
    {
        int pelletCount = 5;
        float spreadAngle = 30f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angle = -spreadAngle / 2 + (spreadAngle / (pelletCount - 1)) * i;
            Vector2 pelletDir = RotateVector(direction, angle);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().SetDirection(pelletDir);
            bullet.GetComponent<Bullet>().damage = weaponSystem.ActiveWeapon.damage;
        }
    }

    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
            v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad)
        );
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        Debug.Log("PLAYER ÖLDÜ - Game Over");
        gameObject.SetActive(false);
    }
}