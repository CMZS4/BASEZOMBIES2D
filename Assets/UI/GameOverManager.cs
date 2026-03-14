using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject gameOverPanel;

    [Header("Texts")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI tokenText;

    public static GameOverManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(int wave, int kills, int tokens)
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // oyunu dondurur

        waveText.text = "Wave Reached: " + wave;
        killText.text = "Zombies Killed: " + kills;
        tokenText.text = "Tokens Earned: " + tokens;
    }

    public void OnPlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameMenu");
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}