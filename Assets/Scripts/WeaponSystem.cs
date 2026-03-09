using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public int damage;
    public float fireRate;      // saniyede kaç mermi
    public int maxAmmo;
    public int currentAmmo;
    public float dropRateBonus; // token drop bonusu

    public Weapon(string name, int dmg, float rate, int ammo, float dropBonus)
    {
        weaponName = name;
        damage = dmg;
        fireRate = rate;
        maxAmmo = ammo;
        currentAmmo = ammo;
        dropRateBonus = dropBonus;
    }
}

public class WeaponSystem : MonoBehaviour
{
    public Weapon[] weapons = new Weapon[]
    {
        new Weapon("Pistol", 1, 2f, 12, 0f),
        new Weapon("AK47",   3, 5f, 30, 0.05f),
        new Weapon("MP5",    2, 8f, 25, 0.02f),
        new Weapon("M249",   4, 10f, 100, 0.08f),
    };

    public int activeWeaponIndex = 0;
    public int secondWeaponIndex = -1; // -1 = boş

    public Weapon ActiveWeapon => weapons[activeWeaponIndex];

    void Update()
    {
        // Q ile silah değiştir
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