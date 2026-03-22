using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int healthPackCount = 0;
    public int barricadeCount = 0;
    public int ammoPackCount = 0;
    public int speedBoostCount = 0;

    public float speedBoostMultiplier = 1.5f;
    public float speedBoostDuration = 5f;

    PlayerController player;
    WaveSpawner waveSpawner;
    WeaponSystem weaponSystem;
    bool isBoosted = false;
    float boostTimer = 0f;

    void Start()
    {
        player = GetComponent<PlayerController>();
        waveSpawner = FindObjectOfType<WaveSpawner>();
        weaponSystem = GetComponent<WeaponSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            UseHealthPack();

        if (Input.GetKeyDown(KeyCode.F))
            UseAmmoPack();

        if (Input.GetKeyDown(KeyCode.B))
            PlaceBarricade();

        if (Input.GetKeyDown(KeyCode.X))
            UseSpeedBoost();

        // Hız boost timer
        if (isBoosted)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                isBoosted = false;
                player.speed /= speedBoostMultiplier;
            }
        }
    }

    public void AddHealthPack() { healthPackCount++; }
    public void AddBarricade() { barricadeCount++; }
    public void AddAmmoPack() { ammoPackCount++; }
    public void AddSpeedBoost() { speedBoostCount++; }

    public void UseHealthPack()
    {
        if (healthPackCount <= 0) return;
        if (player.currentHealth >= player.maxHealth) return;
        healthPackCount--;
        player.currentHealth = Mathf.Min(player.currentHealth + 3, player.maxHealth);
    }

    public void UseAmmoPack()
    {
        if (ammoPackCount <= 0) return;
        if (weaponSystem == null) return;

        var weapon = weaponSystem.ActiveWeapon;

        // Pistol sınırsız, ammo pack'e gerek yok
        if (weapon.ammoType == AmmoType.Unlimited)
        {
            weapon.currentAmmo = weapon.maxAmmo;
        }
        else
        {
            weapon.currentAmmo = weapon.maxAmmo;
            weapon.currentReserve = weapon.maxReserve;
        }

        ammoPackCount--;
        Debug.Log("Ammo Pack kullanıldı! " + weapon.weaponName + " full!");
    }

    public void UseSpeedBoost()
    {
        if (speedBoostCount <= 0) return;
        if (isBoosted) return;

        speedBoostCount--;
        isBoosted = true;
        boostTimer = speedBoostDuration;
        player.speed *= speedBoostMultiplier;
        Debug.Log("Hız artırıcı kullanıldı!");
    }

    void PlaceBarricade()
    {
        if (barricadeCount <= 0) return;

        int closestWindow = -1;
        float closestDist = 3f;

        for (int i = 0; i < waveSpawner.windows.Length; i++)
        {
            float dist = Vector2.Distance(transform.position, waveSpawner.windows[i].position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestWindow = i;
            }
        }

        if (closestWindow >= 0)
        {
            barricadeCount--;
            waveSpawner.PlaceBarricade(closestWindow);
        }
    }
}