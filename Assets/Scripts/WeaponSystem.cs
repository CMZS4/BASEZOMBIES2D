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

    public Weapon(string name, int dmg, float rate, int ammo, int reserve, float dropBonus, AmmoType aType, bool unlimited = false, bool shotgun = false)
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
    }
}

public class WeaponSystem : MonoBehaviour
{
    public int activeWeaponIndex = 0;
    public int secondWeaponIndex = 3;

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
            //        isim      hasar  ateşhızı  sarjör  yedek   dropBonus  ammoType           sınırsız
            new Weapon("Pistol",   5,  2f,   12, 999,  0f,    AmmoType.Unlimited, false),
            new Weapon("Glock",    7,  3.5f, 20, 100,  0.25f, AmmoType.ACP45),
            new Weapon("MP5",      8,  8f,   25, 125,  0.5f,  AmmoType.Ammo545),
            new Weapon("AK47",     13, 5f,   30,  90,  1.0f,  AmmoType.Ammo762),
            new Weapon("Shotgun",  10, 1f,    6,  48,  0.40f, AmmoType.Gauge12, false, true),
            new Weapon("M249",     10, 10f,  50, 100,  1.5f,  AmmoType.Ammo556),
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
        case 0: return 0.2f;  // Pistol
        case 1: return 0.2f;  // Glock
        case 2: return 0.1f;  // MP5
        case 3: return 0.5f;  // AK47
        case 4: return 1.0f;  // Shotgun
        case 5: return 0.1f;  // M249
        default: return 0.2f;
    }
}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SwitchWeapon();
    }

    public void SwitchWeapon()
    {
        if (secondWeaponIndex == -1) return;
        int temp = activeWeaponIndex;
        activeWeaponIndex = secondWeaponIndex;
        secondWeaponIndex = temp;
        Debug.Log("Silah degisti: " + ActiveWeapon.weaponName);
    }

    public bool CanShoot()
    {
        return ActiveWeapon.currentAmmo > 0;
    }

    public void UseAmmo()
    {
        ActiveWeapon.currentAmmo--;
    }

    public void Reload()
    {
        if (ActiveWeapon.ammoType == AmmoType.Unlimited)
        {
            ActiveWeapon.currentAmmo = ActiveWeapon.maxAmmo;
            return;
        }

        int needed = ActiveWeapon.maxAmmo - ActiveWeapon.currentAmmo;
        int available = Mathf.Min(needed, ActiveWeapon.currentReserve);
        ActiveWeapon.currentAmmo += available;
        ActiveWeapon.currentReserve -= available;
        Debug.Log(ActiveWeapon.weaponName + " dolduruldu! Yedek: " + ActiveWeapon.currentReserve);
    }

    public void AddAmmo(AmmoType type, int amount)
    {
        foreach (Weapon w in weapons)
        {
            if (w.ammoType == type)
            {
                w.currentReserve = Mathf.Min(w.currentReserve + amount, w.maxReserve);
            }
        }
    }
}