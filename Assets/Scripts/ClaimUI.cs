using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClaimUI : MonoBehaviour
{
    public static bool panelAcik = false;

    public GameObject claimPanel;
    public TextMeshProUGUI fragmentInfoText;

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
        Time.timeScale = 0f;

        if (fragmentInfoText != null && FragmentManager.instance != null)
            fragmentInfoText.text = "Fragments: " + FragmentManager.instance.GetFragments();
    }

    void OnContinue()
    {
        claimPanel.SetActive(false);
        panelAcik = false;
        Time.timeScale = 1f;
        waveSpawner.ContinueGame();
    }

    void OnClaim()
    {
        claimPanel.SetActive(false);
        panelAcik = false;
        Time.timeScale = 1f;

        int fragments = FragmentManager.instance != null ? FragmentManager.instance.GetFragments() : 0;

        // Fragmentleri PlayerPrefs'e kaydet
        FragmentManager.instance?.ClaimFragments();

        WaveSpawner ws = FindObjectOfType<WaveSpawner>();
        int wave = ws != null ? ws.currentWave : 0;
        int kills = ws != null ? ws.totalKills : 0;

        GameOverManager.instance.ShowGameOver(wave, kills, fragments);
    }
}