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
    public TextMeshProUGUI fragmentText; // token → fragment

    public static GameOverManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(int wave, int kills, int fragments)
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        waveText.text = "Wave Reached: " + wave;
        killText.text = "Zombies Killed: " + kills;
        fragmentText.text = "Fragments Claimed: " + fragments; // token → fragments
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