using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject inventoryPanel;
    public GameObject shopPanel;

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
    }

    public void ShowShop()
    {
        if (mainPanel) mainPanel.SetActive(false);
        if (shopPanel) shopPanel.SetActive(true);
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