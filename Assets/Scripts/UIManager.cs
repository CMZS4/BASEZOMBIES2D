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

    [Header("Silah")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI weapon2NameText;

    [Header("Envanter")]
    public TextMeshProUGUI healthPackText;

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

        hpBar.value = player.currentHealth;
        waveText.text = "Wave " + waveSpawner.currentWave;

        if (timerText != null)
            timerText.text = Mathf.CeilToInt(waveSpawner.waveTimeRemaining) + "s";

        var active = weaponSystem.ActiveWeapon;
        weaponNameText.text = active.weaponName;
        ammoText.text = active.currentAmmo + "/" + active.maxAmmo;

        if (weaponSystem.secondWeaponIndex != -1)
            weapon2NameText.text = weaponSystem.weapons[weaponSystem.secondWeaponIndex].weaponName;
        else
            weapon2NameText.text = "Empty";

        if (healthPackText != null)
            healthPackText.text = "HP: " + inventory.healthPackCount + " | B: " + inventory.barricadeCount;
    }
}