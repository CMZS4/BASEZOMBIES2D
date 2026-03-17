using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject inventoryPanel;
    public GameObject shopPanel;

    [Header("Inventory UI")]
    public TextMeshProUGUI blindBoxCountText;
    public TextMeshProUGUI openResultText;

    string[] weapons = { "Pistol", "Glock", "MP5", "AK-47", "Shotgun", "M249" };

    void Start()
    {
        ShowMain();
    }

    public void ShowMain()
    {
        if (mainPanel) mainPanel.SetActive(true);
        if (settingsPanel) settingsPanel.SetActive(false);
        if (inventoryPanel) inventoryPanel.SetActive(false);
        if (shopPanel) shopPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        if (mainPanel) mainPanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(true);
    }

    public void ShowInventory()
    {
        if (mainPanel) mainPanel.SetActive(false);
        if (inventoryPanel) inventoryPanel.SetActive(true);
        UpdateInventoryUI();
    }

    public void ShowShop()
    {
        if (mainPanel) mainPanel.SetActive(false);
        if (shopPanel) shopPanel.SetActive(true);
    }

    void UpdateInventoryUI()
    {
        int count = PlayerPrefs.GetInt("BlindBoxCount", 0);
        if (blindBoxCountText != null)
            blindBoxCountText.text = "Blind Boxes: " + count;

        if (openResultText != null)
            openResultText.text = "";
    }

    public void OpenBlindBox()
    {
        int count = PlayerPrefs.GetInt("BlindBoxCount", 0);

        if (count <= 0)
        {
            if (openResultText != null)
                openResultText.text = "No Blind Boxes!";
            return;
        }

        PlayerPrefs.SetInt("BlindBoxCount", count - 1);
        PlayerPrefs.Save();

        string result = GetRandomItem();

        if (openResultText != null)
            openResultText.text = "You got: " + result + "!";

        UpdateInventoryUI();
    }

    string GetRandomItem()
    {
        // Rarity belirle
        float roll = Random.value * 100f;
        string rarity;

        if (roll <= 0.025f)     rarity = "⭐ Legendary";
        else if (roll <= 5f)    rarity = "🔴 Epic";
        else if (roll <= 15f)   rarity = "🔵 Rare";
        else if (roll <= 40f)   rarity = "🟢 Uncommon";
        else                    rarity = "⚪ Common";

        // Random silah seç
        string weapon = weapons[Random.Range(0, weapons.Length)];

        return rarity + " " + weapon;
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void OnWalletButton()
    {
        Debug.Log("Wallet - yakında!");
    }

    public void OnBackButton()
    {
        ShowMain();
    }
}