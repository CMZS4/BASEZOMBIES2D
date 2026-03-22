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

    [Header("Shop UI")]
    public TextMeshProUGUI shopFragmentText;
    public TextMeshProUGUI shopMessageText;

    string[] weapons = { "Pistol", "Glock", "MP5", "AK-47", "Shotgun", "M249" };

    // Shop fiyatları
    const int HP_PACK_PRICE = 10;
    const int AMMO_PACK_PRICE = 10;
    const int BARRICADE_PRICE = 10;
    const int SPEED_BOOST_PRICE = 10;

    void Start()
    {
        ShowMain();
    }

    // --- Panel Yönetimi ---

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
        UpdateShopUI();
    }

    // --- Inventory ---

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
        float roll = Random.value * 100f;
        string rarity;

        if (roll <= 0.025f)     rarity = "⭐ Legendary";
        else if (roll <= 5f)    rarity = "🔴 Epic";
        else if (roll <= 15f)   rarity = "🔵 Rare";
        else if (roll <= 40f)   rarity = "🟢 Uncommon";
        else                    rarity = "⚪ Common";

        string weapon = weapons[Random.Range(0, weapons.Length)];
        return rarity + " " + weapon;
    }

    // --- Shop ---

    void UpdateShopUI()
    {
        int fragments = PlayerPrefs.GetInt("TotalFragments", 0);
        if (shopFragmentText != null)
            shopFragmentText.text = "Fragments: " + fragments;
        if (shopMessageText != null)
            shopMessageText.text = "";
    }

    bool SpendFragments(int amount, string itemName)
    {
        int fragments = PlayerPrefs.GetInt("TotalFragments", 0);
        if (fragments < amount)
        {
            if (shopMessageText != null)
                shopMessageText.text = "Not enough fragments!";
            return false;
        }

        PlayerPrefs.SetInt("TotalFragments", fragments - amount);
        PlayerPrefs.Save();

        if (shopMessageText != null)
            shopMessageText.text = itemName + " purchased!";

        UpdateShopUI();
        return true;
    }

    public void BuyHPPack()
    {
        if (SpendFragments(HP_PACK_PRICE, "HP Pack"))
        {
            int current = PlayerPrefs.GetInt("HPPackCount", 0);
            PlayerPrefs.SetInt("HPPackCount", current + 1);
            PlayerPrefs.Save();
        }
    }

    public void BuyAmmoPack()
    {
        if (SpendFragments(AMMO_PACK_PRICE, "Ammo Pack"))
        {
            int current = PlayerPrefs.GetInt("AmmoPackCount", 0);
            PlayerPrefs.SetInt("AmmoPackCount", current + 1);
            PlayerPrefs.Save();
        }
    }

    public void BuyBarricade()
    {
        if (SpendFragments(BARRICADE_PRICE, "Barricade"))
        {
            int current = PlayerPrefs.GetInt("BarricadeCount", 0);
            PlayerPrefs.SetInt("BarricadeCount", current + 1);
            PlayerPrefs.Save();
        }
    }

    public void BuySpeedBoost()
    {
        if (SpendFragments(SPEED_BOOST_PRICE, "Speed Boost"))
        {
            int current = PlayerPrefs.GetInt("SpeedBoostCount", 0);
            PlayerPrefs.SetInt("SpeedBoostCount", current + 1);
            PlayerPrefs.Save();
        }
    }

    // --- Diğer ---

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