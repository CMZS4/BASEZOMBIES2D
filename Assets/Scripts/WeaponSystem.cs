using UnityEngine;

public enum AmmoType { Unlimited, ACP45, Ammo545, Ammo762, Gauge12, Ammo556 }

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public int damage;
    public float fireRate;
    public int maxAmmo;
    public int currentAmmo;
    public int maxReserve;
    public int currentReserve;
    public float dropRateBonus;
    public bool isShotgun;
    public AmmoType ammoType;
    public bool unlimitedAmmo;
    public float reloadTime;

    public Weapon(string name, int dmg, float rate, int ammo, int reserve, float dropBonus, AmmoType aType, bool unlimited = false, bool shotgun = false, float reload = 1.5f)
    {
        weaponName = name;
        damage = dmg;
        fireRate = rate;
        maxAmmo = ammo;
        currentAmmo = ammo;
        maxReserve = reserve;
        currentReserve = reserve;
        dropRateBonus = dropBonus;
        ammoType = aType;
        unlimitedAmmo = unlimited;
        isShotgun = shotgun;
        reloadTime = reload;
    }
}

public class WeaponSystem : MonoBehaviour
{
    public int activeWeaponIndex = 0;
    public int secondWeaponIndex = 3;

    public bool isReloading = false;
    public float reloadProgress = 0f; // 0-1 arası
    float reloadTimer = 0f;

    private Weapon[] _weapons;

    public Weapon[] weapons
    {
        get
        {
            if (_weapons == null || _weapons.Length == 0)
                InitWeapons();
            return _weapons;
        }
    }

    void Awake()
    {
        InitWeapons();
    }

    void InitWeapons()
    {
        _weapons = new Weapon[]
        {
            //                                                                              reload
            new Weapon("Pistol",  5,  2f,   12, 999, 0f,    AmmoType.Unlimited, false, false, 1.0f),
            new Weapon("Glock",   7,  3.5f, 20, 100, 0.25f, AmmoType.ACP45,     false, false, 1.2f),
            new Weapon("MP5",     8,  8f,   25, 125, 0.5f,  AmmoType.Ammo545,   false, false, 1.8f),
            new Weapon("AK47",    13, 5f,   30,  90, 1.0f,  AmmoType.Ammo762,   false, false, 2.2f),
            new Weapon("Shotgun", 10, 1f,    6,  48, 0.40f, AmmoType.Gauge12,   false, true,  2.5f),
            new Weapon("M249",    10, 10f,  50, 100, 1.5f,  AmmoType.Ammo556,   false, false, 3.5f),
        };
    }

    public Weapon ActiveWeapon => weapons[activeWeaponIndex];

    public float GetTotalDropRate()
    {
        return 14f + ActiveWeapon.dropRateBonus;
    }

    public float GetKnockback()
    {
        switch (activeWeaponIndex)
        {
            case 0: return 0.2f;
            case 1: return 0.2f;
            case 2: return 0.1f;
            case 3: return 0.5f;
            case 4: return 1.0f;
            case 5: return 0.1f;
            default: return 0.2f;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SwitchWeapon();

        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            reloadProgress = reloadTimer / ActiveWeapon.reloadTime;

            if (reloadTimer >= ActiveWeapon.reloadTime)
                FinishReload();
        }
    }

    public void SwitchWeapon()
    {
        if (secondWeaponIndex == -1) return;
        isReloading = false;
        reloadTimer = 0f;
        reloadProgress = 0f;
        int temp = activeWeaponIndex;
        activeWeaponIndex = secondWeaponIndex;
        secondWeaponIndex = temp;
    }

    public bool CanShoot()
    {
        return ActiveWeapon.currentAmmo > 0 && !isReloading;
    }

    public void UseAmmo()
    {
        ActiveWeapon.currentAmmo--;
    }

    public void Reload()
    {
        if (isReloading) return;
        if (ActiveWeapon.currentAmmo == ActiveWeapon.maxAmmo) return;

        if (ActiveWeapon.ammoType == AmmoType.Unlimited)
        {
            ActiveWeapon.currentAmmo = ActiveWeapon.maxAmmo;
            return;
        }

        if (ActiveWeapon.currentReserve <= 0) return;

        isReloading = true;
        reloadTimer = 0f;
        reloadProgress = 0f;
    }

    void FinishReload()
    {
        int needed = ActiveWeapon.maxAmmo - ActiveWeapon.currentAmmo;
        int available = Mathf.Min(needed, ActiveWeapon.currentReserve);
        ActiveWeapon.currentAmmo += available;
        ActiveWeapon.currentReserve -= available;

        isReloading = false;
        reloadTimer = 0f;
        reloadProgress = 0f;
    }

    public void AddAmmo(AmmoType type, int amount)
    {
        foreach (Weapon w in weapons)
        {
            if (w.ammoType == type)
                w.currentReserve = Mathf.Min(w.currentReserve + amount, w.maxReserve);
        }
    }
}