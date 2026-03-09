using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Player HP")]
    public Slider hpBar;

    [Header("Wave")]
    public TextMeshProUGUI waveText;

    [Header("Silah")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI weapon2NameText;

    PlayerController player;
    WaveSpawner waveSpawner;
    WeaponSystem weaponSystem;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        waveSpawner = FindObjectOfType<WaveSpawner>();
        weaponSystem = FindObjectOfType<WeaponSystem>();

        hpBar.maxValue = player.maxHealth;
        hpBar.value = player.maxHealth;
    }

    void Update()
    {
        if (player == null) return;

        hpBar.value = player.currentHealth;
        waveText.text = "Wave " + waveSpawner.currentWave;

        // Aktif silah
        var active = weaponSystem.ActiveWeapon;
        weaponNameText.text = active.weaponName;
        ammoText.text = active.currentAmmo + "/" + active.maxAmmo;

        // 2. silah
        if (weaponSystem.secondWeaponIndex != -1)
            weapon2NameText.text = weaponSystem.weapons[weaponSystem.secondWeaponIndex].weaponName;
        else
            weapon2NameText.text = "Empty";
    }
}