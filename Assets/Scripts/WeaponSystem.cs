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
            new Weapon("Pistol",  1, 2f,   12, 999, 0f,  AmmoType.Unlimited, false),
            new Weapon("Glock",   1, 3.5f, 20, 100, 1.5f, AmmoType.ACP45),
            new Weapon("MP5",     2, 8f,   25, 125, 2.0f, AmmoType.Ammo545),
            new Weapon("AK47",    3, 5f,   30,  90, 3.0f, AmmoType.Ammo762),
            new Weapon("Shotgun", 5, 1f,    6,  48, 1.0f, AmmoType.Gauge12, false, true),
            new Weapon("M249",    4, 10f,  50, 100, 4.0f, AmmoType.Ammo556),
        };
    }

    public Weapon ActiveWeapon => weapons[activeWeaponIndex];

    public float GetTotalDropRate()
    {
        return 14f + ActiveWeapon.dropRateBonus;
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
        Debug.Log("Silah değişti: " + ActiveWeapon.weaponName);
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
        // Pistol - yedek sınırsız
        if (ActiveWeapon.ammoType == AmmoType.Unlimited)
        {
            ActiveWeapon.currentAmmo = ActiveWeapon.maxAmmo;
            Debug.Log("Pistol dolduruldu!");
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
                Debug.Log(type + " mermi eklendi: " + amount);
            }
        }
    }
}