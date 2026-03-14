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
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
        inventoryPanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void ShowInventory()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        shopPanel.SetActive(false);
    }

    public void ShowShop()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        shopPanel.SetActive(true);
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