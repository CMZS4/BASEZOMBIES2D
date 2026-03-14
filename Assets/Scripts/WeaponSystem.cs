using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public int damage;
    public float fireRate;
    public int maxAmmo;
    public int currentAmmo;
    public float dropRateBonus;
    public bool isShotgun;

    public Weapon(string name, int dmg, float rate, int ammo, float dropBonus, bool shotgun = false)
    {
        weaponName = name;
        damage = dmg;
        fireRate = rate;
        maxAmmo = ammo;
        currentAmmo = ammo;
        dropRateBonus = dropBonus;
        isShotgun = shotgun;
    }
}

public class WeaponSystem : MonoBehaviour
{
    public int activeWeaponIndex = 0;
    public int secondWeaponIndex = -1;

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
            new Weapon("Pistol",  1, 2f,   12,  0f),
            new Weapon("Glock",   1, 3.5f, 20,  0f),
            new Weapon("MP5",     2, 8f,   25,  0f),
            new Weapon("AK47",    3, 5f,   30,  0f),
            new Weapon("Shotgun", 5, 1f,   6,   0f, true),
            new Weapon("M249",    4, 10f,  100, 0f),
        };
    }

    public Weapon ActiveWeapon => weapons[activeWeaponIndex];

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
        ActiveWeapon.currentAmmo = ActiveWeapon.maxAmmo;
        Debug.Log(ActiveWeapon.weaponName + " dolduruldu!");
    }
}