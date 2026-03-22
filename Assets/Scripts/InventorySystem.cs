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

        // Shop'tan satın alınanları yükle
        LoadFromShop();
    }

    void LoadFromShop()
    {
        healthPackCount += PlayerPrefs.GetInt("HPPackCount", 0);
        ammoPackCount += PlayerPrefs.GetInt("AmmoPackCount", 0);
        barricadeCount += PlayerPrefs.GetInt("BarricadeCount", 0);
        speedBoostCount += PlayerPrefs.GetInt("SpeedBoostCount", 0);

        // Yükledikten sonra sıfırla (bir kez kullanılsın)
        PlayerPrefs.SetInt("HPPackCount", 0);
        PlayerPrefs.SetInt("AmmoPackCount", 0);
        PlayerPrefs.SetInt("BarricadeCount", 0);
        PlayerPrefs.SetInt("SpeedBoostCount", 0);
        PlayerPrefs.Save();
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

        if (weapon.ammoType == AmmoType.Unlimited)
            weapon.currentAmmo = weapon.maxAmmo;
        else
        {
            weapon.currentAmmo = weapon.maxAmmo;
            weapon.currentReserve = weapon.maxReserve;
        }

        ammoPackCount--;
    }

    public void UseSpeedBoost()
    {
        if (speedBoostCount <= 0) return;
        if (isBoosted) return;

        speedBoostCount--;
        isBoosted = true;
        boostTimer = speedBoostDuration;
        player.speed *= speedBoostMultiplier;
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