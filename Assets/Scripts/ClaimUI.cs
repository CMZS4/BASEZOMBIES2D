using UnityEngine;
using UnityEngine.UI;

public class ClaimUI : MonoBehaviour
{
    public static bool panelAcik = false;

    public GameObject claimPanel;

    Button continueButton;
    Button claimButton;
    WaveSpawner waveSpawner;

    void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        claimPanel.SetActive(false);
        panelAcik = false;

        continueButton = claimPanel.transform.Find("ContinueButton").GetComponent<Button>();
        claimButton = claimPanel.transform.Find("ClaimButton").GetComponent<Button>();

        continueButton.onClick.AddListener(OnContinue);
        claimButton.onClick.AddListener(OnClaim);
    }

    public void ShowClaimScreen()
    {
        claimPanel.SetActive(true);
        panelAcik = true;
    }

    void OnContinue()
    {
        claimPanel.SetActive(false);
        panelAcik = false;
        waveSpawner.ContinueGame();
    }

    void OnClaim()
    {
        claimPanel.SetActive(false);
        panelAcik = false;
        Debug.Log("TOKENLAR CLAIM EDİLDİ!");
        FindObjectOfType<PlayerController>().Die();
    }
}