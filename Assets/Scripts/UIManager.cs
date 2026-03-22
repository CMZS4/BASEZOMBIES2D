using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Player HP")]
    public Slider hpBar;

    [Header("Wave")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;

    [Header("Silah 1 (Aktif)")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI ammoText;

    [Header("Silah 2 (Pasif)")]
    public TextMeshProUGUI weapon2NameText;
    public TextMeshProUGUI ammo2Text;

    [Header("Envanter")]
    public TextMeshProUGUI healthPackText;
    public TextMeshProUGUI barricadeText;
    public TextMeshProUGUI ammoPackText;
    public TextMeshProUGUI speedBoostText;
    public TextMeshProUGUI fragmentText;

    PlayerController player;
    WaveSpawner waveSpawner;
    WeaponSystem weaponSystem;
    InventorySystem inventory;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        waveSpawner = FindObjectOfType<WaveSpawner>();
        weaponSystem = FindObjectOfType<WeaponSystem>();
        inventory = FindObjectOfType<InventorySystem>();

        hpBar.maxValue = player.maxHealth;
        hpBar.value = player.maxHealth;
    }

    void Update()
    {
        if (player == null) return;

        // HP
        hpBar.value = player.currentHealth;

        // Wave + Timer
        waveText.text = "Wave " + waveSpawner.currentWave;
        if (timerText != null)
            timerText.text = Mathf.CeilToInt(waveSpawner.waveTimeRemaining) + "s";

        // Aktif silah
        var active = weaponSystem.ActiveWeapon;
        weaponNameText.text = active.weaponName;

        if (active.unlimitedAmmo)
            ammoText.text = active.currentAmmo + "/" + active.maxAmmo + " ∞";
        else
            ammoText.text = active.currentAmmo + "/" + active.maxAmmo + " | " + active.currentReserve;

        // Pasif silah
        if (weaponSystem.secondWeaponIndex != -1)
        {
            var second = weaponSystem.weapons[weaponSystem.secondWeaponIndex];
            weapon2NameText.text = second.weaponName;
            if (ammo2Text != null)
            {
                if (second.unlimitedAmmo)
                    ammo2Text.text = second.currentAmmo + "/" + second.maxAmmo + " ∞";
                else
                    ammo2Text.text = second.currentAmmo + "/" + second.maxAmmo + " | " + second.currentReserve;
            }
        }
        else
        {
            weapon2NameText.text = "Empty";
            if (ammo2Text != null)
                ammo2Text.text = "";
        }

        // Envanter
        if (healthPackText != null)
            healthPackText.text = "HP Pack: " + inventory.healthPackCount;

        if (barricadeText != null)
            barricadeText.text = "Barikat: " + inventory.barricadeCount;

        if (ammoPackText != null)
            ammoPackText.text = "Ammo Pack: " + inventory.ammoPackCount;

        if (speedBoostText != null)
            speedBoostText.text = "Speed: " + inventory.speedBoostCount;

        // Fragment
        if (fragmentText != null && FragmentManager.instance != null)
            fragmentText.text = "Fragments: " + FragmentManager.instance.fragmentCount;
    }
}