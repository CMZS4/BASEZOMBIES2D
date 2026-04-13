using UnityEngine;
using TMPro;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem instance;

    [Header("Referanslar")]
    public Transform playerTransform;
    public TextMeshPro comboText;

    [Header("Ayarlar")]
    public float comboTimeout = 3f;
    public float maxDamageBonus = 0.5f;
    public float bonusPerCombo = 0.05f;

    public int comboCount = 0;
    public float damageMultiplier = 1f;

    float lastKillTime;
    bool comboActive = false;
    float displayTimer = 0f;
    WeaponSystem weaponSystem;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        weaponSystem = FindObjectOfType<WeaponSystem>();
        if (comboText != null) comboText.enabled = false;
    }

    void Update()
    {
        bool isReloading = weaponSystem != null && weaponSystem.isReloading;

        if (comboActive && !isReloading)
        {
            if (Time.time - lastKillTime >= comboTimeout)
                ResetCombo();
        }

        if (displayTimer > 0)
        {
            displayTimer -= Time.deltaTime;
            if (displayTimer <= 0 && comboText != null)
                comboText.enabled = false;
        }

        if (comboText != null && playerTransform != null && comboText.enabled)
        {
            comboText.transform.position = playerTransform.position + new Vector3(0f, 1.2f, 0f);
            comboText.transform.rotation = Quaternion.identity;
        }
    }

    public void RegisterKill()
    {
        float timeSinceLast = Time.time - lastKillTime;
        bool isReloading = weaponSystem != null && weaponSystem.isReloading;

        if (comboActive && (timeSinceLast <= comboTimeout || isReloading))
        {
            comboCount++;
        }
        else
        {
            comboCount = 1;
            comboActive = true;
        }

        lastKillTime = Time.time;
        damageMultiplier = 1f + Mathf.Min((comboCount - 1) * bonusPerCombo, maxDamageBonus);
        ShowCombo();
    }

    void ShowCombo()
    {
        if (comboText == null || comboCount < 2) return;

        comboText.enabled = true;
        comboText.text = "x" + comboCount + " COMBO!";

        if (comboCount >= 8)      comboText.color = new Color(1f, 0.2f, 0f);
        else if (comboCount >= 5) comboText.color = new Color(1f, 0.6f, 0f);
        else                      comboText.color = new Color(1f, 0.9f, 0f);

        displayTimer = 1.5f;
    }

    void ResetCombo()
    {
        comboCount = 0;
        damageMultiplier = 1f;
        comboActive = false;
        if (comboText != null) comboText.enabled = false;
    }
}